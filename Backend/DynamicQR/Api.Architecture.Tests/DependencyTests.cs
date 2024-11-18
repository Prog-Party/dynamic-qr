using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;

namespace Api.Architecture.Tests
{
    public class DependencyTests
    {
        protected readonly Assembly apiAssembly = typeof(Program).Assembly;
        protected readonly Assembly coreAssembly = typeof(DynamicQR.Domain.Models.QrCode).Assembly;
        protected readonly Assembly applicationAssembly = typeof(DynamicQR.Application.QrCodes.Commands.CreateQrCode.CommandHandler).Assembly;
        protected readonly Assembly infrastructureAssembly = typeof(DynamicQR.Infrastructure.Services.QrCodeRepositoryService).Assembly;

        [Fact]
        public void ApiAssemblyHasDependenciesNeededToSetupContainer()
        {
            apiAssembly.Should().Reference(coreAssembly);
            apiAssembly.Should().Reference(applicationAssembly);
            apiAssembly.Should().Reference(infrastructureAssembly);
        }

        [Fact]
        public void CoreAssemblyDoesNotHaveOutwardsDependencies()
        {
            coreAssembly.Should().NotReference(apiAssembly);
            coreAssembly.Should().NotReference(applicationAssembly);
            coreAssembly.Should().NotReference(infrastructureAssembly);
        }

        [Fact]
        public void ApplicationAssemblyHasOnlyInwardsDependencies()
        {
            applicationAssembly.Should().Reference(coreAssembly);
            applicationAssembly.Should().NotReference(apiAssembly);
            applicationAssembly.Should().NotReference(infrastructureAssembly);
        }

        [Fact]
        public void InfrastructureAssemblyHasOnlyInwardsDependencies()
        {
            //infrastructureAssembly.Should().NotReference(coreAssembly);
            //infrastructureAssembly.Should().Reference(applicationAssembly);
            infrastructureAssembly.Should().NotReference(apiAssembly);
        }
    }
}