using Interfaces;
using UnityEngine;

namespace UI
{
    [DefaultExecutionOrder(-1000)]
    public class PopupController : MonoBehaviour
    {

        public static PopupController Instance { get; private set; }
        private IPopupFactory currentFactory;
        
        private GameObject popup;
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void CreatePopup(IPopupFactory factory, IPopupItem item)
        {
            if (currentFactory == factory) return;
            
            if (currentFactory != null)
            {
                currentFactory.Discard();
            }

            currentFactory = factory;

            popup = Instantiate(factory.Prefab());

            factory.BindTo(popup, item);
        }

    }
}
