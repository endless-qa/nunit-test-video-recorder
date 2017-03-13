## Nunit Video Recorder 

A simple .NET library for Windows designed for recording video from the screen during Nunit test runs. It's based on the [SharpAvi](https://github.com/baSSiLL/SharpAvi) library and inspired by the [Video Recorder Java](https://github.com/SergeyPirogov/video-recorder-java).

### How to install

### Usage
Recording video for Nunit tests can be done in 2 ways:

1. By applying the **[Video]** attribute on the test method level. This attribute and its settings affect the specific test (test method) only and have priority over another related attribute and settings that can be applied on the class level.

The **[Video]** attribute has 2 optional properties:
- the **Name** property stands for assigning a custom video file name for the particular test (instead of the test method name by default);
- the **Mode** property is designed for specifying the video saving mode for the particular test ("Always" (default) or "OnlyWhenFailed").
You are allowed to specify all or any of these properties for each test method as well as none of them. Here are the examples of the usage:
```C#
[Test]
[Video(Name = "MyFavouriteTest", Mode = SaveMe.OnlyWhenFailed)]
public void MyTest1()
{
    // your code here
}

[Test, Video(Mode = SaveMe.Always)]
public void MyTest2()
{
    // your code here
}


[Test, Video]
public void MyTest3()
{
    // your code here
}
```

2. By applying the **[WatchDog]** attribute on the class level. This attribute and its settings affect all tests (test methods) in the specific class and can be overriden by the [Video] attribute on the test method level (if necessary).

The **[WatchDog]** attribute has the only one mandatory **Mode** property which defines the global video saving mode ("AllTests" or "FailedTestsOnly"). Here are the examples of the usage:
```C#
[TestFixture, WatchDog(SaveInClass.AllTests)]
public class MyClass {}

// or

[WatchDog(SaveInClass.FailedTestsOnly)]
[TestFixture]
public class MyAnotherClass { }
```
