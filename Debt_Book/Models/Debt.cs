using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debt_Book.Models
{
    public class Debt
    {
        #region fields
        private int _debtValue;
        private readonly DateTime _date;
        #endregion

        #region properties

        public int DebtValue
        {
            get => _debtValue;
        }

        public DateTime Date
        {
            get => _date;
        }

        #endregion
    }
}
