using BatchFileProcessing.Demo;

BatchFileProcessingDemo processor = new();


Console.WriteLine("Generating Files..");
processor.GenerateFiles();

//Console.WriteLine("Executing sequentially");
//processor.ExecuteSequential();

//Console.WriteLine("Executing parallelly");
//processor.ExecuteParallel();