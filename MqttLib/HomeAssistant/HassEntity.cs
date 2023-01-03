using System;

using Newtonsoft.Json;

namespace MqttLib.HomeAssistant
{
    public class HassEntity : MqttEntity
    {
        private readonly HassComponent component;
        public string BaseTopic { get; }
        public string CommandTopic { get; }
        public string StateTopic { get; }
        public HassEntity(MqttService mqtt, HassEntityDescriptor entity, HassComponent component) : base(mqtt)
        {
            if (entity == null || entity.Device == null)
                throw new ArgumentException("Entity and Entity.Device cannot be null", nameof(entity));

            BaseTopic = $"{entity.Route}/{entity.EntityId}";
            CommandTopic = BaseTopic + "/set";
            StateTopic = BaseTopic + "/state";

            Entity = entity;
            this.component = component;
            SubscribeTopic(CommandTopic, (p) =>
            {
                log.Debug($"Set msg: {p}");
                OnSet(p);
            });

            SubscribeTopic(StateTopic, (p) =>
            {
                log.Debug($"State msg: {p}");
                OnState(p);
            });
        }

        public HassEntityDescriptor Entity { get; }

        public void PublishDiscovery(string topic)
        {
            DiscoveryMessage message = GetDiscoveryMessage();
            message.Name = Entity.Name;
            message.UniqueId = Entity.EntityId;
            message.CommandTopic = CommandTopic;
            message.StateTopic = StateTopic;
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

        protected void PublishState(string payload) => PublishMessage(StateTopic, payload, true);
        protected virtual DiscoveryMessage GetDiscoveryMessage() => new();
        protected virtual void OnSet(string payload) { }
        protected virtual void OnState(string payload) { }
    }
}