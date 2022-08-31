using System;
using Newtonsoft.Json;
using NLog;

namespace MqttLib.HomeAssistant
{
    public class SampleLight : HassEntity
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public bool HasBrightness { get; set; }
        public int? Brightness { get; protected set; }
        public bool State { get; protected set; }

        public SampleLight(MqttService mqtt, HassEntityDescriptor entity) : base(mqtt, entity, HassComponent.light)
        {
            SubscribeSubTopic("toggle", (_) => Toggle());
        }

        public void Toggle() => SetState(!State, null);

        public void SetState(bool state, int? brightness)
        {
            State = state;
            if (HasBrightness && brightness != null)
                Brightness = brightness;

            LightMessage message = new LightMessage
            {
                State = state ? "ON" : "OFF",
                Brightness = this.Brightness
            };

            PublishState(JsonConvert.SerializeObject(message, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }

        protected override DiscoveryMessage GetDiscoveryMessage()
        {
            return new BrightnessLightDiscoveryMessage
            {
                Brightness = HasBrightness
            };
        }
        protected override void OnSet(string payload)
        {
            LightMessage message = JsonConvert.DeserializeObject<LightMessage>(payload);
            State = message.On;
            Brightness = message.Brightness;

            log.Info($"SET received, State: {State}, Brightness {message.Brightness}");

            // Status update to fake a real light behavior
            PublishState(payload);
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