using System.Security.Cryptography;
using file_encryption_decryption.Abstractions;

namespace file_encryption_decryption.Services;

public class EncryptionService : IEncryptionService
{
    private const int SaltSize = 16;
    private const int IvSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public void EncryptFile(string inputPath, string outputPath, string password)
    {
        byte[] plaintext = File.ReadAllBytes(inputPath);
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] key = DeriveKey(password, salt, KeySize);
        byte[] iv = RandomNumberGenerator.GetBytes(IvSize);

        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor();
        byte[] ciphertext = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);

        using var fs = new FileStream(outputPath, FileMode.Create);
        fs.Write(salt, 0, salt.Length);
        fs.Write(iv, 0, iv.Length);
        fs.Write(ciphertext, 0, ciphertext.Length);
    }

    public void DecryptFile(string inputPath, string outputPath, string password)
    {
        byte[] fileBytes = File.ReadAllBytes(inputPath);

        if (fileBytes.Length < SaltSize + IvSize)
            throw new InvalidDataException("El archivo cifrado est\u00e1 corrupto o no es v\u00e1lido.");

        var salt = new byte[SaltSize];
        var iv = new byte[IvSize];
        Buffer.BlockCopy(fileBytes, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(fileBytes, SaltSize, iv, 0, IvSize);

        int ciphertextOffset = SaltSize + IvSize;
        int ciphertextLength = fileBytes.Length - ciphertextOffset;
        var ciphertext = new byte[ciphertextLength];
        Buffer.BlockCopy(fileBytes, ciphertextOffset, ciphertext, 0, ciphertextLength);

        byte[] key = DeriveKey(password, salt, KeySize);

        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] plaintext = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

        File.WriteAllBytes(outputPath, plaintext);
    }

    private static byte[] DeriveKey(string password, byte[] salt, int keySize)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(keySize);
    }
}
