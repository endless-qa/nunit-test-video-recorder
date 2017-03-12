using System.IO;
using NUnit.Framework;

namespace NunitVideoRecorder.Internal
{
    public class RecorderFactory
    {
        private const string DefaultExtension = ".avi";
        private readonly string _defaultOutputPath = TestContext.CurrentContext.TestDirectory;
        private readonly VideoConfigurator _configurator;

        private RecorderFactory(VideoConfigurator configurator)
        {
            _configurator = configurator;
        }
        public static RecorderFactory Instance => new RecorderFactory(new VideoConfigurator());

        public Recorder Create(string testName)
        {
            var fileName = testName;
            if (!fileName.Contains("."))
            {
                fileName = fileName + DefaultExtension;
            }

            var path = Path.Combine(_defaultOutputPath, fileName);
            var encoder = EncoderProvider.GetAvailableEncoder(_configurator);

            return new Recorder(path, encoder, _configurator);
        }
    }
}