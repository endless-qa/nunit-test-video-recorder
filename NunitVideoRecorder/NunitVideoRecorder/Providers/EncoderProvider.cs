using SharpAvi.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NunitVideoRecorder.Providers
{
    class EncoderProvider
    {
        static int screenWidth = SystemInformation.VirtualScreen.Width;
        static int screenHeight = SystemInformation.VirtualScreen.Height;

        public static IVideoEncoder GetEncoder(CodecProvider provider)
        {
            if (provider.isMpeg4CodecsAvailable)
            {
                return new Mpeg4VideoEncoderVcm(screenWidth, screenHeight, 30, 0, 100, provider.GetOptimalCodecs());
            }
            else
            {
                return new MotionJpegVideoEncoderWpf(screenWidth, screenHeight, 100);
            }
        }
    }
}
