using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject canvasObject;
    public TextMeshProUGUI rank;
    public GameObject winText;
    public GameObject loseText;
    public bool hasStarted = false;
    public bool hasFinished = false;
    
    [SerializeField] private TextMeshProUGUI countdown;
    private WaitForSeconds oneSecond = new WaitForSeconds(1);

    private Transform finishCheckpoint;
    private PlayerController[] players = null;
    private CarAI[] bots = null;

    private void Awake()
    {
        Instance = this;
        players = FindObjectsOfType<PlayerController>();
        bots = FindObjectsOfType<CarAI>();
    }

    private void Start()
    {
        finishCheckpoint = CheckpointList.CheckPointList[CheckpointList.CheckPointList.Length - 1];
        finishCheckpoint.gameObject.AddComponent<FinishLine>();
        StartCoroutine(StartCount());
    }

    public IEnumerator StartCount()
    {
        countdown.gameObject.SetActive(true);
        countdown.text = "3";
        yield return oneSecond;
        countdown.text = "2";
        yield return oneSecond;
        countdown.text = "1";
        yield return oneSecond;
        countdown.text = "Start";
        hasStarted = true;
        yield return oneSecond;
        countdown.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Reset car if press R
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reset each car in scene
            foreach(PlayerController player in players)
            {
                StartCoroutine(player.ResetCar());
            }
            foreach(CarAI bot in bots)
            {
                bot.ResetAI();
            }

            // Reset GameManager
            winText.SetActive(false);
            loseText.SetActive(false);
            canvasObject.SetActive(false);
            hasFinished = false;
            hasStarted = false;
            StartCoroutine(StartCount());
            return;
        }
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public class FinishLine : MonoBehaviour
{

    private List<string> finishedCars = new List<string>();
    private int place = 0;

    private void OnTriggerEnter(Collider other)
    {
        place++;
        string carName = other.transform.parent.name;

        finishedCars.Add(place + "  " + carName);
        GameManager.Instance.rank.text = String.Join("\r\n", finishedCars);
        if (carName == "Player") ShowFinished();
    }

    private void ShowFinished()
    {
        GameManager.Instance.canvasObject.SetActive(true);

        if (place == 1) GameManager.Instance.winText.SetActive(true);
        else GameManager.Instance.loseText.SetActive(true);

        GameManager.Instance.hasFinished = true;
    }
}
