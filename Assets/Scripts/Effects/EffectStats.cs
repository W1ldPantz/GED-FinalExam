using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(fileName = "MyEffectStats", menuName = "ScriptableObjects/MyEffectStats")]
    public class EffectStats : ScriptableObject
    {
        [SerializeField, TextArea] private string description;
        [SerializeField, ColorUsage(true, true)] private Color effectColor;
        [SerializeField,Min(0)] private float cost;
        [SerializeField,Min(0)] private float duration;

        public string Description => description;
        public Color EffectColor => effectColor;
        public float Cost  => cost;
        public float Duration  => duration;
    }
}
