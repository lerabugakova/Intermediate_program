// See https://aka.ms/new-console-template for more information
using FirstTask;
using System.Security.Cryptography;

Console.WriteLine("Start!");
var test = new ClassToRefactor();
var password = "MySecurePassword123!";

var salt = new byte[16];
using var rng = RandomNumberGenerator.Create();
rng.GetBytes(salt);

var hashedPassword = test.GeneratePasswordHashUsingSalt(password, salt);

Console.WriteLine("Salt (Base64): " + Convert.ToBase64String(salt));
Console.WriteLine("Password Hash: " + hashedPassword);
Console.WriteLine("Done");
Console.ReadLine();