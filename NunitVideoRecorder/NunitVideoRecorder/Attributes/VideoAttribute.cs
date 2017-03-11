using NUnit.Framework;
using System;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;
using System.IO;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VideoAttribute : NUnitAttribute, ITestAction
    {
        public string Name { get; set; }
        public SaveMe Mode { get; set; } = SaveMe.Always;

        private bool saveFailedOnly;
        private Recorder videoRecorder;
        private Task recording;

        public VideoAttribute() { }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        public void BeforeTest(ITest test)
        {
            SetVideoSavingMode(Mode);

            if (string.IsNullOrWhiteSpace(Name))
            {
                videoRecorder = new Recorder(test.Name);
            }
            else
            {
                videoRecorder = new Recorder(Name.Trim());
            }            

            videoRecorder.SetConfiguration();

            recording = new Task(new Action(ActivateVideoRecording));
            recording.Start();            
        }

        public void AfterTest(ITest test)
        {
            videoRecorder.Stop();
            recording.Wait();

            if (saveFailedOnly)
            {
                if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
                {
                    DeleteRelatedVideo();
                }
            }
        }

        private void ActivateVideoRecording()
        {
            videoRecorder.Start();
        }

        private void SetVideoSavingMode(SaveMe mode)
        {
            switch (mode)
            {
                case SaveMe.Always:
                {
                    saveFailedOnly = false;
                    break;
                }
                case SaveMe.OnlyWhenFailed:
                {
                    saveFailedOnly = true;
                    break;
                }
                default: throw new ArgumentException("Saving mode is not valid!");
            }
        }

        private void DeleteRelatedVideo()
        {
            try
            {
                File.Delete(videoRecorder.GetOutputFile());
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }
    }
}
