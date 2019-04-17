
using System;

namespace WildHare.Web.SchemaModels
{
	public class DataTypesSchema
	{
		public string TypeName { get; set; }

		public int ProviderDbType { get; set; }

		public long ColumnSize { get; set; }

		public string CreateFormat { get; set; }

		public string CreateParameters { get; set; }

		public string DataType { get; set; }

		public bool IsAutoIncrementable { get; set; }

		public bool IsBestMatch { get; set; }

		public bool IsCaseSensitive { get; set; }

		public bool IsFixedLength { get; set; }

		public bool IsFixedPrecisionScale { get; set; }

		public bool IsLong { get; set; }

		public bool IsNullable { get; set; }

		public bool IsSearchable { get; set; }

		public bool IsSearchableWithLike { get; set; }

		public bool IsUnsigned { get; set; }

		public short MaximumScale { get; set; }

		public short MinimumScale { get; set; }

		public bool IsConcurrencyType { get; set; }

		public bool IsLiteralSupported { get; set; }

		public string LiteralPrefix { get; set; }

		public string LiteralSuffix { get; set; }
	}
}