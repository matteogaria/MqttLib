using MqttLib.HomeAssistant;
using MQTTnet.Extensions.ManagedClient;
using System;

namespace MqttLib.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IManagedMqttClient mqtt = MqttService.CreateClient(new MqttBrokerConfiguration
            {
                ClientId = "mqttLib.Demo",
                IpAddress = "10.39.24.10",
                Password = "esphome",
                Username = "esphome",
                ReconnectDelay = 5000
            });

            MqttService service = new MqttService(mqtt);

            var device = new HassDeviceDescriptor
            {
                Id = "mqttLib_sample_device",
                Manufactorer = "m.g. Inc.",
                Model = "mqttLib.Demo",
                Name = "mqtt lib sample device",
                SoftwareVersion = "0.0.1"
            };

            SampleLight light = new SampleLight(
                service,
                new HassEntityDescriptor
                {
                    EntityId = "mqttLib_sample_light",
                    Name = "mqttlib sample light",
                    Route = "mqttlib/sample",
                    Device = device
                });

            light.HasBrightness = false;
            light.PublishDiscovery("homeassistant");

            SampleFan fan = new SampleFan(
                service,
                new HassEntityDescriptor
                {
                    EntityId = "mqttlib_sample_fan",
                    Name = "mqttlib samble fan",
                    Route = "mqttlib/fan",
                    Device = device
                });
            fan.PublishDiscovery("homeassistant");

            bool run = true;
            while (run)
            {
                ConsoleKey k = Console.ReadKey().Key;
                switch(k)
                {
                    case ConsoleKey.Q:
                        run = false;
                        break;
                    case ConsoleKey.D1:
                        light.Toggle();
                        break;
                    case ConsoleKey.D2:
                        fan.Toggle();
                        break;
                }
            }

        }
    }
}