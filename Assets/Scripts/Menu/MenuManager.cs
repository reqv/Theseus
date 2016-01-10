using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	    
	}

    public void NewGame()
    {
        Application.LoadLevel("Main");
    }

    public void Authors()
    {
        Application.LoadLevel("Authors");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
