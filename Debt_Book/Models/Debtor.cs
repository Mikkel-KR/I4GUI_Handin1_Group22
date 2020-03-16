using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace Debt_Book.Models
{
    public class Debtor : BindableBase
    {
        private string _name;
        private ObservableCollection<Debt> _debts;
        private double _totalDebt;

        public Debtor(string name)
        {
            _name = name;
            _debts = new ObservableCollection<Debt>();
            _totalDebt = 0;
        }

        public Debtor()
        {
        }

        #region Properties

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<Debt> Debts
        {
            get => _debts;
            set => SetProperty(ref _debts, value);
        }

        public double TotalDebt
        {
            get => _totalDebt;
            set => SetProperty(ref _totalDebt, value);
        }

        #endregion
    }
}
