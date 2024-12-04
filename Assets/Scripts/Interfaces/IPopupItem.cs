using UI;

namespace Interfaces
{
    public interface IPopupItem
    {
        public IPopupFactory GetFactory();

        public void BuildFactory()
        {
                PopupController.Instance.CreatePopup(GetFactory(), this);
        }
    }
}