using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FCM_PushNotification.Models
{
    public class MessageModel
    {
        [JsonProperty(propertyName: "to")]
        public string To { get; set; }

        [JsonProperty(propertyName: "notification")]
        public Notification Notification { get; set; }

        [JsonProperty(propertyName: "data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonProperty(propertyName: "priority")]
        public string Priority { get; set; } = "high"; // "normal

        [JsonProperty(propertyName: "collapse_key")]
        public string CollapseKey { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty(propertyName: "delay_while_idle")]
        public bool DelayWhileIdle { get; set; }

        [JsonProperty(propertyName: "time_to_live", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }

        [JsonProperty(propertyName: "content_available")]
        public bool ContentAvailable { get; set; } = true;
    }

    public class Notification
    {
        [JsonProperty(propertyName: "body")]
        public string Body { get; set; }

        [JsonProperty(propertyName: "title")]
        public string Title { get; set; }

        [JsonProperty(propertyName: "vibrate")]
        public int[] Vibrate { get; set; } = { 500, 110, 500 };

        [JsonProperty(propertyName: "icon")]
        public string Icon { get; set; }

        [JsonProperty(propertyName: "color")]
        public string Color { get; set; }

        [JsonProperty(propertyName: "sound")]
        public string Sound { get; set; } = "default";

        [JsonProperty(propertyName: "click_action")]
        public string ClickAction { get; set; }

        [JsonProperty(propertyName: "tag")]
        public string Tag { get; set; }

        [JsonProperty(propertyName: "dir")]
        public string Dir { get; set; } = "rtl";
    }
}