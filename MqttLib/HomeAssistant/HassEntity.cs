using Newtonsoft.Json;
using System;

namespace MqttLib.HomeAssistant
{
    public class HassEntity : MqttEntity
    {
        private readonly HassComponent component;

        public HassEntity(MqttService mqtt, HassEntityDescriptor entity, HassComponent component) : base(mqtt, entity.Route)
        {
            if (entity == null || entity.Device == null)
                throw new ArgumentException("Entity and Entity.Device cannot be null", nameof(entity));

            Entity = entity;
            this.component = component;
            SubscribeSubTopic("set", (p) =>
            {
                log.Info($"Set msg: {p}");
                OnSet(p);
            });
        }

        public HassEntityDescriptor Entity { get; }

        public void PublishDiscovery(string topic)
        {
            DiscoveryMessage message = GetDiscoveryMessage();
            message.Name = Entity.Name;
            message.UniqueId = Entity.EntityId;
            message.CommandTopic = $"{Entity.Route}/set";
            message.StateTopic = $"{Entity.Route}/state";
            message.Schema = "json";

            message.Device = Entity.Device;

            JsonSerializerSettings settings = new();
            settings.NullValueHandling = NullValueHandling.Ignore;

            string json = JsonConvert.SerializeObject(message, settings);
            Mqtt.PublishMessage($"{topic}/{GetComponentTopic()}/{Entity.Device.Id}/{Entity.EntityId}/config", json, retain: true);

            string GetComponentTopic()
                => component switch
                {
                    HassComponent.lock_ => "lock",
                    HassComponent.switch_ => "switch",
                    _ => component.ToString()
                };
        }

        protected void PublishState(string payload) => PublishEntityMessage("state", payload);
        protected virtual DiscoveryMessage GetDiscoveryMessage() => new();
        protected virtual void OnSet(string payload) { }
    }
}