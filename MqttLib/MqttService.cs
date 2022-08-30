using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqttLib
{
    public class MqttService
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IManagedMqttClient client;
        public readonly object routeLock = new();
        public static IManagedMqttClient CreateClient(MqttBrokerConfiguration cfg)
        {
            var options = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromMilliseconds(cfg.ReconnectDelay))
              .WithClientOptions(new MqttClientOptionsBuilder()
                  .WithClientId(cfg.ClientId)
                  .WithTcpServer(cfg.IpAddress)
                  .WithCredentials(cfg.Username, cfg.Password)
                  .Build())
              .Build();

            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            mqttClient.StartAsync(options).Wait();
            return mqttClient;
        }

        public Dictionary<string, Action<byte[]>> Routes { get; } = new();
        public MqttService(IManagedMqttClient client)
        {
            this.client = client;
            client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
        }

        public void PublishMessage(string topic, string payload, bool retain = false)
            => Task.Run(() => client.EnqueueAsync(topic, payload, retain: retain));

        public async Task<bool> Register(string route, Action<byte[]> callback)
        {
            lock (routeLock)
            {
                if (Routes.ContainsKey(route))
                    return false;
                Routes.Add(route, callback);
            }

            await client.SubscribeAsync(route);
            return true;
        }

        private async Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            log.Trace($"MQTT message on: {arg.ApplicationMessage.Topic}");

            if (Routes.TryGetValue(arg.ApplicationMessage.Topic, out Action<byte[]> cb))
                await Task.Run(() => cb?.Invoke(arg.ApplicationMessage.Payload));
        }
    }
}