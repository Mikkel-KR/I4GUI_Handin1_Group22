using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Debt_Book.Models;
using Debt_Book.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class DetailsWindowViewModel : BindableBase
    {
        private string _title;
        private Debtor _currentDebtor;
        private string _textBoxValue;

        public DetailsWindowViewModel(string title, Debtor debtor)
        {
            Title = title;
            CurrentDebtor = debtor;
            TextBoxValue = "0";
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
            get => _textBoxValue;
            set => SetProperty(ref _textBoxValue, value);
        }

        #endregion

        #region Commands

        /**********************************/
        // ADD NEW DEBT TO CURRENT DEBTOR //
        /**********************************/
        private ICommand addDebtCommand;
        public ICommand AddDebtCommand => addDebtCommand ??
                                          (addDebtCommand = new DelegateCommand(AddDebtCommandExecute));
        private void AddDebtCommandExecute()
        {
            double debtValue;

            try
            {
                debtValue = double.Parse(TextBoxValue);
            }
            catch
            {

                MessageBox.Show("The debt-value has to be a value!", "Error-1", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (debtValue > -1000000000 && debtValue < 1000000000 && debtValue != 0)
            {
                CurrentDebtor.Debts.Add(new Debt(debtValue));
                CurrentDebtor.TotalDebt += debtValue;
            }
            else
            {
                MessageBox.Show("The debt-value has to be between -1000000000 and 1000000000, and not be zero!", "Error-2", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
