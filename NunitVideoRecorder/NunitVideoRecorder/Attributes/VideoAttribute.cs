using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework.Internal;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : Attribute, ITestAction
    {
        private string OutputFileName;
        Recorder recorder;

        public VideoAttribute(string fileName)
        {
            OutputFileName = fileName;
        }

        public void BeforeTest(ITest test)
        {
            recorder = new Recorder(OutputFileName);
            recorder.SetConfiguration();
            ActivateVideoRecording();
        }

        public void AfterTest(ITest test)
        {
            recorder.Stop();
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        async void ActivateVideoRecording()
        {
            await Task.Run(() => recorder.Start());
        }
    }
}
