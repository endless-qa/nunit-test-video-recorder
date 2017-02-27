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
        private Recorder TestRecorder;
        private Task Recording;

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
                TestRecorder = new Recorder(test.Name);
                TestRecorder.SetConfiguration();

                Recording = new Task(new Action(ActivateVideoRecording));
                Recording.Start();
            }
        }

        public void AfterTest(ITest test)
        {
            if (!IsAttributeAppliedToTest(test))
            {
                TestRecorder.Stop();
                Recording.Wait(); 
            }
        }

        private void ActivateVideoRecording()
        {
            TestRecorder.Start();
        }


        private bool IsAttributeAppliedToTest(ITest test)
        {
            var attributesSet = test.Method.MethodInfo.CustomAttributes;
            return FindEntry(attributesSet, "VideoAttribute");
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
