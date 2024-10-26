using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager manager = (GameManager)target;
        if (GUILayout.Button("Set Day")) {
            manager.SetIsDay(true);
        }
        if (GUILayout.Button("Set Night")) {
            manager.SetIsDay(false);
        }
        if (GUILayout.Button("Set Rain")) {
            manager.SetIsRain(true);
        }
        if (GUILayout.Button("Set no Rain")) {
            manager.SetIsRain(false);
        }
    }
}
