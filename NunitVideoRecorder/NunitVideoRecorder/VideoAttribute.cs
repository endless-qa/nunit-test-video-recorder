using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NunitVideoRecorder.Internal;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : NUnitAttribute, ITestAction
    {
        public string Name { get; set; }
        public SaveMe Mode { get; set; } = SaveMe.Always;

        private bool _saveFailedOnly;
        private Recorder _recording;

        public VideoAttribute() { }

        public ActionTargets Targets => ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            SetVideoSavingMode(Mode);
            var testName = string.IsNullOrWhiteSpace(Name) ? test.Name : Name.Trim();
            _recording = RecorderFactory.Instance.Create(testName);
            _recording.Start();            
        }

        public void AfterTest(ITest test)
        {
            _recording?.Stop();

            if (_saveFailedOnly
                && Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Success))
                {
                    DeleteRelatedVideo();
                }
        }

        private void SetVideoSavingMode(SaveMe mode)
        {
            switch (mode)
            {
                case SaveMe.Always:
                {
                    _saveFailedOnly = false;
                    break;
                }
                case SaveMe.OnlyWhenFailed:
                {
                    _saveFailedOnly = true;
                    break;
                }
                default: throw new ArgumentException("Saving mode is not valid!");
            }
        }

        private void DeleteRelatedVideo()
        {
            try
            {
                File.Delete(_recording.OutputFilePath);
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }
    }
}
