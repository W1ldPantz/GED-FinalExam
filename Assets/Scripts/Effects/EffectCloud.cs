using Effects.Variants;
using JetBrains.Annotations;
using UnityEngine;

namespace Effects
{
    public class EffectCloud : MonoBehaviour
    {
        private IEffect _effect;

        //Only called once;
        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize([NotNull] IEffect effect)
        {
            #if UNITY_EDITOR
            if (_effect != null)
            {
                Debug.LogWarning("Trying to double initialize a potion");
                return;
            }
            #endif
            _effect = effect;
            ParticleSystem system = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();

            var main = system.main;
            var color = main.startColor;
            color.color = effect.GetEffectColor();
            main.startColor = color;
            main.duration = effect.GetDuration();
            
            //transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            system.Play();
            
            Destroy(gameObject, effect.GetDuration() + main.startLifetime.constantMax);
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb && rb.TryGetComponent(out EffectHandler handler))
            {
                handler.TryApply(_effect);
            }
        }
    }
}
