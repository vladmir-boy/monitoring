﻿using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.PlatformAbstractions;

namespace Greentube.Monitoring.AspNetCore.Tests
{
    public class VersionServiceTests
    {
        [Fact]
        public void GetVersionInformation_ValidVersion()
        {
            // Arrange
            var environment = Substitute.For<IHostingEnvironment>();
            const string expectedEnvionmentName = "Test environment name";
            environment.EnvironmentName.Returns(expectedEnvionmentName);

            // Act
            var target = new VersionService(environment, GetType().GetTypeInfo().Assembly);
            var actual = target.GetVersionInformation();

            // Assert
            Assert.NotNull(actual);
            Assert.True(Regex.IsMatch(actual.AssemblyFileVersion, @"^(\d+\.){2}\d+(\.\d+)?$"), 
                $"AssemblyFileVersion {actual.AssemblyFileVersion} is not valid.");
            Assert.True(Regex.IsMatch(actual.AssemblyInformationalVersion, "[0-9a-fA-F]{5,40}"), 
                $"AssemblyInformationalVersion: {actual.AssemblyInformationalVersion} doesn't contain a valid SHA1.");
            Assert.Same(expectedEnvionmentName, actual.EnvironmentName);
            Assert.True(actual.StartupTimeUtc != default(DateTimeOffset));
            Assert.Equal(PlatformServices.Default.Application.RuntimeFramework.ToString(), actual.RuntimeFramework);
        }
    }
}
