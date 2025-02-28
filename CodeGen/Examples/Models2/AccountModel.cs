using System;
using System.Collections.Generic;

namespace CodeGen.Examples.Models2
{
    public class AccountModel
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public DateTime Created { get; set; }

        public virtual List<InvoiceModel> Invoices { get; set; } = [];

        public override string ToString()
        {
            return $"{AccountName} AccountId: {AccountId}";
        }
    }
}
