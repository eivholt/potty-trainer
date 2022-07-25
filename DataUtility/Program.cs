using Azure;
using Azure.Data.Tables;
using Data;
using Data.TableEntities;

var storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
var pottytrainerTableUsers = "users";
var pottytrainerTableAssignments = "assignments";
var pottytrainerTableAssignmentsForUser = "assignmentsforuser";
var pottytrainerTableCompletedAssignments = "completedassignments";
var m_tableServiceClient = new TableServiceClient(storageConnectionString);

// Users
var userTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableUsers);
var deleteUsersTableResult = await userTableClient.DeleteAsync();
Console.WriteLine("\t" + deleteUsersTableResult);

var retryCreateTable = true;

while (retryCreateTable)
{
    try
    {
        var createUsersTableResult = await userTableClient.CreateAsync();
        Console.WriteLine("\t" + createUsersTableResult);
        retryCreateTable = false;
    }
    catch (RequestFailedException rfe)
    {
        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
        {
            Console.WriteLine("Table staged for deletion, retrying...");
            await Task.Delay(5000);
        }
    }
}

//var users = new List<User>();
//for (int i = 0; i < 1000; i++)
//{
//    users.Add(new User
//    {
//        RowKey = Guid.NewGuid().ToString(),
//        Name = $"Stresstest_{i}",
//        Avatar = "twa-girl",
//        Goal = 1000,
//        XP = i,
//    });
//}

var users = DataGenerator.UserData.GetUsers();

foreach (var user in users)
{
    var addEntityResult = await userTableClient.AddEntityAsync(UserEntity.GetEntity(user));
    Console.WriteLine($"Created User: {user.Name}");
}

// Assignments
var assignmentsTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableAssignments);

var deleteAssignmentsTableResult = await assignmentsTableClient.DeleteAsync();
Console.WriteLine("\t" + deleteAssignmentsTableResult);

await Task.Delay(5000);


retryCreateTable = true;

while (retryCreateTable)
{
    try
    {
        var createAssignmentsTableResult = await assignmentsTableClient.CreateAsync();
        Console.WriteLine("\t" + createAssignmentsTableResult);
        retryCreateTable = false;
    }
    catch (RequestFailedException rfe)
    {
        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
        {
            await Task.Delay(5000);
        }
    }
}

var assignments = DataGenerator.UserData.GetAssignments();

foreach (var assignment in assignments)
{
    var addEntityResult = await assignmentsTableClient.AddEntityAsync(AssignmentEntity.GetEntity(assignment));
    Console.WriteLine($"Created Assignment: {addEntityResult.ToString()}");
}

// Assignments for users

var assignmentsForUserTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableAssignmentsForUser);

retryCreateTable = true;

while (retryCreateTable)
{
    try
    {
        var createAssignmentsForUserTableResult = await assignmentsForUserTableClient.CreateAsync();
        Console.WriteLine("\t" + createAssignmentsForUserTableResult);
        retryCreateTable = false;
    }
    catch (RequestFailedException rfe)
    {
        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
        {
            await Task.Delay(5000);
        }
    }
}

foreach(var user in DataGenerator.UserData.GetUsers())
{
    var userWithAssignments = DataGenerator.UserData.GetUserWithAssignments(user.RowKey);
    foreach(var assignment in userWithAssignments.Assignments)
    {
        var addEntityResult = await assignmentsForUserTableClient.AddEntityAsync(AssignmentForUserEntity.GetEntity(assignment, user));
        Console.WriteLine($"Created Assignment for user: {addEntityResult.ToString()}");
    }
}

// Completed assignments
var completedAssignmentsTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableCompletedAssignments);

retryCreateTable = true;

while (retryCreateTable)
{
    try
    {
        var createCompletedAssignmentsTableResult = await completedAssignmentsTableClient.CreateAsync();
        Console.WriteLine("\t" + createCompletedAssignmentsTableResult);
        retryCreateTable = false;
    }
    catch (RequestFailedException rfe)
    {
        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
        {
            await Task.Delay(5000);
        }
    }
}