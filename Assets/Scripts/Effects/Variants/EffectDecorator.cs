using System.Text;
using Characters;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Effects.Variants
{
    public abstract class EffectDecorator : IEffect
    {
        
        private readonly StringBuilder _stringBuilder;
        private readonly Color _effectColor;
        private readonly float _cost;
        private readonly float _duration;
        protected Player Player;
        private readonly IEffect _effectDecorator;

        protected abstract string EffectName { get; }
        public EffectDecorator(IEffect effectDecorator)
        {
            //Each class must declare this
            Debug.Assert(EffectName != null, nameof(EffectName) + " != null");
            EffectStats stats = VariantLoader.GetEffectStats(EffectName);
            _effectDecorator = effectDecorator;
            
            //We might as well bake the data...
            _effectColor = stats.EffectColor * effectDecorator.GetEffectColor();
            _cost = stats.Cost + effectDecorator.GetCost();
            _duration += stats.Duration + effectDecorator.GetDuration();
            _stringBuilder = effectDecorator.GetDescription().AppendLine("- " + stats.Description);
        }
        
      
        
        public StringBuilder GetDescription() => _stringBuilder;
        public Color GetEffectColor() => _effectColor;
        public float GetCost() => _cost;

        public void OnAdded(Player player)
        {
            Player = player;
            OnApplied();
            _effectDecorator.OnAdded(Player);
        }

        protected virtual void OnApplied() {  }
        public virtual void Tick(float dt) { _effectDecorator.Tick(dt);}
        public virtual void OnRemove() { _effectDecorator.OnRemove();  }
        public float GetDuration() => _duration;
        
    }
}
