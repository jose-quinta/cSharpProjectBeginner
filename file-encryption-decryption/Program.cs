using System.Security.Cryptography;
using file_encryption_decryption.Abstractions;
using file_encryption_decryption.Models;
using file_encryption_decryption.Services;

IEncryptionService encryption = new EncryptionService();
FileDialogService dialog = new FileDialogService();
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

    if (opcion == "5")
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
                List<string> files = dialog.ListTextFiles();
                string? inputPath = dialog.SelectFile(files, "Archivos .txt disponibles:");

                if (inputPath == null)
                {
                    menu.ShowError("No hay archivos .txt o la ruta es inv\u00e1lida.");
                    break;
                }

                string password = dialog.GetPassword();
                string outputPath = dialog.GetOutputPath(inputPath, OperationType.Encrypt);

                var fileInfo = new FileInfo(inputPath);
                encryption.EncryptFile(inputPath, outputPath, password);

                var encInfo = new FileInfo(outputPath);
                history.AddRecord(new FileOperation
                {
                    FileName = Path.GetFileName(inputPath),
                    Operation = OperationType.Encrypt,
                    Success = true,
                    Message = $"Archivo cifrado: {outputPath}",
                    FileSize = encInfo.Length,
                    Timestamp = DateTime.Now
                });
                menu.ShowResult(true, "Cifrado exitoso.", outputPath, encInfo.Length);
                break;
            }
            case "2":
            {
                List<string> files = dialog.ListEncFiles();
                string? inputPath = dialog.SelectFile(files, "Archivos .enc disponibles:");

                if (inputPath == null)
                {
                    menu.ShowError("No hay archivos .enc o la ruta es inv\u00e1lida.");
                    break;
                }

                string password = dialog.GetPasswordForDecrypt();
                string outputPath = dialog.GetOutputPath(inputPath, OperationType.Decrypt);

                var fileInfo = new FileInfo(inputPath);
                encryption.DecryptFile(inputPath, outputPath, password);

                var decInfo = new FileInfo(outputPath);
                history.AddRecord(new FileOperation
                {
                    FileName = Path.GetFileName(inputPath),
                    Operation = OperationType.Decrypt,
                    Success = true,
                    Message = $"Archivo descifrado: {outputPath}",
                    FileSize = decInfo.Length,
                    Timestamp = DateTime.Now
                });
                menu.ShowResult(true, "Descifrado exitoso.", outputPath, decInfo.Length);
                break;
            }
            case "3":
            {
                List<FileOperation> records = history.Load();
                menu.ShowHistory(records);
                break;
            }
            case "4":
            {
                if (menu.ConfirmAction("\u00bfLimpiar todo el historial"))
                {
                    history.Clear();
                    menu.ShowClearHistory();
                }
                break;
            }
        }

        if (opcion != "5")
            menu.Pause();
    }
    catch (CryptographicException)
    {
        menu.ShowError("Contrase\u00f1a incorrecta o archivo corrupto.");
        history.AddRecord(new FileOperation
        {
            FileName = "desconocido",
            Operation = opcion == "2" ? OperationType.Decrypt : OperationType.Encrypt,
            Success = false,
            Message = "Error de clave/archivo corrupto",
            Timestamp = DateTime.Now
        });
        menu.Pause();
    }
    catch (InvalidDataException ex)
    {
        menu.ShowError(ex.Message);
        menu.Pause();
    }
    catch (InvalidOperationException ex)
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
