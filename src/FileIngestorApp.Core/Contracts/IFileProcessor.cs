namespace FileIngestorApp.Core.Contracts;

public interface IFileProcessor
{
    int GetHighEarnersCount(string filePath, int highSalary);
}