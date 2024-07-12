using Unity.Collections;
using UnityEngine;

public class EditorDiscordWebhooksConfig : ScriptableObject
{
    [WriteOnly]
    public string Name;
    [WriteOnly]
    public string WebhooksAPI;
    [WriteOnly]
    public bool Logging = true;
}
