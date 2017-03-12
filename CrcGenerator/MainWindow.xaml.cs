using Launcher;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace CrcGenerator
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var archivosValidos = e.Data.GetDataPresent(DataFormats.FileDrop, true);
            if (archivosValidos)
            {
                try
                {
                    var crc = File.CreateText("BasicLauncher.crc");
                    StringBuilder contenidoCrc = new StringBuilder();
                    var carpeta = (string[])e.Data.GetData(DataFormats.FileDrop);
                    DirectoryInfo dir = new DirectoryInfo(carpeta[0]);
                    Environment.CurrentDirectory = dir.FullName;
                    CreateCrcFile(dir, contenidoCrc);
                    crc.WriteLine(contenidoCrc);
                    crc.Close();
                    contenidoCrc.Clear();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show($"Error encontrado: No se puede encontrar la carpeta.");
                }

            }
        }
        
        private void CreateCrcFile(DirectoryInfo rootDir, StringBuilder contenidoCrc)
        {
            //Agrego al crc los archivos del root path
            FindFilesOnFolder(rootDir, contenidoCrc, "Launcher.exe");
            //Busco en las subcarpetas de forma recursiva
            FindSubfolders(rootDir, contenidoCrc);
        }
        private void FindSubfolders(DirectoryInfo rootDir, StringBuilder contenidoCrc)
        {
            var directorios = rootDir.GetDirectories();

            foreach (var subdir in directorios)
            {
                FindFilesOnFolder(subdir, contenidoCrc);
                FindSubfolders(subdir, contenidoCrc);
            }
        }

        private void FindFilesOnFolder(DirectoryInfo dir, StringBuilder contenidoCrc)
        {
            foreach (var file in dir.GetFiles())
            {
                var relativePath = file.DirectoryName.Replace(Environment.CurrentDirectory, ".");
                var archivo = new Archivo(file.Name, relativePath + @"\", file.Length, file.DirectoryName);
                contenidoCrc.AppendLine($"{archivo.Nombre} * {archivo.Checksum} * {archivo.RutaLocal}");
            }
        }

        private void FindFilesOnFolder(DirectoryInfo dir, StringBuilder contenidoCrc, string exceptionFile)
        {
            foreach (var file in dir.GetFiles())
            {
                if (!file.Name.Equals(exceptionFile))
                {
                    var archivo = new Archivo(file.Name, @".\", file.Length, file.DirectoryName);
                    contenidoCrc.AppendLine($"{archivo.Nombre} * {archivo.Checksum} * {archivo.RutaLocal}");
                }
            }
        }
    }
}
