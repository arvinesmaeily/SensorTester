namespace SensorTester
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Permissions.CheckStatusAsync<Permissions.LocationAlways>().ContinueWith(t =>
            {
                if (t.Result != PermissionStatus.Granted)
                {
                    Permissions.RequestAsync<Permissions.LocationAlways>();
                }
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}