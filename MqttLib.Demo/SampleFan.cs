
using NLog;

namespace MqttLib.HomeAssistant
{
    public class SampleFan : HassEntity
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        public bool State { get; protected set; }

        public SampleFan(MqttService mqtt, HassEntityDescriptor entity) : base(mqtt, entity, HassComponent.fan)
        {
            SubscribeTopic(BaseTopic + "/toggle", (_) => Toggle());
        }

        public void Toggle() => SetState(!State);

        public void SetState(bool state)
        {
            State = state;
            log.Info($"Fan state: {State}");
            PublishState(State ? "ON" : "OFF");
        }

        protected override void OnSet(string payload)
        {
            bool on = payload == "ON";
            log.Debug($"SET received: {payload}");
            SetState(on);
        }
    }
}