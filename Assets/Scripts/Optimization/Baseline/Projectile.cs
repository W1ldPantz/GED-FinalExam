using Interfaces;
using Optimization.ScriptableObjects;
using UnityEngine;
using Utility;

namespace Optimization.Baseline
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileSo stats;
        private int _pierce;
        private float _lifeTime;
        
        private Transform _refTransform;
        public ProjectileSo Stats => stats;

        private Vector3 _velocity;
        
        
        private void OnEnable()
        {
            _pierce = stats.Pierce;
            _lifeTime = stats.LifeTime;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Launch(Vector3 forceModifier)
        {
            _velocity = forceModifier * stats.SpeedModifier;
        }

        public bool Tick(float dt)
        {
            transform.position += dt * _velocity;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1,
                    StaticUtility.DefaultLayer))
            {
                
                Rigidbody rb = hit.rigidbody;
                if(hit.transform.TryGetComponent(out IDamagable damagable) || (rb && rb.TryGetComponent(out damagable)))
                {
                    damagable.TakeDamage(stats.Damage);

                }

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (--_pierce <= 0)
                {
                    gameObject.SetActive(false);
                    return false;
                }

            }

            
            _lifeTime -= dt;

            if (_lifeTime < 0)
            {
                    
                gameObject.SetActive(false);
                return false;
            }
            
            return true;
        }
    }
}
