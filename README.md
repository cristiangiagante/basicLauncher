# basicLauncher es una libreria para construir launchers facil

## ::Changelog::
- Generador de tabla de integridad de archivos
- Libreria basicLauncher
	+ Mapeo de archivos de updates a objetos
	+ Informacion contextual sobre el estado de los updates
	+ Metodos pre construidos para generar un launcher
	+ Generacion de metodos personalizados para extender el launcher por fuera del codigo fuente del nucleo de basicLauncher

## EXAMPLES:

```csharp
using Launcher;
using System;

namespace BasicLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
			//Creo un nuevo launcher, le paso el nombre del ejecutable, la ruta del archivo de tabla de integridad y si usara customs o no.
            Launcher.Launcher launcher = new Launcher.Launcher("main.exe", "http://hosting/basicLauncher/BasicLauncher.crc", true);

			//Selecciono un archivo remoto (archivos que forman parte de la actualizacion) y compruebo su integridad CRC.
            var valido=launcher.ArchivosRemotos[0].VerificarIntegridad();

			//Verifico la integridad de forma global y comienzo la descarga de los archivos si es necesario.
            launcher.VerificarCambiosYDescargar();

			//Informacion de la instancia de launcher
            Console.WriteLine($"Ejecutable: {Informacion.EjecutableMain + Environment.NewLine}");
            Console.WriteLine($"Pendientes: {Informacion.ArchivosPendientes + Environment.NewLine}");
            Console.WriteLine($"Totales: {Informacion.ArchivosDescargados + Environment.NewLine}");
            Console.WriteLine($"Porcentaje: {Informacion.Porcentaje}%");

			//Muestro si hay algun error en la descarga
            Console.WriteLine(Informacion.Error);

			//Espero a presionar una tecla para cerrar la ventana de la consola.
            Console.ReadLine();
        }
    }
}

```

## Acerca del proyecto
basicLauncher es una libreria open source con el fin de generar una capa de abstraccion para facilitar el desarrollo de la funcionalidad
basica de un launcher y proveer caracteristicas avanzadas con el paso del tiempo.

## Colaboradores
Es bienvenida gente de todos los niveles para colaborar con el proyecto si les interesa

## Autor

Cristian Adrian Giagante