using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace bios_vil3
{
    public partial class SecondWindow : Window
    {
        public Employee NewEmployee { get; private set; }

        public SecondWindow()
        {
            InitializeComponent();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AgeTextBox.Text, out int age))
            {
                var newEmployee = new Employee
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Age = age,
                    Position = PositionTextBox.Text
                };
                Close(newEmployee);
            }
            else
            {
                // Show an error message about invalid age aa
            }
        }
    }
}
