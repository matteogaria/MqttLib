using Newtonsoft.Json;

namespace MqttLib.HomeAssistant
{
    public class HassEntity : MqttEntity
    {
        private readonly HassComponent component;

        public HassEntity(MqttService mqtt, HassDeviceDescriptor device, HassEntityDescriptor entity, HassComponent component) : base(mqtt, entity.Route)
        {
            Device = device;
            Entity = entity;
            this.component = component;
            SubscribeSubTopic("set", (p) =>
            {
                log.Info($"Set msg: {p}");
                OnSet(p);
            });
        }

        public HassDeviceDescriptor Device { get; }
        public HassEntityDescriptor Entity { get; }

        public void PublishDiscovery(string topic)
        {
            DiscoveryMessage message = GetDiscoveryMessage();
            message.Name = Entity.Name;
            message.UniqueId = Entity.EntityId;
            message.CommandTopic = $"{Entity.Route}/set";
            message.StateTopic = $"{Entity.Route}/state";
            message.Schema = "json";

            message.Device = Device;

            JsonSerializerSettings settings = new();
            settings.NullValueHandling = NullValueHandling.Ignore;

            string json = JsonConvert.SerializeObject(message, settings);
            Mqtt.PublishMessage($"{topic}/{GetComponentTopic()}/{Device.Id}/{Entity.EntityId}/config", json, retain: true);

            string GetComponentTopic()
                => component switch
                {
                    HassComponent.lock_ => "lock",
                    HassComponent.switch_ => "switch",
                    _ => component.ToString()
                };
        }

        protected void SetState(string payload) => PublishEntityMessage("state", payload);
        protected virtual DiscoveryMessage GetDiscoveryMessage() => new();
        protected virtual void OnSet(string payload) { }


    }
}