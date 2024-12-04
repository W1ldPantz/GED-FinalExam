using System.Collections.Generic;
using Effects.Variants;
using UnityEngine;
using World.Game;

namespace Effects
{
    public static class EffectFactory
    {
        public static IEffect BuildEffect(ref Stack<PlacedIngredient> ingredients)
        {
            IEffect effect = new MundaneEffect();

            while (ingredients.TryPop(out PlacedIngredient ingredient))
            {
                switch (ingredient.MyProp.name.ToLower())
                {
                    case "sphere":
                        effect = new HairyEffect(effect);
                        break;
                    case "cube":
                        effect = new ShrinkingEffect(effect);
                        break;
                    default:
                        Debug.Log("Added invalid ingredient");
                        break;
                }
            }

            return effect;
        }
        
    }
}
