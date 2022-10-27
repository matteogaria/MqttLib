
namespace MqttLib.HomeAssistant
{
    public class HassEntityDescriptor
    {
        public string EntityId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public bool UpdateOnStart { get; set; }
        public HassDeviceDescriptor Device { get; set; }
    }
}