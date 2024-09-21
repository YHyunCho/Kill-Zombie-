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

    private AudioSource mainSound;

    void Start()
    {
        mainSound = GetComponent<AudioSource>();

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
            Debug.Log("User Name : " + GameManager.Instance.userName);
            SceneManager.LoadScene(1);
            
        } else  
        {
            Debug.Log("Enter User Name");
        }
    }

    public void InputUserName()
    {
        GameManager.Instance.userName = inputName.text;
    }
}
