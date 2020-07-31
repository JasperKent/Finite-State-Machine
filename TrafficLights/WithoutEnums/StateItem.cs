using TrafficLights.Code;

namespace TrafficLights.WithoutEnums
{
    public class StateItem
    {
        public string CurrentState { get; set; }
        public PelicanActions Action { get; set; }

        public string NextState { get; set; }
        public int? Timeout { get; set; }
        public PelicanLights[] LightsOn { get; set; } = { };
        public PelicanLights[] LightsFlashing { get; set; } = { };
    }
}
