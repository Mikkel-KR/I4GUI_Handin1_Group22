using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Debt_Book.Models;
using Debt_Book.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class OverviewWindowViewModel : BindableBase
    {
        private ObservableCollection<Debtor> _debtors;
        private int _currentIndex = -1;
        private Debtor _currentDebtor = null;

        public OverviewWindowViewModel()
        {
            _debtors = new ObservableCollection<Debtor>
            {
                #if DEBUG
                new Debtor("Magnus Kyneb"),
                new Debtor("Jeppe Dybdal"),
                new Debtor("Mikkel Rasmussen"),
                new Debtor("Markus Hansen"),
                new Debtor("Frederik Poulsen"),
                new Debtor("Nikolaj Pedersen")

                #endif

            };
        }

        #region Properties

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

        #endregion

        #region Commands

        /***************/
        // FILE - EXIT //
        /***************/
        private ICommand exitCommand;

        public ICommand ExitCommand => exitCommand ?? (exitCommand = new DelegateCommand((() =>
        {
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
                    Debtors.Add(newDebtor);
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
            var vm = new DetailsWindowViewModel($"{CurrentDebtor.Name} - Debts Overview", CurrentDebtor);
            var dlg = new DetailsWindow();
            dlg.DataContext = vm;
            dlg.Owner = App.Current.MainWindow;
            
            if (dlg.ShowDialog() == true)
            {
                // ??
            }

        }, () => { return CurrentIndex >= 0; }).ObservesProperty((() => CurrentIndex)));

        /***************************/
        // CHANGE BACKGROUND COLOR //
        /***************************/
        private ICommand _changeColorCommand;
        public ICommand ChangeColorCommand => _changeColorCommand ?? (_changeColorCommand = new DelegateCommand<string>((color) =>
        {
              var convertedColor = (Color) ColorConverter.ConvertFromString(color);
              SolidColorBrush newBrush = new SolidColorBrush(convertedColor);
              Application.Current.Resources["BackgroundColor"] = newBrush;
        }));
                                                  
        #endregion

    }
}
