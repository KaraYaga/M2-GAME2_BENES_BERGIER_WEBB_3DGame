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
        // Load Intro before game
        SceneManager.LoadScene("Intro");
    }

    public void IntroSkip()
    {
        // Finish Intro, Load Game
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        //Quit Game
        Application.Quit();
    }

    public void Menu()
    {
        //Load scene MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}
