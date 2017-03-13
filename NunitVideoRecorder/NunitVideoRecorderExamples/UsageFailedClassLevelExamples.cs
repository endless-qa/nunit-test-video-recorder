using System;
using System.Threading;
using NUnit.Framework;
using NunitVideoRecorder;

namespace NunitVideoRecorderExamples
{
    [WatchDog(SaveInClass.FailedTestsOnly)]
    [TestFixture]
    public class UsageFailedClassLevelExamples
    {
        [Test]
        public void FailedButRecordedTest()
        {
            Thread.Sleep(10000);
            Assert.Fail("I just failed but I was recorded!");
        }
        
        [Test]
        public void PassesNotRecordedTest()
        {
            Thread.Sleep(10000);
            Assert.Pass("I just passed but I wasn't recordered...");
        }

        [Test]
        public void InconclusiveButRecordedTest()
        {
            Thread.Sleep(10000);
            Assert.Inconclusive("I am inconclusive but I'm also recorded!");
        }

        [Test]
        public void ExceptionFailedRecordedTest()
        {
            Thread.Sleep(10000);
            Assert.Throws<TimeoutException>(() => { throw new ArgumentException(); }, "I threw different exception, so I was also recorded!");
        }

        [Test]
        [Video(Name = "Very important test", Mode = SaveMe.Always)]
        public void PassedButCustomlyRecordedTest()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed but I was recordered because I'm important...");
        }
    }
}