using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debt_Book.Models
{
    public class Debt
    {
        private double debtValue;
        private readonly DateTime date;


        #region properties
        public double DebtValue
        {
            get => debtValue;
        }

        public DateTime Date
        {
            get => date;
        }
        #endregion

        public Debt(double debt)
        {
            debtValue = debt;
            date = DateTime.Now;
        }
    }
}
