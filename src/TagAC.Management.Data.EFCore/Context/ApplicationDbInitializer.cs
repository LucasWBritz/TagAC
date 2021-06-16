using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Data.EFCore.Context
{
    public class ApplicationDbInitializer
    {
        // Used to seed 2 doors. So we don't need to add them manually later.
        public static async Task SeedDoors(ISmartLockRepository repository)
        {
            var doorExists = await repository.GetAll().FirstOrDefaultAsync(x => x.Name == "Door1");
            if(doorExists == null)
            {
                await repository.CreateAsync(new Domain.Entities.SmartLock()
                {
                    Name = "Door1"
                });
                
                await repository.CreateAsync(new Domain.Entities.SmartLock()
                {
                    Name = "Door2"
                });

                await repository.UnitOfWork.CommitAsync();
            }
        }
    }
}
