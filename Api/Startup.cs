using Api.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IUserData, UserTable>();
        builder.Services.AddSingleton<IAssignmentData, AssignmentTable>();
    }
}
