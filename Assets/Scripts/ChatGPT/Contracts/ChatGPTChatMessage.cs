using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class ChatGPTChatMessage
{
    [field: SerializeField]
    [JsonProperty("role")]
    public string Role { get; set; }

    [field: SerializeField]
    [JsonProperty("content")]
    public string Content { get; set; }
}