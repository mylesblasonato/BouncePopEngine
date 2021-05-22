using UnityEngine;
using UnityEngine.Events;

namespace TigerForge.EasyEventManager
{
    public class EasyEventListener : MonoBehaviour
    {
        [SerializeField] EasyEvent _easyEvent;
        [SerializeField] UnityEvent _easyEventCallback;
        [SerializeField] int _senderId;

        void Awake()
        {
            EventManager.StartListening(_easyEvent.Event, Invoke);
        }

        void OnDestroy()
        {
            EventManager.StopListening(_easyEvent.Event, Invoke);
        }

        void Invoke()
        {
            if(_easyEvent.ID == _senderId)
                _easyEventCallback?.Invoke();
        }
    }
}
