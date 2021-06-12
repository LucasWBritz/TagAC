using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Apis.Management.Controllers
{
    [Route("[controller]")]
    public class CredentialsController
    {
        private readonly ISmartLockRepository _repository;
        public CredentialsController(ISmartLockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task Test()
        {
            await Task.CompletedTask;
        }
    }
}
