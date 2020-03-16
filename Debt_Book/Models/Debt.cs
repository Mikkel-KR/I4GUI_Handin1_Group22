using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Debt_Book.Models
{
    public class Debt
    {
        private double debtValue;
        private DateTime date;

        public Debt(double debt)
        {
            debtValue = debt;
            date = DateTime.Now;
        }

        public Debt()
        {
        }

        #region properties

        public double DebtValue
        {
            get => debtValue;
            set => debtValue = value;
        }
        public DateTime Date
        {
            get => date;
            set => date = value;
        }
        #endregion
    }
}
