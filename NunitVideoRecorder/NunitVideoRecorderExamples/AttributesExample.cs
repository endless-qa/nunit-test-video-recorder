using NUnit.Framework;
using NunitVideoRecorder;
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
    }
}