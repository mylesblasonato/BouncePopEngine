using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;

public class FlickController : MonoBehaviour
{
    [SerializeField] Match3Controller _match3Controller;
    [SerializeField] SwipeController _swipeController;
    public int SwipeTouchCount = 1;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;
    [Range(0.0f, 10.0f)]
    public float SwipeThresholdSeconds;
    public GameObject Image;
    public float flickForce = 20f;
    
    SwipeGestureRecognizer swipe;
    GameObject _ball;
    
    void Start()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0;
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
    }

    void Update()
    {
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        swipe.EndMode = SwipeMode;
    }

    void Tap_Updated(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            _ball = FingersUtilityExtensions.GetTouchedObject(gesture);
            if(_ball.CompareTag("Ball"))
                _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (_ball.GetComponent<BallController>() == null) return;
            _match3Controller._isFlicking = true;
        }

        if(gesture.State == GestureRecognizerState.Ended)
        {
            _match3Controller._isFlicking = false;
        }
    }

    void Swipe_Updated(GestureRecognizer gesture)
    {
        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            if (_ball.GetComponent<BallController>() == null) return;
            var rb = _ball.GetComponent<Rigidbody2D>();
            if(rb != null) rb.AddForce(new Vector2(swipe.DeltaX, swipe.DeltaY) * gesture.Speed * flickForce * Time.deltaTime);
            _ball = null;           
        }
    }
}