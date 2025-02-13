// Copyright 2025, Logan.dlp, All rights reserved.

#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorDiscordWebhooksConfig))]
public class EditorDiscordWebhooksConfigEditor : Editor
{
    private SerializedObject _serializedObjectTarget;

    private void OnEnable()
    {
        _serializedObjectTarget = new(target);
    }

    public override void OnInspectorGUI()
    {
        _serializedObjectTarget.Update();
        
        EditorDiscordWebhooksConfig config = (EditorDiscordWebhooksConfig)target;
        config.Username = EditorGUILayout.TextField("Username", config.Username);

        config.WebhooksAPI = EditorGUILayout.TextField("Webhooks API", config.WebhooksAPI);

        if (_serializedObjectTarget.ApplyModifiedProperties())
        {
            EditorUtility.SetDirty((target));
        }
        
        config.Logging = EditorGUILayout.Toggle("Logging", config.Logging);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Save"))
        {
            if (config.Username.Length != 0)
            {
                PlayerPrefs.SetString("WebhooksUsername", config.Username);
            }
            
            PlayerPrefs.SetInt("WebhooksLogging", Convert.ToInt32(config.Logging));

            if (config.WebhooksAPI.Length != 0)
            {
                PlayerPrefs.SetString("WebhooksAPI", config.WebhooksAPI);
            }

            if (config.Logging)
            {
                Debug.Log("Your parameter as been saved.");
            }
        }

        if (GUILayout.Button("Reset"))
        {
            if (PlayerPrefs.HasKey("WebhooksLogging") && PlayerPrefs.GetInt("WebhooksLogging") == 1)
            {
                Debug.Log("Your parameter as been reset.");
            }
            
            PlayerPrefs.DeleteKey("WebhooksUsername");
            PlayerPrefs.DeleteKey("WebhooksLogging");
            PlayerPrefs.DeleteKey("WebhooksAPI");
            
            config.Username = String.Empty;
            config.Logging = false;
            config.WebhooksAPI = String.Empty;
        }
        
        GUILayout.EndHorizontal();
    }
}

#endif