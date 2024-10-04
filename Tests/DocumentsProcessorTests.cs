using LookupStoreFeature;
using LookupStoreFeature.Contracts;
using Moq;
using FileInfo = LookupStoreFeature.Contracts.FileInfo;

namespace Tests
{
    public class DocumentsProcessorTests
    {
        [Fact]
        public void DocumentsProcessor_WordLookupProcessingInTxtFile_ShouldThrowsAnException_WhenFileDoestNotExist()
        {
            var fileWrapperMock = new Mock<IFileWrappers>();

            fileWrapperMock.Setup(x => x.GetOneFileByClient(It.IsAny<string>(), It.IsAny<FileType>())).Returns((FileInfo)null);

            var processor = new DocumentsProcessor(fileWrapperMock.Object);

            // TODO: test the messages sentences to enhance this test
            Assert.Throws<InvalidOperationException>(() => processor.WordLookupProcessingInTxtFile("some_client_identifier"));
        }

        [Fact]
        public void DocumentsProcessor_WordLookupProcessingInTxtFile_ShouldThrowsAnException_WhenFileDoestNotHaveCorrectFirstLine()
        {
            var fileWrapperMock = new Mock<IFileWrappers>();
            var malformedFirstLine = "malformedFirstLine";

            fileWrapperMock.Setup(x => x.GetOneFileByClient(It.IsAny<string>(), It.IsAny<FileType>())).Returns(new FileInfo("\\path\\file.txt", "123"));

            fileWrapperMock.Setup(x => x.FileReadLines(It.IsAny<string>())).Returns([malformedFirstLine]);

            var processor = new DocumentsProcessor(fileWrapperMock.Object);

            // TODO: test the messages sentences to enhance this test
            Assert.Throws<InvalidOperationException>(() => processor.WordLookupProcessingInTxtFile("some_client_identifier"));
        }

        [Fact]
        public void DocumentsProcessor_WordLookupProcessingInTxtFile_ShouldProcessWordsCorrectly()
        {
            var documentIdExpected = "123";
            var fileWrapperMock = new Mock<IFileWrappers>();
            var file = new string[] { "lookup|pop", "papa pop", "pop pobre", "pedra ilha" };

            fileWrapperMock.Setup(x => x.GetOneFileByClient(It.IsAny<string>(), It.IsAny<FileType>())).Returns(new FileInfo($"\\path\\{documentIdExpected}_file.txt", documentIdExpected));

            fileWrapperMock.Setup(x => x.FileReadLines(It.IsAny<string>())).Returns(file);

            var processor = new DocumentsProcessor(fileWrapperMock.Object);

            var result = processor.WordLookupProcessingInTxtFile("some_client_identifier");

            Assert.Equal(documentIdExpected, result.DocumentId);
            Assert.Equal(["pop"], result.WordsFound);
        }

        // I know, is necessary put more tests here...
    }
}
