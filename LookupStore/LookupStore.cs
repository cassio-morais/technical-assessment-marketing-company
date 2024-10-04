using LookupStoreFeature.Contracts;

namespace LookupStoreFeature
{
    public class LookupStore : ILookupStore
    {
        public void Record(string client, string documentId, IEnumerable<string> keywords)
        {
            // simulation saving on database...
            Console.WriteLine("Saving...");
            Console.WriteLine($"ProcessingID | ClientId | DocumentId | WordsFound");
            Console.WriteLine($"{Guid.NewGuid()} | {client} | {documentId} | {string.Join(",", keywords)}");
        }
    }
}
