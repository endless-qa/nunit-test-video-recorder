using System;
using System.Threading;
using NUnit.Framework;
using NunitVideoRecorder;

namespace NunitVideoRecorderExamples
{
    [WatchDog(SaveInClass.FailedTestsOnly)]
    [TestFixture]
    public class ClassLevelFailedTestsExamples
    {
        [Test]
        public void FailingTest_ModeFailed()
        {
            Thread.Sleep(10000);
            Assert.Fail("I just failed but I was recorded!");
        }
        
        [Test]
        public void PassingTest_ModeFailed()
        {
            Thread.Sleep(10000);
            Assert.Pass("I just passed but I wasn't recordered...");
        }

        [Test]
        public void InconclusiveTest_ModeFailed()
        {
            Thread.Sleep(10000);
            Assert.Inconclusive("I am inconclusive but I'm also recorded!");
        }

        [Test]
        public void ExceptionTest_ModeFailed()
        {
            Thread.Sleep(10000);
            Assert.Throws<TimeoutException>(() => { throw new ArgumentException(); }, "I threw different exception, so I was also recorded!");
        }

        [Test]
        [Video(Name = "Very important test", Mode = SaveMe.Always)]
        public void PassingTest_CustomSetting_ModeFailed()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed but I was recordered because I'm important...");
        }
    }
}