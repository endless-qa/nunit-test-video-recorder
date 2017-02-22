using NUnit.Framework;
using NunitVideoRecorder;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class AnnotationsExample
    {
        [Video("testsss.avi")]
        [Test]
        public void AnnotationWithCustomVideoName()
        {
            Thread.Sleep(10000);
        }
    }
}