using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TrafficLights.Code;

namespace TrafficLights.WithoutEnums
{
    public class StateMachine
    {
        private readonly Dictionary<string, StateItem>[] _items = { new Dictionary<string, StateItem>(), new Dictionary<string, StateItem>() };

        private string _currentState;

        private readonly Pelican _pelican;

        public StateMachine(Pelican pelican, string filename)
        {
            _pelican = pelican;

            string json = File.ReadAllText(filename);

            StateItem[] itemList = JsonConvert.DeserializeObject<StateItem[]>(json);

            foreach (var item in itemList)
                AddItem(item);

            _currentState = "PrepareToStart";
            Action(PelicanActions.Timeout);
        }

        public void Action(PelicanActions action)
        {
            if (!_items[(int)action].ContainsKey(_currentState))
                throw new InvalidOperationException("Pelican is not in a valid state for this action.");
            else
            {
                StateItem item = _items[(int)action][_currentState];

                _pelican.AllOff();

                foreach (PelicanLights l in item.LightsOn)
                    _pelican[l].State = LightState.On;

                foreach (PelicanLights l in item.LightsFlashing)
                    _pelican[l].State = LightState.Flashing;

                if (item.Timeout != null)
                    _pelican.SetTimeout(item.Timeout.Value);

                _currentState = item.NextState;
            }
        }

        private void AddItem(StateItem item)
        {
            _items[(int)item.Action].Add(item.CurrentState, item);
        }
    }
}
