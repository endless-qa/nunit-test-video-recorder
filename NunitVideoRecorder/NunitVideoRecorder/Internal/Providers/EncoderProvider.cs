using SharpAvi.Codecs;

namespace NunitVideoRecorder.Internal
{
    class EncoderProvider
    {
        public static IVideoEncoder GetAvailableEncoder(VideoConfigurator cfg)
        {
            // If there are Mpeg4 video codecs installed in the system -> take an internally preferred one
            if (Mpeg4VideoEncoderVcm.GetAvailableCodecs().Length > 0)
            {
                return new Mpeg4VideoEncoderVcm(cfg.Width, cfg.Height, cfg.FramePerSecond, cfg.FrameCount, (int)cfg.Quality);
            }
            // Otherwise -> take MotionJpeg
            else
            {
                return new MotionJpegVideoEncoderWpf(cfg.Width, cfg.Height, (int)cfg.Quality);
            }
        }
    }
}
