using MqttLib.HomeAssistant;
using MQTTnet.Extensions.ManagedClient;

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
            HassEntity entity = new HassEntity(
                service,
                device,
                new HassEntityDescriptor
                {
                    EntityId = "mqttLib_sample_",
                    Name = "mqttlib sample ",
                    Publish = true,
                    Route = "mqttlib/sample"
                },
                HassComponent.light);

            Console.ReadLine();

        }
    }
}