using System.Linq;
using System.Threading.Tasks;
using TagAC.Management.Domain.Commands.GrantAccess;
using TagAC.Management.Domain.Commands.RevokeAccess;
using TagAC.Management.Domain.Queries.ListAccessControl;
using Xunit;

namespace TagAC.Management.Tests.CommandTests
{
    [Collection(nameof(TestFixture))]
    public class RevokeAccessCommandTest
    {
        private readonly TestFixture _fixture;
        public RevokeAccessCommandTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Default_Status_Is_Always_Unauthorized()
        {
            string rfid = "1234";

            var locks = await _fixture.SeedSmartLocks();

            var mediator = _fixture.GetMediator();

            var queryResponse = await mediator.Send(new ListAccessControlQuery()
            {
                RFID = rfid
            });

            foreach (var item in queryResponse.Data)
            {
                // By default you always get unauthorized.
                Assert.True(item.Status == TagAC.Domain.Enums.AuthorizationStatus.Unauthorized);
            }
        }

        [Fact]
        public async Task Should_Revoke_Access()
        {
            string rfid = "1234";

            var locks = await _fixture.SeedSmartLocks();

            var door1 = locks.FirstOrDefault(x => x.Name == "Door1");

            var mediator = _fixture.GetMediator();

            // First Gonna Grant Access, then Revoke.
            var commandResponse = await mediator.Send(new GrantAccessCommand()
            {
                RFID = rfid,
                SmartLockId = door1.Id
            });

            commandResponse = await mediator.Send(new RevokeAccessCommand()
            {
                RFID = rfid,
                SmartLockId = door1.Id
            });

            var queryResponse = await mediator.Send(new ListAccessControlQuery()
            {
                RFID = rfid
            });

            Assert.True(queryResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var accessCredential = queryResponse.Data.FirstOrDefault(x => x.SmartLockId == door1.Id && x.RFID == rfid);
            Assert.NotNull(accessCredential);
            Assert.True(accessCredential.Status == TagAC.Domain.Enums.AuthorizationStatus.Unauthorized);
        }
    }
}
