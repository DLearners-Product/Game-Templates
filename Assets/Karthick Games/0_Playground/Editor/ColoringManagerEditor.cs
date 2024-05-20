using UnityEditor;
using UnityEngine;

namespace ColorGame
{

    [CustomEditor(typeof(ColoringManager))]
    public class ColoringManagerEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColoringManager coloringManager = (ColoringManager)target;

            // // Display a ColorField in the inspector
            coloringManager.newColor = EditorGUILayout.ColorField("Q1 Color", coloringManager.newColor);


            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Apply"))
            {
                // Apply the new color to the sprite renderer
                coloringManager.ApplyColor();
            }


            GUILayout.EndHorizontal();



            if (GUILayout.Button("Q2"))
            {

            }
            if (GUILayout.Button("Q3"))
            {

            }
            if (GUILayout.Button("Q4"))
            {

            }
            if (GUILayout.Button("Q5"))
            {

            }






        }












    }
}