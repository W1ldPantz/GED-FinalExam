using System;
using UnityEngine;

namespace Optimization.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
    public class ProjectileSo : ScriptableObject
    {
        [SerializeField] private int damage;
        [SerializeField, Min(1)] private int pierce;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float speedModifier;
        [SerializeField] private float lifeTime = 3;
        private static int _id;
        private int _myId;
        
        public int Damage => damage;
        public int Pierce => pierce;
        public float ExplosionRadius => explosionRadius;
        public float SpeedModifier => speedModifier;
        
        public int GetID => _myId;
        public float LifeTime => lifeTime;

        private void OnValidate()
        {
            _myId = _id++;
        }
    }
}
