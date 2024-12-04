using Effects;
using Effects.Variants;
using Interfaces;
using UnityEngine;
using Utility;

namespace World
{
    [SelectionBase]
    public class Potion : Prop, IDamagable
    {
        private IEffect _effect;
        
        private float _previousSpeed;
        
        [SerializeField] private float breakForce = -25; // This number should be squared manually so 10 --> 100.
        [SerializeField] private EffectCloud cloud;
        public void InitializeEffect(IEffect effect)
        {
            _effect = effect;

            MeshRenderer mr = transform.GetChild(2).GetComponent<MeshRenderer>();
            
            mr.material.SetColor(StaticUtility.ColorID, _effect.GetEffectColor());
            
            print("Initialized Potion");
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            _previousSpeed = Rb.linearVelocity.sqrMagnitude;
        }

        private void OnCollisionEnter(Collision other)
        {
            //While using SqrMagnitude is logically faster, the difference is very minor.
            Vector3 velocity = Rb.linearVelocity;
            float speed = velocity.sqrMagnitude;
            
            //If it's negative, then we've suddenly slowed down
            if (speed - _previousSpeed < breakForce)
            {
                Break(velocity);
            }
        }

        public void TakeDamage(float amount)
        {
            Vector3 dir = Vector3.up;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                dir = hit.normal;
            Break(dir);
        }

        public void TakeDamage(float amount, Vector3 force)
        {
            Break(-force);
        }


       
        private void Break(Vector3 direction)
        {
            var c = Instantiate(cloud, transform.position, Quaternion.LookRotation(direction));
            c.Initialize(_effect);
            Destroy(gameObject);
        }
    }
}
