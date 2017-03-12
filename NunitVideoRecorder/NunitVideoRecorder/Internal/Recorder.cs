using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SharpAvi.Codecs;
using SharpAvi.Output;


// TODO: Implement sorting video files by folders with class names
// TODO: Add an option for custom output path, not TestDirectory only
// TODO: Implement functionality for bringing Windows application window on top

namespace NunitVideoRecorder.Internal
{
    class Recorder
    {
        private string outputFileName;
        private string outputPath = TestContext.CurrentContext.TestDirectory;
        private string fullPathToSavedVideo;
        private const string VIDEO_EXTENSION = ".avi";
        private int screenWidth = SystemInformation.VirtualScreen.Width;
        private int screenHeight = SystemInformation.VirtualScreen.Height;

        private VideoConfigurator configurator;
        private IVideoEncoder selectedEncoder;

        private AviWriter fileWriter;
        private byte[] frameData;
        private IAviVideoStream videoStream;
        private bool stopRecording = false;

        public Recorder(string name)
        {
            outputFileName = string.Concat(name, VIDEO_EXTENSION);
            fullPathToSavedVideo = Path.Combine(outputPath, outputFileName);
            configurator = new VideoConfigurator();
            selectedEncoder = EncoderProvider.GetAvailableEncoder(configurator);
        }

        public void SetConfiguration()
        {
            fileWriter = new AviWriter(fullPathToSavedVideo)
            {
                FramesPerSecond = configurator.FramePerSecond,
                EmitIndex1 = true
            };

            videoStream = fileWriter.AddEncodingVideoStream(selectedEncoder, true, screenWidth, screenHeight);

            frameData = new byte[videoStream.Width * videoStream.Height * 4];
        }

        public void Start()
        {
            while (!stopRecording)
            {
                GetSnapshot(frameData);
                videoStream.WriteFrameAsync(true, frameData, 0, frameData.Length);
            }

            fileWriter.Close();
        }

        public void Stop()
        {
            stopRecording = true;
        }

        public string GetOutputFile()
        {
            return fullPathToSavedVideo;
        }

        private void GetSnapshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
                var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);
            }
        }        
    }
}
