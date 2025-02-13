// Copyright 2025, Logan.dlp, All rights reserved.

#if UNITY_EDITOR

using UnityEngine;

public class EditorDiscordWebhooksConfig : ScriptableObject
{
    public string Username { get; set; }
    public bool Logging { get; set; }
    public string WebhooksAPI { get; set; }
}

#endif