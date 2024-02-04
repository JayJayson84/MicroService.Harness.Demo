using System.Security.Cryptography;

namespace MicroService.Security.Encryption.Contracts;

public record EncryptionOptions
{
    public int? KeySize { get; set; }
    public int? BlockSize { get; set; }
    public int? DerivationIterations { get; set; }
    public CipherMode? CipherMode { get; set; }
    public PaddingMode? PaddingMode { get; set; }
}
