using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debt_Book.Models;
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
                new Debtor("Magnus"),
                new Debtor("Jeppe"),
                new Debtor("Mikkel"),
                new Debtor("Markus"),
                new Debtor("Frederik"),
                new Debtor("Nikolaj")
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
    }
}
