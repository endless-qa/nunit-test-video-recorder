using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NunitVideoRecorder.Internal;

namespace NunitVideoRecorder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WatchDogAttribute : NUnitAttribute, ITestAction
    {        
        private const string WANTED_ATTRIBUTE = "VideoAttribute";
        private Recorder _recording;
        private bool _saveFailedOnly;

        public WatchDogAttribute(SaveInClass mode)
        {
            SetVideoSavingMode(mode);
        }

        public ActionTargets Targets { get; } = ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (!IsVideoAttributeAppliedToTest(test))
            {
                _recording = RecorderFactory.Instance.Create(test.Name);
                _recording.Start();
            }
        }

        public void AfterTest(ITest test)
        {
            if (!IsVideoAttributeAppliedToTest(test))
            {
                _recording?.Stop();

                if (_saveFailedOnly && Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Success))
                {
                    DeleteRelatedVideo();
                }
            }      
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
                    _saveFailedOnly = false;
                    break;
                }
                case SaveInClass.FailedTestsOnly:
                {
                    _saveFailedOnly = true;
                    break;
                }
                default: throw new ArgumentException("Saving mode is not valid!");
            }
        }

        private void DeleteRelatedVideo()
        {
            try
            {
                File.Delete(_recording.OutputFilePath);
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);                
            }
        }
    }
}
