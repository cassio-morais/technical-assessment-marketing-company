using LookupStoreFeature;

var someArbitraryClient = "64208919-A745-4054-A5DA-8D46AF5D3A13";

var documentsProcessor = new DocumentsProcessor(new FileWrappers());

var result = documentsProcessor.WordLookupProcessingInTxtFile(someArbitraryClient);

var lookupStore = new LookupStore();

lookupStore.Record(someArbitraryClient, result.DocumentId, result.WordsFound);