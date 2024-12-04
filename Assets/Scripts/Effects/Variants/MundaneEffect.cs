using System.Text;
using Characters;
using UnityEngine;

namespace Effects.Variants
{
    public class MundaneEffect : IEffect
    {
        public StringBuilder GetDescription() => new StringBuilder("Mundane Effect");
        public Color GetEffectColor() => Color.white;
        public float GetDuration() => 0;
        public float GetCost() => 0;
        public void OnAdded(Player player) {  }

        public void Tick(float dt) {  }

        public void OnRemove() {   }
    }
}