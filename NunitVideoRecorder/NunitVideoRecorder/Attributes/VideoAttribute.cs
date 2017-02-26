using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : Attribute, ITestAction
    {
        public string Name { get; set; }
        private Recorder TestRecorder;
        private Task Recording;

        public VideoAttribute() { }

        public VideoAttribute(string fileName)
        {
            Name = fileName;
        }

        public void BeforeTest(ITest test)
        {
            if (Name == null)
            {
                TestRecorder = new Recorder(test.Name);
            }
            else
            {
                TestRecorder = new Recorder(Name);
            }            

            TestRecorder.SetConfiguration();

            Recording = new Task(new Action(ActivateVideoRecording));
            Recording.Start();
        }

        public void AfterTest(ITest test)
        {
            TestRecorder.Stop();
            Recording.Wait();
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        private void ActivateVideoRecording()
        {
            TestRecorder.Start();
        }
    }
}
