using System.Linq;
using System.Threading.Tasks;
using TagAC.Management.Domain.Commands.GrantAccess;
using TagAC.Management.Domain.Queries.ListAccessControl;
using Xunit;

namespace TagAC.Management.Tests.CommandTests
{
    [Collection(nameof(TestFixture))]
    public class GrantAccessCommandTest
    {
        private readonly TestFixture _fixture;
        public GrantAccessCommandTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Grant_Access()
        {
            string rfid = "1234";

            var locks = await _fixture.SeedSmartLocks();

            var door1 = locks.FirstOrDefault(x => x.Name == "Door1");

            var mediator = _fixture.GetMediator();

            var commandResponse = await mediator.Send(new GrantAccessCommand()
            {
                RFID = rfid,
                SmartLockId = door1.Id
            });

            Assert.True(commandResponse.StatusCode == System.Net.HttpStatusCode.OK);
            var queryResponse = await mediator.Send(new ListAccessControlQuery()
            {
                RFID = rfid
            });

            Assert.True(queryResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var accessCredential = queryResponse.Data.FirstOrDefault(x => x.SmartLockId == door1.Id && x.RFID == rfid);
            Assert.NotNull(accessCredential);
            Assert.True(accessCredential.Status == TagAC.Domain.Enums.AuthorizationStatus.Authorized);
        }
    }
}
