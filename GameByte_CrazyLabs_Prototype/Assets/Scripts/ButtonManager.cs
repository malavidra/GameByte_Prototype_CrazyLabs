using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject _panel1, _panel2;
    public GameObject _startGame, _glass;
    public Animator _glassAnim, _ice;
    public SoundManager sndMng;

    private void Start()
    {
        sndMng.PlaySound("start");
    }

    public void changePage()
    {
        if (_panel1.activeSelf)
        {
            _panel1.SetActive(false);
            _panel2.SetActive(true);
        }
        else
        {
            _panel1.SetActive(true);
            _panel2.SetActive(false);
        }
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
        StartGame();
    }

    public void StartGame1()
    {
        sndMng.PlaySound("presek");
        _glassAnim.SetTrigger("start");
        _ice.SetTrigger("start");
    }
    public void StartGame()
    {
        _startGame.SetActive(true);
        Destroy(_glass);
        Destroy(GameObject.Find("Cube (1)"));
        Destroy(GameObject.Find("StartButton"));
        Destroy(GameObject.Find("StartingTiles"));
    }
}
