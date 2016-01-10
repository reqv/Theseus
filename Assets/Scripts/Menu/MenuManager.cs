using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _authors;

    public void NewGame()
    {
        Application.LoadLevel("Main");
    }

    public void Authors()
    {
        _menuCanvas.SetActive(false);
        _authors.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
