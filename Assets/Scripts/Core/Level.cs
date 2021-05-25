using System;
using DigitalRubyShared;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] FingersScript _fingers;
    [SerializeField] GameObject _loseScreen;
    [SerializeField] string _nextLevel;
    [SerializeField] TextMeshProUGUI _turns;
    [SerializeField] MMFeedbacks _feedbacks;
    public Transform _balls;
    public int _ballsLeft = 0;
    public int _turnsRemaining = 0;

    void Awake()
    {
        _turnsRemaining = int.Parse(_turns.text);
        _ballsLeft = _balls.childCount;
        _fingers.enabled = true;
    }

    void Start()
    {
        _feedbacks.PlayFeedbacks();
    }

    public void CheckMatchesLeft()
    {
        if (_ballsLeft == 0 && _turnsRemaining >= 0)
        {
            _fingers.enabled = false;

            if (int.Parse(_turns.text) == 1)
            {
                _turnsRemaining--;
                _turns.text = _turnsRemaining.ToString();
            }
            LevelManager.Instance.LoadLevel(_nextLevel);
        }
    }

    public void CheckTurnsLeft()
    {
        if (_turnsRemaining <= 0 && _ballsLeft > 0)
        {
            _fingers.enabled = false;
            _turnsRemaining = 0;
            _turns.text = _turnsRemaining.ToString();
            LevelManager.Instance.OpenUI(_loseScreen);
            Invoke("LevelProcessor", 3f);
        }
        else
        {
            _turns.text = _turnsRemaining.ToString();
        }
    }
    
    void LevelProcessor()
    {
        LevelManager.Instance.CloseUI(_loseScreen);
        LevelManager.Instance.ReloadLevel();
    }
}