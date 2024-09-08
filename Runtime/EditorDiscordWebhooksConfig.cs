// Copyright 2024, Logan, All rights reserved.

using UnityEngine;

public class EditorDiscordWebhooksConfig : ScriptableObject
{
    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }
    private string _username;

    
    public string WebhooksAPI;
    public bool Logging = true;
}
