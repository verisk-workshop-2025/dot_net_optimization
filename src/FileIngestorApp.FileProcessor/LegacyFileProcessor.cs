using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using Newtonsoft.Json;

namespace FileIngestorApp.FileProcessor;

public class LegacyFileProcessor : IFileProcessor
{
    public int GetHighEarnersCount(string filePath, int highSalary)
    {
        string text = File.ReadAllText(filePath);
        string[] lines = text.Split(Environment.NewLine);

        // parsing the employees
        List<Employee> employees = [];
        foreach (string json in lines)
        {
            if (string.IsNullOrEmpty(json))
            {
                continue;
            }

            Employee deserialized = JsonConvert.DeserializeObject<Employee>(json) ?? throw new NullReferenceException();
            employees.Add(deserialized);
        }

        List<Employee> highEarners = [];
        foreach (Employee employee in employees)
        {
            if (employee.Salary >= highSalary)
            {
                highEarners.Add(employee);
            }
        }

        return highEarners.Count;
    }
}