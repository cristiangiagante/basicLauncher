# basicLauncher it's a library to build launchers easy

::Changelog::
- Crc updates generator
- basicLauncher library


EXAMPLES:

Launcher.Launcher launcher = new Launcher.Launcher("main.exe", "http://myHost/myUpdatesFolder/BasicLauncher.crc", true);
            var valido=launcher.ArchivosRemotos[0].VerificarIntegridad();
            launcher.VerificarCambiosYDescargar();
            Console.WriteLine($"Ejecutable: {Informacion.EjecutableMain + Environment.NewLine}");
            Console.WriteLine($"Pendientes: {Informacion.ArchivosPendientes + Environment.NewLine}");
            Console.WriteLine($"Totales: {Informacion.ArchivosDescargados + Environment.NewLine}");
            Console.WriteLine($"Porcentaje: {Informacion.Porcentaje}%");

            Console.WriteLine(Informacion.Error);
            Console.ReadLine();	