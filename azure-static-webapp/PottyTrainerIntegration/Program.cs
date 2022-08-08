using Api.Table;
using Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddSingleton<IUserData, UserTable>();
        s.AddSingleton<IAssignmentData, AssignmentTable>();
    })
    .Build();

host.Run();
