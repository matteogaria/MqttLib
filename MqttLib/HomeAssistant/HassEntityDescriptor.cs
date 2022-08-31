
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

    public enum HassComponent
    {

        alarm_control_panel,
        binary_sensor,
        button,
        camera,
        climate,
        cover,
        device_automation,
        device_tracker,
        fan,
        humidifier,
        light,
        lock_,
        number,
        scene,
        siren,
        select,
        sensor,
        switch_,
        tag,
        vacuum
    }
}