using DrawingSystem.Commands;
using UnityEngine;

namespace DrawingSystem
{
    public class DrawTool : MonoBehaviour, IDrawingTool
    {
        [SerializeField] private int brushSize;
        [SerializeField, Range(0,1)] private float fallOff = 0.5f; private Color color;
        //private List<Vector2> points;
        Vector2 point;


        private DrawCommand _command;
        
        private Color[] finalSnapshot;
        
        //When we begin our press, we must begin transacting / recording
        public void OnLeftClickBegin(Vector2 location)
        {
            //We can assume once we begin drawing, that the colour will remain constant.
            color = CanvasController.Instance.color;

            _command = new DrawCommand();

        }
        
        //while the mouse is updating, draw over each tile
        public void OnLeftClickUpdated(Vector2 location)
        {
            if ((point - location).sqrMagnitude > 1)
            {
              point = location;
              // Iterate over a square bounding box around the circle
              int radius = brushSize / 2;
              int count = brushSize * brushSize + radius * 4 + 1;
              int[] locations = new int[count];
              Color[] colors = new Color[count]; // default size, no expansion needed.
              Vector2Int textureSpace = CanvasController.Instance.GetPixelFromScreenSpace(point);
              int iterator = 0;
              for (int y = -radius; y <= radius; y++)
              {
                  for (int x = -radius; x <= radius; x++)
                  {
                     
                      int pixelX = textureSpace.x + x;
                      int pixelY = textureSpace.y + y;
                      //If we're even on the canvas.
                      if (pixelX >= 0 && pixelX < 512 && pixelY >= 0 && pixelY < 256)
                      {
                          // Calculate the distance from the center point
                          float distance = Mathf.Sqrt(x * x + y * y);

                          Color c = color;
                          c.a *= Mathf.Clamp01(1 - (distance / radius) * (fallOff+0.5f)); // 1 - (%distance from middle) * falloff% ()
                           
                          locations[iterator] = pixelY * 512 + pixelX;
                          colors[iterator] = c;
                      }
                      else
                      {
                          locations[iterator] = -1;
                          colors[iterator] = color;
                      }

                      iterator += 1;
                  }
              }
              
              CanvasController.Instance.DrawPixels(locations, colors);
              _command.PushPoints(locations, colors);
            }
        }

        //TODO: When we end our press, we should send our transaction as a command
        public void OnLeftClickEnd(Vector2 location)
        {
            CanvasController.Instance.PushCommand(_command);
        }
    }
}
