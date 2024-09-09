// Copyright 2024, Logan, All rights reserved.

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorDiscordWebhooksConfig))]
public class EditorDiscordWebhooksConfigEditor : Editor
{
    private SerializedObject _serializedObjectTarget;
    private SerializedProperty _webhooksAPIProperty;

    private void OnEnable()
    {
        _serializedObjectTarget = new(target);
        _webhooksAPIProperty = _serializedObjectTarget.FindProperty("WebhooksAPI");
    }

    public override void OnInspectorGUI()
    {
        EditorDiscordWebhooksConfig config = (EditorDiscordWebhooksConfig)target;
        config.Username = EditorGUILayout.TextField("Username", config.Username);

        _serializedObjectTarget.Update();
        EditorGUILayout.PropertyField(_webhooksAPIProperty, new GUIContent("Webhooks API"));

        if (_serializedObjectTarget.ApplyModifiedProperties())
        {
            EditorUtility.SetDirty((target));
        }
        
        config.Logging = EditorGUILayout.Toggle("Logging", config.Logging);

        if (GUILayout.Button("Save"))
        {
            if (config.Username.Length != 0)
            {
                PlayerPrefs.SetString("WebhooksUsername", config.Username);
            }
            
            PlayerPrefs.SetInt("WebhooksLogging", Convert.ToInt32(config.Logging));
        }

        if (GUILayout.Button("Reset"))
        {
            PlayerPrefs.DeleteKey("WebhooksUsername");
            PlayerPrefs.DeleteKey("WebhooksLogging");
            
            config.Username = String.Empty;
            config.Logging = false;
        }
    }
}