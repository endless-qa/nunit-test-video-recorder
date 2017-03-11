using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Reflection;
using System.IO;

namespace NunitVideoRecorder.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WatchDogAttribute : NUnitAttribute, ITestAction
    {
        private Recorder videoRecorder;
        private Task recording;
        private const string WANTED_ATTRIBUTE = "VideoAttribute";
        private bool saveFailedOnly;

        public WatchDogAttribute(SaveInClass mode)
        {
            SetVideoSavingMode(mode);
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
            if (!IsVideoAttributeAppliedToTest(test))
            {
                videoRecorder = new Recorder(test.Name);
                videoRecorder.SetConfiguration();

                recording = new Task(new Action(ActivateVideoRecording));
                recording.Start();
            }
        }

        public void AfterTest(ITest test)
        {
            if (!IsVideoAttributeAppliedToTest(test))
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
        }

        private void ActivateVideoRecording()
        {
            videoRecorder.Start();
        }

        private bool IsVideoAttributeAppliedToTest(ITest test)
        {
            var testAttributesSet = test.Method.MethodInfo.CustomAttributes;
            return FindEntry(testAttributesSet, WANTED_ATTRIBUTE);
        }

        private bool FindEntry(IEnumerable<CustomAttributeData> attributesSet, string wantedAttribute)
        {
            bool isAttributeFound = false;

            foreach (var attribute in attributesSet)
            {
                if (attribute.AttributeType.Name == wantedAttribute)
                {
                    isAttributeFound = true;
                }
            }
            return isAttributeFound;
        }

        private void SetVideoSavingMode(SaveInClass mode)
        {
            switch (mode)
            {
                case SaveInClass.AllTests:
                {
                    saveFailedOnly = false;
                    break;
                }
                case SaveInClass.FailedTestsOnly:
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
