using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : NUnitAttribute, ITestAction
    {
        public string OutputFileName { get; set; }
        private Recorder videoRecorder;
        private Task recording;

        public VideoAttribute() { }

        public VideoAttribute(string testName)
        {
            OutputFileName = testName;
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

            if (OutputFileName == null)
            {
                videoRecorder = new Recorder(test.Name);
            }
            else
            {
                videoRecorder = new Recorder(OutputFileName);
            }            

            videoRecorder.SetConfiguration();

            recording = new Task(new Action(ActivateVideoRecording));
            recording.Start();            
        }

        public void AfterTest(ITest test)
        {
            Console.WriteLine(TestContext.CurrentContext.Result.Outcome);
            videoRecorder.Stop();
            recording.Wait();
        }

        private void ActivateVideoRecording()
        {
            videoRecorder.Start();
        }
    }
}
