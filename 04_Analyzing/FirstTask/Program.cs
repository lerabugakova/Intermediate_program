// See https://aka.ms/new-console-template for more information
using FirstTask;

Console.WriteLine("Start!");
var test = new ClassToRefactor();
test.GeneratePasswordHashUsingSalt();
Console.WriteLine("Done");
Console.ReadLine();

