namespace ChecksumCorrector.ConsoleApp
{
    public record AppSettings
    {
        public string[]? PathFiles { get; init; } = Array.Empty<string>();
    }
}
