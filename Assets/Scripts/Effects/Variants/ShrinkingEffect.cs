using UnityEngine;

namespace Effects.Variants
{
    public class ShrinkingEffect : EffectDecorator
    {
        public ShrinkingEffect(IEffect demo) : base(demo) { }
        private Vector3 originalScale;
        protected override string EffectName => "ShrinkingPotion";
        
        protected override void OnApplied()
        {
            base.OnApplied();
            Debug.Log("Shrinking Potion Effect Applied");
            originalScale = Player.transform.localScale;
        }

        public override void Tick(float dt)
        {
            base.Tick(dt);
            Vector3 vec = Player.transform.localScale;
            vec.y = Mathf.Max(0.1f,  vec.y  -0.1f * dt);
            Player.transform.localScale = vec;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            Player.transform.localScale = originalScale;
        }
    }
}
