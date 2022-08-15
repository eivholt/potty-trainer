using Api.Table;
using Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PottyTrainerIntegration.OAuth2;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddHttpClient();
        s.AddSingleton<IOauth2Client, WithingsOauth2Client>();
        s.AddSingleton<IUserData, UserTable>();
        s.AddSingleton<IAssignmentData, AssignmentTable>();
        s.AddSingleton<IAuthData, AuthData>();
    })
    .Build();

host.Run();
