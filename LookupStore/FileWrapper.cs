using LookupStoreFeature.Contracts;

namespace LookupStoreFeature
{
    public class FileWrappers : IFileWrappers
    {
        private const string fakeClientStorage = "FakeClientsStorage";

        public IEnumerable<string> FileReadLines(string path)
        {
            return File.ReadLines(path);
        }

        public Contracts.FileInfo GetOneFileByClient(string clientIdentifier, FileType fileType)
        {
            try
            {
                var directory = Directory.GetCurrentDirectory();
                string relativePath = $"..\\..\\..\\{fakeClientStorage}\\{clientIdentifier}";
                string fullPath = Path.Combine(directory, relativePath);
                var lengthIdentifier = 3;

                var extension = fileType.ToString().ToLower();
                var filesInDirectory = Directory.GetFiles(fullPath, $"*.{extension}");

                if (!filesInDirectory.Any())
                    return null!;

                var documentIdentifier = filesInDirectory[0].Substring(filesInDirectory[0].LastIndexOf("\\") + 1, lengthIdentifier); // very rudimentary, just for example

                return new Contracts.FileInfo(filesInDirectory[0], documentIdentifier);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
