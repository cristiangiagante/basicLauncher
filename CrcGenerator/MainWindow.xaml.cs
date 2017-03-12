using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Launcher;

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
            var archivosValidos = e.Data.GetDataPresent(DataFormats.FileDrop,true);
            if (archivosValidos)
            {
                try
                {
                    var crc = File.CreateText("BasicLauncher.crc");
                    StringBuilder contenidoCrc = new StringBuilder();
                    var carpeta = (string[]) e.Data.GetData(DataFormats.FileDrop);
                    DirectoryInfo dir = new DirectoryInfo(carpeta[0]);
                    foreach (var file in dir.GetFiles())
                    {
                        if (!file.Name.Equals("Launcher.exe"))
                        {
                            var archivo = new Archivo(file.Name, file.DirectoryName, file.Length);
                            contenidoCrc.AppendLine($"{archivo.Nombre} * {archivo.Checksum} * {archivo.RutaLocal}");
                        }
                    }
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
    }
}
