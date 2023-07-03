using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static MVVMExample.MainViewModel;

namespace MVVMExample
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private DataItem _data;
        public RelayCommand SaveCommand;

        public int EditWindowValueID
        { 
            get { return _data.Id; } 
            set { _data.Id = value; OnPropertyChanged("EditWindowValueID"); } 
        }

        public string EditWindowValueName
        {
            get { return _data.Name; }
            set { _data.Name = ConvertToNameFormat(value); OnPropertyChanged("EditWindowValueName"); }
        }

        public string EditWindowValueSurname
        {
            get { return _data.Surname; }
            set { _data.Surname = ConvertToNameFormat(value); OnPropertyChanged("EditWindowValueSurname"); }
        }

        public string EditWindowValueEmail
        {
            get { return _data.Email; }
            set { _data.Email = value; OnPropertyChanged("EditWindowValueEmail"); }
        }

        public string EditWindowValuePhone
        {
            get { return _data.Phone; }
            set { _data.Phone = value; OnPropertyChanged("EditWindowValuePhone"); }
        }

        public DataItem Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public ICommand AcceptButton { get; private set; }
        public ICommand CancelButton { get; private set; }

        private void Init(RelayCommand save)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            AcceptButton = new RelayCommand(AcceptValue);
            CancelButton = new RelayCommand(CalcelValue);
            SaveCommand = save;
        }

        public EditViewModel(RelayCommand save, DataItem item)
        {
            SaveCommand = save;
            Data = item;
            Init(save);
        }

        private string ConvertToNameFormat(string text)
        {
            string firstLetter = text.Substring(0, 1).ToUpper();
            string restOfText = text.Substring(0 + 1).ToLower();

            return firstLetter + restOfText;
        }

        private bool CheckEmailForm()
        {
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            return Regex.IsMatch(_data.Email, emailPattern);
        }

        private bool CheckPhoneForm()
        {
            string firstChar = _data.Phone.Substring(0, 1);
            string restOfText = _data.Phone.Substring(0 + 1);

            string phonePatternFirst = @"^[0-9+]+$";
            string phonePatternRest = @"^[0-9]+$";

            return ((Regex.IsMatch(firstChar, phonePatternFirst)) && (Regex.IsMatch(restOfText, phonePatternRest)));
        }

        private void AcceptValue()
        {
            bool flag = false;
            
            if (!CheckEmailForm())
            {
                MessageBox.Show("Niepoprawna forma emaila.");
                flag = true;
            }

            if (!CheckPhoneForm())
            {
                MessageBox.Show("Niepoprawna forma phone.");
                flag = true;
            }

            if(!flag) SaveCommand.ExecuteParameters(_data);
        }

        private void CalcelValue()
        {
            SaveCommand.ExecuteParameters(null);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Nie znany błąd w oknie EditViewModel.");
            Environment.Exit(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
