// Copyright 2024, Logan.dlp, All rights reserved.

using System;
using UnityEditor;
using UnityEngine;

public class EditorDiscordWebhooksConfig : ScriptableObject
{
    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }
    private string _username;

    public bool Logging
    {
        get { return _logging; }
        set { _logging = value; }
    }
    private bool _logging;
    
    public string WebhooksAPI;
    public string DiscordMention;
}