using AdvanticaAutomationTestClient.ViewModels;
using System.Windows;

namespace AdvanticaAutomationTestClient.Views
{
    /// <summary>
    /// Логика взаимодействия для UpdateWorkerView.xaml
    /// </summary>
    public partial class UpdateWorkerView : Window
    {
        public UpdateWorkerView()
        {
            InitializeComponent();
            this.DataContext = new UpdateWorkerViewModel();
        }
    }
}
