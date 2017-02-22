using NunitVideoRecorder.Enums;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NunitVideoRecorder
{
    public class VideoConfigurator
    {
        public int Width { get; } = SystemInformation.VirtualScreen.Width;
        public int Height { get; } = SystemInformation.VirtualScreen.Height;
        public int FramePerSecond { get; set; } = 30;
        public int FrameCount { get; } = 0;
        public VideoQuality Quality { get; set; } = VideoQuality.HIGH;
       
        public VideoConfigurator()
        {

        }

        public VideoConfigurator(int fps, VideoQuality quality)
        {
            FramePerSecond = fps;
            Quality = quality;
        }     
    }
}
