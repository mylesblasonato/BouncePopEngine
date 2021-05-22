using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DigitalRubyShared;
using TigerForge.EasyEventManager;
using UnityEngine.EventSystems;

public class FlickController : MonoBehaviour
{
    [SerializeField] MMFeedbacks _gameFeelSequence;
    [SerializeField] float _force = 100f;
    [SerializeField] string _tag = "Ball";
    [SerializeField] float _distance = 1f;

    SwipeGestureRecognizer _flickGesture;
    Rigidbody2D _rb;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _flickGesture = new SwipeGestureRecognizer();
        _flickGesture.StateUpdated += Swipe_Updated;
        _flickGesture.DirectionThreshold = 0;
        _flickGesture.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = SwipeTouchCount;
        _flickGesture.PlatformSpecificView = Image;
        _flickGesture.ThresholdSeconds = SwipeThresholdSeconds;
        FingersScript.Instance.AddGesture(swipe);
        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
    }

    void OnDisable()
    {
        if (FingersScript.HasInstance)
        {
            FingersScript.Instance.RemoveGesture(_ballMoveGesture);
        }
    }

    readonly List<RaycastResult> raycast = new List<RaycastResult>();
    Transform draggingBall;
    Vector3 dragOffset;

    void OnDrag(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            PointerEventData p = new PointerEventData(EventSystem.current);
            p.position = new Vector2(gesture.FocusX, gesture.FocusY);
            raycast.Clear();
            EventSystem.current.RaycastAll(p, raycast);
            foreach (RaycastResult result in raycast)
            {
                if (result.gameObject.CompareTag(_tag))
                {
                    // we have a ball!
                    Vector3 dragPos = Camera.main.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, 0f));
                    draggingBall = result.gameObject.transform;
                    dragOffset = draggingBall.position - dragPos;
                    draggingBall.GetComponent<FlickController>()._rb.velocity = Vector2.zero;
                    draggingBall.GetComponent<FlickController>().Feedback();
                    break;
                }
            }

            if (draggingBall == null)
            {
                gesture.Reset();
            }
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            Vector3 dragPos = Camera.main.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, 0f));
            Vector3 pos = draggingBall.transform.position;

            // don't mess with the z
            pos.x = dragPos.x + dragOffset.x;
            pos.y = dragPos.y + dragOffset.y;
            draggingBall.transform.position = pos;

            if (Mathf.Abs(gesture.DistanceX) > _distance && Mathf.Abs(gesture.DistanceY) > _distance)
            {
                draggingBall.GetComponent<FlickController>()._rb.AddForce(new Vector2(gesture.VelocityX, gesture.VelocityY) * _force * Time.deltaTime);
                draggingBall = null;
            }
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            
        }
    }
    
    public void Feedback()
    {
        _gameFeelSequence.PlayFeedbacks();
    }
}