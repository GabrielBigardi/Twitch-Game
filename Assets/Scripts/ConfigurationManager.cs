using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    public InputField channelInput;
    public InputField tokenInput;
    public InputField userInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("Twitch_MsgChannel"))
        {
            channelInput.text = PlayerPrefs.GetString("Twitch_MsgChannel");
        }

        if (PlayerPrefs.HasKey("Twitch_OAuthToken"))
        {
            tokenInput.text = PlayerPrefs.GetString("Twitch_OAuthToken");
        }

        if (PlayerPrefs.HasKey("Twitch_Username"))
        {
            userInput.text = PlayerPrefs.GetString("Twitch_Username");
        }
    }

    public void GetToken()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }

    public void SaveChanges()
    {
        if (channelInput.text != "")
            PlayerPrefs.SetString("Twitch_MsgChannel", channelInput.text.ToLower());

        if (tokenInput.text != "")
            PlayerPrefs.SetString("Twitch_OAuthToken", tokenInput.text.ToLower());

        if (userInput.text != "")
            PlayerPrefs.SetString("Twitch_Username", userInput.text.ToLower());

        SceneManager.LoadScene(2);
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteKey("Twitch_MsgChannel");
        PlayerPrefs.DeleteKey("Twitch_OAuthToken");
        PlayerPrefs.DeleteKey("Twitch_Username");

        channelInput.text = "";
        tokenInput.text = "";
        userInput.text = "";

    }

}
