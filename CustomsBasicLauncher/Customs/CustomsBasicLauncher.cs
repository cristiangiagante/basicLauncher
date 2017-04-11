using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Launcher
{
    public class Customs //(Personalizaciones disponibles: Ninguna)
    {

    }
    public class ArchivoCustoms //(Personalizaciones disponibles: Descarga de archivo remoto)
    {
        public string labelSpeed { get; set; }
        public int progressBar { get; set; }
        public string labelPerc { get; set; }
        public string labelDownloaded { get; set; }
        public string Error { get; set; }
        public int ArchivosDescargados { get; set; }

        WebClient webClient;               // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();
        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    Error += "Descarga detallada: " + ex.Message;
                }
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            labelSpeed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar = e.ProgressPercentage;

            // Show the percentage on our label.
            labelPerc = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            labelDownloaded = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                Error += "Descarga detallada: Descarga cancelada.";
            }
            else
            {
                ArchivosDescargados++;
            }
        }
    }
    public class InformacionCustoms //(Personalizaciones disponibles: Ninguna)
    {

    }
}
