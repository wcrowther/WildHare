using System;

namespace WildHare.Tests.Models;

// There are several ways to do this but one way would be to have an abstract base class with inheritance and polymorphism (in c# here)

public abstract class Animal
{
	public abstract void Speak();
}

public class Cat : Animal
{
	public override void Speak() => Console.WriteLine("meow");
}

public class Dog : Animal
{
	public override void Speak() => Console.WriteLine("bark");
}

// Another solution would be to have an Interface like IAnimal that represents all the things an Animal can do:

public interface IAnimal
{
	void Speak();
}

public class Cat2 : IAnimal
{
	public void Speak() => Console.WriteLine("meow");
}

public class Dog2 : IAnimal
{
	public void Speak() => Console.WriteLine("bark");
}
