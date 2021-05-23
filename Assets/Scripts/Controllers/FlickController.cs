﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;
using Ludiq.PeekCore;

public class FlickController : MonoBehaviour
{
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
        }
    }

    void Swipe_Updated(GestureRecognizer gesture)
    {
        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            if (_ball == null) return;
            var rb = _ball.GetComponent<Rigidbody2D>();
            if(rb != null) rb.AddForce(new Vector2(swipe.DeltaX.Normalized(), swipe.DeltaY.Normalized()) * gesture.Speed * flickForce * Time.deltaTime);
            _ball = null;
        }
    }
}