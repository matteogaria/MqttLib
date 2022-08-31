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
                IpAddress = "10.39.24.50",
                Password = "device",
                Username = "device",
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

            SampleLight lightWithBrightness = new SampleLight(
                service,
                new HassEntityDescriptor
                {
                    EntityId = "mqttLib_sample_brightnesslight",
                    Name = "mqttlib sample brightnesslight ",
                    Route = "mqttlib/sample",
                    Device = device
                });

            lightWithBrightness.HasBrightness = true;
            lightWithBrightness.PublishDiscovery("homeassistant");
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
                        lightWithBrightness.Toggle();
                        break;
                    case ConsoleKey.D3:
                        lightWithBrightness.SetState(lightWithBrightness.State, lightWithBrightness.Brightness - 5);
                        break;
                    case ConsoleKey.D4:
                        lightWithBrightness.SetState(lightWithBrightness.State, lightWithBrightness.Brightness + 5);
                        break;
                }
            }

        }
    }
}