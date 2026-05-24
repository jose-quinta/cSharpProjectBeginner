# CSharp Project Beginner - Template Base

Este repositorio es un **template base** para crear proyectos junior de C#. Contiene un proyecto de consola minimalista en .NET 6 listo para ser usado como punto de partida para ejercicios de aprendizaje.

## Estructura del Proyecto

```
cSharpProjectBeginner/
├── Program.cs                    # Punto de entrada de la aplicación
├── cSharpProjectBeginner.csproj  # Archivo de configuración del proyecto
├── cSharpProjectBeginner.sln     # Archivo de solución de Visual Studio
├── .gitignore                    # Archivos ignorados por Git
└── README.md                     # Este documento
```

## Archivos Explicados

### Program.cs

```csharp
Console.WriteLine("Hello, World!");
```

- Usa **top-level statements** (C# 9+): no necesita `class Program` ni `static void Main`. El compilador genera el punto de entrada automáticamente.
- Se puede escribir código directamente sin envolverlo en clases o métodos.
- Para proyectos más grandes, se pueden agregar clases adicionales y llamarlas desde aquí.

### cSharpProjectBeginner.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

| Propiedad | Valor | Significado |
|-----------|-------|-------------|
| `OutputType` | `Exe` | Genera un ejecutable de consola |
| `TargetFramework` | `net6.0` | Apunta a .NET 6 (LTS) |
| `ImplicitUsings` | `enable` | Importa automáticamente `System`, `System.Linq`, `System.IO`, `System.Collections.Generic`, etc. |
| `Nullable` | `enable` | Habilita el análisis de tipos nullable para evitar `NullReferenceException` |

### cSharpProjectBeginner.sln

Archivo de solución que agrupa el proyecto. Permite abrir y gestionar el proyecto desde Visual Studio o JetBrains Rider. Actualmente contiene un solo proyecto (`cSharpProjectBeginner.csproj`).

### .gitignore

Ignora archivos generados por:
- **IDE**: `.vs/`, `.vscode/`, `.idea/`
- **Compilación**: `bin/`, `obj/`, `*.exe`, `*.dll`, `*.pdb`
- **NuGet**: `packages/`, `*.nupkg`
- **Sistema operativo**: `.DS_Store`, `Thumbs.db`

## Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) o superior
- Opcional: Visual Studio 2022, VS Code, o JetBrains Rider

## Cómo Usar Este Template

### Ejecutar el proyecto

```bash
dotnet run
```

Salida esperada:

```
Hello, World!
```

### Compilar el proyecto

```bash
dotnet build
```

### Publicar (generar un ejecutable standalone)

```bash
dotnet publish -c Release -o publish
```

## Cómo Crear un Nuevo Proyecto Junior Desde Este Template

### Opción 1: Clonar y reiniciar Git (recomendada)

```bash
# 1. Clonar el template
git clone <url-de-este-repo> mi-nuevo-proyecto
cd mi-nuevo-proyecto

# 2. Eliminar el historial Git del template
rm -rf .git

# 3. Inicializar un nuevo repositorio
git init
git add .
git commit -m "Initial commit: mi nuevo proyecto"

# 4. Conectar a un repositorio remoto nuevo
git remote add origin <url-del-nuevo-repo>
git push -u origin main
```

### Opción 2: Usar GitHub Template (si este repo está marcado como template)

1. Ir a `github.com/tu-usuario/cSharpProjectBeginner`
2. Click en **"Use this template"** → **"Create a new repository"**
3. Poner nombre al nuevo repositorio
4. Clonar el nuevo repositorio y empezar a trabajar

## Ideas de Proyectos Junior

| Proyecto | Conceptos a practicar |
|----------|----------------------|
| **Calculadora** | Variables, operadores, `if/else`, `switch`, funciones |
| **Adivina el número** | Bucles (`while`, `for`), `Random`, entrada/salida |
| **To-Do List (consola)** | Listas, clases, CRUD básico |
| **Tic-Tac-Toe** | Arrays bidimensionales, lógica de juego, validación |
| **Conversor de unidades** | Métodos, enumeraciones, `switch` |
| **Gestor de contactos** | Clases, listas, serialización JSON |
| **Juego de palabras** | `string`, `char`, manipulación de texto |
| **Simulador de dado** | `Random`, estadísticas básicas, bucles |

## Flujo de Trabajo Recomendado con Git

Para cada proyecto junior, se recomienda un repo independiente con este flujo:

```
1. Crear repo desde el template
2. Hacer commits pequeños por cada funcionalidad
3. Usar mensajes descriptivos:
   - "feat: add number guessing logic"
   - "fix: handle invalid input"
   - "refactor: extract validation to method"
4. Al terminar el ejercicio, hacer el último commit
```

## Comandos Útiles de Git

```bash
# Ver estado del repositorio
git status

# Añadir archivos al staging
git add .              # Todos los archivos
git add Program.cs     # Un archivo específico

# Hacer commit
git commit -m "mensaje descriptivo"

# Ver historial
git log --oneline

# Ver cambios realizados
git diff
```
