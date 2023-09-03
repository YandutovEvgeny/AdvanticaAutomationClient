using AdvanticaAutomationTestClient.ViewModels;
using System;
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
            var updateWorkerViewModel = new UpdateWorkerViewModel();
            this.DataContext = updateWorkerViewModel;
            if(updateWorkerViewModel.CloseAction == null)
            {
                updateWorkerViewModel.CloseAction = new Action(Close);
            }
        }
    }
}
