using NLog;
using System;
using System.Text;

namespace MqttLib
{
    public class MqttEntity
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        protected MqttService Mqtt { get; }
        public string Topic { get; }

        public MqttEntity(MqttService mqtt, string topic)
        {
            Mqtt = mqtt;
            Topic = topic;
        }

        protected void SubscribeSubTopic(string subTopic, Action<string> callback)
            => Mqtt.Register($"{Topic}/{subTopic}", (payload) => callback?.Invoke(Encoding.UTF8.GetString(payload))).Wait();

        protected void PublishEntityMessage(string subTopic, string content)
            => Mqtt.PublishMessage($"{Topic}/{subTopic}", content);
    }
}