using System.Text;

namespace WildHare.Extensions;

public static class StringBuilderExtensions
{  
	 /// <summary>Append to StringBuilder when {ifCondition} is true.</summary>
	 public static void AppendIf(this StringBuilder sb, bool ifCondition, string text) 
	 {
		  if(ifCondition)
			   sb.Append(text);
	 }
}

