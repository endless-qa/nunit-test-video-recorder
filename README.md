## Nunit Test Video Recorder 

[![Build Status](https://travis-ci.org/endless-qa/nunit-test-video-recorder.svg?branch=master)](https://travis-ci.org/endless-qa/nunit-test-video-recorder)

A simple .NET library for Windows designed for recording video from the screen during Nunit test runs. It's based on the [SharpAvi](https://github.com/baSSiLL/SharpAvi) library and inspired by the [Video Recorder Java](https://github.com/SergeyPirogov/video-recorder-java).

## Table of contents
* [Downloads](#downloads)
* [Documentation](#documentation)
* [License](#license)
* [Known limitations](#known-limitations)

## Downloads
The latest version of the Nunit Video Recorder is [available on NuGet](https://www.nuget.org/packages/Nunit.Video.Recorder/).


## Documentation
The best place to start from is the [Getting started](https://github.com/endless-qa/nunit-test-video-recorder/wiki/Getting-started) page. It contains explanations of the main workflow and principles of the NUnit Video Recorder as well as links to the attributes and examples of usage.

## License
Nunit Test Video Recorder is distributed under the [MIT license](https://github.com/endless-qa/nunit-test-video-recorder/wiki/License).

## Known limitations
- No support for video recording of Nunit tests marked with [Repeat], [Retry], [Combinatorial] or any other attributes that manage to run a single test multiple times;
- No Selenium Grid support;
- No multi-display support.

All the above-mentioned is under investigation and may be implemented in future releases.
