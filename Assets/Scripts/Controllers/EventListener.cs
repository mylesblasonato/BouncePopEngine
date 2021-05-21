using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using MoreMountains.Feedbacks;
using DigitalRubyShared;

public class EventListener : MonoBehaviour
{
    [SerializeField] MMFeedbacks _gameFeelSequence;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("OnPrint", PrintTest);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("OnPrint", PrintTest);
    }

    
    Canvas canvas;
    PanGestureRecognizer letterMoveGesture;

    private void OnEnable()
    {
        canvas = GetComponentInChildren<Canvas>();
        letterMoveGesture = new PanGestureRecognizer();
        letterMoveGesture.ThresholdUnits = 0.0f; // start right away
        //letterMoveGesture.StateUpdated += LetterGestureUpdated;
        FingersScript.Instance.AddGesture(letterMoveGesture);
    }

    private void OnDisable()
    {
        if (FingersScript.HasInstance)
        {
            FingersScript.Instance.RemoveGesture(letterMoveGesture);
        }
    }
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.EmitEvent("OnPrint");
        }
    }

    void PrintTest()
    {
        _gameFeelSequence.PlayFeedbacks();
    }
}
