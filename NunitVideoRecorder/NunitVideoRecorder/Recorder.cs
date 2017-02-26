using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using NunitVideoRecorder.Providers;
using NUnit.Framework;
using System.IO;

namespace NunitVideoRecorder
{
    class Recorder
    {
        private string FileName;
        private string DefaultOutputPath = TestContext.CurrentContext.TestDirectory;
        private int ScreenWidth = SystemInformation.VirtualScreen.Width;
        private int ScreenHeight = SystemInformation.VirtualScreen.Height;

        private VideoConfigurator Configurator;
        private IVideoEncoder SelectedEncoder;

        AviWriter FileWriter;
        byte[] FrameData;
        IAviVideoStream VideoStream;
        private bool StopRecording = false;

        public Recorder(string name)
        {
            FileName = name + ".avi";
            Configurator = new VideoConfigurator();
            SelectedEncoder = EncoderProvider.GetAvailableEncoder(Configurator);
        }

        public void SetConfiguration()
        {
            FileWriter = new AviWriter(Path.Combine(DefaultOutputPath, FileName))
            {
                FramesPerSecond = Configurator.FramePerSecond,
                EmitIndex1 = true
            };

            VideoStream = FileWriter.AddEncodingVideoStream(SelectedEncoder, true, ScreenWidth, ScreenHeight);

            FrameData = new byte[VideoStream.Width * VideoStream.Height * 4];
        }

        public void Start()
        {
            while (!StopRecording)
            {
                GetSnapshot(FrameData);
                VideoStream.WriteFrameAsync(true, FrameData, 0, FrameData.Length);
            }            
        }

        public void Stop()
        {
            StopRecording = true;
            //Thread.Sleep(2000);           // Debugging for correct closing all streams
            FileWriter.Close();
        }

        private void GetSnapshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(ScreenWidth, ScreenHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, new Size(ScreenWidth, ScreenHeight));
                var bits = bitmap.LockBits(new Rectangle(0, 0, ScreenWidth, ScreenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);
            }
        }
    }
}
