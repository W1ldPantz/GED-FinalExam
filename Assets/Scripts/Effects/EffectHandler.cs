using System;
using System.Collections.Generic;
using Characters;
using Effects.Variants;
using UnityEngine;

namespace Effects
{
   

    public class EffectHandler : MonoBehaviour
    {
        private struct LifeBinding
        {
            public IEffect Effect;
            public float Life;
        }
        
        private readonly List<LifeBinding> effects = new();
        private Player p;
        private void Awake()
        {
            p = GetComponent<Player>();
        }

        /// <summary>
        /// Tries to apply an effect to our handler
        /// Notifies when a new effect is applied
        /// Applies effects if the level or duration is greater than the current level or duration of an applied effect.  Level > Duration
        /// </summary>
        /// <param name="effect"></param>
        public void TryApply(IEffect effect)
        {
            effect.OnAdded(p);
            effects.Add(new LifeBinding(){Effect = effect,Life =  effect.GetDuration()});
        }

        public void FixedUpdate()
        {
            float dt = Time.deltaTime;

            for (int i =  effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];
                effect.Effect.Tick(dt);
                effect.Life -= dt;
                effects[i] = effect;
                if (effect.Life < 0) RemoveEffect(i);
            }
        }

        public void RemoveEffect(int index)
        {
            effects[index].Effect.OnRemove();
            effects.RemoveAt(index);
        }
    }
}