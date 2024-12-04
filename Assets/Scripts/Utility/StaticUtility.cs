using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Utility
{
    public static class StaticUtility
    {
        #region Layer
        //----------------- LAYER -------------------//
        public static readonly int DefaultLayer = 1<<LayerMask.NameToLayer("Default");
        public static readonly int WaterLayer = 1<<LayerMask.NameToLayer("Water");
        public static readonly int EnemyLayer = 1<<LayerMask.NameToLayer("Enemy");
        public static readonly int PlayerLayer = 1<<LayerMask.NameToLayer("Player");
        private static readonly int UILayer = 1<<LayerMask.NameToLayer("UI");
        private static readonly int IgnoreLayer = 1<<LayerMask.NameToLayer("Ignore Raycast");
        private static readonly int FXLayer = 1<<LayerMask.NameToLayer("TransparentFX");
        
        public static readonly int GroundLayers = DefaultLayer;
        public static readonly int GrabLayers = DefaultLayer;
        #endregion
        
        #region Shader
        //--------------------------- SHADER -----------------------------//
        public static readonly int ColorID = Shader.PropertyToID("_Color");
        public static readonly int FillID = Shader.PropertyToID("_Fill");
        #endregion

        #region Animation
        //---------------------------ANIMATION---------------------------------//

        

        #endregion



    }
    
    public static class StringGameParserUtility
    {
        #region Utility

        private static Dictionary<string, string> _mappedStrings = new();
        static StringGameParserUtility()
        {
            
        }
        
        //------------------------- UTILITY ----------------------//
        public static string TextMeshProData(this string text)
        {
            //Iterate through each word and detect it 
            string[] data = text.Split(' ');
            StringBuilder str = new StringBuilder(data.Length);

            foreach (var word in data)
            {
                
                str.Append(word);
            }
            
            return str.ToString();
        }
        #endregion
    }
}
