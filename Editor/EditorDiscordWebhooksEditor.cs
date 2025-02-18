// Copyright 2025, Logan.dlp, All rights reserved.

#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;

public class EditorDiscordWebhooksEditor : EditorWindow
{
    private static EditorDiscordWebhooksConfig _config;
    private static string _note;
    
    public static void ShowConfig()
    {
        FetchConfig();
        string path = GetConfigPath();
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<EditorDiscordWebhooksConfig>(path).GetInstanceID());
    }

    [MenuItem("Tools/Discord Webhooks", false, int.MaxValue)]
    public static void ShowWindows()
    {
        EditorWindow window = GetWindow<EditorDiscordWebhooksEditor>("Discord Webhooks");

        Vector2 size = new(400, 150);
        
        window.maxSize = size;
        window.minSize = size;
        
        window.Show();
    }

    [InitializeOnLoadMethod]
    private static void OnInitialize()
    {
        ShowConfig();
    }

    private static void FetchConfig()
    {
        if (_config != null) return;
        string path = GetConfigPath();
        if (path == null)
        {
            AssetDatabase.CreateAsset(CreateInstance<EditorDiscordWebhooksConfig>(), 
                $"Assets/{nameof(EditorDiscordWebhooksConfig)}.asset");
        }

        _config = AssetDatabase.LoadAssetAtPath<EditorDiscordWebhooksConfig>(path);
    }

    private static string GetConfigPath()
    {
        List<string> pathList = AssetDatabase.FindAssets(nameof(EditorDiscordWebhooksConfig))
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(c => c.EndsWith(".asset"))
            .ToList();
        if (pathList.Count > 1) Debug.LogWarning($"Multiple {nameof(EditorDiscordWebhooksConfig)} assets found. Delete one.");
        return pathList.FirstOrDefault();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Note :");
        _note = EditorGUILayout.TextArea(_note, GUILayout.Height(70));
        
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        Color baseColor = GUI.backgroundColor;

        GUI.backgroundColor = new Color(0.6196078431f, 0.1647058824f, 0.1647058824f, 1);
        
        if (GUILayout.Button("Show Config"))
        {
            ShowConfig();
        }
        
        GUI.backgroundColor = baseColor;
        
        if (GUILayout.Button("Send Message"))
        {
            SendMessage();
        }
        
        EditorGUILayout.EndHorizontal();
        
    }
    
    public static void SendMessage()
    {
        if (_config == null)
        {
            Debug.LogWarning($"{nameof(EditorDiscordWebhooksEditor)} asset was not found, it must have been created.");
            ShowConfig();
        }

        if (!PlayerPrefs.HasKey("WebhooksAPI"))
        {
            Debug.LogError("Please insert the Webhooks of your Discord server.");
            ShowConfig();
            return;
        }
        
        string autorMessage;
        if (!PlayerPrefs.HasKey("WebhooksUsername") || PlayerPrefs.GetString("WebhooksUsername").Length == 0)
        {
            autorMessage = "a member of the team";
        }
        else
        {
            autorMessage = PlayerPrefs.GetString("WebhooksUsername");
        }

        if (_note != null && _note.Length > 0)
        {
            _note = $"\n>>> **__Note :__**\n```fix\n{_note}\n```";
        }
        else
        {
            _note = "\n>>> ";
        }

        IEnumerator RequestWebhooksAPI()
        {
            WWWForm requestContent = new();
            requestContent.AddField("content", $"@here **__{autorMessage}__** needs a help.{_note}\n-# *If you take care of it please put the reaction :white_check_mark: and delete the message after the request is made.*");
            using (UnityWebRequest request = UnityWebRequest.Post(PlayerPrefs.GetString("WebhooksAPI"), requestContent))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Webrequest Error : {request.error}.");
                }
                else
                {
                    if (Convert.ToBoolean(PlayerPrefs.GetInt("WebhooksLogging")))
                    {
                        Debug.Log("Your message has been sent.");
                    }
                }
            }
            _note = null;
        }
        
        EditorCoroutineUtility.StartCoroutineOwnerless(RequestWebhooksAPI());
    }
}

#endif