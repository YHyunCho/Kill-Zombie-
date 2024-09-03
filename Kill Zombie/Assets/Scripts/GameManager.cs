using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] fireWoodPrefabs;
    public Text levelText;
    public Text startCountText;
    public bool isLevelUp;

    private CameraHandler swichCamera;

    public bool isGameActive = false;
    public bool isFireWoodDestroyed;
    public bool isHitByBullet;
    public string currentCamera;

    public int level;
    public int killCount;
    public int zombieCount;
    public float spawnZombieRate;

    public Vector3 fireWoodPosition;
    public Vector3[] randomPosition = new[] {new Vector3(0, 0.38f, -11.358f), new Vector3(0, 0.38f, 11.358f),
                                             new Vector3(6.15f, 0.38f, -12.21f), new Vector3(6.15f, 0.38f, 12.21f),
                                             new Vector3(-6.15f, 0.38f, -12.21f), new Vector3(-6.15f, 0.38f, 12.21f)};

    private int startCnt;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        swichCamera = GameObject.Find("Cameras").GetComponent<CameraHandler>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGameActive = false;
        startCnt = 3;
        level = 1;
        spawnZombieRate = 2.5f;
        levelText.text = "Level " + level;

        startCountText.gameObject.SetActive(true);
        StartCoroutine(StartCount());
    }

    public void GameStart()
    {
        isGameActive = true;
        SpawnFireWood();
    }

    IEnumerator StartCount()
    {
        while (startCnt > -1)
        {
            yield return new WaitForSeconds(1);

            startCountText.text = "" + startCnt;

            startCnt -= 1;

            if(startCnt == -1)
            {
                startCountText.gameObject.SetActive(false);
                GameStart();
            }
        }
    }

    public void GameOver()
    {
        isGameActive = false;
    }

    void SpawnFireWood()
    {
        killCount = 0;
        zombieCount = 0;
        isFireWoodDestroyed = false;
        fireWoodPosition = randomPosition[Random.Range(0, randomPosition.Length)];
        Instantiate(fireWoodPrefabs[GameManager.Instance.level-1], fireWoodPosition, fireWoodPrefabs[GameManager.Instance.level-1].transform.rotation);
    }

    public void UpdateBodyCount()
    {
        killCount += 1;
        Debug.Log("You killed : " + killCount +", Spawned Zombie : " + zombieCount);
    }

    public void UpdateLevel()
    {
        if (killCount == zombieCount && isFireWoodDestroyed)
        {
            level += 1;
            spawnZombieRate -= 0.5f;


            if (level == 6)
            {
                GameOver();
                Debug.Log("You Win!");

            } else if (level < 6)
            {
                levelText.text = "Level " + level;
                isLevelUp = true;
                swichCamera.ActivateThirdPersonCamera();
                SpawnFireWood();
            }
        }
    }
}
