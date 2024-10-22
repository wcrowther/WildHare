using CodeGen.Entities;
using CodeGen.Models;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.Adapters
{ 
	public static partial class Adapter
	{
		public static InvoiceItemModel ToInvoiceItemModel (this Account entity)
		{
			return entity == null ? null : new InvoiceItemModel
			{
				// No Match // AccountId            = entity.AccountId,
			   // No Match // AccountName          = entity.AccountName,
			   Created							   = entity.Created,
			   // No Match // Invoices             = entity.Invoices.ToInvoiceModelList()
			};
		}

		public static Account ToAccount (this InvoiceItemModel model)
		{
			return model == null ? null : new Account
			{
				// No Match // InvoiceItemId      = model.InvoiceItemId,
			   // No Match // InvoiceId           = model.InvoiceId,
			   // No Match // Fee                 = model.Fee,
			   // No Match // Product             = model.Product,
			   // No Match // Description         = model.Description,
			   Created							  = model.Created
			};
		}
		
		public static List<InvoiceItemModel> ToInvoiceItemModelList (this IEnumerable<Account> entityList)
		{
			return entityList?.Select(a => a.ToInvoiceItemModel()).ToList() ?? new List<InvoiceItemModel>();
		}
		
		public static List<Account> ToAccountList (this IEnumerable<InvoiceItemModel> modelList)
		{
			return modelList?.Select(a => a.ToAccount()).ToList() ?? new List<Account>();
		}
	}
}