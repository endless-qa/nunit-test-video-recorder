using NunitVideoRecorder.Enums;
using System.Windows.Forms;

namespace NunitVideoRecorder
{
    public class VideoConfigurator
    {
        public int Width { get; } = SystemInformation.VirtualScreen.Width;
        public int Height { get; } = SystemInformation.VirtualScreen.Height;
        public int FramePerSecond { get; set; } = 25;
        public int FrameCount { get; } = 0;
        public VideoQuality Quality { get; set; } = VideoQuality.High;
       
        public VideoConfigurator() { }

        public VideoConfigurator(int fps, VideoQuality quality)
        {
            FramePerSecond = fps;
            Quality = quality;
        }     
    }
}
