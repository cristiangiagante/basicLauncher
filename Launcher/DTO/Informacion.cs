using System;

namespace Launcher
{
    public abstract class Informacion:InformacionCustoms
    {
        public static int ArchivosPendientes { get; set; }
        public static int ArchivosDescargados { get; set; }
        public static string EjecutableMain { get; set; }
        public static EjecutableInfo EjecutarInfo{ get; set; }
        public static int Porcentaje { get; set; }
        public static Uri RutaCrc { get; set; }
        public static string Error { get; set; }
        public static bool Actualizado { get; set; }
    }
}
