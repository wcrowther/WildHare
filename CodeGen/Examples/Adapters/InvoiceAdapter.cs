using CodeGen.Entities;
using CodeGen.Models;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.Adapters
{ 
	public static partial class Adapter
	{
		public static InvoiceModel ToInvoiceModel (this Invoice entity)
		{
			return entity == null ? null : new InvoiceModel
			{
				InvoiceId            = entity.InvoiceId,
				AccountId            = entity.AccountId,
				InvoiceDate          = entity.InvoiceDate,
				Created              = entity.Created,
				InvoiceItems         = entity.InvoiceItems.ToInvoiceItemModelList()
			};
		}

		public static Invoice ToInvoice (this InvoiceModel model)
		{
			return model == null ? null : new Invoice
			{
				InvoiceId            = model.InvoiceId,
				AccountId            = model.AccountId,
				InvoiceDate          = model.InvoiceDate,
				Created              = model.Created,
				InvoiceItems         = model.InvoiceItems.ToInvoiceItemList()
			};
		}
		
		public static List<InvoiceModel> ToInvoiceModelList (this IEnumerable<Invoice> entityList)
		{
			return entityList?.Select(a => a.ToInvoiceModel()).ToList() ?? [];
		}
		
		public static List<Invoice> ToInvoiceList (this IEnumerable<InvoiceModel> modelList)
		{
			return modelList?.Select(a => a.ToInvoice()).ToList() ?? [];
		}
	}
}