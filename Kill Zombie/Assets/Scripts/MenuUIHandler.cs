using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public InputField inputName;

    public void GameStartButton()
    {
        Debug.Log("push button");
        SceneManager.LoadScene(1);
    }

    public void InputUserName()
    {
        GameManager.Instance.userName = inputName.text;
    }
}
