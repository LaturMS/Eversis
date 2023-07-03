using System;
using System.Windows;

namespace MVVMExample
{
    public partial class EditWindow : Window
    {
        private DataItem _selectedRow;

        public EditWindow(){}

        public EditWindow(RelayCommand save, DataItem _selectedRow)
        {
            InitializeComponent();
            DataContext = new EditViewModel(save, _selectedRow);
        }
    }
}
