Random aleatorio = new();
int numeroSecreto = aleatorio.Next(1, 101);
int intentos = 0;
bool acertado = false;

Console.WriteLine("=== ADIVINA EL NÚMERO ===");
Console.WriteLine("He elegido un número entre 1 y 100.");
Console.WriteLine("¿Puedes adivinar cuál es?\n");

while (!acertado)
{
    Console.Write("Ingresa tu número: ");
    string entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out int guess))
    {
        Console.WriteLine("Por favor, ingresa un número válido.");
        continue;
    }

    intentos++;

    if (guess < numeroSecreto)
    {
        Console.WriteLine("El número es MAYOR.");
    }
    else if (guess > numeroSecreto)
    {
        Console.WriteLine("El número es MENOR.");
    }
    else
    {
        acertado = true;
        Console.WriteLine($"\n Felicidades! Adivinaste el {numeroSecreto} en {intentos} intentos.");
    }
}
