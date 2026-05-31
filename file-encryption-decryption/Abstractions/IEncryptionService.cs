namespace file_encryption_decryption.Abstractions;

public interface IEncryptionService
{
    void EncryptFile(string inputPath, string outputPath, string password);
    void DecryptFile(string inputPath, string outputPath, string password);
}
