using UnityEngine;

namespace DrawingSystem
{
    [RequireComponent(typeof(Camera)), ExecuteAlways]
    public class MiniCamera : MonoBehaviour
    {
        private Camera miniMapCamera; 
        public Rect coords = new Rect(0,0.75f, 0.25f, 0.25f);

        private void Awake()
        {
            miniMapCamera = GetComponent<Camera>();
            miniMapCamera.rect = coords;
        }

        #if UNITY_EDITOR
        void Update()
        {
            miniMapCamera.rect = coords;
        }
        #endif
    }
}
