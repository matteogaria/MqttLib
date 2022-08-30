using Newtonsoft.Json;
using System;

namespace MqttLib.HomeAssistant
{
    public class LightComponent : HassEntity
    {
        private readonly bool brightness;

        public LightComponent(MqttService mqtt, HassDeviceDescriptor device, HassEntityDescriptor entity, HassComponent component, bool brightness) : base(mqtt, device, entity, component)
        {
            SubscribeSubTopic("toggle", (_) => OnToggle?.Invoke());
            this.brightness = brightness;
        }

        public event Action<bool, int> OnSetReceived;
        public event Action OnToggle;

        public void SetState(bool state, int brightness)
        {
            LightMessage message = new LightMessage
            {
                State = state ? "ON" : "OFF",
                Brightness = brightness
            };

            SetState(JsonConvert.SerializeObject(message));
        }

        protected override DiscoveryMessage GetDiscoveryMessage()
        {
            return new BrightnessLightDiscoveryMessage
            {
                Brightness = brightness
            };
        }
        protected override void OnSet(string payload)
        {
            LightMessage message = JsonConvert.DeserializeObject<LightMessage>(payload);
            OnSetReceived?.Invoke(message.On, message.Brightness ?? 255);
            SetState(payload);
        }

        protected class LightMessage
        {
            [JsonProperty("state")]
            public string State { get; set; }

            [JsonProperty("brightness")]
            public int? Brightness { get; set; }

            [JsonIgnore]
            public bool On { get => State.ToLower() == "on"; set => State = value ? "ON" : "OFF"; }
        }

    }
}
