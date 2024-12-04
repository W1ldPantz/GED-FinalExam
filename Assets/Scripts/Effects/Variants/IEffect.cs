using System.Text;
using Characters;
using UnityEngine;

namespace Effects.Variants
{
    public interface IEffect
    {
        public StringBuilder GetDescription();
        public Color GetEffectColor();
        public float GetDuration();
        public float GetCost();

        //public void OnEffectApplied();
        //public void OnEffectTick(float deltaTime);
        //public void OnEffectEnded();
        void OnAdded(Player player);
        void Tick(float dt);
        void OnRemove();
    }
}