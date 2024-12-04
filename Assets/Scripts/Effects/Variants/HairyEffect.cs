using UnityEngine;

namespace Effects.Variants
{
    public class HairyEffect : EffectDecorator
    {

        public HairyEffect(IEffect decorator) : base(decorator) {  }

        protected override string EffectName => "HairyPotion";

        protected override void OnApplied()
        {
            base.OnApplied();
            Debug.Log("Player is now hairy");
        }

        public override void OnRemove()
        {
            base.OnRemove();
            Debug.Log("Player is no longer hairy");
        }

    }
}
