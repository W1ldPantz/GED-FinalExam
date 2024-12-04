using Characters.Components;
using Interfaces;
using Managers;
using UnityEngine;
using Utility;

namespace Characters
{
    public class Player : MonoBehaviour, IDamagable
    {
        public float speed;
        public float maxSpeed;
        public float jumpHeight;
        
       
        
        [SerializeField] private Transform head;
        public Transform Head => head;

        public InteractionComponent PlayerInteractionComponent { get; private set; }
        public MovementComponent PlayerMovementComponent { get; private set; }
        
        private void Awake()
        {
            PlayerInteractionComponent = GetComponent<InteractionComponent>();
            PlayerMovementComponent = GetComponent<MovementComponent>();
            PlayerControls.InitPlayer(this);
            PlayerControls.EnableGameControls();
        }


        public void TakeDamage(float amount)
        {
            
        }

        public void TakeDamage(float amount, Vector3 force)
        {
            
        }

        public void Die()
        {
            
        }
    }
}