using System;
using Autofac;
using Moq;
using NUnit.Framework;
using Serilog;

namespace AutofacSerilogIntegration.Tests
{
    [TestFixture]
    public class UpdatingContainerTests
    {
        [Test]
        public void Should_update_all_dependency()
        {
            // Register
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceWithLogger>().AsImplementedInterfaces();
            builder.RegisterLogger();
            var container = builder.Build();

            // Update
            var newBuilder = new ContainerBuilder();
            var newLogger = Mock.Of<ILogger>(l => l.ForContext(It.IsAny<Type>()) == l);
            newBuilder.RegisterInstance(newLogger).As<ILogger>();
            newBuilder.Update(container);

            // Resolve
            var service = container.Resolve<IServiceWithLogger>();

            // Assert
            Assert.AreSame(newLogger, service.Logger);
        }
    }

    public interface IServiceWithLogger
    {
        ILogger Logger { get; }
    }

    public class ServiceWithLogger : IServiceWithLogger
    {
        public ILogger Logger { get; }

        public ServiceWithLogger(ILogger logger)
        {
            Logger = logger;
        }
    }
}
