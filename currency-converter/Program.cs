using currency_converter.Abstractions;
using currency_converter.Models;
using currency_converter.Services;

IRateService rateService = new RateService();
HistoryService history = new HistoryService();
MenuService menu = new MenuService();

bool salir = false;
while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMainMenu();
    string opcion = menu.GetChoice();
    Console.WriteLine();

    if (string.IsNullOrEmpty(opcion))
    {
        menu.ShowError("Opci\u00f3n inv\u00e1lida.");
        menu.Pause();
        continue;
    }

    if (opcion == "7")
    {
        if (menu.ConfirmAction("\u00bfSalir"))
            salir = true;
        continue;
    }

    try
    {
        switch (opcion)
        {
            case "1":
            {
                decimal amount = menu.GetAmount();
                CurrencyCode from = menu.GetCurrency("Moneda de origen:");
                CurrencyCode to = menu.GetCurrency("Moneda de destino:");
                decimal result = rateService.Convert(amount, from, to);

                double rate = rateService.CurrentRates != null
                    ? (double)result / (double)amount
                    : 0;

                var record = new ConversionRecord
                {
                    Amount = amount,
                    From = from,
                    To = to,
                    Result = result,
                    Rate = Math.Round(rate, 6),
                    Timestamp = DateTime.Now
                };

                menu.ShowConversionResult(record);
                history.AddRecord(record);
                break;
            }
            case "2":
            {
                CurrencyCode baseCurrency = CurrencyCode.USD;
                ExchangeRate rates = rateService.GetRates(baseCurrency);
                menu.ShowRateTable(rates);
                break;
            }
            case "3":
            {
                CurrencyCode baseCurrency = menu.GetCurrency("Nueva moneda base:");
                ExchangeRate rates = rateService.GetRates(baseCurrency);
                menu.ShowRateTable(rates);
                break;
            }
            case "4":
            {
                rateService.RefreshRates();
                menu.ShowRefreshMessage(rateService.CurrentRates?.LastUpdated ?? DateTime.Now);
                break;
            }
            case "5":
            {
                List<ConversionRecord> records = history.Load();
                menu.ShowHistory(records);
                break;
            }
            case "6":
            {
                ExchangeRate rates = rateService.GetRates(CurrencyCode.USD);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"rates_export_{timestamp}.txt";
                var lines = new List<string>
                {
                    "========================================",
                    "  CURRENCY CONVERTER - TASAS DE CAMBIO",
                    "========================================",
                    $"  Base: USD",
                    $"  Exportado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                    "========================================",
                    ""
                };
                foreach (var kvp in rates.Rates.OrderBy(r => (int)r.Key))
                {
                    lines.Add($"  {kvp.Key,-6} {MenuService.CurrencySymbol(kvp.Key)} {MenuService.CurrencyName(kvp.Key),-22} {kvp.Value,12:F6}");
                }
                lines.Add("");
                lines.Add("========================================");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), string.Join(Environment.NewLine, lines));
                menu.ShowExportSuccess(fileName);
                break;
            }
        }

        if (opcion != "7")
            menu.Pause();
    }
    catch (Exception ex)
    {
        menu.ShowError($"Error inesperado: {ex.Message}");
        menu.Pause();
    }
}
