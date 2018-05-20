using Newtonsoft.Json;

namespace FCM_PushNotification.Models
{
    public class Notification
    {
        [JsonProperty("body")] public string Body { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("vibrate")] public int[] Vibrate { get; set; } = {500, 110, 500};

        [JsonProperty("icon")] public string Icon { get; set; }

        [JsonProperty("color")] public string Color { get; set; }

        [JsonProperty("sound")] public string Sound { get; set; } = "default";

        [JsonProperty("click_action")] public string ClickAction { get; set; }

        [JsonProperty("tag")] public string Tag { get; set; }

        [JsonProperty("dir")] public string Dir { get; set; } = "rtl";
    }
}