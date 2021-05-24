using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Controller : MonoBehaviour
{
    [SerializeField] SwipeController _swipeController;
    GameObject _currentMatchToCheck;
    List<GameObject> _matches;

    int _matchCount = 0;

    void Awake()
    {
        _matches = new List<GameObject>();
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
                else
                {
                    foreach (var match in _matches)
                    {
                        var m = match;
                        m.SetActive(false);
                        _matches.Remove(match);
                    }
                    Debug.Log("MATCH");
                    return;
                }    
            }
        }

        if (_matchCount > 1)
        {
            foreach (var match in _matches)
            {
                var m = match;
                m.SetActive(false);
                _matches.Remove(match);
            }
            Debug.Log("MATCH");
        }
    }
}
