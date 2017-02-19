using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class VideoAttribute : Attribute, ITestAction
    {

        private string _OutputFileName;
        Recorder recorder;

        public VideoAttribute(string name)
        {
            _OutputFileName = name;

        }

        public void BeforeTest(ITest test)
        {
            recorder = new Recorder(_OutputFileName);
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
