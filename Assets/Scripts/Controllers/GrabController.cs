using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRubyShared;

public class GrabController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] Transform _swipeVfx;
    [SerializeField] float _sizeMultiplier = 2f;
    #endregion

    GrabController _ball;
    void Tap_Updated(GestureRecognizer gesture)
    {

        if (gesture.State == GestureRecognizerState.Began)
        {
            _ball = FingersUtilityExtensions.GetTouchedObject(gesture).GetComponent<GrabController>();
            if (_ball.CompareTag("Ball"))
            {
                _ball.GetComponent<GrabController>().Expand();
            }
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (_ball == null) return;
            _ball.GetComponent<GrabController>().Shrink();
            _ball = null;
        }
    }

    public void Expand()
    {
        _scale = true;
        _t = 0;
        _timeDirection = 1f;
        _startScale = _ball.transform.localScale;
        _targetScale = (_ball.transform.localScale * _sizeMultiplier);
    }

    public void Shrink()
    {
        _scale = true;
        _t = 0;
        _timeDirection = -1f;
        _startScale = _ball.transform.localScale;
        _targetScale = new Vector3(1, 1, 1);
    }

    #region UNITY
    GameObject _fingerManager;
    void Start()
    {
        _fingerManager = GameObject.FindGameObjectWithTag("FingerManager");
        _startScale = new Vector3();
        CreateLongPressGesture();
    }

    bool _scale = false;
    Vector3 _startScale, _targetScale;
    void Update()
    {
        if (!_scale || _ball == null) return;
        ChangeBallScale(_timeDirection);
    }

    #endregion
    #region HELPERS
    float _t = 0; // lerp time
    float _timeDirection; // lerp anim direction
    void ChangeBallScale(float timeDirection)
    {
        if (_t < 1)
        {
            _t += (Time.deltaTime * timeDirection);
            _ball.transform.localScale = Vector3.Lerp(_startScale, _targetScale, _t);
        }
        else
        {
            _scale = false;
        }
    }
    void CreateLongPressGesture()
    {
        LongPressGestureRecognizer tap = new LongPressGestureRecognizer();
        tap.MinimumDurationSeconds = 0f;
        tap.StateUpdated += Tap_Updated;
        FingersScript.Instance.AddGesture(tap);
        tap.AllowSimultaneousExecutionWithAllGestures();
    }
    #endregion
}