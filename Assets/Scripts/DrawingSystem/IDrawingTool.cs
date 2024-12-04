using UnityEngine;

namespace DrawingSystem
{
    public interface IDrawingTool
    {
        public void OnLeftClickBegin(Vector2 location)
        {
        }

        public void OnLeftClickUpdated(Vector2 location)
        {
            
        }

        public void OnLeftClickEnd(Vector2 location)
        {
            
        }
        public void OnRightClickBegin(Vector2 location){}
        public void OnRightClickUpdated(Vector2 location){}
        public void OnRightClickEnd(Vector2 location){}
        public void OnDeselected(){}
        public void OnSelected(){}
    }
}
