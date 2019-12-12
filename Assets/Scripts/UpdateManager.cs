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

    public string baseURL;

    void Awake()
    {
        print("Versão atual do jogo: " + Application.version);
        StartCoroutine("VersionCheckRequest");
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
            if(Application.version == version)
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
        string path = Directory.GetCurrentDirectory();

        var request = new UnityWebRequest(baseURL + "gameupdate.zip");
        request.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(path, "gameupdate.zip");
        var downloadHandler = new DownloadHandlerFile(resultFile);
        downloadHandler.removeFileOnAbort = true;
        request.downloadHandler = downloadHandler;

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            print(request.error);
        else
        {
            string path3 = Directory.GetCurrentDirectory();
            string pathToUpdater3 = Path.Combine(path3, "update.bat");
            Process.Start(pathToUpdater3);
            Application.Quit();
        }
    }
}
