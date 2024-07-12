// Copyright 2024, Logan, All rights reserved.

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

    public static void SendMessage()
    {
        if (_config == null)
        {
            Debug.LogWarning($"{nameof(EditorDiscordWebhooksEditor)} asset was not found, it must have been created.");
            ShowConfig();
        }

        if (_config.WebhooksAPI == null || _config.WebhooksAPI.Length == 0)
        {
            Debug.LogError("Please insert the Webhooks of your Discord server.");
            ShowConfig();
            return;
        }

        IEnumerator RequestWebhooksAPI()
        {
            string nameForMessage;
            if (_config.Name == null || _config.Name.Length == 0)
            {
                nameForMessage = "a member of the team";
            }
            else
            {
                nameForMessage = _config.Name;
            }

            if (_note.Length > 0)
            {
                _note = $"\n> **Note :** {_note}.\n";
            }

            WWWForm requestContent = new();
            requestContent.AddField("content",
                $"@here **__{nameForMessage}__** needs a help.{_note}\n*If you take care of it please put the reaction :white_check_mark: and delete the message after the request is made.*");
            using (UnityWebRequest request = UnityWebRequest.Post(_config.WebhooksAPI, requestContent))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Webrequest Error : {request.error}.");
                }
                else
                {
                    if (_config.Logging)
                    {
                        Debug.Log("Your message has been sent.");
                    }
                }
            }

            _note = null;
        }
        
        EditorCoroutineUtility.StartCoroutineOwnerless(RequestWebhooksAPI());
        
    }
    
    public static void ShowConfig()
    {
        FetchConfig();
        string path = GetConfigPath();
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<EditorDiscordWebhooksConfig>(path).GetInstanceID());
    }

    [MenuItem("Window/Tools/Editor Discord Webhooks", false, int.MaxValue)]
    public static void ShowWindows()
    {
        EditorWindow win = GetWindow<EditorDiscordWebhooksEditor>("Editor Discord Webhooks");

        Vector2 size = new(400, 100);
        
        win.maxSize = size;
        win.minSize = size;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        if (GUILayout.Button("Show Config"))
        {
            ShowConfig();
        }
        
        EditorGUILayout.Separator();
        _note = EditorGUILayout.TextField("Note :", _note);
        
        if (GUILayout.Button("Send Message"))
        {
            SendMessage();
        }
    }

    [InitializeOnLoadMethod]
    private static void OnInitialize()
    {
        FetchConfig();
    }

    private static void FetchConfig()
    {
        while (true)
        {
            if (_config != null) return;
            string path = GetConfigPath();
            if (path == null)
            {
                AssetDatabase.CreateAsset(CreateInstance<EditorDiscordWebhooksConfig>(), 
                    $"Assets/{nameof(EditorDiscordWebhooksConfig)}.asset");
                continue;
            }

            _config = AssetDatabase.LoadAssetAtPath<EditorDiscordWebhooksConfig>(path);
            break;
        }
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
}
