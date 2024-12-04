using UnityEngine;

namespace Interfaces
{
    public interface IPopupFactory
    {
        GameObject Prefab();
        void BindTo(GameObject popup, IPopupItem item);

        void Discard();
    }
}
