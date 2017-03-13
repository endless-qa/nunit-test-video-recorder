using System.IO;
using NUnit.Framework;
using System.Linq;

namespace NunitVideoRecorder.Internal
{
    public class RecorderFactory
    {
        private const string DefaultExtension = ".avi";
        private const string VideoFolderName = "Video";
        private readonly string _defaultOutputPath = TestContext.CurrentContext.TestDirectory;
        private readonly VideoConfigurator _configurator;

        private RecorderFactory(VideoConfigurator configurator)
        {
            _configurator = configurator;
        }

        public static RecorderFactory Instance => new RecorderFactory(new VideoConfigurator());

        public Recorder Create(string videoName)
        {
            var fileName = RemoveForbiddenSymbols(videoName) + DefaultExtension;

            var classSubFolder = PrepareClassSubfolder();

            var path = Path.Combine(classSubFolder, fileName);

            var encoder = EncoderProvider.GetAvailableEncoder(_configurator);

            return new Recorder(path, encoder, _configurator);
        }

        private string RemoveForbiddenSymbols(string inputName)
        {
            var fixedName = inputName.Replace(".", "");

            return Path.GetInvalidFileNameChars().Aggregate(fixedName, (symbol, s) => symbol.Replace(s.ToString(), string.Empty));
        }

        private string PrepareClassSubfolder()
        {
            DirectoryInfo videoSubFolder = Directory.CreateDirectory(Path.Combine(_defaultOutputPath, VideoFolderName));

            string className = TestContext.CurrentContext.Test.ClassName.Split('.').Last();

            DirectoryInfo classSubfolder = Directory.CreateDirectory(Path.Combine(videoSubFolder.FullName, className));

            return classSubfolder.FullName;
        }
    }
}