using CodeGen.Entities;
using CodeGen.Models;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.Adapters
{ 
	public static partial class Adapter
	{
		public static InvoiceItemModel ToInvoiceItemModel (this Invoice entity)
		{
			return entity == null ? null : new InvoiceItemModel
			{
				InvoiceId            = entity.InvoiceId,
// No Match // AccountId            = entity.AccountId,
// No Match // InvoiceDate          = entity.InvoiceDate,
Created              = entity.Created,
// No Match // InvoiceItems         = entity.InvoiceItems.ToInvoiceItemModelList()
			};
		}

		public static Invoice ToInvoice (this InvoiceItemModel model)
		{
			return model == null ? null : new Invoice
			{
				// No Match // InvoiceItemId        = model.InvoiceItemId,
InvoiceId            = model.InvoiceId,
// No Match // Fee                  = model.Fee,
// No Match // Product              = model.Product,
// No Match // Description          = model.Description,
Created              = model.Created
			};
		}
		
public static List<InvoiceItemModel> ToInvoiceItemModelList (this IEnumerable<Invoice> entityList)
{
	return entityList?.Select(a => a.ToInvoiceItemModel()).ToList() ?? new List<InvoiceItemModel>();
}

public static List<Invoice> ToInvoiceList (this IEnumerable<InvoiceItemModel> modelList)
{
	return modelList?.Select(a => a.ToInvoice()).ToList() ?? new List<Invoice>();
}
	}
}