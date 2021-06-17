using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
using UnityEngine.UI;

using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Text.RegularExpressions;

public class ZipEx : MonoBehaviour
{


    [SerializeField] Image[] images;

    //---------------
    // 파일다운로드
    //---------------

    public void LoadFile()
    {
        try
        {
            using (WebClient client = new WebClient())
            {
               //  client.DownloadFile(url, savePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    // 2. 비동기 다운로드
    public class Downloader
    {
        private volatile bool _complete;
        public bool DownloadCompleted { get => _complete; }

        public void DownloadFile(string address, string location)
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
                Debug.LogWarning("Download has been canceled");
            else
                Debug.Log("Download has completed!");

            _complete = true;
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Debug.Log($"UserState : {e.UserState}, ByteReceived : {e.BytesReceived}, TotalBytesToReceive : {e.TotalBytesToReceive}, ProgressPercentage :{e.ProgressPercentage}");
        }
    }


    //------------------------
    // 압축풀기
    //------------------------
    public void ExtractZip()
    {
        FastZip fastZip = new FastZip();
        // fastZip.ExtractZip(savePath, Application.persistentDataPath, null);
    }

    //------------------------
    // 압축풀기(비동기)
    //------------------------
    public void ExtractZipAsync()
    {
        var waiting = new AsyncWaiting();

        FastZipEvents zipEvents = new FastZipEvents();

        // 폴더내부의 개별파일 완료시 callback
        zipEvents.CompletedFile = new ICSharpCode.SharpZipLib.Core.CompletedFileHandler((object sender, ScanEventArgs e) => 
        {

        });

        zipEvents.Progress = new ProgressHandler((object sender, ProgressEventArgs e) => 
        {
            Debug.Log(e.Target);
        });

        zipEvents.ProcessFile = new ProcessFileHandler((object sender, ScanEventArgs e) =>
        {

        });

        zipEvents.ProcessDirectory = new ProcessDirectoryHandler((object sender, DirectoryEventArgs e) => 
        {
            Debug.Log(e.HasMatchingFiles);
        });

        FastZip fastZip = new FastZip(zipEvents);
        // fastZip.ExtractZip(savePath, Application.persistentDataPath, null);

        Debug.Log("압축풀기 완료되었습니다.");
    }

    public void UnZipp(string srcDirPath, string destDirPath)
    {
        ZipInputStream zipIn = null;
        FileStream streamWriter = null;

        try
        {
            // Directory.CreateDirectory(Path.GetDirectoryName(destDirPath));

            zipIn = new ZipInputStream(File.OpenRead(srcDirPath));
            ZipEntry entry;

            while ((entry = zipIn.GetNextEntry()) != null)
            {
                string dirPath = Path.GetDirectoryName(destDirPath + entry.Name);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (!entry.IsDirectory)
                {
                    streamWriter = File.Create(destDirPath + entry.Name);
                    int size = 2048;
                    byte[] buffer = new byte[size];

                    while ((size = zipIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        streamWriter.Write(buffer, 0, size);
                    }
                }

                streamWriter.Close();
            }
            
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            // do nothing
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (zipIn != null)
            {
                zipIn.Close();
            }

            if (streamWriter != null)
            {
                streamWriter.Close();
            }
        }
    }



    //-----------------------
    // 파일경로 찾기
    //-----------------------
    public string [] GetFilePaths()
    {
        string [] names = Directory.GetFiles($"{Application.persistentDataPath}/GameEvent_DrawingDiary", "*spriteatlas", SearchOption.AllDirectories);
        return names;
    }


    //-----------------------
    // 에디터에서 에셋 로드
    //-----------------------
    public void LoadAsset()
    {
        string path = GetFilePaths()[0];
        path = path.Replace("\\", "/");
        SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
        Sprite[] spriteArr = new Sprite[spriteAtlas.spriteCount];

        spriteAtlas?.GetSprites(spriteArr);

        for (int i = 0; i < images?.Length; i++)
            images[i].sprite = spriteArr[i];
    }

    const string fileName = "drawing_resource";
    readonly string zipURL = $"http://anipang2-cdn.stzgame.com/pds/1617590244/{fileName}.zip";

    ZipDownloader downloader = new ZipDownloader();
    SimpleTextureLoader textureLoader = new SimpleTextureLoader();

    public void Start()
    {
        downloader.DownloadAndExtractZip(() =>
        {
            string[] pngPaths = Directory.GetFiles($"{Application.persistentDataPath}/{fileName}", "*png", SearchOption.AllDirectories);

            var names = pngPaths.Where(x => x.Contains("resource_total")).ToList();

            for(int i =0; i<images?.Length; i++)
                images[i].sprite = textureLoader.GetSprite(names[i]);

        }, zipURL);
    }
}
