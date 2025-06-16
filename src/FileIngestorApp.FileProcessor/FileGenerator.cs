using Bogus;
using FileIngestorApp.Core.Models;
using Newtonsoft.Json;

namespace FileIngestorApp.FileProcessor;

public class FileGenerator
{
    public void GenerateFile(int size, string path)
    {
        FileInfo file = new(path);
        if (file.Directory?.Exists == false)
        {
            file.Directory.Create();
            return;
        }
        File.WriteAllLines(file.FullName, GeneratePersonData(size).Select(x => JsonConvert.SerializeObject(x)));
    }

    private IEnumerable<Employee> GeneratePersonData(int size)
    {
        int[] salaries = { 1000, 2000, 3000, 5000, 10000 };

        List<Employee> persons = new Faker<Employee>()
            .RuleFor(x => x.Id, f => f.IndexGlobal)
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Salary, f => f.PickRandom(salaries))
            .Generate(size).ToList();

        return persons;
    }
}
