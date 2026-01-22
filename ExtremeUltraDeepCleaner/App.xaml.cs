using System.Windows;

namespace ExtremeUltraDeepCleaner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Check if running as administrator
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                MessageBox.Show(
                    "This application requires Administrator privileges.\n\nPlease right-click and select 'Run as Administrator'.",
                    "Administrator Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                
                Current.Shutdown();
                return;
            }
        }
    }
}
