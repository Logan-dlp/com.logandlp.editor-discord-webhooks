// Copyright 2024, Logan, All rights reserved.

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorDiscordWebhooksConfig))]
public class EditorDiscordWebhooksConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorDiscordWebhooksConfig config = (EditorDiscordWebhooksConfig)target;

        config.Username = EditorGUILayout.TextField("Username", config.Username);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(config);
        }
        
        //base.OnInspectorGUI();
    }
}