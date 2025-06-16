using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using Newtonsoft.Json;

namespace FileIngestorApp.FileProcessor;
public class OptimizedFileProcessor : IFileProcessor
{
    public int GetHighEarnersCount(string filePath, int highSalary)
    {
        int count = 0;

        Parallel.ForEach(File.ReadAllLines(filePath), json =>
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            Employee deserialized = JsonConvert.DeserializeObject<Employee>(json) ?? throw new NullReferenceException();
            if (deserialized.Salary >= highSalary)
            {
                Interlocked.Increment(ref count);
            }
        });

        return count;
    }

    public int GetHighEarnersCount2(string filePath, int highSalary)
    {
        string text = File.ReadAllText(filePath);
        string[] lines = text.Split(Environment.NewLine);
        int count = 0;

        // parsing the employees
        foreach (string json in lines)
        {
            if (string.IsNullOrEmpty(json))
            {
                continue;
            }
            Employee deserialized = JsonConvert.DeserializeObject<Employee>(json) ?? throw new NullReferenceException();
            if (deserialized.Salary >= highSalary)
            {
                count++;
            }
        }

        return count;
    }
    public int GetHighEarnersCount3(string filePath, int highSalary)
    {
        int count = 0;

        // parsing the employees
        foreach (string json in File.ReadLines(filePath))
        {
            if (string.IsNullOrEmpty(json))
            {
                continue;
            }
            Employee deserialized = JsonConvert.DeserializeObject<Employee>(json) ?? throw new NullReferenceException();
            if (deserialized.Salary >= highSalary)
            {
                count++;
            }
        }

        return count;
    }
}
