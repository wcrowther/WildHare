using CodeGen.Entities;
using CodeGen.Models;

namespace CodeGen.Generators;

public partial class CodeGenAdapters
{
	public void RunAdaptersList()
	{
		AdaptersTemplate(typeof(Account), typeof(AccountModel), true, true);
		AdaptersTemplate(typeof(Invoice), typeof(InvoiceModel), true, true);
		AdaptersTemplate(typeof(InvoiceItem), typeof(InvoiceItemModel), true, true);
	}
}