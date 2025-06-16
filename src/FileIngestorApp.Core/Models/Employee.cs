namespace FileIngestorApp.Core.Models;

public class Employee
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public int Salary { get; init; }
    public Employee()
    {
        Id = 0;
        FirstName = "";
        LastName = "";
        Salary = 0;
    }

    public Employee(int id, string firstName, string lastName, int salary)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
    }
}
