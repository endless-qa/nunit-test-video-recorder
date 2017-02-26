using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : Attribute, ITestAction
    {
        public string Name { get; set; }
        private Recorder recorder;

        public VideoAttribute() { }

        public VideoAttribute(string fileName)
        {
            Name = fileName;
        }

        public void BeforeTest(ITest test)
        {

            if (Name == null)
            {
                recorder = new Recorder(test.Name);
            }
            else
            {
                recorder = new Recorder(Name);
            }

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
