
namespace MqttLib.HomeAssistant
{
    public sealed class HassSensor : HassEntity
    {
        private readonly HassDeviceClass sensorType;
        private readonly string measurementUnit;

        public HassSensor(MqttService mqtt, HassEntityDescriptor entity, HassDeviceClass sensorType, string measurementUnit) : base(mqtt, entity, HassComponent.sensor)
        {
            this.sensorType = sensorType;
            this.measurementUnit = measurementUnit;
        }

        protected override DiscoveryMessage GetDiscoveryMessage()
        {
            return new SensorDiscoveryMessage
            {
                MeasurementUnit = measurementUnit,
                DeviceClass = sensorType.ToString()
            };
        }

        public void PublishValue(double value)
            => PublishState(value.ToString("0.##"));
    }
}
