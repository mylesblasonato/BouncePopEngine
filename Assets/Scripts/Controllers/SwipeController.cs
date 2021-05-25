using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;
using MoreMountains.Feedbacks;

public class SwipeController : MonoBehaviour
{
    [SerializeField] Transform _swipeVfx;
    [SerializeField] MMFeedbacks _feedbacks;
    [SerializeField] Match3Controller _match3Controller;
    [SerializeField] LayerMask _ballMask;
    [SerializeField] float _swipeLength = 10f;
    public int SwipeTouchCount = 1;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;
    [Range(0.0f, 10.0f)] public float SwipeThresholdSeconds;
    SwipeGestureRecognizer swipe;
    GameObject _ball;
    Vector2 _startPos, _endPos;
    List<GameObject> _balls;

    public List<GameObject> Balls => _balls;

    void Awake()
    {
        _balls = new List<GameObject>();
    }

    void Start()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0;
        swipe.MinimumSpeedUnits = 1f;
        swipe.MinimumDistanceUnits = 1f;
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.ThresholdSeconds = SwipeThresholdSeconds;
        FingersScript.Instance.AddGesture(swipe);
        LongPressGestureRecognizer tap = new LongPressGestureRecognizer();
        tap.MinimumDurationSeconds = 0f;
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
        tap.AllowSimultaneousExecutionWithAllGestures();
        swipe.AllowSimultaneousExecutionWithAllGestures();

        _cam = Camera.main;
    }

    void Update()
    {/*
        if (Input.GetMouseButton(0))
        {
            _swipeVfx.gameObject.SetActive(true);
            _swipeVfx.transform.position = new Vector3(_cam.ScreenToWorldPoint(Input.mousePosition).x,
                _cam.ScreenToWorldPoint(Input.mousePosition).y, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _swipeVfx.gameObject.SetActive(false);
        }

        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.EndMode = SwipeMode;

        if (raycast && !_match3Controller._isFlicking)
        {
            RaycastHit2D[] swipeHit = Physics2D.RaycastAll(
                _startPos, _endPos - _startPos, _swipeLength, _ballMask);
            if (swipeHit.Length > 1)
            {
                foreach (var hit in swipeHit)
                {
                    _balls.Add(hit.collider.gameObject);
                }

                _startPos = Vector2.zero;
                _endPos = Vector2.zero;
                _match3Controller.CheckMatch(_balls);
            }

            LevelManager.Instance._currentLevel.GetComponent<Level>()._turnsRemaining--;
            LevelManager.Instance._currentLevel.GetComponent<Level>().CheckTurnsLeft();
            raycast = false;
        }
        */
    }

    Camera _cam;

    void Tap_Updated(GestureRecognizer gesture)
    {/*
        if (gesture.State == GestureRecognizerState.Began)
        {
            _startPos = Vector2.zero;
            _endPos = Vector2.zero;
            _startPos = _cam.ScreenToWorldPoint(new Vector2(gesture.FocusX, gesture.FocusY));
        }*/
    }

    void Swipe_Updated(GestureRecognizer gesture)
    {
       /* SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            _endPos = _cam.ScreenToWorldPoint(new Vector2(gesture.FocusX, gesture.FocusY));
            raycast = true;
        }
        else if (swipe.State == GestureRecognizerState.Executing)
        {
            _feedbacks.PlayFeedbacks();
        }*/
    }

    bool raycast = false;

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(_startPos, _endPos);
    }
}