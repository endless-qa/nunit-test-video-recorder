using SharpAvi.Codecs;

namespace NunitVideoRecorder.Providers
{
    class EncoderProvider
    {
        public static IVideoEncoder GetAvailableEncoder(VideoConfigurator cfg)
        {
            if (Mpeg4VideoEncoderVcm.GetAvailableCodecs().Length > 0)
            {
                return new Mpeg4VideoEncoderVcm(cfg.Width, cfg.Height, cfg.FramePerSecond, cfg.FrameCount, (int)cfg.Quality);
            }
            else
            {
                return new MotionJpegVideoEncoderWpf(cfg.Width, cfg.Height, (int)cfg.Quality);
            }
        }
    }
}
