namespace MauiAppProva2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new Views.LoginApp());
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            
            window.Width = 400;
            window.Height = 700;

            return window;
        }
    }
}
