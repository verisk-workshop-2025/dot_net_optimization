namespace FileIngestorApp.Core.Contracts;

public interface IFileProcessor
{
    void ProcessBranchesData(string inputDirectory, string outputDirectory);
}