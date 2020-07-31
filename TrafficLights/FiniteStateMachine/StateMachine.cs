using Newtonsoft.Json;
using System;
using System.IO;
using TrafficLights.Code;

namespace TrafficLights.FiniteStateMachine
{
    public class StateMachine
    {
        private readonly StateItem[,] _items;

        private PelicanStates _currentState;

        private readonly Pelican _pelican;

        public StateMachine(Pelican pelican, string filename)
        {
            _pelican = pelican;

            _items = new StateItem[(int)PelicanActions.Timeout + 1, (int)PelicanStates.Rerequest + 1];
                   
            string json = File.ReadAllText(filename);

            StateItem[] itemList = JsonConvert.DeserializeObject<StateItem[]>(json);

            foreach (var item in itemList)
                AddItem(item);

            _currentState = PelicanStates.PrepareToStart;
            Action(PelicanActions.Timeout);
        }

        public void Action(PelicanActions action)
        {
            StateItem item = _items[(int)action, (int)_currentState];

            if (item == null)
                throw new InvalidOperationException("Pelican is not in a valid state for this action.");

            _pelican.AllOff();

            foreach (PelicanLights l in item.LightsOn)
                _pelican[l].State = LightState.On;

            foreach (PelicanLights l in item.LightsFlashing)
                _pelican[l].State = LightState.Flashing;

            if (item.Timeout != null)
                _pelican.SetTimeout(item.Timeout.Value);

            _currentState = item.NextState;
        }

        private void AddItem(StateItem item)
        {
            _items[(int)item.Action, (int)item.CurrentState] = item;
        }
    }
}
