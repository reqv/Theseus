using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _authors;
    [SerializeField] private GameObject _historyCanvas;
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _play;
    [SerializeField] private GameObject _soundManager;
    private AudioSource[] _aSource;

    void Start()
    {
        _aSource = _soundManager.GetComponents<AudioSource>();
    }

    public void NewGame()
    {
        _menuCanvas.SetActive(false);
        _historyCanvas.SetActive(true);
        _back.SetActive(true);
        _play.SetActive(true);
        _aSource[1].Play();
    }

    public void Play()
    {
        Application.LoadLevel("Main");
    }

    public void Back()
    {
        _menuCanvas.SetActive(true);
        _historyCanvas.SetActive(false);
        _play.SetActive(false);
        _authors.SetActive(false);
        _back.SetActive(false);
        _aSource[1].Stop();
    }

    public void Authors()
    {
        _menuCanvas.SetActive(false);
        _authors.SetActive(true);
        _back.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
