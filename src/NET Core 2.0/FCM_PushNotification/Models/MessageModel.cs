using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FCM_PushNotification.Models
{
    public class MessageModel
    {
        [JsonProperty("to")] public string To { get; set; }

        [JsonProperty("notification")] public Notification Notification { get; set; }

        [JsonProperty("data")] public Dictionary<string, string> Data { get; set; }

        [JsonProperty("priority")] public string Priority { get; set; } = "high"; // "normal

        [JsonProperty("collapse_key")] public string CollapseKey { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty("delay_while_idle")] public bool DelayWhileIdle { get; set; }

        [JsonProperty("time_to_live", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }

        [JsonProperty("content_available")] public bool ContentAvailable { get; set; } = true;
    }
}