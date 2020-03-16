using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Debt_Book.Models;
using Debt_Book.Views;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class OverviewWindowViewModel : BindableBase
    {
        private ObservableCollection<Debtor> _debtors;
        private int _currentIndex = -1;
        private Debtor _currentDebtor = null;
        private bool _dirty = false;
        private string _filePath = "";
        private string _fileName = "";
        private string _appTitle = "Debtors Assignment";
        private string _title = "Untitled";

        public OverviewWindowViewModel()
        {
            _debtors = new ObservableCollection<Debtor>
            {
                #if DEBUG
                new Debtor("Magnus"),
                new Debtor("Mikkel"),
                new Debtor("Markus"),
                new Debtor("Frederik"),
                new Debtor("Jeppe"),
                new Debtor("Nikolaj"),
                #endif
            };

            #if DEBUG
            Dirty = true;
            #endif

        }

        #region Properties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<Debtor> Debtors
        {
            get => _debtors;
            set => SetProperty(ref _debtors, value);
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set => SetProperty(ref _currentIndex, value);
        }

        public Debtor CurrentDebtor
        {
            get => _currentDebtor;
            set => SetProperty(ref _currentDebtor, value);
        }

        public bool Dirty
        {
            get => _dirty;
            set => SetProperty(ref _dirty, value);
        }

        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        #endregion

        #region Commands

        /***************/
        // FILE - EXIT //
        /***************/
        private ICommand exitCommand;

        public ICommand ExitCommand => exitCommand ?? (exitCommand = new DelegateCommand((() =>
        {
            if (Dirty)
            {
                var result = MessageBox.Show("Any unsaved data will be lost. Do you wish to continue?", "WARNING",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            Environment.Exit(1);

        }
        ))); // Indsæt således at man kun kan "exit" hvis filen er gemt / messagebox der spørger om man vil exit hvis filen ikke er gemt...

        /****************/
        // DEBTOR - ADD //
        /****************/
        private ICommand addCommand;
        public ICommand AddCommand => addCommand ?? (addCommand = new DelegateCommand(() =>
        {
            Debtor newDebtor = new Debtor("");
            var vm = new AddDebtorWindowViewModel(newDebtor);
            var dlg = new AddDebtorWindow();
            dlg.DataContext = vm;
            dlg.Owner = App.Current.MainWindow;

            if (dlg.ShowDialog() == true)
            {
                if (newDebtor.TotalDebt > -1000000000 && newDebtor.TotalDebt < 1000000000)
                {
                    Debtors.Add(newDebtor);
                    Dirty = true;
                }
                else
                    MessageBox.Show("The debt-value has to be between -1000000000 and 1000000000",
                        "Error-2", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }

        }));

        /************************/
        // DEBTOR - SHOW DEBTS  //
        /************************/
        private ICommand showDebtsCommand;
        public ICommand ShowDebtsCommand => showDebtsCommand ?? (showDebtsCommand = new DelegateCommand(() =>
        {
            //Debtor tempDebtor = CurrentDebtor.Clone();


            ObservableCollection<Debt> tempDebt = new ObservableCollection<Debt>();
            foreach (var debt in CurrentDebtor.Debts)
            {
                tempDebt.Add(debt);
            }

            var vm = new DetailsWindowViewModel($"{CurrentDebtor.Name} - Debts Overview", CurrentDebtor);
            var dlg = new DetailsWindow();
            dlg.DataContext = vm;
            dlg.Owner = App.Current.MainWindow;

            if (dlg.ShowDialog() == true)
            {
                //Intet heri, men den skal være der, så vinduet åbner.
                //Ved at have denne boks, kan vi yderligere tjekke for om der er tilføjet nye debt i det nedenstående kode:
            }

            if (tempDebt != CurrentDebtor.Debts)
            {
                Dirty = true;
            }
               

        }, () => { return CurrentIndex >= 0; }).ObservesProperty((() => CurrentIndex)));

        /***************************/
        // CHANGE BACKGROUND COLOR //
        /***************************/
        private ICommand changeColorCommand;
        public ICommand ChangeColorCommand => changeColorCommand ?? (changeColorCommand = new DelegateCommand<string>((color) =>
        {
              var convertedColor = (Color) ColorConverter.ConvertFromString(color);
              SolidColorBrush newBrush = new SolidColorBrush(convertedColor);
              Application.Current.Resources["BackgroundColor"] = newBrush;
        }));

        /**************/
        // FILE - NEW //
        /**************/
        private ICommand newCommand;

        public ICommand NewCommand => newCommand ?? (newCommand = new DelegateCommand(() =>
        {
            if (Dirty)
            {
                MessageBoxResult result = MessageBox.Show("Any unsaved data will be lost. Do you wish to continue?", "WARNING",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            FilePath = "";
            FileName = "";
            Debtors.Clear();
            Dirty = false;
            Title = "Untitled - " + _appTitle;

        }));

        /***************/
        // FILE - OPEN //
        /***************/
        private ICommand openCommand;

        public ICommand OpenCommand => openCommand ?? (openCommand = new DelegateCommand(() =>
        {

            var dialog = new OpenFileDialog
            {
                Filter = "XML Document|*.xml|Text Document|*.txt|All Files|*.*",
                DefaultExt = "xml"
            };
            if (FilePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(FilePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                if (Dirty)
                {
                    MessageBoxResult result = MessageBox.Show("Any unsaved data will be lost. Do you wish to continue?", "WARNING",
                        MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                FilePath = dialog.FileName;
                FileName = Path.GetFileName(FilePath);

                ObservableCollection<Debtor> tempDebtors = new ObservableCollection<Debtor>();

                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Debtor>));

                try
                {
                    TextReader reader = new StreamReader(FilePath);
                    tempDebtors = (ObservableCollection<Debtor>)serializer.Deserialize(reader);
                    reader.Close();
                    Debtors = tempDebtors;
                    Dirty = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }));

        /******************/
        // FILE - SAVE AS //
        /******************/
        private ICommand saveAsCommand;
        public ICommand SaveAsCommand => saveAsCommand ?? (saveAsCommand = new DelegateCommand(() =>
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XML Document|*.xml|Text Document|*.txt|All Files|*.*",
                DefaultExt = "xml"
            };
            if (_filePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(FilePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                FilePath = dialog.FileName;
                FileName = Path.GetFileName(FilePath);
                SaveFile();
                Title = FileName + " - " + _appTitle;
            }
        }));

        /***************/
        // FILE - SAVE //
        /***************/
        private ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand(SaveFile, SaveFile_CanExecute)
                                           .ObservesProperty(() => FileName)
                                           .ObservesProperty(() => FilePath)
                                           .ObservesProperty(() => Dirty));

        private bool SaveFile_CanExecute()
        {
            if (Dirty && !string.IsNullOrWhiteSpace(FileName) &&
                !string.IsNullOrWhiteSpace(FilePath))
                return true;
            return false;
        }

        private void SaveFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Debtor>));
                TextWriter writer = new StreamWriter(FilePath);
                serializer.Serialize(writer, Debtors);
                writer.Close();
                
                Dirty = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
