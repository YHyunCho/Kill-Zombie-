using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public InputField inputName;
    public Text topPlayer;

    void Start()
    {
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
