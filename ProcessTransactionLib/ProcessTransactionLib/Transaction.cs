using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTransactionLib
{
    public class Transaction
    {
        public string Id { get; set; }
        public string CreaditCardId { get; set; }
        public double Amount { get; set; }
        public string TerminalId { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public bool Completed { get; set; }

    }
}
