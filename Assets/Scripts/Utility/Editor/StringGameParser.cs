using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utility.Editor
{
    //An editor only script that allows words to be bound to: images, colors, links - to later be used in Text Mesh Pro
    //It stores any necesary references in a baked dictionary which is used in the static Utility function below
    //Written by ChatGPT
    public class StringGameParser : EditorWindow
    {
        private string word = "";
        private string textMeshProTag = "";
        
        // Static dictionary to map words to associated TextMeshPro tags, colors, or links
        private static Dictionary<string, string> wordMappings = new Dictionary<string, string>();

        // Method to add mappings between words and their TextMeshPro replacements
        public static void AddMapping(string word, string textMeshProTag)
        {
            wordMappings.TryAdd(word, textMeshProTag);
        }

        // Method to clear all mappings
        public static void ClearMappings()
        {
            wordMappings.Clear();
        }


        // Utility to display the current mappings in the console (for debug purposes)
        public static void DisplayMappings()
        {
            foreach (var mapping in wordMappings)
            {
                Debug.Log($"Word: {mapping.Key}, Replacement: {mapping.Value}");
            }
        }

        [MenuItem("Tools/String Game Parser")]
        public static void ShowWindow()
        {
            GetWindow<StringGameParser>("String Game Parser");
        }

        private void OnGUI()
        {
            GUILayout.Label("Add Word Mappings for TextMeshPro", EditorStyles.boldLabel);

            word = EditorGUILayout.TextField("Word", word);
            textMeshProTag = EditorGUILayout.TextField("TextMeshPro Tag", textMeshProTag);

            if (GUILayout.Button("Add Mapping"))
            {
                if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(textMeshProTag))
                {
                    AddMapping(word, textMeshProTag);
                    Debug.Log($"Added Mapping: {word} -> {textMeshProTag}");
                }
                else
                {
                    Debug.LogWarning("Both word and tag must be provided.");
                }
            }

            if (GUILayout.Button("Clear Mappings"))
            {
                ClearMappings();
                Debug.Log("Cleared all mappings.");
            }

            if (GUILayout.Button("Display Mappings"))
            {
                DisplayMappings();
            }
        }
    }
}