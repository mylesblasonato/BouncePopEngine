using System;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Controller : MonoBehaviour
{   
    [SerializeField] SwipeController _swipeController;
    [SerializeField] AudioClip _matchSfx;
    Level _level;
    GameObject _currentMatchToCheck;
    List<GameObject> _matches;
    MMFeedback _feedback;
    int _matchCount = 0;

    public bool _isFlicking = false;

    void Awake()
    {
        _matches = new List<GameObject>();
        _level = GetComponent<Level>();
    }
    public void CheckMatch(List<GameObject> elements)
    {
        if (elements.Count > 0)
        {
            _currentMatchToCheck = elements[0];
            foreach (var ball in elements)
            {
                if (_currentMatchToCheck.GetComponent<BallController>().TypeOfBall ==
                    ball.GetComponent<BallController>().TypeOfBall)
                {
                    _matches.Add(ball);
                    _matchCount++;
                }                  
            }
        }

        if (_matchCount > 1)
        {
            foreach (var match in _matches)
            {
                match.GetComponent<MMFeedbacks>().PlayFeedbacks();
                match.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                MundoSound.Play(_matchSfx, 1f);
            }
            ClearLists(elements);
            _level.CheckMatchesLeft();
        }
        else
        {
            ClearLists(elements);
            return;
        }
    }

    void ClearLists(List<GameObject> elements)
    {
        _matches.RemoveAll(v => v.GetComponent<BallController>() != null);
        elements.RemoveAll(e => e.GetComponent<BallController>() != null);
        _level = LevelManager.Instance._currentLevel.GetComponent<Level>();
        _matchCount = 0;
        _currentMatchToCheck = null;       
    }

    void FixedUpdate()
    {
        var remaining = 0;
        foreach (Transform ball in LevelManager.Instance._currentLevel.GetComponent<Level>()._balls)
        {
            if (ball.gameObject.activeSelf)
            {
                remaining++;
            }
        }

        if (_level != null)
            _level._ballsLeft = remaining;

        LevelManager.Instance._currentLevel.GetComponent<Level>().CheckMatchesLeft();
    }
}