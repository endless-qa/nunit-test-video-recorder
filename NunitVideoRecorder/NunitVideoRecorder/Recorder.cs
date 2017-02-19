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

namespace NunitVideoRecorder
{
    class Recorder
    {
        private string name;
        private string defaultPath = @"Y:\\";
        int screenWidth = SystemInformation.VirtualScreen.Width;
        int screenHeight = SystemInformation.VirtualScreen.Height;

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
            writer = new AviWriter(defaultPath + name)
            {
                FramesPerSecond = 30,
                EmitIndex1 = true
            };

            // New way
            //CodecProvider codecProvider = new CodecProvider();
            //FourCC[] selectedCodec = codecProvider.GetOptimalCodecs();

            // Old way
            FourCC selectedCodec = KnownFourCCs.Codecs.X264;

            // New way
            //var encoder = EncoderProvider.GetEncoder(codecProvider);

            // Old way
            var encoder = new Mpeg4VideoEncoderVcm(screenWidth, screenHeight, 30, 0, 100, selectedCodec);
            //var encoder = new MotionJpegVideoEncoderWpf(screenWidth, screenHeight, 100);


            stream = writer.AddEncodingVideoStream(encoder, true, screenWidth, screenHeight);

            stream.Width = screenWidth;
            stream.Height = screenHeight;
            //stream.Codec = KnownFourCCs.Codecs.Uncompressed;
            //stream.BitsPerPixel = BitsPerPixel.Bpp32;


            frameData = new byte[stream.Width * stream.Height * 4];
        }

        public void Start()
        {

            while (!stopRecording)
            {
                GetScreenshot(frameData);
                stream.WriteFrame(true, // is key frame? (many codecs use concept of key frames, for others - all frames are keys)
                                  frameData, // array with frame data
                                  0, // starting index in the array
                                  frameData.Length // length of the data
                );

            }
        }

        public void Stop()
        {
            stopRecording = true;
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
