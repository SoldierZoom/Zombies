using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool isPaused=false;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject PauseMenuUI;

    void Start() {
        PauseMenuUI.gameObject.SetActive(false);
    } 
    void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            if(isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        isPaused = false;
        PauseMenuUI.gameObject.SetActive(false);
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
    public void ExitToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void PauseGame() {
        isPaused=true;
        PauseMenuUI.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}
