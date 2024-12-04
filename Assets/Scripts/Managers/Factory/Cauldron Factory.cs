using System.Text;
using Interfaces;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using World.Game;

namespace Managers.Factory
{
    [CreateAssetMenu(fileName = "CauldronFactory", menuName = "Utility/Cauldron Factory", order = 1)]

    public class CauldronFactory : FactoryScriptable, IPopupFactory
    {
        private Cauldron _cauldron;
        private GameObject _parent;
        private TextMeshProUGUI _text;
        private Button _button;
        public GameObject Prefab() => FactoryPrefab;
        
        

        public void BindTo(GameObject popup, IPopupItem item)
        {
            //Name
            
            _cauldron = item as Cauldron;

            if (!_cauldron) return;
            
            //Ingredients
            
            Transform transform = popup.transform.GetChild(0);

            _text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();


            IngredientsChanged();
            _cauldron.OnIngredientsChanged += IngredientsChanged;

            //Undo Button
            _button = transform.GetChild(1).GetComponent<Button>();
            _button.onClick.AddListener(_cauldron.Undo);

            //Observe OnIngredientAdded
        }

        public void Discard()
        {
            Destroy(_parent);
            _cauldron.OnIngredientsChanged -= IngredientsChanged;
            _button.onClick.RemoveListener(_cauldron.Undo);
        }

        private void IngredientsChanged()
        {
            StringBuilder builder = new();
            foreach (PlacedIngredient myIngredient in _cauldron.Ingredients)
            {
                builder.AppendLine(myIngredient.MyProp.name);
            }
            _text.text = builder.ToString();
        }
    }
}
