using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public InputField inputName;
    public Text topPlayer;

    public AudioClip clickSound;
    public AudioClip warnSound;

    public GameObject menuUI;
    public GameObject howtoplay;
    public GameObject warning;

    private AudioSource mainSound;

    void Start()
    {
        mainSound = GetComponent<AudioSource>();

        if (GameManager.Instance.userName != "")
        {
            inputName.text = GameManager.Instance.userName;
        }

        if (GameManager.Instance.score != 0)
        {
            topPlayer.text = GameManager.Instance.topUserName + ", " + GameManager.Instance.score + " Sec";
        }
        else
        {
            topPlayer.text = "No one has taken the first place yet\nHurry and make it yours!";
        }
    }

    public void GameStartButton()
    {
        mainSound.PlayOneShot(clickSound, 1.0f);

        if (GameManager.Instance.userName != "")
        {
            warning.gameObject.SetActive(false);
            SceneManager.LoadScene(1);
            
        } else  
        {
            warning.gameObject.SetActive(true);
            mainSound.PlayOneShot(warnSound, 1.0f);
        }
    }

    public void InputUserName()
    {
        GameManager.Instance.userName = inputName.text;
    }

    public void ClickHowToPlayButton()
    {
        warning.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(false);
        howtoplay.gameObject.SetActive(true);
        mainSound.PlayOneShot(clickSound, 1.0f);
    }

    public void ExitHowToPlay()
    {
        menuUI.gameObject.SetActive(true);
        howtoplay.gameObject.SetActive(false);
        mainSound.PlayOneShot(clickSound, 1.0f);
    }
}
