namespace LookupStoreFeature.Contracts
{
    public record DocumentLookupProcessingResult(string DocumentId, IEnumerable<string> WordsFound);
}