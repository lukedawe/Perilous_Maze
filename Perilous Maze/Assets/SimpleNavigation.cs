using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
