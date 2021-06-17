using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;
using ICSharpCode.SharpZipLib.Zip;
using System.Linq;

public class ZipDownloader
{
    public class Downloader
    {
        private bool _complete;
        public bool DownloadCompleted { get => _complete; }

        public void DownloadFileAsync(string address, string location)
        {
            WebClient client = new WebClient();
            System.Uri url = new System.Uri(address);

            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(Completed);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            client.DownloadFileAsync(url, location);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                UnityEngine.Debug.LogWarning("Download has been cancelled");
            else
                UnityEngine.Debug.Log("Download has completed!");

            _complete = true;
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
           //  UnityEngine.Debug.Log($"UserState : {e.UserState}, ByteReceived : {e.BytesReceived}, TotalBytesToReceive : {e.TotalBytesToReceive}, ProgressPercentage :{e.ProgressPercentage}");
        }
    }

    public async Task DownloadFileAsync(string address, string location)
    {
        Downloader downloader = new Downloader();
        downloader.DownloadFileAsync(address, location);

        await Task.Factory.StartNew(() => { while (!downloader.DownloadCompleted) ; });
    }

    public async Task ExtractZipAsync(string targetPath, string savePath)
    {
        string folderName = targetPath.Split('/').LastOrDefault()?.Replace(".zip", "");

        if(Directory.Exists($"{savePath}/{folderName}"))
        {
            UnityEngine.Debug.Log("Extract File is exist!");
            return;
        }

        await Task.Factory.StartNew(()=>{

            try
            {
                using (ZipInputStream zipIn = new ZipInputStream(File.OpenRead(targetPath)))
                {
                    ZipEntry entry;
                    while ((entry = zipIn.GetNextEntry()) != null)
                    {
                        string dirPath = Path.GetDirectoryName($"{savePath}\\{entry.Name}");

                        UnityEngine.Debug.Log(entry.Name);

                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);

                        if (!entry.IsDirectory)
                        {
                            using (FileStream streamWriter = File.Create($"{savePath}\\{entry.Name}"))
                            {
                                int size = 2048;
                                byte[] buffer = new byte[size];

                                while ((size = zipIn.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    streamWriter.Write(buffer, 0, size);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }

        });
    }

    public async void DownloadAndExtractZip(Action inCallback, string inUrl)
    {
        string fileName = inUrl.Split('/').LastOrDefault();
        string address = inUrl;
        string savePath = UnityEngine.Application.persistentDataPath;

        await DownloadFileAsync(address,  $"{savePath}\\{fileName}" );
        await ExtractZipAsync($"{savePath}/{fileName}", savePath);

        UnityEngine.Debug.Log("다운로드 및 압축풀기 완료!");

        inCallback?.Invoke();
    }
}
