using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TagAC.Apis.Management.Behaviors;
using TagAC.Management.Domain.Commands;
using TagAC.Management.Domain.Commands.GrantAccess;

namespace TagAC.Apis.Management.Configuration
{
    public static class MediatorConfiguration
    {
        public static void ConfigureMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Command).Assembly);

            // Register Validators
            services.Scan(scan => scan
              .FromAssemblyOf<IValidationHandler>()
                .AddClasses(classes => classes.AssignableTo<IValidationHandler>())
                  .AsImplementedInterfaces()
                  .WithTransientLifetime());


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
        }
    }
}
