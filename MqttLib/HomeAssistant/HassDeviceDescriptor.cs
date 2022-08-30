using Newtonsoft.Json;

namespace MqttLib.HomeAssistant
{
    public class HassDeviceDescriptor
    {
        [JsonProperty("ids")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sw")]
        public string SoftwareVersion { get; set; }
        [JsonProperty("mdl")]
        public string Model { get; set; }
        [JsonProperty("mf")]
        public string Manufactorer { get; set; }
    }
}