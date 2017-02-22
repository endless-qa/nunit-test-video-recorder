using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NunitVideoRecorder.Enums;
using NunitVideoRecorder.Providers;
using NUnit.Framework;
using System.IO;

namespace NunitVideoRecorder
{
    class Recorder
    {
        private string name;
        private string defaultPath = TestContext.CurrentContext.TestDirectory;
        private int screenWidth = SystemInformation.VirtualScreen.Width;
        private int screenHeight = SystemInformation.VirtualScreen.Height;

        public Recorder(string name)
        {
            this.name = name;
        }

        AviWriter writer;
        byte[] frameData;
        IAviVideoStream stream;
        private bool stopRecording = false; 

        public void SetConfiguration()
        {
            writer = new AviWriter(Path.Combine(defaultPath, name))
            {
                FramesPerSecond = 25,
                EmitIndex1 = true
            };

            VideoConfigurator configurator = new VideoConfigurator(25, VideoQuality.HIGH);
            var encoder = EncoderProvider.GetAvailableEncoder(configurator);            

            stream = writer.AddEncodingVideoStream(encoder, true, screenWidth, screenHeight);

            frameData = new byte[stream.Width * stream.Height * 4];
        }

        public void Start()
        {
            while (!stopRecording)
            {
                GetScreenshot(frameData);
                stream.WriteFrameAsync(true, frameData, 0, frameData.Length);
            }            
        }

        public void Stop()
        {
            stopRecording = true;
            Thread.Sleep(2000);
            writer.Close();
        }

        private void GetScreenshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));
                var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);
            }
        }
    }
}
