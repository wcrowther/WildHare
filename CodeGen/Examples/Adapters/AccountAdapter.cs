using CodeGen.Entities;
using CodeGen.Models;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.Adapters
{ 
	public static partial class Adapter
	{
		public static AccountModel ToAccountModel (this Account entity)
		{
			return entity == null ? null : new AccountModel
			{
				AccountId            = entity.AccountId,
				AccountName          = entity.AccountName,
				Created              = entity.Created,
				Invoices             = entity.Invoices.ToInvoiceModelList()
			};
		}

		public static Account ToAccount (this AccountModel model)
		{
			return model == null ? null : new Account
			{
				AccountId            = model.AccountId,
				AccountName          = model.AccountName,
				Created              = model.Created,
				Invoices             = model.Invoices.ToInvoiceList()
			};
		}
		
		public static List<AccountModel> ToAccountModelList (this IEnumerable<Account> entityList)
		{
			return entityList?.Select(a => a.ToAccountModel()).ToList() ?? [];
		}
		
		public static List<Account> ToAccountList (this IEnumerable<AccountModel> modelList)
		{
			return modelList?.Select(a => a.ToAccount()).ToList() ?? [];
		}
	}
}