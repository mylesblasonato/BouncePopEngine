using System;
using UnityEngine;

namespace TigerForge.EasyEventManager
{
    [CreateAssetMenu(fileName = "New Easy Event", menuName = "TigerForge/Create Easy Event")]
    public class EasyEvent : ScriptableObject
    {
        [SerializeField] int _id;
        public string Event => name;

        public void Invoke()
        {
            EventManager.EmitEvent(name);
        }
    }
}
