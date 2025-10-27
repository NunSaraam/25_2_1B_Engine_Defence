using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecneLoadManager : MonoBehaviour
{
    public string targetScene = "Main";
    public void LoadScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}
