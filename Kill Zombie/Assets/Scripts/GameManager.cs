using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Dictionary<string, int> userScore = new Dictionary<string, int>();

    public string userName;
    public string topUserName;
    public int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScore();
    }

    private void Start()
    {
        foreach (KeyValuePair<string, int> entry in userScore)
        {
            Debug.Log("Key : " + entry.Key + ", Valye : " + entry.Value);
        }
    }

    [System.Serializable]
    class SaveData
    {
        //public Dictionary<string, int> userScore = new Dictionary<string, int>();
        public string topUserName;
        public int time;
    }

    public void SaveScore(int time)
    {
        SaveData data = new SaveData();

        if (data.time < time)
        {
            data.time = time;
            data.topUserName = userName;
        }

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            topUserName = data.topUserName;
            score = data.time;
        }
    }
}