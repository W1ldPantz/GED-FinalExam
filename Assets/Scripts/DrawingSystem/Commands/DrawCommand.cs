using System.Collections.Generic;
using Color = UnityEngine.Color;

namespace DrawingSystem.Commands
{
    public class DrawCommand : ICanvasCommand
    {

        private readonly List<Color> _colors = new();
        private readonly List<int> _points = new();

        
        //Lazy, but works.
        private readonly Color[] previousScreen = CanvasController.Instance.Snapshot();
        
        public void Redo()
        {
            CanvasController.Instance.DrawPixels(_points.ToArray(), _colors.ToArray());
        }

        public void Undo()
        {
            CanvasController.Instance.DrawPixels(previousScreen);   
        }

        public void PushPoints(int [] newPoints, Color[] colors)
        {
            _points.AddRange(newPoints);
            _colors.AddRange(colors);
        }
    }
}