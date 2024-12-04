using System;
using Effects.Variants;
using UnityEngine;
using World;

namespace Effects
{
    public class DebugPotionFactory : MonoBehaviour
    {
        [SerializeField] private Potion prefav;
        private void Start()
        {
            Potion a =Instantiate(prefav, transform.position, Quaternion.identity);
            Potion b =Instantiate(prefav, transform.position, Quaternion.identity);
            Potion c =Instantiate(prefav, transform.position, Quaternion.identity);

            IEffect effect = new MundaneEffect();
            a.InitializeEffect(effect);

            effect = new ShrinkingEffect(effect);
            b.InitializeEffect(effect);

            effect = new HairyEffect(effect);
            c.InitializeEffect(effect);
        }
    }
}