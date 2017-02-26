using NUnit.Framework;
using NunitVideoRecorder;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class AttributesExample
    {
        [Video(Name = "Suspicious test")]
        [Test]
        public void AttributeUsageWithCustomVideoName()
        {
            Thread.Sleep(10000);
        }

        [Video]
        [Test]
        public void AttributeUsageWithDefaultVideoName()
        {
            Thread.Sleep(10000);
        }

        [Video(Name = "AlwaysFailedTest")]
        [Test]
        public void AttributeUsageWithFailedTest()
        {
            Thread.Sleep(10000);
            Assert.Fail("I don't like to pass!");
        }

        [Video]
        [Test]
        public void AttributeUsageWhenTestCatchesExceptions()
        {
            Thread.Sleep(10000);
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
        }
    }
}