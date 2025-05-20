using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using System.Timers;

namespace SensorTester.Pages;

public partial class SensorsPage : ContentPage
{

    public SensorsPage()
	{
		InitializeComponent();
    }

    #region Accelerometer
    private void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        Dispatcher.Dispatch(() => {
            TextAccelerometer.Text = "X: " + e.Reading.Acceleration.X.ToString("0.00") + "\n" + 
                                     "Y: " + e.Reading.Acceleration.Y.ToString("0.00") + "\n" + 
                                     "Z: " + e.Reading.Acceleration.Z.ToString("0.00");
        });

    }
    private void Accelerometer_ShakeDetected(object? sender, EventArgs e)
    {
        if (CheckBoxShakeDetect.IsChecked)
        {
            Toast.Make("Shake detected!", ToastDuration.Long).Show();

            ShakeTask ??= Task.Run(() => {
                
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                Thread.Sleep(200);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                Thread.Sleep(500);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                Thread.Sleep(200);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                Thread.Sleep(500);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                Thread.Sleep(200);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                Thread.Sleep(500);
                ShakeTask = null;
            });
            
        }

    }
    private void SwitchAccelerometer_Toggled(object sender, ToggledEventArgs e)
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring && SwitchAccelerometer.IsToggled)
            {
                // Turn on accelerometer
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                Accelerometer.Default.Start(SensorSpeed.UI);

            }
            else if (Accelerometer.Default.IsMonitoring && !SwitchAccelerometer.IsToggled)
            {
                // Turn off accelerometer
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                TextAccelerometer.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchAccelerometer.IsToggled = Accelerometer.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Accelerometer not supported!").Show();
            SwitchAccelerometer.IsToggled = false;
        }
    }
    #endregion

    #region Barometer
    private void Barometer_ReadingChanged(object? sender, BarometerChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextBarometer.Text = "Pressure: " + e.Reading.PressureInHectopascals.ToString("0.00") + " hPa\n";
        });
    }
    private void SwitchBarometer_Toggled(object sender, ToggledEventArgs e)
    {
        if (Barometer.Default.IsSupported)
        {
            if (!Barometer.Default.IsMonitoring && SwitchBarometer.IsToggled)
            {
                // Turn on barometer
                Barometer.Default.ReadingChanged += Barometer_ReadingChanged;
                Barometer.Default.Start(SensorSpeed.UI);
            }
            else if (Barometer.Default.IsMonitoring && !SwitchBarometer.IsToggled)
            {
                // Turn off barometer
                Barometer.Default.Stop();
                Barometer.Default.ReadingChanged -= Barometer_ReadingChanged;
                TextBarometer.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchBarometer.IsToggled = Barometer.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Barometer not supported!").Show();
            SwitchBarometer.IsToggled = false;
        }
    }
    #endregion

    #region Compass
    private void Compass_ReadingChanged(object? sender, CompassChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextCompass.Text = "Heading: " + e.Reading.HeadingMagneticNorth.ToString("0.00") + "°\n";
        });
    }
    private void SwitchCompass_Toggled(object sender, ToggledEventArgs e)
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring && SwitchCompass.IsToggled)
            {
                // Turn on compass
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
            }
            else if (Compass.Default.IsMonitoring && !SwitchCompass.IsToggled)
            {
                // Turn off compass
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                TextCompass.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchCompass.IsToggled = Compass.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Compass not supported!").Show();
            SwitchCompass.IsToggled = false;
        }
    }
    #endregion

    #region Geolocation
    private void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        
        Dispatcher.Dispatch(() =>
        {
            TextGeolocation.Text = "Timestamp: " + e.Location.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
                        "Latitude: " + e.Location.Latitude.ToString("0.00") + "\n" +
                       "Longitude: " + e.Location.Longitude.ToString("0.00") + "\n";
            if (e.Location.Altitude.HasValue)
            {
                TextGeolocation.Text += "Altitude: " + e.Location.Altitude.Value.ToString("0.00") + " m\n";

            }
            if (e.Location.Course.HasValue)
            {
                TextGeolocation.Text += "Course: " + e.Location.Course.Value.ToString("0.00") + "°\n";
            }
            if (e.Location.Speed.HasValue)
            {
                TextGeolocation.Text += "Speed: " + e.Location.Speed.Value.ToString("0.00") + " m/s\n";
            }
            if (e.Location.Accuracy.HasValue)
            {
                TextGeolocation.Text += "Horizontal Accuracy: " + e.Location.Accuracy.Value.ToString("0.00") + " m\n";
            }
            if (e.Location.VerticalAccuracy.HasValue)
            {
                TextGeolocation.Text += "Vertical Accuracy: " + e.Location.VerticalAccuracy.Value.ToString("0.00") + " m\n";
            }
            TextGeolocation.Text += "Is Accuracy Reduced: " + e.Location.ReducedAccuracy + "\n";
            TextGeolocation.Text += "Is from mock provider: " + e.Location.IsFromMockProvider.ToString() + "\n";
        });
    }
    private void SwitchGeolocation_Toggled(object sender, ToggledEventArgs e)
    {
        if (!Geolocation.Default.IsListeningForeground && SwitchGeolocation.IsToggled)
        {
            // Turn on geolocation
            Geolocation.Default.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(2)));
            GeolocationTimer.Elapsed += GeolocationTimer_Elapsed;
            GeolocationTimer.Start();
            //Geolocation.Default.LocationChanged += Geolocation_LocationChanged;
        }
        else if (Geolocation.Default.IsListeningForeground && !SwitchGeolocation.IsToggled)
        {
            // Turn off geolocation
            Geolocation.Default.StopListeningForeground();
            GeolocationTimer.Elapsed -= GeolocationTimer_Elapsed;
            GeolocationTimer.Stop();
            //Geolocation.Default.LocationChanged -= Geolocation_LocationChanged;
            TextGeolocation.Text = "--";
        }
        else
        {
            Toast.Make("Something went wrong!").Show();
            SwitchGeolocation.IsToggled = Geolocation.Default.IsListeningForeground;
        }
    }
    private void GeolocationTimer_Elapsed(object? sender, ElapsedEventArgs e1)
    {
        Location? location = Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1))).Result;
        if(location == null)
        {
            return;
        }
        Dispatcher.Dispatch(() =>
        {
            TextGeolocation.Text = "Timestamp: " + location.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
                        "Latitude: " + location.Latitude.ToString("0.00") + "\n" +
                       "Longitude: " + location.Longitude.ToString("0.00") + "\n";
            if (location.Altitude.HasValue)
            {
                TextGeolocation.Text += "Altitude: " + location.Altitude.Value.ToString("0.00") + " m\n";

            }
            if (location.Course.HasValue)
            {
                TextGeolocation.Text += "Course: " + location.Course.Value.ToString("0.00") + "°\n";
            }
            if (location.Speed.HasValue)
            {
                TextGeolocation.Text += "Speed: " + location.Speed.Value.ToString("0.00") + " m/s\n";
            }
            if (location.Accuracy.HasValue)
            {
                TextGeolocation.Text += "Horizontal Accuracy: " + location.Accuracy.Value.ToString("0.00") + " m\n";
            }
            if (location.VerticalAccuracy.HasValue)
            {
                TextGeolocation.Text += "Vertical Accuracy: " + location.VerticalAccuracy.Value.ToString("0.00") + " m\n";
            }
            TextGeolocation.Text += "Is Accuracy Reduced: " + location.ReducedAccuracy + "\n";
            TextGeolocation.Text += "Is from mock provider: " + location.IsFromMockProvider.ToString() + "\n";
        });
    }
    #endregion

    #region Gyroscope
    private void Gyroscope_ReadingChanged(object? sender, GyroscopeChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextGyroscope.Text = "X: " + e.Reading.AngularVelocity.X.ToString("0.00") + "\n" +
                                 "Y: " + e.Reading.AngularVelocity.Y.ToString("0.00") + "\n" +
                                 "Z: " + e.Reading.AngularVelocity.Z.ToString("0.00") + "\n" +
                                 "Length" + e.Reading.AngularVelocity.Length().ToString("0.00");
        });
    }
    private void SwitchGyroscope_Toggled(object sender, ToggledEventArgs e)
    {
        if (Gyroscope.Default.IsSupported)
        {
            if (!Gyroscope.Default.IsMonitoring && SwitchGyroscope.IsToggled)
            {
                // Turn on gyroscope
                Gyroscope.Default.ReadingChanged += Gyroscope_ReadingChanged;
                Gyroscope.Default.Start(SensorSpeed.UI);
            }
            else if (Gyroscope.Default.IsMonitoring && !SwitchGyroscope.IsToggled)
            {
                // Turn off gyroscope
                Gyroscope.Default.Stop();
                Gyroscope.Default.ReadingChanged -= Gyroscope_ReadingChanged;
                TextGyroscope.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchGyroscope.IsToggled = Gyroscope.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Gyroscope not supported!").Show();
            SwitchGyroscope.IsToggled = false;
        }
    }
    #endregion

    #region Magnetometer
    private void Magnetometer_ReadingChanged(object? sender, MagnetometerChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextMagnetometer.Text = "X: " + e.Reading.MagneticField.X.ToString("0.00") + "\n" +
                                    "Y: " + e.Reading.MagneticField.Y.ToString("0.00") + "\n" +
                                    "Z: " + e.Reading.MagneticField.Z.ToString("0.00") + "\n" +
                                    "Length" + e.Reading.MagneticField.Length().ToString("0.00");
        });
    }
    private void SwitchMagnetometer_Toggled(object sender, ToggledEventArgs e)
    {
        if (Magnetometer.Default.IsSupported)
        {
            if (!Magnetometer.Default.IsMonitoring && SwitchMagnetometer.IsToggled)
            {
                // Turn on magnetometer
                Magnetometer.Default.ReadingChanged += Magnetometer_ReadingChanged;
                Magnetometer.Default.Start(SensorSpeed.UI);
            }
            else if (Magnetometer.Default.IsMonitoring && !SwitchMagnetometer.IsToggled)
            {
                // Turn off magnetometer
                Magnetometer.Default.Stop();
                Magnetometer.Default.ReadingChanged -= Magnetometer_ReadingChanged;
                TextMagnetometer.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchMagnetometer.IsToggled = Magnetometer.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Magnetometer not supported!").Show();
            SwitchMagnetometer.IsToggled = false;
        }
    }
    #endregion

    #region OrientationSensor
    private void OrientationSensor_ReadingChanged(object? sender, OrientationSensorChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextOrientationSensor.Text = "X: " + e.Reading.Orientation.X.ToString("0.00") + "\n" +
                                         "Y: " + e.Reading.Orientation.Y.ToString("0.00") + "\n" +
                                         "Z: " + e.Reading.Orientation.Z.ToString("0.00") + "\n" +
                                         "Length" + e.Reading.Orientation.Length().ToString("0.00");
        });
    }
    private void SwitchOrientationSensor_Toggled(object sender, ToggledEventArgs e)
    {
        if (OrientationSensor.Default.IsSupported)
        {
            if (!OrientationSensor.Default.IsMonitoring && SwitchOrientationSensor.IsToggled)
            {
                // Turn on orientation sensor
                OrientationSensor.Default.ReadingChanged += OrientationSensor_ReadingChanged;
                OrientationSensor.Default.Start(SensorSpeed.UI);
            }
            else if (OrientationSensor.Default.IsMonitoring && !SwitchOrientationSensor.IsToggled)
            {
                // Turn off orientation sensor
                OrientationSensor.Default.Stop();
                OrientationSensor.Default.ReadingChanged -= OrientationSensor_ReadingChanged;
                TextOrientationSensor.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
                SwitchOrientationSensor.IsToggled = OrientationSensor.Default.IsMonitoring;
            }
        }
        else
        {
            Toast.Make("Orientation sensor not supported!").Show();
            SwitchOrientationSensor.IsToggled = false;
        }
    }
    #endregion

    #region Battery
    private void Battery_InfoChanged(object? sender, BatteryInfoChangedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            TextBatterySensor.Text = "Level: " + Convert.ToInt32(e.ChargeLevel * 100) + "%\n" +
                               "State: " + e.State.ToString() + "\n" +
                               "Source: " + e.PowerSource.ToString() + "\n";
        });
    }
    private void SwitchBatterySensor_Toggled(object sender, ToggledEventArgs e)
    {
            if (SwitchBatterySensor.IsToggled)
            {
                // Turn on battery
                Battery.BatteryInfoChanged += Battery_InfoChanged;
            }
            else if (!SwitchBatterySensor.IsToggled)
            {
                // Turn off battery
                Battery.Default.BatteryInfoChanged -= Battery_InfoChanged;
            TextBatterySensor.Text = "--";
            }
            else
            {
                Toast.Make("Something went wrong!").Show();
            }
    }
    #endregion

    
}