using Launcher;
using System;

namespace BasicLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Launcher.Launcher launcher = new Launcher.Launcher("main.exe","http://muwlauncher.com/basicLauncher/BasicLauncher.crc");
            Console.WriteLine($"Ejecutable: {Informacion.EjecutableMain + Environment.NewLine}");
            Console.WriteLine($"Pendientes: {Informacion.ArchivosPendientes + Environment.NewLine}");
            Console.WriteLine($"Totales: {Informacion.ArchivosDescargados + Environment.NewLine}");
            Console.WriteLine($"Porcentaje: {Informacion.Porcentaje}%");

            Console.WriteLine(Informacion.Error);
            Console.ReadLine();
        }
    }
}
