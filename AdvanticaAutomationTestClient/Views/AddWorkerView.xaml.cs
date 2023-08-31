using AdvanticaAutomationTestClient.ViewModels;
using System.Windows;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AddWorkerView.xaml
    /// </summary>
    public partial class AddWorkerView : Window
    {
        public AddWorkerView()
        {
            InitializeComponent();
            var addWorkerViewModel = new AddWorkerViewModel();
            this.DataContext = addWorkerViewModel;
            if (addWorkerViewModel.CloseAction == null)
                addWorkerViewModel.CloseAction = new (Close);
        }
    }
}
