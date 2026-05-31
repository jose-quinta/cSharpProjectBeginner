namespace file_encryption_decryption.Models;

public class FileOperation
{
    public string FileName { get; set; } = string.Empty;
    public OperationType Operation { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
