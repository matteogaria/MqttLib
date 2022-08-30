using System;

namespace MqttLib.HomeAssistant
{
    public class LightComponent : HassEntity
    {
        public LightComponent(MqttService mqtt, HassDeviceDescriptor device, HassEntityDescriptor entity, HassComponent component, string discoveryTopic = "homeassistant") : base(mqtt, device, entity, component, discoveryTopic)
        {
            SubscribeSubTopic("toggle", (_) => OnToggle?.Invoke());
        }

        public event Action<bool> OnSetReceived;
        public event Action OnToggle;

        public void SetState(bool state) => SetState(state ? "ON" : "OFF");

        protected override void OnSet(string payload)
        {
            OnSetReceived?.Invoke(payload == "ON");
        }

    }
}
