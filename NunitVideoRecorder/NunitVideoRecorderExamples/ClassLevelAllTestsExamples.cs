using System;
using System.Threading;
using NUnit.Framework;
using NunitVideoRecorder;

namespace NunitVideoRecorderExamples
{
    [TestFixture, WatchDog(SaveInClass.AllTests)]
    class ClassLevelAllTestsExamples
    {
        [Test]
        public void FailingTest_ModeAll()
        {
            Thread.Sleep(10000);
            Assert.Fail("I just failed but I was recorded!");
        }

        [Test]
        public void PassingTest_ModeAll()
        {
            Thread.Sleep(10000);
            Assert.Pass("I just passed and I was recordered...");
        }

        [Test]
        public void InconclusiveTest_ModeAll()
        {
            Thread.Sleep(10000);
            Assert.Inconclusive("I am inconclusive but I'm also recorded!");
        }

        [Test]
        public void ExceptionTest_ModeAll()
        {
            Thread.Sleep(10000);
            Assert.Throws<TimeoutException>(() => { throw new TimeoutException(); }, "Doesn't matter what I trow, I will be recorded!");
        }
    }
}