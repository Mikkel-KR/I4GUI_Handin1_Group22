using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debt_Book.Models;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class DetailsWindowViewModel : BindableBase
    {
        private string _title;
        private Debtor _currentDebtor;

        public DetailsWindowViewModel(string title, Debtor debtor)
        {
            Title = title;
            CurrentDebtor = debtor;
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

        #endregion
    }
}
