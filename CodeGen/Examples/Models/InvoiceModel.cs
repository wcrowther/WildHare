using System;
using System.Collections.Generic;
using WildHare.Web.Entities;

namespace CodeGen.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }

        public int AccountId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime Created { get; set; }

        public virtual List<InvoiceItemModel> InvoiceItems { get; set; } = new List<InvoiceItemModel>();

        public override string ToString()
        {
            return $"{InvoiceId} AccountId: {AccountId}";
        }
    }
}
