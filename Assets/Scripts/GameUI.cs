using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _gameWonUI;

    private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        GuardNavigation.OnPlayerSpotted += ShowGameOverUI;
        ExitPath.OnEnter += ShowGameWonUI;
    }

    private void OnDestroy()
    {
        GuardNavigation.OnPlayerSpotted -= ShowGameOverUI;
        ExitPath.OnEnter -= ShowGameWonUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void ShowGameOverUI(object sender, EventArgs e)
    {
        OnGameOver(_gameOverUI);
    }

    private void ShowGameWonUI(object sender, EventArgs e)
    {
        OnGameOver(_gameWonUI);
    }

    private void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.gameObject.SetActive(true);
        _isGameOver = true;
    }
}
