namespace MicroService.Security.Encryption.Contracts;

public record EncryptionContract
{
    public string Value { get; init; } = null!;
    public string Key { get; init; } = null!;
    public string? Salt { get; init; }
    public EncryptionOperation EncryptionOperation { get; init; }
    public EncryptionMethod EncryptionMethod { get; init; }
    public EncryptionOptions? EncryptionOptions { get; init; }
}