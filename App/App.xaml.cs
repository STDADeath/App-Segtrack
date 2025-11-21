namespace QRMySqlApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // ⚠️ IMPORTANTE: Cambiar de MainPage a AppShell
            MainPage = new AppShell();
        }

        // Métodos del ciclo de vida de la aplicación
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            // Configurar tamaño de ventana para Desktop (opcional)
            window.Width = 400;
            window.Height = 800;
            window.MinimumWidth = 375;
            window.MinimumHeight = 667;

            return window;
        }
    }
}