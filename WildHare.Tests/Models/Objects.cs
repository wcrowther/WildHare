using System;
using System.Collections.Generic;
using System.Text;
using WildHare.Tests.Interfaces;

namespace WildHare.Tests.Models
{
    public class Fruit : I_Fruit, I_Object, I_Food
    {}

    public class Bannana : I_Fruit
    { }

    public class Apple : I_Fruit
    { }

    public class Pear : I_Fruit
    { }

    public class Orange : I_Fruit
    { }

    public class Boat : I_Transportation, I_Object
    { }

    public class Tractor : I_Transportation, I_Object
    { }

    public class Automobile : I_Transportation, I_Object
    { }
}
