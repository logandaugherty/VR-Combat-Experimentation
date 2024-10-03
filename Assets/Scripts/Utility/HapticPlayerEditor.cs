using System;
using UnityEditor;
using UnityEngine;
using static HapticPlayer.Root;

[CustomEditor(typeof(HapticPlayer))]
public class HapticPlayerEditor : Editor
{

    SerializedProperty filePath;
    SerializedProperty hapticClip;
    SerializedObject values;

    const string kAssetPrefix = "Assets/StreamingAssets";

    void OnEnable()
    {
        filePath = serializedObject.FindProperty("filePath");
        hapticClip = serializedObject.FindProperty("hapticClip");
    }

    public override void OnInspectorGUI()
    {
        HapticPlayer myScript = (HapticPlayer)target;

        serializedObject.Update();
        EditorGUILayout.PropertyField(hapticClip);
        EditorGUILayout.PropertyField(filePath);

        if (hapticClip.objectReferenceValue == null)
        {
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(hapticClip.objectReferenceValue.GetInstanceID());
        if (assetPath.StartsWith(kAssetPrefix))
        {
            assetPath = assetPath.Substring(kAssetPrefix.Length);
        }
        filePath.stringValue = assetPath;

        if (GUILayout.Button("Convert to Dictionary"))
        {
            myScript.ConvertDictionary();
        }
        if (GUILayout.Button("Print Dictionary"))
        {
            myScript.PrintDictionary();
        }
        if (GUILayout.Button("Refresh Assets"))
        {
            serializedObject.Update();
            AssetDatabase.Refresh();
            myScript.PrintDictionary();

            myScript.testVar = 1;

        }

    }
}