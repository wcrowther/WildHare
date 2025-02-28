using System;
using System.Collections.Generic;
using WildHare.Web.Entities;

namespace CodeGen.Examples.Models2
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }

        public int AccountId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime Created { get; set; }

        public virtual List<InvoiceItemModel> InvoiceItems { get; set; } = [];

        public override string ToString()
        {
            return $"{InvoiceId} AccountId: {AccountId}";
        }
    }
}
