using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Diagnostics;

public class UpdateManager : MonoBehaviour
{

    public Text verificaText;
    public Text aguardeText;
    public Text infoText;

    public string baseURL;

    string totalSize = "0";
    string totalDownloaded = "0";
    string downloadSpeed = "0";
    long downloaded1, downloaded2;

    void Awake()
    {
        print("Versão atual do jogo: " + Application.version);
        StartCoroutine("VersionCheckRequest");

        infoText.enabled = false;
    }

    IEnumerator VersionCheckRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "version.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
            verificaText.text = "Erro ao obter atualizações";
            aguardeText.text = "Verifique sua conexão e tente novamente";
        }
        else
        {
            string version = www.downloadHandler.text;
            print("Última versão: " + version);
            if (Application.version == version)
            {
                print("Última versão já instalada.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                print("Jogo desatualizado, atualizando.");
                verificaText.text = "Baixando atualização";
                StartCoroutine("DownloadUpdate");
            }
        }

    }

    IEnumerator DownloadUpdate()
    {
        // ========== DETERMINA O TAMANHO TOTAL ==================
        UnityWebRequest webRequest_SIZE = UnityWebRequest.Head(baseURL + "gameupdate.zip");
        yield return webRequest_SIZE.SendWebRequest();
        string size = webRequest_SIZE.GetResponseHeader("Content-Length");

        if (webRequest_SIZE.isNetworkError || webRequest_SIZE.isHttpError)
            print(webRequest_SIZE.error);
        else
        {
            totalSize = BytesToString(Convert.ToInt64(size));
            print("Baixando: " + totalSize);
        }
        // =======================================================



        // =============== FAZ O RESTO ===========================
        string path = Directory.GetCurrentDirectory();

        var request = new UnityWebRequest(baseURL + "gameupdate.zip");
        request.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(path, "gameupdate.zip");
        var downloadHandler = new DownloadHandlerFile(resultFile);
        downloadHandler.removeFileOnAbort = true;
        request.downloadHandler = downloadHandler;

        StartCoroutine(FetchDownloaded(request));
        infoText.enabled = true;

        yield return request.SendWebRequest();

        infoText.enabled = false;

        if (request.isNetworkError || request.isHttpError)
            print(request.error);
        else
        {
            verificaText.text = "Instalando atualização";
            string path3 = Directory.GetCurrentDirectory();
            string pathToUpdater3 = Path.Combine(path3, "update.bat");
            if (File.Exists(pathToUpdater3))
            {
                Process.Start(pathToUpdater3);
                Application.Quit();
            }
            else
            {

            }
        }
    }

    IEnumerator FetchDownloaded(UnityWebRequest request)
    {
        while (true)
        {
            string downloadedBytes = request.downloadedBytes.ToString();
            downloaded1 = Convert.ToInt64(downloadedBytes);
            yield return new WaitForSeconds(1f);
            string downloadedBytes2 = request.downloadedBytes.ToString();
            downloaded2 = Convert.ToInt64(downloadedBytes2);
            long sub = (downloaded2 - downloaded1);
            totalDownloaded = BytesToString(downloaded2);
            downloadSpeed = BytesToString(sub);
            string totalStr = "Tamanho Total: " + totalSize + " | Baixado: " + totalDownloaded + " | Velocidade de Download: " + downloadSpeed + "/s";
            infoText.text = totalStr;
        }
    }

    static String BytesToString(long byteCount)
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
}
