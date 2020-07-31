using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TrafficLights.Code
{
    public enum LightState { Off, On, Flashing}

    public class Light
    {
        private const int FlashTime = 250;

        private LightState _state = LightState.Off;

        public LightState State
        {
            get => _state;
            set
            {
                _state = value;
                SetLight();
            }
        }

        private Image _image;

        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                SetLight();
            }
        }

        private DispatcherTimer _timer;

        private void SetLight()
        {
            if (_image != null)
            {
                StopFlashing();

                switch (_state)
                {
                    case LightState.Off:
                        _image.Visibility = Visibility.Hidden;
                        break;
                    case LightState.On:
                        _image.Visibility = Visibility.Visible;
                        break;
                    case LightState.Flashing:
                        StartFlashing();
                        break;
                }
            }
        }

        private void StopFlashing()
        {
           if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        private void StartFlashing()
        {
            if (_timer == null)
            {
                _image.Visibility = Visibility.Visible;

                bool flashOn = true;

                _timer = new DispatcherTimer(DispatcherPriority.Render);
                _timer.Interval = new TimeSpan(0, 0, 0, 0, FlashTime);               
                _timer.Tick += (s, e) =>
                {
                    _image.Visibility = flashOn ? Visibility.Visible : Visibility.Hidden;
                    flashOn = !flashOn;
                };

                _timer.Start();
            }
        }

    }
}
