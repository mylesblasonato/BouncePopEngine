using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using MoreMountains.Feedbacks;
using DigitalRubyShared;
using TigerForge.EasyEventManager;

public class EventListener : MonoBehaviour
{
    [SerializeField] MMFeedbacks _gameFeelSequence;
    [SerializeField] EasyEvent _OnTap;

    Canvas canvas;
    PanGestureRecognizer letterMoveGesture;

    void OnEnable()
    {
        canvas = GetComponentInChildren<Canvas>();
        letterMoveGesture = new PanGestureRecognizer();
        letterMoveGesture.ThresholdUnits = 0.0f; // start right away
        //letterMoveGesture.StateUpdated += LetterGestureUpdated;
        FingersScript.Instance.AddGesture(letterMoveGesture);
    }

    void OnDisable()
    {
        if (FingersScript.HasInstance)
        {
            FingersScript.Instance.RemoveGesture(letterMoveGesture);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _OnTap.Invoke();
        }
    }

    public void PrintTest()
    {
        _gameFeelSequence.PlayFeedbacks();
    }
}