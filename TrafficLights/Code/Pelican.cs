using System;
using System.Windows.Threading;
using TrafficLights.FiniteStateMachine;
//using TrafficLights.WithoutEnums;

namespace TrafficLights.Code
{
    public enum PelicanActions { Press, Timeout }
    public enum PelicanStates { GoIdle, GoWaiting, PrepareToStop, Stop, PrepareToStart, Rerequest }
    public enum PelicanLights { Red, Amber, Green, RedFigure, GreenFigure, Wait }

    public class Pelican
    {
        public const int WaitTime = 2000;
        public const int StoppingTime = 2000;
        public const int StoppedTime = 4000;
        public const int StartingTime = 4000;

        private readonly Light[] _lights;

        private readonly StateMachine _machine;

        private readonly DispatcherTimer _timer;

        public Pelican()
        {
            _lights = new Light[] { new Light(), new Light(), new Light(), new Light(), new Light(), new Light() };

            _machine = new StateMachine(this, "machine.json");

            _timer = new DispatcherTimer();
            _timer.Tick += (s, e) =>
            {
                _timer.Stop();
                Timeout();
            };
        }

        public Light this[PelicanLights li] => _lights[(int)li];

        public void AllOff()
        {
            foreach (Light l in _lights)
                l.State = LightState.Off;
        }

        public void Press() => _machine.Action(PelicanActions.Press);
        public void Timeout() => _machine.Action(PelicanActions.Timeout);

        public void SetTimeout(int waitTime)
        {
            _timer.Interval = new TimeSpan(0, 0, 0, 0, waitTime);
            _timer.Start();
        }
    }
}
