using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] string _nextLevel;
    public Transform _balls;
    public int _ballsLeft = 0;

    private void Awake()
    {
        _ballsLeft = _balls.childCount;
    }

    public void CheckMatchesLeft()
    {
        if (_ballsLeft <= 0)
        {
            LevelManager.Instance.LoadLevel(_nextLevel);
        }
    }
}