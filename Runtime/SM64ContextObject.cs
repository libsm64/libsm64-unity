using System;
using UnityEngine;

namespace LibSM64
{
    public class SM64ContextObject : MonoBehaviour
    {
        Action _update;
        Action _fixedUpdate;
        
        void Update()      { _update(); }
        void FixedUpdate() { _fixedUpdate(); }
        
        public void BindUpdateListeners( Action update, Action fixedUpdate )
        {
            _update = update;
            _fixedUpdate = fixedUpdate;
        }
    }
}