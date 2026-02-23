using BetaReader.Interfaces;
using BetaReader.Service.Sync;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BetaReader.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IVaultRegistry _vaults;
        private readonly SyncOrchestrator _sync;

        public MainWindow()
        {
            InitializeComponent();
            // Compose dependencies manually for now
        }

        private async void OnPollClick(object sender, RoutedEventArgs e)
        {
            await _sync.PollAsync(CancellationToken.None);
        }
    }
}