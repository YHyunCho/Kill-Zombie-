using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject[] fireWoodPrefabs;
    public Button quitButton;
    public Text levelText;
    public Text startCountText;
    public Text gameOverText;
    public Text playerWinText;

    public AudioClip levelupSound;
    public AudioClip timerSound;
    public AudioClip winSound;

    private AudioSource mainAudio;
    private AudioSource mainSound;
    private CameraHandler swichCamera;

    public bool isLevelUp;
    public bool isGameActive = false;
    public bool isFireWoodDestroyed;
    public bool isHitByBullet;
    public string currentCamera;

    public int timer = 0;
    public int level;
    public int killCount;
    public int zombieCount;
    public float spawnZombieRate;

    public Vector3 fireWoodPosition;
    public List<Vector3> randomPosition = new List<Vector3>() 
                                            {new Vector3(0, 0.38f, 11.358f),
                                             new Vector3(6.15f, 0.38f, -12.21f), new Vector3(6.15f, 0.38f, 12.21f),
                                             new Vector3(-6.15f, 0.38f, -12.21f), new Vector3(-6.15f, 0.38f, 12.21f)};

    private int startCnt;

    private void Start()
    {
        swichCamera = GameObject.Find("Cameras").GetComponent<CameraHandler>();
        mainSound = GameObject.Find("MainSound").GetComponent<AudioSource>();
        mainAudio = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGameActive = false;
        startCnt = 3;
        timer = 0;
        level = 1;
        spawnZombieRate = 3.0f;
        levelText.text = "Level " + level;

        startCountText.gameObject.SetActive(true);
        StartCoroutine(StartCount());
    }

    public void GameStart()
    {
        isGameActive = true;
        mainSound.Play();
        SpawnFireWood();
        StartCoroutine(StartTimer());
    }

    IEnumerator StartCount()
    {
        while (startCnt > -1)
        {
            yield return new WaitForSeconds(1);

            startCountText.text = "" + startCnt;

            startCnt -= 1;

            if (startCnt == -1)
            {
                startCountText.gameObject.SetActive(false);
                GameStart();
            } else
            {
                mainAudio.PlayOneShot(timerSound, 1.0f);
            }
        }
    }

    IEnumerator StartTimer()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);

            timer += 1;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.text = "Game Over\n\n" + GameManager.Instance.userName + ", You Reached Level " + level + "\nBetter Luck Next Time!";
        
        gameOverText.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        levelText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayerWin()
    {
        isGameActive = false;
        mainAudio.PlayOneShot(winSound, 1.0f);
        swichCamera.ActivateThirdPersonCamera();
        playerWinText.text = "Congratulation, " + GameManager.Instance.userName + "!\nYou Win!\n\nYou Completed Level 5 in " + timer + " Seconds"; 
        
        playerWinText.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        levelText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Instance.SaveScore(timer);
    }

    public void ClickQuitButton()
    {
        GameManager.Instance.LoadScore();
        SceneManager.LoadScene(0);
    }

    void SpawnFireWood()
    {
        killCount = 0;
        zombieCount = 0;
        isFireWoodDestroyed = false;

        fireWoodPosition = randomPosition[Random.Range(0, randomPosition.Count-1)];
        Instantiate(fireWoodPrefabs[level - 1], fireWoodPosition, fireWoodPrefabs[level - 1].transform.rotation);

        randomPosition.Remove(fireWoodPosition);
    }

    public void UpdateBodyCount()
    {
        killCount += 1;
        Debug.Log("You killed : " + killCount + ", Spawned Zombie : " + zombieCount);
    }

    public void UpdateLevel()
    {
        if (killCount == zombieCount && isFireWoodDestroyed)
        {
            level += 1;
            spawnZombieRate -= 0.5f;


            if (level == 6)
            {
                PlayerWin();
                Debug.Log("You Win!");

            }
            else if (level < 6)
            {
                levelText.text = "Level " + level;
                isLevelUp = true;
                mainAudio.PlayOneShot(levelupSound, 1.0f);
                swichCamera.ActivateThirdPersonCamera();
                SpawnFireWood();
            }
        }
    }
}
