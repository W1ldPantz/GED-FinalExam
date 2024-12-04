using UnityEngine;

namespace Interfaces
{
    public interface IDamagable
    {
        public void TakeDamage(float amount);
        public void TakeDamage(float amount, Vector3 force);
    }
}
