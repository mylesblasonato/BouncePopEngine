using UnityEngine;
using UnityEngine.Events;

namespace TigerForge.EasyEventManager
{
    public class EasyEventListener : MonoBehaviour
    {
        [SerializeField] EasyEvent _easyEvent;
        [SerializeField] UnityEvent _easyEventCallback;

        void Awake()
        {
            EventManager.StartListening(_easyEvent.Event, Invoke, this.gameObject.name);
        }

        void OnDestroy()
        {
            EventManager.StopListening(_easyEvent.Event, Invoke);
        }

        void Invoke()
        {
            _easyEventCallback?.Invoke();
        }
    }
}
