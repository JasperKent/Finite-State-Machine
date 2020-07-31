using System;
using System.Linq;
using TrafficLights.Code;
using TrafficLights.FiniteStateMachine;
using Xunit;

namespace TrafficLights.test
{
    public class PelicanTests
    {
        private readonly Pelican _pelican;

        public PelicanTests()
        {
            _pelican = new Pelican();
        }

        private void AssertLights(params (PelicanLights light, LightState state)?[] lights)
        {
            foreach (PelicanLights? light in Enum.GetValues(typeof(PelicanLights)).Cast<PelicanLights?>())
            {
                var lit = lights.SingleOrDefault(l => l?.light == light);

                if (lit == null)
                    Assert.Equal(LightState.Off, _pelican[light.Value].State);
                else
                    Assert.Equal(lit.Value.state, _pelican[light.Value].State);
            }
        }

        [Fact]
        public void Creation()
        {
            Assert.NotNull(_pelican);
        }

        [Fact]
        public void InitialState()
        {
            AssertLights((PelicanLights.Green, LightState.On), (PelicanLights.RedFigure, LightState.On));
        }

        [Fact]
        public void Press()
        {
            _pelican.Press();

            AssertLights((PelicanLights.Green, LightState.On), (PelicanLights.RedFigure, LightState.On), (PelicanLights.Wait, LightState.On));
        }

        [Fact]
        public void Stopping()
        {
            _pelican.Press();
            _pelican.Timeout();

            AssertLights((PelicanLights.Amber, LightState.On), (PelicanLights.RedFigure, LightState.On), (PelicanLights.Wait, LightState.On));
        }

        [Fact]
        public void Stopped()
        {
            _pelican.Press();
            _pelican.Timeout();
            _pelican.Timeout();

            AssertLights((PelicanLights.Red, LightState.On), (PelicanLights.GreenFigure, LightState.On));
        }

        [Fact]
        public void Starting()
        {
            _pelican.Press();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Timeout();

            AssertLights((PelicanLights.Amber, LightState.Flashing), (PelicanLights.GreenFigure, LightState.Flashing));
        }

        [Fact]
        public void Started()
        {
            _pelican.Press();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Timeout();

            AssertLights((PelicanLights.Green, LightState.On), (PelicanLights.RedFigure, LightState.On));
        }

        [Fact]
        public void Rerequesting()
        {
            _pelican.Press();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Press();

            AssertLights((PelicanLights.Amber, LightState.Flashing), (PelicanLights.GreenFigure, LightState.Flashing), (PelicanLights.Wait, LightState.On));
        }

        [Fact]
        public void Rerequested()
        {
            _pelican.Press();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Timeout();
            _pelican.Press();
            _pelican.Timeout();

            AssertLights((PelicanLights.Green, LightState.On), (PelicanLights.RedFigure, LightState.On), (PelicanLights.Wait, LightState.On));
        }

        [Fact]
        public void InvalidTimeout()
        {
            Assert.Throws<InvalidOperationException>(() => {
                _pelican.Timeout();
            });
        }
    }
}
