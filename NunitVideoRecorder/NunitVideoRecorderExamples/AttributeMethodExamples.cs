using NUnit.Framework;
using NunitVideoRecorder;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class AttributeMethodExamples
    {
        [Video(OutputFileName = "SuspiciousTest")]
        [Test]
        public void TestWithCustomName_MethodAttribute()
        {
            Thread.Sleep(10000);
        }

        [Video]
        [Test]
        public void TestWithDefaultName_MethodAttribute()
        {
            Thread.Sleep(10000);
        }

        [Video(OutputFileName = "AlwaysFailedTest")]
        [Test]
        public void FailedTestWithCustomName_MethodAttribute()
        {
            Thread.Sleep(10000);
            Assert.Fail("I don't like to pass!");
        }

        [Video]
        [Test]
        public void TestExceptionWithDefaultName_MethodAttribute()
        {
            Thread.Sleep(10000);
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException(); });
        }
    }
}