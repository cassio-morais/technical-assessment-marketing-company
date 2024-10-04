using LookupStoreFeature.Contracts;

namespace LookupStoreFeature
{
    public interface IFileWrappers
    {
        Contracts.FileInfo GetOneFileByClient(string clientIdentifier, FileType fileType);
        IEnumerable<string> FileReadLines(string path);
    }
}
