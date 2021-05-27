using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;

public class FlickController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] Transform _swipeVfx;   
    [SerializeField] float _flickLength = 0.5f;
    
    public int SwipeTouchCount = 1;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;
    [Range(0.0f, 10.0f)] public float SwipeThresholdSeconds;
    public bool _updateTurns;
    #endregion

    float flickForce; // change if need to tune
    SwipeGestureRecognizer swipe;
    GameObject _ball;
    List<GameObject> _balls;
    Rigidbody2D _rb;

    void Tap_Updated(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            _ball = FingersUtilityExtensions.GetTouchedObject(gesture);
            if (_ball.CompareTag("Ball"))
            {
                _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _swipeController.enabled = false;
            }
            else if (_ball.CompareTag("BG"))
                _swipeController.enabled = true;
            if (_ball.GetComponent<BallController>() == null) return;
            _match3Controller._isFlicking = true;
        }
        if (gesture.State == GestureRecognizerState.Ended)
            _match3Controller._isFlicking = false;
    }

    void Swipe_Updated(GestureRecognizer gesture)
    {
        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            if (_ball == null || _ball.GetComponent<BallController>() == null) return;
            var rb = _ball.GetComponent<Rigidbody2D>();
            if (rb != _rb)
            {
                rb.AddForce(new Vector2(swipe.DeltaX, swipe.DeltaY) * swipe.Speed * flickForce * Time.deltaTime);
                rb = null;
                _ball.GetComponent<FlickController>()._updateTurns = true;
            }
            _ball = null;
            _swipeVfx.gameObject.SetActive(false);
        }
    }

    #region UNITY
    GameObject _fingerManager;
    Match3Controller _match3Controller;
    SwipeController _swipeController;
    void Start()
    {
        CreateSwipeGesture();
        CreateLongPressGesture();

        _fingerManager = GameObject.FindGameObjectWithTag("FingerManager");
        _match3Controller = _fingerManager.GetComponent<Match3Controller>();
        _swipeController = _fingerManager.GetComponent<SwipeController>();

        _balls = new List<GameObject>();
        _rb = GetComponent<Rigidbody2D>();

        #if UNITY_EDITOR
                flickForce = 0.3f;
        #else
                flickForce = 0.03f;
        #endif
    }

    void Update()
    {
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.EndMode = SwipeMode;
        if (_updateTurns)
        {
            LevelManager.Instance._currentLevel.GetComponent<Level>()._turnsRemaining--;
            LevelManager.Instance._currentLevel.GetComponent<Level>().CheckTurnsLeft();
            _updateTurns = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            _balls.Add(other.gameObject);
            _balls.Add(gameObject);
            _match3Controller.CheckMatch(_balls);
        }
    }
    #endregion
    #region HELPERS
    void CreateLongPressGesture()
    {
        LongPressGestureRecognizer tap = new LongPressGestureRecognizer();
        tap.MinimumDurationSeconds = 0f;
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
        tap.AllowSimultaneousExecutionWithAllGestures();
        swipe.AllowSimultaneousExecutionWithAllGestures();
    }
    void CreateSwipeGesture()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0f;
        swipe.MinimumDistanceUnits = _flickLength;
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.ThresholdSeconds = SwipeThresholdSeconds;
        FingersScript.Instance.AddGesture(swipe);
    }
    #endregion
}