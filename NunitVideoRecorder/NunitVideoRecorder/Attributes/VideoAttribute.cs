using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : NUnitAttribute, ITestAction
    {
        public string Name { get; set; }
        private Recorder TestRecorder;
        private Task Recording;

        public VideoAttribute() { }

        public VideoAttribute(string fileName)
        {
            Name = fileName;
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
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

        private void ActivateVideoRecording()
        {
            TestRecorder.Start();
        }
    }
}
