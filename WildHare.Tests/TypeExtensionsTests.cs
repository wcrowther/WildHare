using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using WildHare.Models;
using WildHare.Tests.Interfaces;
using WildHare.Tests.Models;
using static System.Environment;
using static WildHare.Xtra.XtraExtensions;

namespace WildHare.Tests
{
	 [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void GetMetaModel_Basic()
        {
            var metaModel = typeof(Item).GetMetaModel();

            Assert.AreEqual("Item",     metaModel.TypeName);
            Assert.AreEqual(5,          metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId",   metaModel.PrimaryKeyName);

            var props = metaModel.GetMetaProperties();

            Assert.AreEqual("ItemId",   props[0].Name);
            Assert.AreEqual("ItemName", props[1].Name);
            Assert.AreEqual("Created",  props[2].Name);
            Assert.AreEqual("Stuff",    props[3].Name);

            Assert.AreEqual(typeof(int),            props[0].PropertyType);
            Assert.AreEqual(typeof(string),         props[1].PropertyType);
            Assert.AreEqual(typeof(DateTime),       props[2].PropertyType);
            Assert.AreEqual(typeof(List<string>),   props[3].PropertyType);
        }

		[Test]
		public void GetMetaModel_With_String()
		{
			string name = "Fred";
			var metaModel = name.GetMetaModel();

			Assert.AreEqual("String", metaModel.TypeName);
		}

		[Test]
        public void GetMetaModel_FromIEnumerable()
        {
            var itemList	= new List<Item>();
            var metaModel	= itemList.GetMetaModel();
            var props		= metaModel.GetMetaProperties();

            Assert.AreEqual(5,          props.Count);
            Assert.AreEqual("ItemId",   metaModel.PrimaryKeyName);

            Assert.AreEqual("ItemId",   props[0].Name);
            Assert.AreEqual("ItemName", props[1].Name);
            Assert.AreEqual("Created",  props[2].Name);
            Assert.AreEqual("Stuff",    props[3].Name);

            Assert.AreEqual(typeof(int),            props[0].PropertyType);
            Assert.AreEqual(typeof(string),         props[1].PropertyType);
            Assert.AreEqual(typeof(DateTime),       props[2].PropertyType);
            Assert.AreEqual(typeof(List<string>),   props[3].PropertyType);
        }

        [Test]
        public void GetMetaModel_FromInstance()
        {
            var item		= new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now };
            var metaModel	= item.GetMetaModel();
            var props		= metaModel.GetMetaProperties();

            Assert.AreEqual(5,          props.Count);
            Assert.AreEqual("ItemId",   metaModel.PrimaryKeyName);

            Assert.AreEqual("ItemId",   props[0].Name);
            Assert.AreEqual("ItemName", props[1].Name);
            Assert.AreEqual("Created",  props[2].Name);
            Assert.AreEqual("Stuff",    props[3].Name);

            Assert.AreEqual(typeof(int),            props[0].PropertyType);
            Assert.AreEqual(typeof(string),         props[1].PropertyType);
            Assert.AreEqual(typeof(DateTime),       props[2].PropertyType);
            Assert.AreEqual(typeof(List<string>),   props[3].PropertyType);
        }

        [Test]
        public void GetMetaModel_FromDictionary()
        {
            var dictionay = new Dictionary<string, Item>();
            var metaModel = dictionay.GetMetaModel();
            var props	  = metaModel.GetMetaProperties();

            Assert.AreEqual("String", metaModel.DictionaryKeyType.Name); // Not string (lowercase) for some reason
            Assert.AreEqual("Item",   metaModel.DictionaryValueType.Name);

            Assert.AreEqual(5,          props.Count);
            Assert.AreEqual("Comparer", props[0].Name);
            Assert.AreEqual("Count",    props[1].Name);
            Assert.AreEqual("Keys",     props[2].Name);
            Assert.AreEqual("Values",   props[3].Name);
            Assert.AreEqual("Item",     props[4].Name);
        }


        [Test]
        public void GetMetaModel_From_Model_Without_Parameterless_Constructor()
        {
            var metaListModel = new List<ClassRequiringCtorParam>().GetMetaModel();

            Assert.AreEqual(1, metaListModel.GetMetaProperties().Count);
            Assert.AreEqual("ClassName", metaListModel.GetMetaProperties().First().Name);

            var list = new List<ClassRequiringCtorParam> { new ClassRequiringCtorParam("Fred") };

            Assert.AreEqual("ClassRequiringCtorParam", list.GetMetaModel().TypeName);
            Assert.AreEqual("Fred", list.First().ClassName);
        }

        [Test]
        public void GetMetaProperties_Basic()
        {
            var item = new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now }; ;
            var metaProperties = item.GetMetaProperties();

            Assert.AreEqual(5,          metaProperties.Count);
            Assert.AreEqual("ItemId",   metaProperties[0].Name);
            Assert.AreEqual("ItemName", metaProperties[1].Name);
            Assert.AreEqual("Created",  metaProperties[2].Name);
            Assert.AreEqual("Stuff",    metaProperties[3].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Excluded()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties("ItemId, ItemName");

            Assert.AreEqual(3,          metaProperties.Count);
            Assert.AreEqual("Created",  metaProperties[0].Name);
            Assert.AreEqual("Stuff",    metaProperties[1].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Included()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties(include: "ItemName,Created");

            Assert.AreEqual(2, metaProperties.Count);
            Assert.AreEqual("ItemName", metaProperties[0].Name);
            Assert.AreEqual("Created", metaProperties[1].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Included_And_Excluded()
        {
            string errorMessage = "The GetMetaProperties method only accepts the exclude OR the include list.";
            var itemList = new List<Item>();

            var ex = Assert.Throws<Exception>
            (
                () => itemList.GetMetaProperties("ItemId", include: "ItemName,Created")
            );
            Assert.AreEqual(errorMessage, ex.Message);
        }

        [Test]
        public void MetaProperty_GetInstanceValue_From_Instance()
        {
            var now = DateTime.Now;
            var item = new Item
            {
                ItemId = 1,
                ItemName = "One",
                Created = now,
                Stuff = new List<string>() { "stuff1", "stuff2" }
            };
            var metaProperties = item.GetMetaProperties();

            Assert.AreEqual(1, metaProperties[0].GetInstanceValue());           // ItemId
            Assert.AreEqual("One", metaProperties[1].GetInstanceValue());      // ItemName
            Assert.AreEqual(now, metaProperties[2].GetInstanceValue());        // Created
            Assert.AreEqual(2, metaProperties[3].GetInstanceValue().Count);     // Stuff
            Assert.AreEqual("stuff1", metaProperties[3].GetInstanceValue()[0]); // Stuff1
            Assert.AreEqual("stuff2", metaProperties[3].GetInstanceValue()[1]); // Stuff2

            // For some reason ElementAt extension method does not work on dynamic List<Item> ?
            // Assert.AreEqual("stuff1", metaProperties[3].GetInstanceValue().ToList().ElementAt(0));
            var list = item.Stuff.ElementAt(0); // this works for non-dynamic
        }

        [Test]
        public void MetaProperty_GetInstanceValue_With_Instance_Injected_Into_Property()
        {
            var item = new Item
            {
                ItemName = "One"
            };

            PropertyInfo propertyInfo = typeof(Item).GetProperties()[1];
            var ItemNameMetaProperty = new MetaProperty(propertyInfo);

            Assert.AreEqual("One", ItemNameMetaProperty.GetInstanceValue(item)); // ItemName
        }

        [Test]
        public void MetaProperty_SetInstanceValue_From_Instance()
        {
            // THIS IS IMPORTANT WHEN TYPES ARE DYNAMIC AS TYPED PROPERTIES 
            // CAN BE SET WITHOUT HAVING TO KNOW THE UNDERLYING COMPILE-TIME TYPE

            var item = new Item
            {
                ItemId = 1,
                ItemName = "One"
            };

            var metaProperties = item.GetMetaProperties();
            metaProperties[0].SetInstanceValue(2);
            metaProperties[1].SetInstanceValue("Two");

            Assert.AreEqual(2, item.ItemId);
            Assert.AreEqual("Two", item.ItemName);
        }

        [Test]
        public void MetaProperty_SetInstanceValue_With_Instance_Injected_Into_Property()
        {
            var item = new Item
            {
                ItemName = "One"
            };

            PropertyInfo propertyInfo = typeof(Item).GetProperties()[1];
            var ItemName_MetaProperty = new MetaProperty(propertyInfo);

            ItemName_MetaProperty.SetInstanceValue("Two", item);

            Assert.AreEqual("Two", item.ItemName);

            // Alternate way
            PropertyInfo propertyInfoAlt = typeof(Item).GetProperties()[1];
            var ItemName_MetaPropertyAlt = new MetaProperty(propertyInfoAlt, item);

            ItemName_MetaPropertyAlt.SetInstanceValue("Three");

            Assert.AreEqual("Three", item.ItemName);
        }

        [Test]
        public void GetDerivedClasses_Basic()
        {
            var typesDerivedFromTeam = typeof(Team).GetDerivedTypes().ToList();

            Assert.AreEqual(6,                  typesDerivedFromTeam.Count());
            Assert.AreEqual("ArsenalTeam",      typesDerivedFromTeam[0].Name);
            Assert.AreEqual("BaseballTeam",     typesDerivedFromTeam[1].Name);
            Assert.AreEqual("FootballTeam",     typesDerivedFromTeam[2].Name);
            Assert.AreEqual("ManUnitedTeam",    typesDerivedFromTeam[3].Name);
            Assert.AreEqual("NflTeam",          typesDerivedFromTeam[4].Name);
            Assert.AreEqual("SoccerTeam",       typesDerivedFromTeam[5].Name);
        }

        [Test]
        public void GetDerivedClasses_Empty_Array()
        {
            // Has no derived classes
            var typesDerivedFromTeam = typeof(BaseballTeam).GetDerivedTypes().ToList();

            Assert.AreEqual(0, typesDerivedFromTeam.Count());
        }

        [Test]
        public void GetDerivedClasses_Ignore_Types()
        {
            var typesDerivedFromTeam = typeof(Team).GetDerivedTypes(new[] { "BaseballTeam", "NflTeam" }).ToList();

            Assert.AreEqual(4,                  typesDerivedFromTeam.Count());
            Assert.AreEqual("ArsenalTeam",      typesDerivedFromTeam[0].Name);
            Assert.AreEqual("FootballTeam",     typesDerivedFromTeam[1].Name);
            Assert.AreEqual("ManUnitedTeam",    typesDerivedFromTeam[2].Name);
            Assert.AreEqual("SoccerTeam",       typesDerivedFromTeam[3].Name);
            // BaseballTeam     excluded
            // NflTeam          excluded
        }

        [Test]
        public void GetDerivedClasses_IncludeBaseType()
        {
            var typesDerivedFromTeam = typeof(Team).GetDerivedTypes(includeBaseType: true).ToList();

            Assert.AreEqual(7,                  typesDerivedFromTeam.Count());
            Assert.AreEqual("ArsenalTeam",      typesDerivedFromTeam[0].Name);
            Assert.AreEqual("BaseballTeam",     typesDerivedFromTeam[1].Name);
            Assert.AreEqual("FootballTeam",     typesDerivedFromTeam[2].Name);
            Assert.AreEqual("ManUnitedTeam",    typesDerivedFromTeam[3].Name);
            Assert.AreEqual("NflTeam",          typesDerivedFromTeam[4].Name);
            Assert.AreEqual("SoccerTeam",       typesDerivedFromTeam[5].Name);
            Assert.AreEqual("Team",             typesDerivedFromTeam[6].Name); // Included
        }

        [Test]
        public void GetDerivedClasses_From_Different_Assembly()
        {
            // Type is in WildHare dll but find derived classes in current assembly
            var thisAssembly = Assembly.GetExecutingAssembly();
            var typesDerivedFromTest = typeof(TestModel).GetDerivedTypes(otherAssembly: thisAssembly).ToList();

            Assert.AreEqual(2, typesDerivedFromTest.Count());
            Assert.AreEqual("DerivedFromTestModel", typesDerivedFromTest[0].Name);
            Assert.AreEqual("DerivedFromTestModel2", typesDerivedFromTest[1].Name);
        }


        [Test]
        public void GetDerivedClasses_From_Different_Assembly_IncludeBaseType()
        {
            // Type TestModel is in WildHare dll but finds derived classes in current assembly
            var thisAssembly = Assembly.GetExecutingAssembly();
            var typesDerivedFromTest = typeof(TestModel).GetDerivedTypes(includeBaseType: true, otherAssembly: thisAssembly).ToList();

            Assert.AreEqual(3, typesDerivedFromTest.Count());
            Assert.AreEqual("DerivedFromTestModel", typesDerivedFromTest[0].Name);
            Assert.AreEqual("DerivedFromTestModel2", typesDerivedFromTest[1].Name);
            Assert.AreEqual("TestModel", typesDerivedFromTest[2].Name);
        }

        [Test]
        public void GetCommonBaseClass_SoccerTeam()
        {
            var manUnited = new ManUnitedTeam();
            var arsenal = new ArsenalTeam();
            var manCity = new SoccerTeam();
            var teams = new List<Team>
            {
                manUnited,
                arsenal,
                manCity
            };

            var teamTypes = teams.Select(s => s.GetType()).ToArray();
            var commonType = teamTypes.GetCommonBaseType();

            Assert.AreEqual("SoccerTeam", commonType.Name);
        }

        [Test]
        public void GetCommonBaseClass_Team()
        {
            var manUnited = new ManUnitedTeam();
            var arsenal = new ArsenalTeam();
            var manCity = new SoccerTeam();
            var falcons = new NflTeam();
            var teams = new List<Team>
            {
                manUnited,
                arsenal,
                manCity,
                falcons
            };

            var teamTypes = teams.Select(s => s.GetType()).ToArray();
            var commonType = teamTypes.GetCommonBaseType();

            Assert.AreEqual("Team", commonType.Name);
        }

        [Test]
        public void GetCommonBaseClass_object()
        {
            var manUnited = new ManUnitedTeam();
            var arsenal = new ArsenalTeam();
            var manCity = new SoccerTeam();
            var falcons = new NflTeam();
            var word = new Word();
            var teams = new List<object>
            {
                manUnited,
                arsenal,
                manCity,
                falcons,
                word
            };

            var teamTypes = teams.Select(s => s.GetType()).ToArray();
            var commonType = teamTypes.GetCommonBaseType();

            Assert.AreEqual("Object", commonType.Name);
        }

        [Test]
        public void GetCommonInterfaces_All_Fruit()
        {
            var objects = new List<object>
            {
                new Bannana(),
                new Apple(),
                new Pear(),
                new Orange(),
                new Fruit()
            }
            .ToArray();

            var interfaces = objects.GetCommonInterfaces();

            Assert.AreEqual(3, interfaces?.Count() ?? 0);
            Assert.AreEqual("I_Fruit", interfaces[0].Name);
        }

        [Test]
        public void GetCommonInterfaces_With_Common_I_Object()
        {
            var objects = new List<object>
            {
                new Fruit(),
                new Boat(),
                new Automobile()
            }
            .ToArray();

            var interfaces = objects.GetCommonInterfaces();

            Assert.AreEqual(1, interfaces?.Count() ?? 1);
            Assert.AreEqual("I_Object", interfaces[0].Name);
        }

        [Test]
        public void GetCommonInterfaces_With_Common_I_Object_GetSpecificity()
        {
            var objects = new List<object>
            {
                new Fruit(),
                new Boat(),
                new Automobile()
            }
            .ToArray();

            var interfaces = objects.GetCommonInterfaces();

            Assert.AreEqual(1, interfaces?.Count() ?? 1);
            Assert.AreEqual("I_Object", interfaces[0].Name);
            Assert.AreEqual(0m, interfaces[0].GetMethod("Specificity").Invoke(null, null));
        }

        [Test]
        public void GetCommonInterfaces_With_Common_I_Fruit_GetSpecificity()
        {
            var fruits = new List<object>
            {
                new Fruit(),
                new Apple(),
                new Pear()
            }
            .ToArray();

            var interfaces = fruits.GetCommonInterfaces();

            Assert.AreEqual(3, interfaces?.Count() ?? 3);
            Assert.AreEqual("I_Fruit", interfaces[0].Name);
            Assert.AreEqual(3m, interfaces[0].GetMethod("Specificity").Invoke(null, null));
        }

        [Test]
        public void GetCommonInterfaces_With_Common_I_Fruit_Sorted_By_Specificity()
        {
            var fruits = new List<object>
            {
                new Fruit(),
                new Apple(),
                new Pear()
            }
            .ToArray();

            var interfaces = fruits.GetCommonInterfaces()
                                    .OrderBy(o => o.GetMethod("Specificity").Invoke(null, null))
                                    .ToArray();

            Assert.AreEqual(3, interfaces?.Count() ?? 0);
            Assert.AreEqual("I_Object", interfaces[0].Name);
            Assert.AreEqual("I_Food", interfaces[1].Name);
            Assert.AreEqual("I_Fruit", interfaces[2].Name);
        }


        [Test]
        public void GetCommonInterfaces_With_Common_I_Fruit_With_Where_Sorted_By_Specificity()
        {
            var fruits = new List<object>
            {
                new Fruit(),
                new Apple(),
                new Pear()
            }
            .ToArray();

            var interfaces = fruits.GetCommonInterfaces()
                                    .Where(w => typeof(I_Object).IsAssignableFrom(w))
                                    .OrderBy(o => o.GetMethod("Specificity").Invoke(null, null))
                                    .ToArray();

            Assert.AreEqual(3, interfaces?.Count() ?? 0);
            Assert.AreEqual("I_Object", interfaces[0].Name);
            Assert.AreEqual("I_Food", interfaces[1].Name);
            Assert.AreEqual("I_Fruit", interfaces[2].Name);
        }

        [Test]
        public void Get_Generic_Type_With_Reflection()
        {
            Type testType = typeof(Dictionary<,>);

            Assert.AreEqual(true, testType.IsGenericType);
            Assert.AreEqual(true, testType.IsGenericTypeDefinition);

            Type[] typeParameters = testType.GetGenericArguments();

            Debug.WriteLine("   List {0} type arguments:", typeParameters.Length);
            foreach (Type tParam in typeParameters)
            {
                if (tParam.IsGenericParameter)
                {
                    Debug.WriteLine("      Type parameter: {0} position {1}", tParam.Name, tParam.GenericParameterPosition);
                }
                else
                {
                    Debug.WriteLine("      Type argument: {0}", tParam);
                }
            }

            Assert.AreEqual(2, typeParameters.Count());
        }

        [Test]
        public void GetMetaAssembly_Basic()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var metaAssembly = assembly.GetMetaAssembly();

            Assert.IsNotNull(metaAssembly);
            // Assert.AreEqual("I_Fruit", );
        }

        [Test]
        public void Test_GetTypesInNamespace_Basic()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypesInNamespace("WildHare.Tests.Models");

            Assert.IsNotNull(types);
            Assert.AreEqual(35, types.Count());
            Assert.AreEqual("Apple", types[1].Name);
        }

        [Test]
        public void Test_GetTypesInNamespace_ExcludeList()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypesInNamespace("WildHare.Tests.Models", "Account,Apple".Split(','));
        
            Assert.IsNotNull(types);
            Assert.AreEqual(33, types.Count());
            Assert.AreEqual("Automobile", types[1].Name);
        }

        [Test]
        public void Test_GetObject_Attributes()
        {
            var obj = new Item();
            var meta = obj.GetMetaModel();

            var attributes      = meta.Attributes();
            var serializable    = meta.AttributeOfType<SerializableAttribute>();
            var props           = meta.GetMetaProperties();

            Assert.AreEqual(1, attributes.Length);
            Assert.IsNotNull(serializable);
            Assert.AreEqual(5, props.Count);
            Assert.AreEqual(3, props[1].Attributes().Count());
            Assert.AreEqual("ItemName", props[1].Name);
            Assert.IsNotNull(props[1].AttributeOfType<MinLengthAttribute>());
            Assert.AreEqual(2, props[1].AttributeOfType<MinLengthAttribute>().Length);
            Assert.AreEqual(50, props[1].AttributeOfType<MaxLengthAttribute>().Length);
        }

        [Test]
        public void Test_Types_In_Assembly_And_Namespace_By_Member()
        {
            Type type    = typeof(Item);
            var typesInNamespace = type.GetAssemblyFromType()
                                       .GetTypesInNamespace(type.Namespace);

            Assert.AreEqual(35, typesInNamespace.Length);

            var typesInAssembly = type.GetAssemblyFromType()
                                      .GetTypesInNamespace();

            Assert.AreEqual(85, typesInAssembly.Length);
        }

        // [Test]
        // public void Test_Get_Type_From_String_TypeName()
        // {
        //     Type type = "WildHare.Tests.Models.Item".GetTypeFromName();
		// 
        //     Assert.IsNotNull(type); 
        //     Assert.AreEqual("Item", type.Name);
        // }

        //[Test, Ignore("CodeGen")]
        public void GetObject_Write_Attributes_ToString()
        {
            string   testRoot           = GetApplicationRoot();
            string   fileName           = "Validators.js";
            string   namespaceStr       = "WildHare.Tests.Models";
            string   pathToWriteTo      = $@"{testRoot}\TextFiles\{fileName}";
            string[] classesToExclude   = "Item, Account".Split(',');

            var assembly = Assembly.Load("WildHare.Tests");
            bool success = GenerateValidators(assembly, namespaceStr, pathToWriteTo, classesToExclude);

            Assert.IsTrue(success);
        }

        // =============================================================================================

        static List<string> validatorsList = new();

        private static bool GenerateValidators(Assembly assembly, string namespaceStr, string pathToWriteTo, string[] excludeClasses = null)
        {
            var sb          = new StringBuilder();
            var typeList    = assembly.GetTypesInNamespace(namespaceStr, excludeClasses); // exclude: 

            foreach (var type in typeList)
            {
                var meta = type.GetMetaModel();
                var props = meta.GetMetaProperties();
                var attributeCount = props.SelectMany(p => p.Attributes()).Count();

                if (attributeCount == 0)
                    continue;

                string classStr = $$"""
                    export const {{meta.TypeName}}Validator =
                    {
                    {{WriteProps(props, validatorsList)}}
                    }


                    """;

                sb.Append(classStr);
            }

            string listStr = validatorsList.Distinct().AsString();

            string output = sb.ToString()
                            .AddStart($"import {{ {listStr} }} from '@vuelidate/validators'{NewLine}{NewLine}");

            bool success = output.WriteToFile(pathToWriteTo, true);
            return success;
        }

        private static string WriteProps(List<MetaProperty> props, List<string> validatorsList)
        {
            const int pad = -20;
            var wp = new StringBuilder();

            foreach (var prop in props)
            {
                wp.Append($"\t{prop.Name + ":", pad}{{");
                wp.Append(WriteAttributes(prop, validatorsList).AddStartEnd(" "," "));
                wp.Append($"}},{NewLine}");
            }

            return wp.ToString().RemoveEnd("," + NewLine);
        }

        private static string WriteAttributes(MetaProperty prop, List<string> validatorsList)
        {
            var wa = new StringBuilder();

            foreach (var attr in prop.Attributes().OfType<Attribute>())
            {
                string attrStr = AttributeString(attr);

                validatorsList.Add(attrStr.GetStartBefore(":"));

                wa.Append(attrStr.AddEnd(", "));
            }

            return wa.ToString().RemoveEnd(", ");
        }

        private static string AttributeString(Attribute attribute) => attribute switch
        {
            RequiredAttribute   _ => $"required",
            MinLengthAttribute  _ => $"minLength: minLength({(attribute as MinLengthAttribute).Length})",
            MaxLengthAttribute  _ => $"maxLength: maxLength({(attribute as MaxLengthAttribute).Length})",
            null or _ => null
        };

        // =============================================================================================

    }
}


// [Test]
// public void Test_This()
// {
//     var fruit = new Fruit();
   
//     var interfaces = fruit.GetType().GetInterfaces();
//     var inter = interfaces.FirstOrDefault();
//     var interfaceName = inter.Name.RemoveStart("I_");
   
//     var i_object = (I_Object)fruit;
   
//     Assert.AreEqual("Fruit", interfaceName);
//     Assert.AreEqual( inter, typeof(I_Object));
//     Assert.IsNotNull(i_object);
   
//     // NOT WORKING YET - is I_Object but not I_Fruit
// }