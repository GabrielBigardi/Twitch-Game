using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    public enum BattleState { Waiting, Started, Finished }
    public BattleState currentBattleState;

    public int maxPlayers;

    public Button novaButton;
    public Button startButton;
    public Button endButton;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(Instance);
    }

    void Start()
    {
        currentBattleState = BattleState.Finished;

        novaButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
    }

    public void CheckAmount()
    {
        int playerAmount = Manager.Instance.currentViewersObjects.Count;
        if (playerAmount >= maxPlayers)
        {
            StartBattle();
        }
    }

    public void NewBattle()
    {
        Manager.Instance.DeleteViewersAll();
        //print("Uma nova batalha foi iniciada, aguardando jogadores");
        currentBattleState = BattleState.Waiting;
        TwitchChat.Instance.SendChat("Atenção todo mundo, uma nova batalha foi iniciada. Para entrar digite !play");

        novaButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
    }

    public void StartBattle()
    {
        //print("Batalha começando, iniciada manualmente ou atingiu o número de jogadores");
        currentBattleState = BattleState.Started;
        TwitchChat.Instance.SendChat("A batalha foi iniciada, boa sorte a todos !");

        novaButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
    }

    public void EndBattle()
    {
        if (currentBattleState == BattleState.Finished)
            return;

        currentBattleState = BattleState.Finished;

        string strVencedores = "";

        if(Manager.Instance.currentViewersNames.Count > 1)
        {
            // ========= Função: pegar o nome do vencedor ===========
            for (int i = 0; i < Manager.Instance.currentViewersNames.Count; i++)
            {
                string nameVencedor = Manager.Instance.currentViewersNames[i];

                if (i < (Manager.Instance.currentViewersNames.Count - 1))
                    strVencedores += ("@" + nameVencedor + ", ");
                else
                    strVencedores += ("@" + nameVencedor + ". Parabéns !");
            }
            // =======================================================

            TwitchChat.Instance.SendChat("A batalha foi finalizada, vencedores: " + strVencedores);
        }
        else if (Manager.Instance.currentViewersNames.Count == 1)
        {
            TwitchChat.Instance.SendChat("A batalha foi finalizada, vencedor: " + Manager.Instance.currentViewersNames[0]);
        }
        else
        {
            TwitchChat.Instance.SendChat("A batalha foi finalizada, não houve vencedores !");
        }
        

        novaButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
    }


}
