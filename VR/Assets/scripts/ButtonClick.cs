using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Test : MonoBehaviour {
    void LoadGame(string name)
    {
        SceneManager.LoadScene(name);
    }
}
