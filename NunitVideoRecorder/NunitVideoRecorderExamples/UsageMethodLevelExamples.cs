using System.Threading;
using NUnit.Framework;
using NunitVideoRecorder;

namespace NunitVideoRecorderExamples
{
    [TestFixture]
    public class UsageMethodLevelExamples
    {
        [Video]
        [Test]
        public void DefaultNamedTest()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed and was recorded because it's enabled by default.");
        }

        [Video(Name = "MyFavouriteTest")]
        [Test]
        public void CustomNamedTest()
        {
            Thread.Sleep(10000);
            Assert.Fail("I failed and was recorded because it's enabled by default.");
        }

        [Video(Mode = SaveMe.OnlyWhenFailed)]
        [Test]
        public void DefaultNamedOnlyWhenFailsTest()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed and I wasn't recorded because it doesn't make sense for now...");
        }

        [Test, Video(Name = "CheckNegativeBehaviour", Mode = SaveMe.OnlyWhenFailed)]
        public void CustomNamedOnlyWhenFailsTest()
        {
            Thread.Sleep(10000);
            Assert.Fail("I failed and that's why I am recorded!");
        }

        [Test, Video(Name = " I?m*p<>o||rta:n/tTe.st ", Mode = SaveMe.Always)]
        public void IllegalSymbolsNamedTest()
        {
            Thread.Sleep(10000);
            Assert.Pass("I passed and I was recorded with a new fixed name!");
        }
    }
}