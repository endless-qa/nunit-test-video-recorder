using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading;
using System;

namespace NunitVideoRecorder.Tests
{
    [TestFixture]
    public class OutputNamesTests
    {
        private int _defaultDelay = 1000;
        private string _videoFolderName = "Video";
        private string _extension = ".avi";
        private readonly string _defaultOutputPath = TestContext.CurrentContext.TestDirectory;
        private DirectoryInfo _classSubfolder;
        private DirectoryInfo _videoFolder;
        public const string CustomTestName = "MyTest";
        public const string IllegalTestName = " M:y/T*e||s<>t.?  ";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _videoFolder = new DirectoryInfo(Path.Combine(_defaultOutputPath, _videoFolderName));
            string className = TestContext.CurrentContext.Test.ClassName.Split('.').Last();
            _classSubfolder = new DirectoryInfo(Path.Combine(_defaultOutputPath, _videoFolderName, className));            
        }

        [TearDown]
        public void TearDown()
        {
            CleanVideoFolder();
        }

        [Test, Video]
        public void CheckAssigningDefaultOutputFileName()
        {
            string expectedName = TestContext.CurrentContext.Test.Name + _extension;
            Thread.Sleep(_defaultDelay);
            var actualFile = new DirectoryInfo(Path.Combine(_classSubfolder.FullName, expectedName));
            Assert.True(File.Exists(actualFile.FullName));
        }

        [Test, Video(Name = CustomTestName)]
        public void CheckAssigningCustomOutputFileName()
        {
            string expectedName = CustomTestName + _extension;
            Thread.Sleep(_defaultDelay);
            var actualFile = new DirectoryInfo(Path.Combine(_classSubfolder.FullName, expectedName));
            Assert.True(File.Exists(actualFile.FullName));
        }

        [Test, Video(Name = "")]
        public void CheckAssigningEmptyOutputFileName()
        {
            string expectedName = TestContext.CurrentContext.Test.Name + _extension;
            Thread.Sleep(_defaultDelay);
            var actualFile = new DirectoryInfo(Path.Combine(_classSubfolder.FullName, expectedName));
            Assert.True(File.Exists(actualFile.FullName));
        }

        [Test, Video(Name = IllegalTestName)]
        public void CheckAssigningIllegalOutputFileName()
        {
            string expectedName = CustomTestName + _extension;
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
