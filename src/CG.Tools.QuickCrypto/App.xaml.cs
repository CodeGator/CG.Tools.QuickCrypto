
namespace CG.Tools.QuickCrypto
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
#if WINDOWS
            window.Width = 800;
#endif
            return window;
        }
    }
}