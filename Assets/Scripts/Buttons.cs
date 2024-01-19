using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
//BUTTON EVENTS
    void Start()
    {
        //START
        Button startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);

        //QUIT
        Button quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);

    }

    public void StartGame()
    {
        // Load scene Intro
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        // Load scene Game
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        //Quit Game
        Application.Quit();
    }

    public void Menu()
    {
        //Load scene MainMenu
        SceneManager.LoadScene(0);
    }
}
