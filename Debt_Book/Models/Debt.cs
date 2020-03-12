using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debt_Book.Models
{
    public class Debt
    {
        private double _debtValue;
        private readonly DateTime _date;


        #region properties
        public double DebtValue
        {
            get => _debtValue;
        }

        public DateTime Date
        {
            get => _date;
        }
        #endregion

        public Debt(double debtValue)
        {
            _debtValue = debtValue;
            _date = DateTime.Now;
        }
    }
}
