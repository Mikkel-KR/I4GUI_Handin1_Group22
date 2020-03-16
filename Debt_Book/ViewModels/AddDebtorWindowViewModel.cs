using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Debt_Book.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class AddDebtorWindowViewModel : BindableBase
    {
        private Debtor newDebtor;
        private string initialDebt;

        public AddDebtorWindowViewModel(Debtor debtor)
        {
            newDebtor = debtor;
        }

        #region Properties
        public Debtor NewDebtor
        {
            get => newDebtor;
            set => SetProperty(ref newDebtor, value);

        }

        public string InitialDebt
        {
            get => initialDebt;
            set => SetProperty(ref initialDebt, value);

        }
        #endregion


        #region Commands
        private ICommand saveCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand(
                                               SaveCommandExecute, SaveCommandCanExecute)
                                           .ObservesProperty(() => NewDebtor.Name)
                                           .ObservesProperty(() => InitialDebt));

        private void SaveCommandExecute()
        {
            double debt = double.Parse(InitialDebt);
            NewDebtor.Debts.Add(new Debt(debt));
            NewDebtor.TotalDebt += debt;
        }

        private bool SaveCommandCanExecute()
        {
            if (string.IsNullOrWhiteSpace(NewDebtor.Name))
                return false;
            if (string.IsNullOrWhiteSpace(InitialDebt))
                return false;

            try
            {
                double.Parse(InitialDebt);
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
