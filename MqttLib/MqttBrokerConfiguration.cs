﻿namespace MqttLib
{
    public class MqttBrokerConfiguration
    {
        public string ClientId { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ReconnectDelay { get; set; }
    }
}