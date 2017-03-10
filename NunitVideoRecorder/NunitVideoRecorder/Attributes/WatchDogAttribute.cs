using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Reflection;

namespace NunitVideoRecorder.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WatchDogAttribute : NUnitAttribute, ITestAction
    {
        private Recorder videoRecorder;
        private Task recording;
        private const string VIDEO_ATTRIBUTE = "VideoAttribute";

        public WatchDogAttribute() { }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        public void BeforeTest(ITest test)
        {
            if (!IsAttributeAppliedToTest(test))
            {
                videoRecorder = new Recorder(test.Name);
                videoRecorder.SetConfiguration();

                recording = new Task(new Action(ActivateVideoRecording));
                recording.Start();
            }
        }

        public void AfterTest(ITest test)
        {
            if (!IsAttributeAppliedToTest(test))
            {
                videoRecorder.Stop();
                recording.Wait(); 
            }            
        }

        private void ActivateVideoRecording()
        {
            videoRecorder.Start();
        }


        private bool IsAttributeAppliedToTest(ITest test)
        {
            var testAttributesSet = test.Method.MethodInfo.CustomAttributes;
            return FindEntry(testAttributesSet, VIDEO_ATTRIBUTE);
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
    }
}
