using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Factory", menuName = "Utility/Factory", order = 1)]
    public class FactoryScriptable : ScriptableObject
    {
        [field: SerializeField] public GameObject FactoryPrefab { get; private set; }
    }
}