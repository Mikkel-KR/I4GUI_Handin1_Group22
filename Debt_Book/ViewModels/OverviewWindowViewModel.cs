using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debt_Book.Models;
using Prism.Mvvm;

namespace Debt_Book.ViewModels
{
    public class OverviewWindowViewModel : BindableBase
    {
        private List<Debtor> _debtors;

        public OverviewWindowViewModel()
        {
            _debtors = new List<Debtor>();
        }

        #region Properties

        public List<Debtor> Debtors
        {
            get { return _debtors; }
            set { SetProperty(ref _debtors, value); }
        }

        #endregion
    }
}
