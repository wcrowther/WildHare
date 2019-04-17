
using WildHare.Web.Models;
using WildHare.Web.Entities;
using System.Linq;
using System.Collections.Generic;



namespace WildHare.Web.Adapters
{
    public static partial class Adapter
    {
        public static InvoiceItemModel ToInvoiceItemModel (this InvoiceItem entity)
        {
            return entity == null ? null : new InvoiceItemModel
            {
                InvoiceItemId = entity.InvoiceItemId,
				InvoiceId = entity.InvoiceId,
				Fee = entity.Fee,
				Product = entity.Product,
				Description = entity.Description,
				Created = entity.Created
            };
        }

        public static List<InvoiceItemModel> ToInvoiceItemModelList (this IEnumerable<InvoiceItem> entityList)
        {
            return entityList?.Select(a => a.ToInvoiceItemModel()).ToList() ?? new List<InvoiceItemModel>();
        }

        public static InvoiceItem ToInvoiceItem (this InvoiceItemModel model)
        {
            return model == null ? null : new InvoiceItem
            {
                InvoiceItemId = model.InvoiceItemId,
				InvoiceId = model.InvoiceId,
				Fee = model.Fee,
				Product = model.Product,
				Description = model.Description,
				Created = model.Created
            };
        }

        public static List<InvoiceItem> ToInvoiceItemList (this IEnumerable<InvoiceItemModel> modelList)
        {
            return modelList?.Select(a => a.ToInvoiceItem()).ToList() ?? new List<InvoiceItem>();
        }
    }
}