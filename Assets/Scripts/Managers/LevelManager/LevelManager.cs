using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SOFloat _currentLevelIndex;
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
    }

    public void LoadLevel(string levelName)
    {
        foreach (var level in _levels)
        {
            if (String.CompareOrdinal(level.name, levelName) == 0)
            {
                _currentLevel.SetActive(false);
                _currentLevel = level;
                _currentLevel.SetActive(true);
                _currentLevelIndex.Value += 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            }
        }
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