using UnityEngine;
using UnityEngine.UI;

namespace DrawingSystem
{
    public class DrawingCanvas : MonoBehaviour
    {
        private static readonly int Label = Shader.PropertyToID("_Label");
        private RawImage drawingArea;    // The UI RawImage where the texture will be displayed
        [SerializeField] private Texture2D inputImage;    // The MxN image we want to compress
        [SerializeField] private MeshRenderer mesh;    // The MxN image we want to compress
    
        private Texture2D texture;
        private Material m;

        private float widthRatio;
        private float heightRatio;
        
        private void Awake()
        {
            drawingArea = transform.GetChild(0).GetComponent<RawImage>();
            texture = new Texture2D(512, 512);
            texture.SetPixels(inputImage.GetPixels());
            texture.Apply();
            drawingArea.texture = texture;
            drawingArea.uvRect = new Rect(0f, 0.5f, 1f, 0.5f);
            m = mesh.material;
            m.SetTexture(Label, texture);

            widthRatio = 512 / drawingArea.rectTransform.rect.width;
            heightRatio = 256 / drawingArea.rectTransform.rect.height;
        }
        
        //points must be a rolled array, describing the indicies on the texture to modify.
        public void UpdateImage(int[] points, Color[] colors)
        {
            // Get the current pixel data from the texture
            Color[] currentColors = texture.GetPixels(0, 256, 512, 256);

            // Iterate through the points and apply the corresponding color from the provided array
            for (int i = 0; i < points.Length; i++)
            {
                int index = points[i];
                if(index == -1) continue;
                Color trueColor = colors[i];
                trueColor.a = 1;
                currentColors[index] = Color.Lerp(currentColors[index] , trueColor, colors[i].a);   // Apply the color at the corresponding index
            }

            // Update the texture with the modified colors
            texture.SetPixels(0, 256, 512, 256, currentColors);
            texture.Apply();
        }
        
        public void UpdateImage(Color[] colors)
        {
            texture.SetPixels(0, 256, 512, 256, colors);
            texture.Apply();
        }

        public Vector2Int GetPixelFromScreenSpace(Vector2 screenPoint)
        {
            screenPoint -= drawingArea.rectTransform.anchoredPosition / 2;
            // Convert screen coordinates to texture coordinates
            int x = Mathf.RoundToInt(screenPoint.x * widthRatio);
            int y = Mathf.RoundToInt(screenPoint.y * heightRatio);

            return new Vector2Int(x, y);
        }


        /*
         * In order to save or update the image, consider that we have a UI image of dimensions M and N
         * We want to compress that image to fit into the texture which is 512x512. However, the texture will only draw to the top 512x256,
         * the pixels on the Y axis after 256 should be pure black.
         * After every change, we need to update the texture
         */
        //Generate Canvas
        public void GenerateCanvas()
        {
            
        }
        
        /*
         * In order to save or update the image, consider that we have a UI image of dimensions M and N
         * We want to compress that image to fit into the texture which is 512x512. However, the texture will only draw to the top 512x256,
         * the pixels on the Y axis after 256 should be pure black.
         * After every change, we need to update the texture
         */

        public void SaveCanvasToTexture(string fileName)
        {
            
        }


        public void Load(string fileLocation)
        {
            throw new System.NotImplementedException();
        }

        public Color GetPixelAt(Vector2 location)
        {
            return texture.GetPixel((int)location.x, (int)location.y);
        }


        public Color[] GetSnapshot()
        {
            return texture.GetPixels(0,256,512,256);
        }
    }
}
