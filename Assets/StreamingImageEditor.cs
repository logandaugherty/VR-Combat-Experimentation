using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StreamingImage))]
public class StreamingImageEditor : Editor
{

    SerializedProperty filePath;
    SerializedProperty hapticClip;

    const string kAssetPrefix = "Assets/StreamingAssets";

    void OnEnable()
    {
        filePath = serializedObject.FindProperty("filePath");
        hapticClip = serializedObject.FindProperty("hapticClip");
    }

    public override void OnInspectorGUI()
    {
        StreamingImage myScript = (StreamingImage)target;

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
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Click Me"))
        {
            myScript.ConvertDictionary();
        }
    }
}