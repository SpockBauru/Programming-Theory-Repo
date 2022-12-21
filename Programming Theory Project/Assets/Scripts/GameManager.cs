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

    public bool HasStarted { get; private set; }
    public bool HasFinished { get; private set; }

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI countdown;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private TextMeshProUGUI rank;

    private WaitForSeconds oneSecond = new WaitForSeconds(1);

    private Transform finishCheckpoint;
    private PlayerController[] players = null;
    private CarAI[] bots = null;
    private List<string> finishedCars = new List<string>();

    private int place = 0;
    private PlayerController playerFinished;

    private void Awake()
    {
        Instance = this;
        players = FindObjectsOfType<PlayerController>();
        bots = FindObjectsOfType<CarAI>();
    }

    private void Start()
    {
        HasStarted = false;
        HasFinished = false;

        finishCheckpoint = CheckpointList.CheckPointList.Last();
        finishCheckpoint.gameObject.AddComponent<FinishLine>();
        StartCoroutine(StartCount());
    }

    private IEnumerator StartCount()
    {
        countdown.gameObject.SetActive(true);
        countdown.text = "3";
        yield return oneSecond;
        countdown.text = "2";
        yield return oneSecond;
        countdown.text = "1";
        yield return oneSecond;
        countdown.text = "Start";
        HasStarted = true;
        yield return oneSecond;
        countdown.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Reset Game if press R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        // Reset each car in scene
        foreach (PlayerController player in players)
        {
            StartCoroutine(player.ResetCar());
        }
        foreach (CarAI bot in bots)
        {
            bot.ResetAI();
        }

        // Reset GameManager
        winText.SetActive(false);
        loseText.SetActive(false);
        endPanel.SetActive(false);
        HasFinished = false;
        HasStarted = false;
        finishedCars.Clear();
        place = 0;
        StartCoroutine(StartCount());
    }

    internal void ShowFinish(Transform car)
    {
        place++;
        string carName = car.parent.name;
        finishedCars.Add(place + "  " + carName);
        rank.text = String.Join("\r\n", finishedCars);

        playerFinished = car.GetComponentInChildren<PlayerController>();
        if (playerFinished != null)
        {
            endPanel.SetActive(true);
            HasFinished = true;

            if (place == 1) winText.SetActive(true);
            else loseText.SetActive(true);
        }
    }

    public void HideFinish()
    {
        endPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    public void ShowHideMenu()
    {
        if (menuPanel.activeSelf) menuPanel.SetActive(false);
        else menuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.ShowFinish(other.transform);
    }
}
