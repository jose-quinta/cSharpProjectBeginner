# CSharp Project Beginner

Solución de Visual Studio con múltiples proyectos junior de C#. Cada proyecto es un ejercicio independiente dentro de una misma solución y un mismo repositorio Git.

## Estructura

```
cSharpProjectBeginner/
├── cSharpProjectBeginner.sln     # Solución que agrupa todos los proyectos
├── .gitignore                    # Ignorado global para todos los proyectos
├── .git/                         # Un solo repositorio Git
├── README.md                     # Este documento
│
├── cSharpProjectBeginner.csproj  # Proyecto template base ("Hello, World!")
├── Program.cs
│
└── calculator/                   # Proyecto junior: Calculadora
    ├── calculator.csproj
    └── Program.cs
```

## Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) o superior
- Opcional: Visual Studio 2022, VS Code o JetBrains Rider

## Cómo ejecutar un proyecto

```bash
# Proyecto template base (por defecto)
dotnet run

# Proyecto específico (calculadora)
dotnet run --project calculator
```

## Cómo compilar todo

```bash
dotnet build
```

## Cómo agregar un nuevo proyecto junior

```bash
# 1. Crear el proyecto
dotnet new console -n nombre-del-proyecto
# Ejemplo: dotnet new console -n guess-number

# 2. Agregarlo a la solución
dotnet sln add nombre-del-proyecto/nombre-del-proyecto.csproj

# 3. Escribir el código en Program.cs
# 4. Probar con: dotnet run --project nombre-del-proyecto
```

### Comando rápido para crear y agregar en un solo paso

```bash
dotnet new console -n guess-number && dotnet sln add guess-number\guess-number.csproj
```

## Proyectos incluidos

| Proyecto | Descripción | Conceptos |
|----------|-------------|-----------|
| `cSharpProjectBeginner` | Template base ("Hello, World!") | Estructura inicial |
| `calculator` | Calculadora con menú interactivo | `while`, `switch`, `double.TryParse`, funciones |
| `guess-number` | Adivina el número secreto | `Random`, `while`, `if/else`, `int.TryParse` |

### Ideas para próximos proyectos

| Proyecto | Conceptos a practicar |
|----------|----------------------|
| **Adivina el número** | Bucles, `Random`, entrada/salida |
| **To-Do List (consola)** | Listas, clases, CRUD básico |
| **Tic-Tac-Toe** | Arrays bidimensionales, lógica de juego |
| **Conversor de unidades** | Métodos, enumeraciones |
| **Gestor de contactos** | Clases, listas, serialización JSON |
| **Juego de palabras** | `string`, manipulación de texto |
| **Simulador de dado** | `Random`, estadísticas básicas |

## Git

Este repositorio usa un solo `.git` para todos los proyectos. Cada cambio se registra en el mismo historial.

```bash
git status           # Ver cambios en todos los proyectos
git add .            # Agregar todo
git commit -m "feat: add calculator project"
git log --oneline    # Ver historial completo
```
