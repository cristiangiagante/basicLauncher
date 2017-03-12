using System;
using System.IO;
using System.Net;

namespace Launcher
{
    public class Archivo
    {
        public string Nombre { get; }
        public float Espacio { get; }
        public string Checksum { get; }
        public string RutaLocal { get; }
        private Uri RutaRemota { get; }

        public Archivo(string nombre, string rutaLocal, float espacio, Uri rutaRemota)
        {
            RutaLocal = rutaLocal;
            Nombre = nombre;
            Espacio = espacio;
            RutaRemota = rutaRemota;
            Checksum = GenerarChecksum();
        }

        public Archivo(string nombre, string crc, string rutaLocal, Uri rutaCrc)
        {
            Nombre = nombre;
            Checksum = crc;
            RutaLocal = rutaLocal;
            RutaRemota = rutaCrc;
        }

        public Archivo(string nombre, string rutaLocal, float espacio)
        {
            RutaLocal = rutaLocal;
            Nombre = nombre;
            Espacio = espacio;
            Checksum = GenerarChecksum();
        }

        public Archivo(string nombre, string crc, string rutaLocal)
        {
            Nombre = nombre;
            Checksum = crc;
            RutaLocal = rutaLocal;
        }

        private void Borrar()
        {
            File.Delete(this.Nombre);
        }

        public void Descargar()
        {
            try
            {
                if (File.Exists($@"{RutaLocal}\{Nombre}"))
                {
                    File.Delete($@"{RutaLocal}\{Nombre}");
                }
                WebClient wc = new WebClient();
                var filtroHostCarpeta = RutaRemota.AbsoluteUri.Replace($"http://{RutaRemota.Host}/", "");
                var carpetaUpdates = filtroHostCarpeta.Replace("/BasicLauncher.crc", "");
                wc.DownloadFile($"http://{RutaRemota.Host}/{carpetaUpdates}/{Nombre}", $@"{RutaLocal}\{Nombre}");
            }
            catch (Exception e)
            {
                Informacion.Error += $"Error en la descarga de archivos: {e.Message} En el archivo: {Nombre}" + Environment.NewLine;
            }    
        }

        private string GenerarChecksum()
        {
            
            using (var lector = new StreamReader(RutaLocal + @"\" + Nombre))
            {
                    var md5Hash = CyberCrypt._MD5.GetMD5Hash(lector.ReadToEnd());
                    return md5Hash;
            }
        }
    }
}
