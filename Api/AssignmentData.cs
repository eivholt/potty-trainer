using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Api;

public interface IAssignmentData
{
    Task<Assignment> AddAssignment(Assignment assignment);
    Task<bool> DeleteAssignment(int id);
    Task<IEnumerable<Assignment>> GetAssignments();
    Task<Assignment> UpdateAssignment(Assignment assignment);
}

public class AssignmentData : IAssignmentData
{
    private readonly List<Assignment> assignments = new List<Assignment>
        {
            new Assignment
            {
                Id = 10,
                Name = "Bæsj",
                Description = "Bæsje i do",
                Weight = 1
            },
            new Assignment
            {
                Id = 20,
                Name = "Tannpuss",
                Description = "Pusse tenner",
                Weight = 1
            },
            new Assignment
            {
                Id = 30,
                Name = "Matte",
                Description = "Gjøre matteoppgave",
                Weight = 1
            },
            new Assignment
            {
                Id = 40,
                Name = "Lese",
                Description = "Lese selv",
                Weight = 1
            }
        };

    private int GetRandomInt()
    {
        var random = new Random();
        return random.Next(100, 1000);
    }

    public Task<Assignment> AddAssignment(Assignment assignment)
    {
        assignment.Id = GetRandomInt();
        this.assignments.Add(assignment);
        return Task.FromResult(assignment);
    }

    public Task<Assignment> UpdateAssignment(Assignment assignment)
    {
        var index = this.assignments.FindIndex(p => p.Id == assignment.Id);
        this.assignments[index] = assignment;
        return Task.FromResult(assignment);
    }

    public Task<bool> DeleteAssignment(int id)
    {
        var index = assignments.FindIndex(p => p.Id == id);
        assignments.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Assignment>> GetAssignments()
    {
        return Task.FromResult(assignments.AsEnumerable());
    }
}
