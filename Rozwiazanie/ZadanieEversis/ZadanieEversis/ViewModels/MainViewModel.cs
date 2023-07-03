using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMExample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DataItem _selectedRow;
        private EditWindow _editWindow;
        public DataItem SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }
        private ObservableCollection<DataItem> _data;
        public ObservableCollection<DataItem> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public ICommand EditCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public MainViewModel()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            EditCommand = new RelayCommand(EditRow);
            LoadCommand = new RelayCommand(Load);
            CloseCommand = new RelayCommand(CloseWindow);

            // Inicjalizacja bazy danych i stworzenie tabeli
            using (var context = new MyDbContext())
            {
                context.Database.Initialize(false);
            }

            //W celu utworzenia bazy która bedzie miała rekordy odpowiadające kryteriom zadania
            CreateNewDB(420, 1419);
        }

        private void SaveDataItem(DataItem item)
        {
            using (var context = new MyDbContext())
            {
                var findIteam = context.DataItems.Find(item.Id);

                findIteam.Name = item.Name;
                findIteam.Surname = item.Surname;
                findIteam.Email = item.Email;
                findIteam.Phone = item.Phone;

                context.SaveChanges();
                Data = new ObservableCollection<DataItem>(context.DataItems.ToList());
            }
        }

        private void LoadDataItem(MyDbContext context, DataItem item)
        {
            var findIteam = context.DataItems.Find(item.Id);
            if (findIteam == null)
                context.DataItems.Add(item);
            else
            {
                findIteam.Name = item.Name;
                findIteam.Surname = item.Surname;
                findIteam.Email = item.Email;
                findIteam.Phone = item.Phone;
            }
        }

        void CreateNewDB(int startIndex, int endIndex)
        {
            using (var context = new MyDbContext())
            {
                context.Database.Delete();
                context.SaveChanges();

                for (int i = 0; i < endIndex; i++)
                {
                    var iteamNew = new DataItem { Id = i, Name = "", Surname = "", Email = "", Phone = "" };
                    LoadDataItem(context, iteamNew);
                }

                context.SaveChanges();

                for (int i = 0; i < startIndex; i++)
                {
                    var blog = context.DataItems.Find(i);
                    if (blog != null) context.DataItems.Remove(blog);
                }

                context.SaveChanges();
            }
        }

        private void EditRow()
        {
            if (_selectedRow == null) return;
            _editWindow = new EditWindow(new RelayCommand(AcceptValuesFormEditWindow), _selectedRow);
            _editWindow.ShowDialog();
        }

        private void AcceptValuesFormEditWindow(object parameter)
        {
            if (parameter == null)
                _editWindow.Close();
            else
            {
                DataItem item = (DataItem)parameter;
                SaveDataItem(item);
                using (var context = new MyDbContext())
                {
                    Data = new ObservableCollection<DataItem>(context.DataItems.ToList());
                }
                _editWindow.Close();
            }
        }

        private void Load()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var lines = File.ReadAllLines(filePath);

                try
                {
                    // Ignorowanie pierwszego wiersza jako nagłówek
                    var dataItems = lines.Skip(1).Select(line =>
                    {
                        var items = line.Split(',');
                        return new DataItem { Id = int.Parse(items[0]), Name = items[1], Surname = items[2], Email = items[3], Phone = items[4] };
                    });

                    // Zapis danych do bazy danych
                    using (var context = new MyDbContext())
                    {
                        foreach (var item in dataItems)
                        {
                            LoadDataItem(context, item);
                        }

                        context.SaveChanges();
                    }
                }
                catch(Exception) 
                {
                    MessageBox.Show("Niepoprawne dane.");
                    throw;
                }

                // Wczytanie danych z bazy danych i wyświetlenie ich na liście
                using (var context = new MyDbContext())
                {
                    Data = new ObservableCollection<DataItem>(context.DataItems.ToList());
                }
            }
        }

        private void CloseWindow()
        {
            Environment.Exit(1);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Nie znany błąd.");
            Environment.Exit(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
