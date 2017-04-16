using Launcher;
using System;

namespace BasicLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Launcher.Launcher launcher = new Launcher.Launcher("main.exe", "http://muwlauncher.com/basicLauncher/BasicLauncher.crc", true);
            
            var valido=launcher.ArchivosRemotos[0].VerificarIntegridad();
            Launcher.Registro.ImportarRegistroDesdeString(@"[HKEY_CURRENT_USER\Software\Blizzard Entertainment\Warcraft III]'Battle.net Gateways'=hex(7):31,30,30,31,00,30,30,00,31,39,38,2e,35,30,2e,31,\38,35,2e,39,33,00,2d,33,00,73,65,72,76,65,72,00,\00");
            launcher.VerificarIntegridadYDescargar();
            Console.WriteLine($"Ejecutable: {Informacion.EjecutableMain + Environment.NewLine}");
            Console.WriteLine($"Pendientes: {Informacion.ArchivosPendientes + Environment.NewLine}");
            Console.WriteLine($"Totales: {Informacion.ArchivosDescargados + Environment.NewLine}");
            Console.WriteLine($"Porcentaje: {Informacion.Porcentaje}%");
            Console.WriteLine(Informacion.Error);
           
            Console.ReadLine();
        }
    }
}
