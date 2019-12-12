using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using SimpleJSON;

public class TwitchChat : MonoBehaviour
{

    public static TwitchChat Instance;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    string username, password, channelName;

    public List<string> currentStreamer;
    public List<string> currentMods;
    public List<string> currentVips;
    public List<string> currentViewers;

    public JSONNode streamerArray;
    public JSONNode modsArray;
    public JSONNode vipsArray;
    public JSONNode viewersArray;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        channelName = PlayerPrefs.GetString("Twitch_MsgChannel");
        password = PlayerPrefs.GetString("Twitch_OAuthToken");
        username = PlayerPrefs.GetString("Twitch_Username");

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (!twitchClient.Connected)
        {
            print("Falha ao conectar, tentando novamente");
            Connect();
        }

        ReadChat();
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
        {
            var message = reader.ReadLine();

            if (message.Contains("PING"))
            {
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
            }

            if (message.Contains("PRIVMSG"))
            {
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                //print(string.Format("{0}: {1}", chatName, message));

                string str = message.Substring(0, 1);


                if (chatName != "streamlabs" && chatName != "moobot" && chatName != "nightbot")
                {
                    //int viewerIndex = listaViewersString.IndexOf(chatName);

                    //Lhama viewerLhama = listaViewersObject[viewerIndex].GetComponent<Lhama>();
                    //viewerLhama.StartCoroutine(viewerLhama.ShowMessage(message, 2f));
                }


                // ==================== COMANDOS ====================
                if (str == "!")
                {
                    string[] splitted = message.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); // split espaço
                    print(splitted[0]);

                    switch (splitted[0]) // switch name
                    {
                        case "!play":
                            if (BattleManager.Instance.currentBattleState == BattleManager.BattleState.Waiting)
                            {
                                if (Manager.Instance.currentViewersNames.Contains(chatName))
                                {
                                    print("Já está em batalha");
                                    SendChat("@" + chatName + " você já está na batalha !");
                                    return;
                                }

                                Manager.Instance.AddViewer(chatName);
                                List<string> listaViewersString = Manager.Instance.currentViewersNames;
                                List<GameObject> listaViewersObject = Manager.Instance.currentViewersObjects;
                                SendChat("@" + chatName + " entrou na batalha, boa sorte !");
                                BattleManager.Instance.CheckAmount();
                            }

                            else if (BattleManager.Instance.currentBattleState == BattleManager.BattleState.Started)
                            {
                                print(chatName + " tentou entrar na batalha já iniciada");
                                SendChat("@" + chatName + " batalha já começou, seja mais rápido na próxima !");
                            }

                            else if (BattleManager.Instance.currentBattleState == BattleManager.BattleState.Finished)
                            {
                                print(chatName + " tentou entrar na batalha mas ela não existe");
                                SendChat("@" + chatName + " aguarde até o streamer começar uma nova batalha !");
                            }
                            break;

                        default:
                            break;
                    }
                }
                // ================== FIM COMANDOS ==================

            }
        }
    }

    public void SendChat(string mensagem)
    {
        writer.WriteLine(":"+ username + "!"+ username + "@"+ username + ".tmi.twitch.tv PRIVMSG #"+ username + " :" + mensagem);
        writer.Flush();
    }

    public IEnumerator HTTPRequest()
    {
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get("http://tmi.twitch.tv/group/user/" + channelName + "/chatters");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var N = JSON.Parse(www.downloadHandler.text);
            var chatters = N["chatters"];

            streamerArray = chatters["broadcaster"];
            modsArray = chatters["moderators"];
            vipsArray = chatters["vips"];
            viewersArray = chatters["viewers"];

            currentStreamer = new List<string>();
            currentMods = new List<string>();
            currentVips = new List<string>();
            currentViewers = new List<string>();

            for (int i = 0; i < streamerArray.Count; i++)
            {
                if (!currentStreamer.Contains(streamerArray[i]))
                {
                    currentStreamer.Add(streamerArray[i]);
                }
            }

            for (int i = 0; i < modsArray.Count; i++)
            {
                if (!currentMods.Contains(modsArray[i]))
                {
                    currentMods.Add(modsArray[i]);
                }
            }

            for (int i = 0; i < vipsArray.Count; i++)
            {
                if (!currentVips.Contains(vipsArray[i]))
                {
                    currentVips.Add(vipsArray[i]);
                }
            }

            for (int i = 0; i < viewersArray.Count; i++)
            {
                if (!currentViewers.Contains(viewersArray[i]))
                {
                    currentViewers.Add(viewersArray[i]);
                }
            }

            Manager.Instance.CheckIfIsViewing();
        }
        
    }

    public void PrintAll()
    {
        print(viewersArray);
    }
}
