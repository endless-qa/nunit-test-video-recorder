using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SharpAvi.Codecs;
using SharpAvi.Output;

// TODO: (POSTPONED) Add an option for custom output path, not TestDirectory only
// TODO: Implement functionality for bringing Windows application window on top

namespace NunitVideoRecorder.Internal
{
    public class Recorder
    {
        private readonly string _outputFilePath;
        private readonly IVideoEncoder _selectedEncoder;
        private readonly VideoConfigurator _configurator;

        private AviWriter _fileWriter;
        private byte[] _frameData;
        private IAviVideoStream _videoStream;

        private CancellationTokenSource _cts;
        private Task _workTask;

        public Recorder(string outputFilePath, IVideoEncoder videoEncoder, VideoConfigurator configurator)
        {
            _outputFilePath = outputFilePath;
            _selectedEncoder = videoEncoder;
            _configurator = configurator;
        }

        public string OutputFilePath => _outputFilePath;

        public void Start()
        {
            _fileWriter = new AviWriter(_outputFilePath)
            {
                FramesPerSecond = _configurator.FramePerSecond,
                EmitIndex1 = true
            };

            _videoStream = _fileWriter.AddEncodingVideoStream(_selectedEncoder, true, _configurator.Width, _configurator.Height);
            _frameData = new byte[_videoStream.Width * _videoStream.Height * 4];
            _cts = new CancellationTokenSource();

            _workTask = Task.Run(async () =>
            {
                Task writeTask = Task.FromResult(true);
                while (!_cts.IsCancellationRequested)
                {
                    GetSnapshot(_frameData);
                    await writeTask;
                    writeTask = _videoStream.WriteFrameAsync(true, _frameData, 0, _frameData.Length);
                }
                await writeTask;
            });
        }

        public void Stop()
        {
            _cts.Cancel();

            try
            {
                _workTask.Wait();
            }
            catch (OperationCanceledException) { }
            finally
            {
                _fileWriter.Close();
            }
        }

        private void GetSnapshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(_configurator.Width, _configurator.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, new Size(_configurator.Width, _configurator.Height));
                var bits = bitmap.LockBits(new Rectangle(0, 0, _configurator.Width, 
                    _configurator.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);
            }
        }        
    }
}
