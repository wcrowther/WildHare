using System;
using System.Collections.Generic;

namespace CodeGen.Models;

public class AccountModel
{
    public int AccountId { get; set; }

    public string AccountName { get; set; }

    public DateTime Created { get; set; }

    public virtual List<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();

    public override string ToString()
    {
        return $"{AccountName} AccountId: {AccountId}";
    }
}
