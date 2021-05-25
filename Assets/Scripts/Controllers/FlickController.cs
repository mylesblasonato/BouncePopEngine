using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;

public class FlickController : MonoBehaviour
{
    [SerializeField] Transform _swipeVfx;
    [SerializeField] Match3Controller _match3Controller;
    [SerializeField] SwipeController _swipeController;
    [SerializeField] float _flickLength = 0.5f;
    public int SwipeTouchCount = 1;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;
    [Range(0.0f, 10.0f)] public float SwipeThresholdSeconds;
    public GameObject Image;
    public float flickForce = 20f;
    public bool _updateTurns;
    SwipeGestureRecognizer swipe;
    GameObject _ball;
    List<GameObject> _balls;

    void Start()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0f;
        swipe.MinimumDistanceUnits = _flickLength;
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.PlatformSpecificView = Image;
        swipe.ThresholdSeconds = SwipeThresholdSeconds;
        FingersScript.Instance.AddGesture(swipe);
        LongPressGestureRecognizer tap = new LongPressGestureRecognizer();
        tap.MinimumDurationSeconds = 0f;
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
        tap.AllowSimultaneousExecutionWithAllGestures();
        swipe.AllowSimultaneousExecutionWithAllGestures();

        _balls = new List<GameObject>();
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
            {
                _swipeController.enabled = true;
            }

            if (_ball.GetComponent<BallController>() == null) return;
            _match3Controller._isFlicking = true;
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {
            _match3Controller._isFlicking = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _balls.Add(other.gameObject);
            _balls.Add(gameObject);
            _match3Controller.CheckMatch(_balls);
        }
    }

    void Swipe_Updated(GestureRecognizer gesture)
    {
        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            if (_ball.GetComponent<BallController>() == null) return;
            var rb = _ball.GetComponent<Rigidbody2D>();
            if (rb != GetComponent<Rigidbody2D>())
            {
                rb.AddForce(new Vector2(swipe.DeltaX, swipe.DeltaY) * swipe.Speed * flickForce * Time.deltaTime);
                rb = null;
                _ball.GetComponent<FlickController>()._updateTurns = true;
            }

            _ball = null;
            _swipeVfx.gameObject.SetActive(false);
        }
    }
}