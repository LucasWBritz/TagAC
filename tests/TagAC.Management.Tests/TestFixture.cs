using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagAC.Apis.Management;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;
using Xunit;

namespace TagAC.Management.Tests
{
    [CollectionDefinition(nameof(TestFixture))]
    public class TestFixtureCollection : ICollectionFixture<TestFixture> { }
    public class TestFixture : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly IServiceScopeFactory _scopeFactory;

        public TestFixture()
        {
            _factory = new ManagementApiAppFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        private class ManagementApiAppFactory : WebApplicationFactory<Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureAppConfiguration((_, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"ConnectionStrings:DefaultConnection", _connectionString}
                    });
                });
            }

            private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=TagACManagement;Trusted_Connection=True;MultipleActiveResultSets=true";
        }     

        public T GetScopedService<T>()
        {
            var scopedDBContext = _scopeFactory.CreateScope();
            return scopedDBContext.ServiceProvider.GetService<T>();
        }
        
        public IMediator GetMediator()
        {
            return GetScopedService<IMediator>();
        }

        private IEnumerable<SmartLock> _inMemorySmartLocks;
        public async Task<IEnumerable<SmartLock>> SeedSmartLocks()
        {
            if (_inMemorySmartLocks != null)
                return _inMemorySmartLocks;

            var smartLockRepositor = GetScopedService<ISmartLockRepository>();
            await smartLockRepositor.CreateAsync(new Domain.Entities.SmartLock()
            {
                Name = "Door1",
                Id = Guid.NewGuid()
            });

            await smartLockRepositor.CreateAsync(new Domain.Entities.SmartLock()
            {
                Name = "Door2",
                Id = Guid.NewGuid()
            });

            await smartLockRepositor.UnitOfWork.CommitAsync();

            _inMemorySmartLocks = smartLockRepositor.GetAll().ToList();
            return _inMemorySmartLocks;
        }

        public async Task DisposeAsync()
        {
            await CleanUp();

            _factory?.Dispose();
        }

        private async Task CleanUp()
        {
            var smartLockRepository = GetScopedService<ISmartLockRepository>();
            foreach (var item in smartLockRepository.GetAll())
            {
                smartLockRepository.Delete(item);
            }
            await smartLockRepository.UnitOfWork.CommitAsync();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
