using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Persistance.Instance.Round = 0;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(1);

        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(1);
    }
}
