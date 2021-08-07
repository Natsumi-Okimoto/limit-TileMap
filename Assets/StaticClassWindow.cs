using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StaticClassWindow : EditorWindow
{
    private Vector2 scroll;

    [MenuItem("Custom/Static Class Window")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<StaticClassWindow>();
    }
    

    // Update is called once per frame
   private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.FloatField("MaxMoveCount", GameData.instance.MaxMoveCount);

        EditorGUILayout.EndScrollView();
    }
}
