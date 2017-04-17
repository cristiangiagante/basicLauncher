using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Launcher
{
    public class Launcher : Customs
    {
        private bool Customs { get; }
        public List<Archivo> ArchivosLocales { get; set; } = new List<Archivo>();
        public List<Archivo> ArchivosRemotos { get; set; } = new List<Archivo>();
        public Uri RutaCrc { get; }
        private string CurrentDirectory { get; set; }
        private List<FileInfo> Files { get; set; }
        public Launcher(string ejecutable, string rutaCrc, bool customs)
        {
            Customs = customs;
            CurrentDirectory = Environment.CurrentDirectory;
            Informacion.EjecutableMain = ejecutable;
            var rutaCrcUri = new Uri(rutaCrc); //Convierto a URI
            Informacion.RutaCrc = rutaCrcUri;
            RutaCrc = rutaCrcUri;
            WebClient br = new WebClient();
            var contenidoCrc = br.DownloadString(RutaCrc);
            br.Dispose();
            var contenidoCrcPorLineas = contenidoCrc.Split('\n');

            try
            {//Remoto
                var contenidoFiltrado = contenidoCrcPorLineas.Where(l => l != "\r" && l != ""); //Filtro para caracteres de retorno que produce el Crc Generator al final del archivo
                foreach (var linea in contenidoFiltrado)
                {
                    var lineaPartida = linea.Split(new string[] { " * " }, StringSplitOptions.RemoveEmptyEntries);
                    var archivo = new Archivo(lineaPartida[0], lineaPartida[1], lineaPartida[2].Replace("\r", ""), RutaCrc)
                    {
                        CustomDownload = Customs
                    };
                    ArchivosRemotos.Add(archivo);
                }
                Informacion.ArchivosPendientes = ArchivosRemotos.Count();
            }
            catch (Exception eRemoto)
            {
                Informacion.Error += $"Error al obtener informacion remota: {eRemoto.Message}" + Environment.NewLine;
            }

            try
            {//Local
                ObtenerArchivosLocales();
            }
            catch (Exception eLocal)
            {
                Informacion.Error += $"Error al obtener informacion local: {eLocal.Message}" + Environment.NewLine;
            }
            if (Informacion.EjecutarInfo.ActualizarAutomaticamente)
            {
                VerificarIntegridadYDescargar();
            }
        }

        public bool VerificarIntegridad()
        {
            bool validacionCrc = false;
            foreach (var file in ArchivosRemotos)
            {
                if (!file.VerificarIntegridad())
                {
                    validacionCrc = false;
                    break;
                }
                else
                {
                    validacionCrc = true;
                }
            }
            return validacionCrc;
        }

        public void VerificarIntegridadYDescargar()
        {
            try
            {//Reseteo los archivos locales y los evaluo nuevamente para comparar con los remotos
                ObtenerArchivosLocales();
            }
            catch (Exception eLocal)
            {
                Informacion.Error += $"Error al obtener informacion local: {eLocal.Message}" + Environment.NewLine;
            }
            foreach (var archivoRemoto in ArchivosRemotos)
            {
                //Si el crc del archivo local no se encuentra entre los remotos
                var crcExiste = ArchivosLocales.Any(l => l.Checksum == archivoRemoto.Checksum && l.RutaLocal == archivoRemoto.RutaLocal);
                if (!crcExiste)
                {
                    archivoRemoto.Descargar();
                    Informacion.ArchivosDescargados++;
                }
                Informacion.ArchivosPendientes--;
                try
                {
                    Informacion.Porcentaje = Informacion.ArchivosDescargados * 100 /
                                                             (Informacion.ArchivosDescargados + Informacion.ArchivosPendientes);
                }
                catch
                {
                    Informacion.Porcentaje = 0;
                    Informacion.Error += "Los archivos ya estan actualizados, no es necesario descargarlos.";
                }
            }
            if (Informacion.EjecutarInfo.EjecucionAutomaticaAlActualizar)
            {
                Ejecutar();
            }
        }

        private void BorrarArchivos(List<string> archivos)
        {

        }

        public void Ejecutar()
        {
            try
            {
                if (Informacion.EjecutarInfo.VerificarIntegridadAlEjecutar)
                {
                    Informacion.EjecutarInfo.VerificarIntegridadAlEjecutar = false;
                    VerificarIntegridadYDescargar();
                }
                if (Informacion.EjecutarInfo.DetenerEjecucionAlVerificarIntegridad)
                {
                    Informacion.Actualizado = VerificarIntegridad();
                    if (Informacion.Actualizado)
                    {
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.CreateNoWindow = true;
                        info.FileName = Informacion.EjecutableMain;
                        info.Arguments = String.IsNullOrEmpty(Informacion.EjecutarInfo.Argumentos) ? "" : Informacion.EjecutarInfo.Argumentos;
                        info.UseShellExecute = false;
                        Process.Start(info);
                    }
                    else
                    {
                        Informacion.Error = "Verificacion CRC no concuerda";
                    }

                }
                else if(!Informacion.EjecutarInfo.EjecucionAutomaticaAlActualizar)
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.CreateNoWindow = true;
                    info.FileName = Informacion.EjecutableMain;
                    info.Arguments = String.IsNullOrEmpty(Informacion.EjecutarInfo.Argumentos) ? "" : Informacion.EjecutarInfo.Argumentos;
                    info.UseShellExecute = false;
                    Process.Start(info);
                }
            }
            catch (Exception e)
            {
                Informacion.Error = e.Message;
            }
        }

        private void ObtenerArchivosLocales()
        {
            Files.Clear();
            DirectoryInfo dir = new DirectoryInfo(".");
            Files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            foreach (FileInfo file in Files)
            {
                if (!file.Name.Equals("BasicLauncher.exe"))
                {
                    var rutaRelativa = file.DirectoryName.Replace(CurrentDirectory, ".");
                    var archivo = new Archivo(file.Name, rutaRelativa + @"\", file.Length, RutaCrc, file.DirectoryName);
                    ArchivosLocales.Add(archivo);
                }
            }
        }
    }
}
