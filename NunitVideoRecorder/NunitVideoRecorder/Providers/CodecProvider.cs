using SharpAvi;
using SharpAvi.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitVideoRecorder.Providers
{
    class CodecProvider
    {
        public bool isMpeg4CodecsAvailable = false;
        CodecInfo[] codecs;

        public CodecProvider()
        {
            codecs = Mpeg4VideoEncoderVcm.GetAvailableCodecs();
            isMpeg4CodecsAvailable = codecs.Length > 0 ? true : false;
        }


        public FourCC[] GetOptimalCodecs()
        {
            if (isMpeg4CodecsAvailable)
            {
                return FindInstalled(codecs);
            }
            else
            {
                return new FourCC[] { KnownFourCCs.Codecs.MotionJpeg };
            }
        }

        private static FourCC[] FindInstalled(CodecInfo[] installedCodecsList)
        {
            List<FourCC> result = new List<FourCC>();

            foreach (CodecInfo codec in installedCodecsList)
            {
                result.Add(codec.Codec);
            }

            return result.ToArray();
        }

    }
}

