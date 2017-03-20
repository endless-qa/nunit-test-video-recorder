using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading;

namespace NunitVideoRecorder.Tests
{
    [TestFixture, WatchDog(SaveInClass.AllTests)]
    class OutputModesTests
    {
        private int _defaultDelay = 1000;
        private string _videoFolderName = "Video";
        private string _extension = ".avi";
        private readonly string _defaultOutputPath = TestContext.CurrentContext.TestDirectory;
        private DirectoryInfo _classSubfolder;
        private DirectoryInfo _videoFolder;
        public const string CustomTestNameOne = "MyTestOne";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _videoFolder = new DirectoryInfo(Path.Combine(_defaultOutputPath, _videoFolderName));
            string className = TestContext.CurrentContext.Test.ClassName.Split('.').Last();
            _classSubfolder = new DirectoryInfo(Path.Combine(_defaultOutputPath, _videoFolderName, className));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CleanVideoFolder();
        }

        [Test, Order(0), Video(Name = CustomTestNameOne, Mode = SaveMe.OnlyWhenFailed)]
        public void FailedTestPrecondition()
        {
            Thread.Sleep(_defaultDelay);
            Assert.Pass();
        }

        [Test, Order(1)]
        public void CheckFailedModeForPassedTest()
        {
            string expectedName = CustomTestNameOne + _extension;
            var actualFile = new DirectoryInfo(Path.Combine(_classSubfolder.FullName, expectedName));
            Assert.False(File.Exists(actualFile.FullName));
        }

        [Test]
        public void CheckSavingAllOnClassLevel()
        {
            string expectedName = TestContext.CurrentContext.Test.Name + _extension;
            Thread.Sleep(_defaultDelay);
            var actualFile = new DirectoryInfo(Path.Combine(_classSubfolder.FullName, expectedName));
            Assert.True(File.Exists(actualFile.FullName));
        }

        private void CleanVideoFolder()
        {
            foreach (FileInfo file in _videoFolder.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo subFolder in _videoFolder.GetDirectories())
            {
                subFolder.Delete(true);
            }
        }
    }
}
