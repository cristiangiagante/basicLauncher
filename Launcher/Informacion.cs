using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Launcher
{
    public abstract class Informacion
    {
        public static int ArchivosPendientes { get; set; }
        public static int ArchivosDescargados { get; set; }
        public static string EjecutableMain { get; set; }
        public static int Porcentaje { get; set; }
        public static Uri RutaCrc { get; set; }
        public static string Error { get; set; }
    }
}
