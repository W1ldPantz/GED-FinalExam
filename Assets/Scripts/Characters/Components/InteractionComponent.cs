using System;
using Interfaces;
using UnityEngine;
using Utility;

namespace Characters.Components
{
    public class InteractionComponent :MonoBehaviour
    {
        [SerializeField] private float reach = 3;
        [SerializeField] private float throwForce = 3;
        private IGrabable _grabable;
        private bool _interactionState;
        private Rigidbody _targetRb;

        private Transform _selectorTransform;
        private Vector3 _previousPosition;
        private Player _player;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
            _selectorTransform = new GameObject("SelectorTransform").transform;
            _selectorTransform.SetParent(_player.Head);
            _selectorTransform.localPosition = new Vector3(0,0,reach);
        }

        private void FixedUpdate()
        {
            
            if (_grabable == null || !_interactionState)
            {
                bool hit = Physics.Raycast(_player.Head.position, _player.Head.forward, out RaycastHit hitInfo, reach, StaticUtility.GrabLayers);

                if (!hit || !hitInfo.rigidbody || !hitInfo.rigidbody.TryGetComponent(out IGrabable grabable))
                {
                   

                    if (_grabable != null)
                    {
                        _grabable?.StopHover();
                        _grabable = null;
                    }
                    return;
                }
                
                if (grabable is IPopupItem popupItem)
                { 
                    popupItem.BuildFactory();
                }
                
                if (_interactionState)
                {
                    _targetRb = grabable.OnGrab();
                    _selectorTransform.localPosition = Vector3.forward* hitInfo.distance;
                }
                else if(grabable != _grabable)
                {
                    _grabable?.StopHover();
                    grabable.OnHover();
                }
                _grabable = grabable;
            }
            else if (_targetRb)
            {
                
                
                _targetRb.linearVelocity = (_selectorTransform.position-_previousPosition) * throwForce; 
                
            }
            print( ( _selectorTransform.position-_previousPosition) * throwForce);
            _previousPosition = _selectorTransform.position;
        }

        private void OnDrawGizmos()
        {
            if (!_player) _player = GetComponent<Player>();
            Gizmos.DrawLine(_player.Head.position + Vector3.down*0.05f,_player.Head.position +  _player.Head.forward * reach);
        }

        public void SetInteractionState(bool state)
        {
            print("Interact: " + state);
            _interactionState = state;
            if (_grabable == null) return;
            if (!_interactionState)
            {
                _grabable.OnRelease();
            }
            else
            {
                _grabable = null;
            }
        }
    }
}