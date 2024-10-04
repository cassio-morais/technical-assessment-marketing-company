namespace LookupStoreFeature.Contracts
{
    internal interface IDocumentsProcessor
    {
        public DocumentLookupProcessingResult WordLookupProcessingInTxtFile(string clientIdentifier);
    }
}