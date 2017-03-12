using System;
using System.IO;
using System.Net;
using System.Text;

namespace Launcher
{
    public partial class Archivo : ArchivoCustoms
    {
        public bool CustomDownload { get; set; }
        public string Nombre { get; }
        public float Espacio { get; }
        public string Checksum { get; }
        public string RutaLocal { get; }
        private Uri RutaRemota { get; }
        private string RutaInterna { get; }
        public Archivo(string nombre, string rutaLocal, float espacio, Uri rutaRemota, string rutaInterna)
        {
            RutaLocal = rutaLocal;
            Nombre = nombre;
            Espacio = espacio;
            RutaRemota = rutaRemota;
            RutaInterna = rutaInterna;
            Checksum = GenerarChecksum(RutaLocal);
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
            Checksum = GenerarChecksum(RutaLocal);
        }

        public Archivo(string nombre, string crc, string rutaLocal)
        {
            Nombre = nombre;
            Checksum = crc;
            RutaLocal = rutaLocal;
        }

        public Archivo(string nombre, string rutaLocal, long espacio, string rutaInterna)
        {
            this.Nombre = nombre;
            this.RutaLocal = rutaLocal;
            this.Espacio = espacio;
            this.RutaInterna = rutaInterna;
            Checksum = GenerarChecksum(RutaInterna);
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
                if(!Directory.Exists(RutaLocal))
                {
                    Directory.CreateDirectory(RutaLocal);
                }
                WebClient wc = new WebClient();
                var filtroHostCarpeta = RutaRemota.AbsoluteUri.Replace($"http://{RutaRemota.Host}/", "");
                var carpetaUpdates = filtroHostCarpeta.Replace("/BasicLauncher.crc", "");
                StringBuilder urlOrigen = new StringBuilder();
                urlOrigen.Append($"http://{RutaRemota.Host}/{carpetaUpdates}/{RutaLocal}/{Nombre}");
                StringBuilder localizacion = new StringBuilder();
                localizacion.Append($@"{RutaLocal}\{Nombre}");
                if(CustomDownload)
                {
                    DownloadFile(urlOrigen.ToString(), localizacion.ToString());
                }
                else
                {
                    wc.DownloadFile(urlOrigen.ToString(), localizacion.ToString());
                }
            }
            catch (Exception e)
            {
                Informacion.Error += $"Error en la descarga de archivos: {e.Message} En el archivo: {Nombre}" + Environment.NewLine + $"Detalles: {e.InnerException.Message}" + Environment.NewLine;
            }    
        }

        private string GenerarChecksum(string ruta)
        {
            
            using (var lector = new StreamReader(ruta + @"\" + Nombre))
            {
                    var md5Hash = CyberCrypt._MD5.GetMD5Hash(lector.ReadToEnd());
                    return md5Hash;
            }
        }
    }
}
