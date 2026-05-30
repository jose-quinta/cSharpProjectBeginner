using temperature_converter.Abstractions;
using temperature_converter.Models;
using temperature_converter.Services;

ITemperatureService service = new TemperatureService();
ConversionHistoryService history = new ConversionHistoryService();
MenuService menu = new MenuService();

bool salir = false;
while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMenu();
    string opcion = menu.GetChoice();
    Console.WriteLine();

    if (string.IsNullOrEmpty(opcion))
    {
        menu.ShowError("Opci\u00f3n inv\u00e1lida.");
        menu.Pause();
        continue;
    }

    if (opcion == "6")
    {
        if (menu.ConfirmExit())
            salir = true;
        continue;
    }

    try
    {
        switch (opcion)
        {
            case "1":
            {
                double value = menu.GetTemperature();
                double result = service.Convert(value, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit);
                string formula = service.GetFormula(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit);
                menu.ShowResult(value, TemperatureUnit.Celsius, result, TemperatureUnit.Fahrenheit, formula);
                history.AddRecord(new ConversionRecord
                {
                    InputValue = value,
                    InputUnit = TemperatureUnit.Celsius,
                    OutputValue = Math.Round(result, 2),
                    OutputUnit = TemperatureUnit.Fahrenheit,
                    Formula = formula,
                    Timestamp = DateTime.Now
                });
                break;
            }
            case "2":
            {
                double value = menu.GetTemperature();
                double result = service.Convert(value, TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius);
                string formula = service.GetFormula(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius);
                menu.ShowResult(value, TemperatureUnit.Fahrenheit, result, TemperatureUnit.Celsius, formula);
                history.AddRecord(new ConversionRecord
                {
                    InputValue = value,
                    InputUnit = TemperatureUnit.Fahrenheit,
                    OutputValue = Math.Round(result, 2),
                    OutputUnit = TemperatureUnit.Celsius,
                    Formula = formula,
                    Timestamp = DateTime.Now
                });
                break;
            }
            case "3":
            {
                double value = menu.GetTemperature();
                double result = service.Convert(value, TemperatureUnit.Celsius, TemperatureUnit.Kelvin);
                string formula = service.GetFormula(TemperatureUnit.Celsius, TemperatureUnit.Kelvin);
                menu.ShowResult(value, TemperatureUnit.Celsius, result, TemperatureUnit.Kelvin, formula);
                history.AddRecord(new ConversionRecord
                {
                    InputValue = value,
                    InputUnit = TemperatureUnit.Celsius,
                    OutputValue = Math.Round(result, 2),
                    OutputUnit = TemperatureUnit.Kelvin,
                    Formula = formula,
                    Timestamp = DateTime.Now
                });
                break;
            }
            case "4":
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Unidad de origen:");
                Console.ResetColor();
                Console.WriteLine(" [1] Celsius   [2] Fahrenheit   [3] Kelvin");
                Console.Write(" Seleccione: ");
                string unitChoice = Console.ReadLine()?.Trim() ?? "";
                TemperatureUnit from = unitChoice switch
                {
                    "1" => TemperatureUnit.Celsius,
                    "2" => TemperatureUnit.Fahrenheit,
                    "3" => TemperatureUnit.Kelvin,
                    _ => TemperatureUnit.Celsius
                };

                double value = menu.GetTemperature();
                var results = service.ConvertAll(value, from);
                menu.ShowAllConversions(value, from, results);
                break;
            }
            case "5":
            {
                List<ConversionRecord> records = history.Load();
                menu.ShowHistory(records);
                if (records.Count > 0)
                {
                    Console.Write(" Ver detalle (n\u00famero) o Enter para continuar: ");
                    string? input = Console.ReadLine()?.Trim();
                    if (int.TryParse(input, out int idx) && idx >= 1 && idx <= records.Count)
                    {
                        menu.ShowConversionDetail(records[idx - 1]);
                    }
                }
                break;
            }
        }

        if (opcion != "6")
            menu.Pause();
    }
    catch (ArgumentException ex)
    {
        menu.ShowError(ex.Message);
        menu.Pause();
    }
    catch (Exception ex)
    {
        menu.ShowError($"Error inesperado: {ex.Message}");
        menu.Pause();
    }
}
