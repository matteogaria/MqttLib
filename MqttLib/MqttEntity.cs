using System;
using System.Text;

using NLog;

namespace MqttLib
{
    public class MqttEntity
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        protected MqttService Mqtt { get; }

        public MqttEntity(MqttService mqtt)
        {
            Mqtt = mqtt;
        }

        protected void SubscribeTopic(string topic, Action<string> callback)
           => Mqtt.Register(topic, (payload) => callback?.Invoke(Encoding.UTF8.GetString(payload))).Wait();

        protected void PublishMessage(string topic, string content)
            => Mqtt.PublishMessage(topic, content);
    }
}