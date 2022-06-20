using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTransactionLib
{
    public class TransactionHistory
    {
        public string Name { get; set; }

        public Transaction Transaction { get; set; }

        public string Result { get; set; }

        public string Message { get; set; }
    }
}
