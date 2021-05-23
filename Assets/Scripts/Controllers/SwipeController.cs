using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;

public class SwipeController : MonoBehaviour
{
    [SerializeField] LayerMask _ballMask;
    public int SwipeTouchCount = 1;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;
    [Range(0.0f, 10.0f)] public float SwipeThresholdSeconds;
    SwipeGestureRecognizer swipe;
    GameObject _ball;
    Vector2 _startPos, _endPos;
    List<GameObject> _balls;

    void Awake()
    {
        _balls = new List<GameObject>();
    }

    void Start()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0;
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.ThresholdSeconds = SwipeThresholdSeconds;
        FingersScript.Instance.AddGesture(swipe);
        LongPressGestureRecognizer tap = new LongPressGestureRecognizer();
        tap.MinimumDurationSeconds = 0f;
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
        tap.AllowSimultaneousExecutionWithAllGestures();
        swipe.AllowSimultaneousExecutionWithAllGestures();
    }

    void Update()
    {
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.EndMode = SwipeMode;

        if (Input.GetMouseButtonDown(0))
        {
            //_startPos = Input.mousePosition.normalized;
        }
    }
    
    void Tap_Updated(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            _startPos = new Vector2(gesture.DeltaX, gesture.DeltaY);
        }
    }
    
    void Swipe_Updated(GestureRecognizer gesture)
    {
        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            _endPos = new Vector2(gesture.DeltaX, gesture.DeltaY);
            RaycastHit[] swipeHit = Physics.RaycastAll(_startPos, _endPos - _startPos, 1000f, _ballMask);
            foreach (var hit in swipeHit)
            {
                _balls.Add(hit.collider.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(_startPos, _endPos - _startPos);
    }
}





















