using System.Text.Json;
using basic_quiz_application.Models;

namespace basic_quiz_application.Services;

public class QuestionLoaderService
{
    private static readonly string DataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
    private static readonly string FilePath = Path.Combine(DataDir, "questions.json");

    public List<Question> Load()
    {
        if (!Directory.Exists(DataDir))
            Directory.CreateDirectory(DataDir);

        if (!File.Exists(FilePath))
        {
            List<Question> defaults = GetDefaultQuestions();
            Save(defaults);
            return defaults;
        }

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Question>>(json) ?? GetDefaultQuestions();
    }

    public void Save(List<Question> questions)
    {
        if (!Directory.Exists(DataDir))
            Directory.CreateDirectory(DataDir);

        string json = JsonSerializer.Serialize(questions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public List<string> GetCategories(List<Question> questions)
    {
        return questions
            .Select(q => q.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }

    public static List<Question> GetDefaultQuestions()
    {
        return new List<Question>
        {
            // Matemáticas
            new() { Text = "¿Cuál es la raíz cuadrada de 144?", Options = new() { "10", "11", "12", "14" }, CorrectIndex = 2, Category = "Matemáticas", Difficulty = "Fácil" },
            new() { Text = "¿Cuánto es 15 × 15?", Options = new() { "200", "215", "225", "250" }, CorrectIndex = 2, Category = "Matemáticas", Difficulty = "Fácil" },
            new() { Text = "¿Cuál es el resultado de 2^10?", Options = new() { "512", "1024", "2048", "256" }, CorrectIndex = 1, Category = "Matemáticas", Difficulty = "Media" },
            new() { Text = "¿Cuál es el valor de π (pi) aproximado a 2 decimales?", Options = new() { "3.14", "3.16", "3.12", "3.18" }, CorrectIndex = 0, Category = "Matemáticas", Difficulty = "Fácil" },

            // Ciencia
            new() { Text = "¿Qué planeta es conocido como el 'Planeta Rojo'?", Options = new() { "Venus", "Júpiter", "Marte", "Saturno" }, CorrectIndex = 2, Category = "Ciencia", Difficulty = "Fácil" },
            new() { Text = "¿Cuál es el elemento químico más abundante en el universo?", Options = new() { "Oxígeno", "Hidrógeno", "Carbono", "Helio" }, CorrectIndex = 1, Category = "Ciencia", Difficulty = "Media" },
            new() { Text = "¿A qué temperatura hierve el agua a nivel del mar?", Options = new() { "90°C", "100°C", "110°C", "120°C" }, CorrectIndex = 1, Category = "Ciencia", Difficulty = "Fácil" },
            new() { Text = "¿Cuántos huesos tiene el cuerpo humano adulto?", Options = new() { "186", "196", "206", "216" }, CorrectIndex = 2, Category = "Ciencia", Difficulty = "Media" },

            // Historia
            new() { Text = "¿En qué año llegó Cristóbal Colón a América?", Options = new() { "1490", "1492", "1498", "1502" }, CorrectIndex = 1, Category = "Historia", Difficulty = "Fácil" },
            new() { Text = "¿Quién fue el primer presidente de Estados Unidos?", Options = new() { "Thomas Jefferson", "George Washington", "Abraham Lincoln", "John Adams" }, CorrectIndex = 1, Category = "Historia", Difficulty = "Fácil" },
            new() { Text = "¿En qué año cayó el Muro de Berlín?", Options = new() { "1987", "1988", "1989", "1990" }, CorrectIndex = 2, Category = "Historia", Difficulty = "Media" },
            new() { Text = "¿Qué civilización construyó Machu Picchu?", Options = new() { "Azteca", "Maya", "Inca", "Olmeca" }, CorrectIndex = 2, Category = "Historia", Difficulty = "Fácil" },

            // Geografía
            new() { Text = "¿Cuál es el río más largo del mundo?", Options = new() { "Amazonas", "Nilo", "Misisipi", "Yangtsé" }, CorrectIndex = 0, Category = "Geografía", Difficulty = "Media" },
            new() { Text = "¿Cuál es la capital de Australia?", Options = new() { "Sídney", "Melbourne", "Canberra", "Brisbane" }, CorrectIndex = 2, Category = "Geografía", Difficulty = "Media" },
            new() { Text = "¿Cuántos países tiene América del Sur?", Options = new() { "10", "11", "12", "13" }, CorrectIndex = 2, Category = "Geografía", Difficulty = "Difícil" },
            new() { Text = "¿Cuál es el país más grande del mundo por área?", Options = new() { "China", "Estados Unidos", "Canadá", "Rusia" }, CorrectIndex = 3, Category = "Geografía", Difficulty = "Fácil" },

            // Tecnología
            new() { Text = "¿Quién fundó Microsoft?", Options = new() { "Steve Jobs", "Bill Gates", "Mark Zuckerberg", "Jeff Bezos" }, CorrectIndex = 1, Category = "Tecnología", Difficulty = "Fácil" },
            new() { Text = "¿Qué significa 'HTTP'?", Options = new() { "HyperText Transfer Protocol", "High Tech Transfer Process", "HyperText Transmission Program", "High Transfer Text Protocol" }, CorrectIndex = 0, Category = "Tecnología", Difficulty = "Fácil" },
            new() { Text = "¿En qué año se lanzó el primer iPhone?", Options = new() { "2005", "2006", "2007", "2008" }, CorrectIndex = 2, Category = "Tecnología", Difficulty = "Media" },
            new() { Text = "¿Cuál es el lenguaje de programación más antiguo de los siguientes?", Options = new() { "Python", "Java", "C", "JavaScript" }, CorrectIndex = 2, Category = "Tecnología", Difficulty = "Media" },

            // Cultura General
            new() { Text = "¿Cuál es la obra más famosa de Shakespeare?", Options = new() { "Hamlet", "Romeo y Julieta", "Macbeth", "Otelo" }, CorrectIndex = 1, Category = "Cultura General", Difficulty = "Fácil" },
            new() { Text = "¿Quién pintó la Mona Lisa?", Options = new() { "Miguel Ángel", "Rafael", "Leonardo da Vinci", "Donatello" }, CorrectIndex = 2, Category = "Cultura General", Difficulty = "Fácil" },
            new() { Text = "¿Cuál es el deporte más popular del mundo?", Options = new() { "Baloncesto", "Fútbol", "Tenis", "Cricket" }, CorrectIndex = 1, Category = "Cultura General", Difficulty = "Fácil" },
            new() { Text = "¿Cuántos anillos olímpicos hay?", Options = new() { "3", "4", "5", "6" }, CorrectIndex = 2, Category = "Cultura General", Difficulty = "Fácil" },
        };
    }
}
