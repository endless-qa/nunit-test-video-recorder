using NUnit.Framework;
using NunitVideoRecorder;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class AnnotationsExample
    {
        [Video("output.avi")]
        [Test]
        public void AnnotationWithCustomVideoName()
        {
            Thread.Sleep(15000);
            Assert.Pass();
        }
    }
}