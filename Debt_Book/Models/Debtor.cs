using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Debt_Book.Models
{
    public class Debtor : BindableBase
    {
        private string _name;
        private List<Debt> _debts;
        private double _totalDebt;

        public Debtor(string name)
        {
            _name = name;
            _debts = new List<Debt>();
            _totalDebt = 0;
        }

        #region Properties

        public string Name
        {
            get => _name;
        }

        public List<Debt> Debts
        {
            get => _debts;
            set
            {
                SetProperty(ref _debts, value);
                TotalDebt += Debts.Last().DebtValue;
            }
        }

        public double TotalDebt
        {
            get => _totalDebt;
            set => SetProperty(ref _totalDebt, value);
        }

        #endregion
    }
}
