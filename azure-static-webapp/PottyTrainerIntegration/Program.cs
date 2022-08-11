using Api.Table;
using Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddHttpClient();
        s.AddSingleton<IUserData, UserTable>();
        s.AddSingleton<IAssignmentData, AssignmentTable>();
        s.AddSingleton<IAuthData, AuthData>();
    })
    .Build();

host.Run();
