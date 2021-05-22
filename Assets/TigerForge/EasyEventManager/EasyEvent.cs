using System;
using UnityEngine;
using Object = System.Object;

namespace TigerForge.EasyEventManager
{
    [CreateAssetMenu(fileName = "New Easy Event", menuName = "TigerForge/Create Easy Event")]
    public class EasyEvent : ScriptableObject
    {
        [SerializeField] int _id;
        public int ID => _id;
        public string Event => name;

        public void Invoke(int id)
        {
            _id = id;
            EventManager.EmitEvent(name);
        }
    }
}
