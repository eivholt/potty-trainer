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

var retryCreateTable = true;

//Users
//var userTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableUsers);
//var deleteUsersTableResult = await userTableClient.DeleteAsync();
//Console.WriteLine("\t" + deleteUsersTableResult);

//while (retryCreateTable)
//{
//    try
//    {
//        var createUsersTableResult = await userTableClient.CreateAsync();
//        Console.WriteLine("\t" + createUsersTableResult);
//        retryCreateTable = false;
//    }
//    catch (RequestFailedException rfe)
//    {
//        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
//        {
//            Console.WriteLine("Table staged for deletion, retrying...");
//            await Task.Delay(5000);
//        }
//    }
//}

//var users = DataGenerator.UserData.GetUsers();

//foreach (var user in users)
//{
//    var addEntityResult = await userTableClient.AddEntityAsync(UserEntity.GetEntity(user));
//    Console.WriteLine($"Created User: {user.Name}");
//}

// Assignments
var assignmentsTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableAssignments);
var deleteAssignmentsTableResult = await assignmentsTableClient.DeleteAsync();
Console.WriteLine("Table assignments deleted: " + deleteAssignmentsTableResult.Status);

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
            Console.WriteLine("Table busy, retrying in 5 seconds..");
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
var deleteAssignmentsForUserTableResult = await assignmentsForUserTableClient.DeleteAsync();
Console.WriteLine("Table assignmentsforuser deleted: " + deleteAssignmentsForUserTableResult.Status);

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
            Console.WriteLine("Table busy, retrying in 5 seconds..");
            await Task.Delay(5000);
        }
    }
}

foreach (var user in DataGenerator.UserData.GetUsers())
{
    var userWithAssignments = DataGenerator.UserData.GetUserWithAssignments(user.RowKey);
    foreach (var assignment in userWithAssignments.Assignments)
    {
        var addEntityResult = await assignmentsForUserTableClient.AddEntityAsync(AssignmentForUserEntity.GetEntity(assignment, user));
        Console.WriteLine($"Created Assignment for user: {addEntityResult.ToString()}");
    }
}

// Completed assignments
//var completedAssignmentsTableClient = m_tableServiceClient.GetTableClient(pottytrainerTableCompletedAssignments);

//var completedAssignments = completedAssignmentsTableClient.QueryAsync<CompletedAssignmentEntity>();

//await foreach(var completedAssignmentEntity in completedAssignments)
//{
//    var patchedEntity = new CompletedAssignmentEntity(completedAssignmentEntity.RowKey.ToUpper())
//    {
//        AssignmentRowKey = completedAssignmentEntity.AssignmentRowKey,
//        UserRowKey = completedAssignmentEntity.UserRowKey,
//        TimeCompleted = completedAssignmentEntity.Timestamp.Value.UtcDateTime,
//        XP = completedAssignmentEntity.XP,
//        Name = completedAssignmentEntity.Name
//    };

//    await completedAssignmentsTableClient.AddEntityAsync<CompletedAssignmentEntity>(patchedEntity);
//    await completedAssignmentsTableClient.DeleteEntityAsync(CompletedAssignmentEntity.PartitionKeyName, completedAssignmentEntity.RowKey);
//}


//var deleteCompletedAssignmentsTableResult = await completedAssignmentsTableClient.DeleteAsync();
//Console.WriteLine("Table completedassignments deleted: " + deleteCompletedAssignmentsTableResult.Status);

//retryCreateTable = true;

//while (retryCreateTable)
//{
//    try
//    {
//        var createCompletedAssignmentsTableResult = await completedAssignmentsTableClient.CreateAsync();
//        Console.WriteLine("\t" + createCompletedAssignmentsTableResult);
//        retryCreateTable = false;
//    }
//    catch (RequestFailedException rfe)
//    {
//        if (rfe.ErrorCode.Equals("TableBeingDeleted"))
//        {
//            Console.WriteLine("Table busy, retrying in 5 seconds..");
//            await Task.Delay(5000);
//        }
//    }
//}