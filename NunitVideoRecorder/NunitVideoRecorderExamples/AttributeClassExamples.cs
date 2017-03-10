using NUnit.Framework;
using NunitVideoRecorder;
using NunitVideoRecorder.Attributes;
using System;
using System.Threading;

namespace NunitVideoRecorderExamples
{
    [WatchDog]
    [TestFixture]
    public class AttributeClassExamples
    {
        [Test]
        public void TestWithDefaultName_ClassAttribute()
        {
            Thread.Sleep(10000);
            Assert.Pass();
        }

        [Video(OutputFileName = "MyFavouriteTest")]
        [Test]
        public void TestWithCustomName_ClassAttribute()
        {
            Thread.Sleep(10000);
            Assert.Pass();
        }
    }
}
