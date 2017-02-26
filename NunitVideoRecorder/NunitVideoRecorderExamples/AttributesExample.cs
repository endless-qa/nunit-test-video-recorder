using NUnit.Framework;
using NunitVideoRecorder;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class AttributesExample
    {
        [Video("myTest")]
        [Test]
        public void AttributeWithCustomVideoName()
        {
            Thread.Sleep(10000);
        }

        //[Video]
        [Test]
        public void AttributeWithDefaultVideoName()
        {
            Thread.Sleep(10000);
        }
    }
}