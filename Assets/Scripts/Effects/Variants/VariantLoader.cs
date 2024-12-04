using System.Collections.Generic;
using UnityEngine;

namespace Effects.Variants
{
    public static class VariantLoader
    {
        private static readonly Dictionary<string, EffectStats> Variants = new();
        
        public static EffectStats GetEffectStats(string name)
        {
            //Lazily Loaded... May be better to preload everything
            if (Variants.TryGetValue(name, out EffectStats effectStats)) return effectStats;
            EffectStats stats = Resources.Load<EffectStats>("Effects/" + name);
            Variants.Add(name, stats);
            return stats;
        }
    }
}