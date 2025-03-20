using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace bios_vil3
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var secondWindow = new SecondWindow();
            var result = await secondWindow.ShowDialog<Employee>(this);
        
            if (result is Employee newEmployee)
            {
                newEmployee.Id = Employees.Count + 1;
                Employees.Add(newEmployee);
                System.Diagnostics.Debug.WriteLine($"Added new employee: {newEmployee.FirstName} {newEmployee.LastName}");
                System.Diagnostics.Debug.WriteLine($"Employees count: {Employees.Count}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No employee was added");
            }
        }

        private void RemoveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (EmployeeGrid.SelectedItem is Employee selectedEmployee)
            {
                Employees.Remove(selectedEmployee);
            }
        }

        private async void SaveCsvButton_Click(object? sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filters.Add(new FileDialogFilter() { Name = "CSV Files", Extensions = { "csv" } });

            var result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                await SaveToCsvAsync(result);
            }
        }

        private async void LoadCsvButton_Click(object? sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "CSV Files", Extensions = { "csv" } });

            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                await LoadFromCsvAsync(result[0]);
            }
        }

        private async Task SaveToCsvAsync(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("Id,FirstName,LastName,Age,Position");
                foreach (var employee in Employees)
                {
                    await writer.WriteLineAsync($"{employee.Id},{employee.FirstName},{employee.LastName},{employee.Age},{employee.Position}");
                }
            }
        }

        private async Task LoadFromCsvAsync(string filePath)
        {
            Employees.Clear();
            using (var reader = new StreamReader(filePath))
            {
                await reader.ReadLineAsync(); // Skip header
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var values = line.Split(',');
                    if (values.Length == 5 && int.TryParse(values[0], out int id) && int.TryParse(values[3], out int age))
                    {
                        Employees.Add(new Employee
                        {
                            Id = id,
                            FirstName = values[1],
                            LastName = values[2],
                            Age = age,
                            Position = values[4]
                        });
                    }
                }
            }
        }
    }
}