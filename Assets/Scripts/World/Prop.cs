using System;
using Interfaces;
using UnityEngine;

namespace World
{
    [RequireComponent(typeof(Rigidbody))]
    public class Prop : MonoBehaviour, IGrabable
    {
        /// TODO: prop based saving system
        /// Should automatically save every prop (including new ones)
        /// Transform is always saved
        /// Each prop can override what details need to be saved

        private Vector3 _previousPosition;
        private Vector3 _previousVelocity;
        
        protected Rigidbody Rb;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        //Render Outline
        public virtual void OnHover()
        {
            print("Hovered");
        }
        
        //Move the object with the 
        public virtual Rigidbody OnGrab()
        {
            print("Grabbed");
            //Rb.isKinematic = true;
            Rb.useGravity = false;
            return Rb;
        }

        public virtual void OnRelease()
        {
            if(Rb == null) return; // We've been destroyed
            print("Released");
            //Rb.isKinematic = false;
            
            Rb.useGravity = true;
            //transform.parent = null;
            //Rb.AddForce(_previousVelocity * 10, ForceMode.Impulse);
        }

        public void StopHover()
        {
            print("Stop Hovering");
        }

        protected virtual void FixedUpdate()
        {
            _previousVelocity = transform.position-_previousPosition;
            _previousPosition = transform.position;
        }
    }
}
