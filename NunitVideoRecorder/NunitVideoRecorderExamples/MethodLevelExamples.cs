using NUnit.Framework;
using NunitVideoRecorder;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class MethodLevelExamples
    {
        [Video]
        [Test]
        public void DefaultName_MethodLevel()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed and was recorded because it's enabled by default.");
        }

        [Video(Name = "SuspiciousTest")]
        [Test]
        public void CustomName_MethodLevel()
        {
            Thread.Sleep(10000);
            Assert.Fail("I failed and was recorded because it's enabled by default.");
        }

        [Video(Mode = SaveMe.OnlyWhenFailed)]
        [Test]
        public void DefaultName_Pass_MethodLevel()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed and I wasn't recorded because it doesn't make sense for now...");
        }

        [Test, Video(Name = "CheckNegativeBehaviour", Mode = SaveMe.OnlyWhenFailed)]
        public void DefaultName_Fail_MethodLevel()
        {
            Thread.Sleep(10000);
            Assert.Fail("I failed and that's why I am recorded!");
        }
    }
}