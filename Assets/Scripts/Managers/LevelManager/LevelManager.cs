using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SOFloat _currentLevelIndex;
    public GameObject _menuScreen;
    public GameObject _winScreen;
    public List<GameObject> _levels;
    static LevelManager _instance;
    public static LevelManager Instance => _instance;

    public GameObject _currentLevel;

    void Awake()
    {
        _instance = this;
        _levels = new List<GameObject>();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            _levels.Add(child.gameObject);
        }

        _currentLevel = _levels[(int)_currentLevelIndex.Value];
        _currentLevel.SetActive(true);

        if (_currentLevelIndex.Value >= 0)
        {
            _menuScreen.SetActive(false);
        }
        _winScreen.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        foreach (var level in _levels)
        {
            if (String.CompareOrdinal(level.name, levelName) == 0)
            {
                _winScreen.SetActive(true);
                _level = level;
                Invoke("LevelProcessor", 3f);
                break;
            }
        }
    }

    GameObject _level;
    void LevelProcessor()
    {
        if ((int)_currentLevelIndex.Value < _levels.Count - 1)
        {
            _currentLevel.SetActive(false);
            _currentLevel = _level;
            _currentLevel.SetActive(true);
            _currentLevelIndex.Value += 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            _menuScreen.SetActive(true);
            _winScreen.SetActive(false);
        }
    }

    public void RestartGame()
    {
        _currentLevelIndex.Value = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OpenUI(GameObject go)
    {
        go.SetActive(true);
    }
    
    public void CloseUI(GameObject go)
    {
        go.SetActive(false);
    }
}