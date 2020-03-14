using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Debt_Book.Models;
using Debt_Book.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class DetailsWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        private string _title;
        private Debtor _currentDebtor;
        private string textBoxValue;

        public DetailsWindowViewModel(string title, Debtor debtor)
        {
            Title = title;
            CurrentDebtor = debtor;
            textBoxValue = "";
        }

        #region Properties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Debtor CurrentDebtor
        {
            get => _currentDebtor;
            set => SetProperty(ref _currentDebtor, value);
        }

        public string TextBoxValue
        {
            get => textBoxValue;
            set
            {
                if (textBoxValue != value)
                {
                    textBoxValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        /**********************************/
        // ADD NEW DEBT TO CURRENT DEBTOR //
        /**********************************/
        /**************************************************************************************************************************/
        // Har lidt problemer her. Kan ikke få den til at tjekke om den kan execute, medmindre jeg trykker væk fra Add-debt boxen. /
        // Dette kan man kun gøre hvis der allerede er en debt i listboxen...                                                      /
        /**************************************************************************************************************************/
        private ICommand addDebtCommand;
        public ICommand AddDebtCommand => addDebtCommand ??
                                          (addDebtCommand = new DelegateCommand(AddDebtCommandExecute,
                                              AddDebtCommandCanExecute).ObservesProperty((() => TextBoxValue)));
        private void AddDebtCommandExecute()
        {
            var debtValue = Int32.Parse(TextBoxValue);

            CurrentDebtor.Debts.Add(new Debt(debtValue));
        }

        private bool AddDebtCommandCanExecute()
        {
            Int32 val;

            try
            {
                val = Int32.Parse(TextBoxValue);
            }
            catch (Exception ex)
            {
                return false;
            }

            return (val > -1000000000 && val < 1000000000 && val != 0);

        }


        #endregion


        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}
