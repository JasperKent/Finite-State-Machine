using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TrafficLights.Code;

namespace TrafficLights.FiniteStateMachine
{
    public class StateItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PelicanStates CurrentState { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PelicanActions Action { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PelicanStates NextState { get; set; }

        public int? Timeout { get; set; }

        [JsonProperty("LightsOn", ItemConverterType = typeof(StringEnumConverter))]
        public PelicanLights[] LightsOn { get; set; } = { };

        [JsonProperty("LightsFlashing", ItemConverterType = typeof(StringEnumConverter))]
        public PelicanLights[] LightsFlashing { get; set; } = { };
    }
}
