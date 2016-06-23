using System.Windows;
using System.Windows.Controls;

namespace Nexus.Shell {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));

            base.OnStartup(e);

            Bootstrapper bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            (sender as TextBox).SelectAll();
        }
    }
}
