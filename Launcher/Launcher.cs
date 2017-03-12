using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Launcher
{
    public class Launcher
    {
        public static List<Archivo> ArchivosLocales { get; set; } = new List<Archivo>();
        public static List<Archivo> ArchivosRemotos { get; set; } = new List<Archivo>();
        public Uri RutaCrc { get; }
        private string CurrentDirectory { get; set; }
        public Launcher(string ejecutable, string rutaCrc)
        {
            CurrentDirectory = Environment.CurrentDirectory;
            Informacion.EjecutableMain = ejecutable;
            var rutaCrcUri=new Uri(rutaCrc); //Convierto a URI
            Informacion.RutaCrc = rutaCrcUri;
            RutaCrc = rutaCrcUri;
            DirectoryInfo dir = new DirectoryInfo(".");
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            WebClient br=new WebClient();
            var contenidoCrc = br.DownloadString(RutaCrc);
            br.Dispose();
            var contenidoCrcPorLineas = contenidoCrc.Split('\n');

            try
            {//Remoto
                var contenidoFiltrado = contenidoCrcPorLineas.Where(l => l!="\r" && l!=""); //Filtro para caracteres de retorno que produce el Crc Generator al final del archivo
                foreach (var linea in contenidoFiltrado)
                {
                    var lineaPartida = linea.Split(new string[] { " * " }, StringSplitOptions.RemoveEmptyEntries);
                    var archivo = new Archivo(lineaPartida[0], lineaPartida[1], lineaPartida[2].Replace("\r", ""),RutaCrc);
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

                foreach (FileInfo file in files)
                {
                    if (!file.Name.Equals("BasicLauncher.exe"))
                    {
                        var rutaRelativa = file.DirectoryName.Replace(CurrentDirectory, ".");
                        var archivo = new Archivo(file.Name, rutaRelativa+ @"\", file.Length, RutaCrc, file.DirectoryName);
                        ArchivosLocales.Add(archivo);
                    }
                }
            }
            catch (Exception eLocal)
            {
                Informacion.Error += $"Error al obtener informacion local: {eLocal.Message}" + Environment.NewLine;
            }

            VerificarCambios();
        }

        private void VerificarCambios()
        {
            foreach (var archivoRemoto in ArchivosRemotos)
            {
                //Si el crc del archivo local no se encuentra entre los remotos
                var crcExiste = ArchivosLocales.Any(r => r.Checksum == archivoRemoto.Checksum && r.RutaLocal==archivoRemoto.RutaLocal);
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
        }

        private void BorrarArchivos(List<string> archivos)
        {

        }
    }
}
