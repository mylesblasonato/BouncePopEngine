using System.Collections.Generic;
using DigitalRubyShared;
using UnityEngine;

public static class FingersUtilityExtensions
{
    public static GameObject GetTouchedObject(GestureRecognizer gesture)
    {
        List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.PointerEventData eventData =
            new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventData.position = new Vector2(gesture.FocusX, gesture.FocusY);
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
        return results[0].gameObject != null ? results[0].gameObject : null;
    }
}