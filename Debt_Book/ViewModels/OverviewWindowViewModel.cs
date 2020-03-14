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

            Debtors.Last().TotalDebt = 100;
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

        #region OverviewCommands

        /******************/
        // ADD NEW DEBTOR //
        /******************/
        private ICommand addCommand;
        public ICommand AddCommand => addCommand ?? (addCommand = new DelegateCommand(() =>
        {
            AddDebtorWindow wn = new AddDebtorWindow();
            wn.Show();

            //Mere mangler i denne funktion!

        }));

        /*************************/
        // SHOW DEBTS FOR DEBTOR //
        /*************************/
        private ICommand showDebtsCommand;
        public ICommand ShowDebtsCommand => showDebtsCommand ?? (showDebtsCommand = new DelegateCommand(() =>
        {
            var vm = new DetailsWindowViewModel($"{CurrentDebtor.Name} - Debts Overview", CurrentDebtor);
            vm.CurrentDebtor.Debts.Add(new Debt(200));
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


        /****************************************************************************************************************/
        // Denne command skal i en ny MVVM for AddDebtorWindow - Debtors name property bør yderligere have en set metode /
        /****************************************************************************************************************/

        #region AddDebtorCommands

        private ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand<string>(SaveCommand_Execute));

        private void SaveCommand_Execute(string DebtorName)
        {
            if (DebtorName == "")
            {
                MessageBox.Show("You must enter a debtor name in the Debtor Name textbox!", "Unable to add debtor",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Debtors.Add(new Debtor(DebtorName));
            }
        }

        #endregion

        #endregion
    }
}
