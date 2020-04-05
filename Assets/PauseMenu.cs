using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject Parent;
    public Button Quit, MainMenu;

    private bool ac = false;
    // Start is called before the first frame update
    void Start()
    {
        Quit.onClick.AddListener(QuitGame);
        MainMenu.onClick.AddListener(GoToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ac = !ac;
            Parent.SetActive(ac);
        }
    }
    private void QuitGame()
    {
        Application.Quit();
    }
    private void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
