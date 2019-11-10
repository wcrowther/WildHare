using System;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;

namespace WildHare
{
    public class DocMember
    {
        private Assembly _currentAssembly;

        public DocMember(Assembly currentAssembly)
        {
            _currentAssembly = currentAssembly;
        }

        public string RawName
        {
            get { return rawName; }
            set
            {
                rawName = value;
                Process();
            }
        }

        public bool IsExtensionMethod { get; set; }

        public string Summary { get; set; }

        public string Namespace { get; private set; }

        public string ClassName { get; private set; }

        public string GenericType { get; private set; }

        public string Member { get; private set; }

        public string MemberType { get; private set; }

        public string MethodParams { get; private set; }

        public string Documentation { get; set; }

        public override string ToString()
        {

            return Member + GenericType + " " + MethodParams;
        }

        private string rawName;

        // ==================================================
        // Private
        // ==================================================

        private void Process()
        {
            if (rawName.IsNullOrEmpty())
                return;

            var temp = rawName;
            temp = GetMemberType(temp);
            temp = GetMethodParams(temp);

            var tempArray = temp.Split('.');
            tempArray = GetMember(tempArray);
            tempArray = GetClass(tempArray);
            GetNamespace(tempArray);
            GetParamsGenericType();

            // Use reflection to get underlying type
            GetMemberType();
        }

        private string GetMemberType(string temp)
        {
            if (temp.Substring(1, 1) == ":")
            {
                switch (temp.Substring(0, 1))
                {
                    case "M": MemberType = "Method";    break;
                    case "T": MemberType = "Type";      break;
                    case "F": MemberType = "Field";     break;
                    case "P": MemberType = "Property";  break;
                    case "C": MemberType = "Ctor";      break;
                    case "E": MemberType = "Events";    break;
                    default:  MemberType = null;        break;
                }
                temp = temp.Substring(2);
            }
            return temp;
        }

        private string GetMethodParams(string temp)
        {
            var tempArray = temp.Split('(');
            temp = tempArray[0];
            Member = temp;


            if (tempArray.Length > 1 && !tempArray[1].IsNullOrEmpty())
            {
                var prams = tempArray[1]
                    .RemoveStartEnd("(", ")")
                    .Split(',')
                    .Select(a => a.FromDotNetTypeToCSharpType());

                string pramsJoined  = string.Join(", ", prams);
                MethodParams = IsExtensionMethod ? "this " : "";
                MethodParams += pramsJoined.AddStartEnd("(", ")");
            }
            return temp;
        }

        private string[] GetMember(string[] tempArray)
        {
            if (tempArray.Length > 1)
            {
                Member = tempArray.LastOrDefault();
                Array.Resize(ref tempArray, tempArray.Length - 1); // Remove last item
            }
            return tempArray;
        }

        private string[] GetClass(string[] tempArray)
        {
            if (tempArray.Length > 1)
            {
                ClassName = tempArray.LastOrDefault();
                Array.Resize(ref tempArray, tempArray.Length - 1); // Remove last item
            }
            return tempArray;
        }

        private void GetNamespace(string[] tempArray)
        {
            Namespace = string.Join(".", tempArray);
        }

        private void GetParamsGenericType()
        {
            if (Member.IsNullOrEmpty())
                return;

            var s = Member.Split('`');
            if (s.Length > 1 && !s[1].IsNullOrEmpty())
            {
                GenericType = "<T>"; //s[1].AddStartEnd("<", ">");
            }
            Member = s[0];
        }

        private void GetMemberType()
        {
            string typeName = $"{Namespace}.{ClassName}, {_currentAssembly}";
            var type = Type.GetType(typeName);

            var x = type;
        }
    }
}
