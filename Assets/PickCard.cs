using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickCard : MonoBehaviour
{
    public Button NextLevelButton;

    private void Start()
    {
        NextLevelButton.onClick.AddListener(NextLevel);
    }
    private void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
