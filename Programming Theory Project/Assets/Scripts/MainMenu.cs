using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject eventSystem;

    private void Start()
    {
        SceneManager.sceneLoaded += UnloadStart;
    }
    public void StartGame()
    {
        loadingPanel.SetActive(true);
        Destroy(eventSystem);
        SceneManager.LoadSceneAsync("MainGame", LoadSceneMode.Additive);

    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void UnloadStart(Scene s, LoadSceneMode mode)
    {
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
