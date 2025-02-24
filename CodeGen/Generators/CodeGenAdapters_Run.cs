using CodeGen.Entities;
using CodeGen.Models;

namespace CodeGen.Generators;

public partial class CodeGenAdapters
{
	public void RunAdapterList()
	{
		GenerateAdapter(typeof(Account), typeof(AccountModel), true);
		GenerateAdapter(typeof(Invoice), typeof(InvoiceModel), true);
		GenerateAdapter(typeof(InvoiceItem), typeof(InvoiceItemModel), true);
	}
}