using LookupStoreFeature.Contracts;

namespace LookupStoreFeature
{
    public class DocumentsProcessor : IDocumentsProcessor
    {
        private readonly IFileWrappers _fileWrappers;

        public DocumentsProcessor(IFileWrappers fileWrappers)
        {
            _fileWrappers = fileWrappers;
        }

        public DocumentLookupProcessingResult WordLookupProcessingInTxtFile(string clientIdentifier)
        {
            var fileInDirectory = _fileWrappers.GetOneFileByClient(clientIdentifier, FileType.Txt);

            if (fileInDirectory is null) // maybe exceptions is not the best way to handle this type of errors
                throw new InvalidOperationException("There's no file in directory");

            var wordsFound = new HashSet<string>();

            try
            {
                bool firstLine = true;
                var wordsSplitedToSearch = new string[] { };
                // maybe I can process this lines in parallel or even this all called
                foreach (var line in _fileWrappers.FileReadLines(fileInDirectory.Path))
                {
                    if (firstLine)
                    {
                        if (CanNotBeProcessed(line))
                            throw new InvalidOperationException("File does not ready to be processed");

                        wordsSplitedToSearch = GetWordsToSearch(line);

                        firstLine = false;
                    }

                    foreach (var word in wordsSplitedToSearch)
                    {
                        // I think that is not the best way to lookup a word here, maybe regex to handle outliers
                        if (line.Contains(word.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            wordsFound.Add(word.Trim());
                            Console.WriteLine(word + " found!");

                        }
                        Console.WriteLine("line searched: " + line);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            DeleteFile(fileInDirectory.Path);

            return new DocumentLookupProcessingResult(fileInDirectory.Identifier, wordsFound);
        }

        private void DeleteFile(string path)
        {
            // Fake delete (only for no new creation needed)
            Console.WriteLine($"Path to delete: {path}");
        }

        private static bool CanNotBeProcessed(string line)
        {
            return !line.StartsWith(ProcessingType.Lookup.ToString().ToLower() + "|");
        }

        private static string[] GetWordsToSearch(string line)
        {
            try
            {
                string[] wordsSplitedToSearch;
                var wordsStartAt1 = line.IndexOf("|") + 1;
                var wordsLength1 = line.Length - wordsStartAt1;
                var words1 = line.Substring(wordsStartAt1, wordsLength1);

                wordsSplitedToSearch = words1.Split(',');
                return wordsSplitedToSearch;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
