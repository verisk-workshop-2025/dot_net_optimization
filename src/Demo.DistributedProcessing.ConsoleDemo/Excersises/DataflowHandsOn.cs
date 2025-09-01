using System.Threading.Tasks.Dataflow;

namespace Demo.DistributedProcessing.ConsoleDemo.Excersises;

// Create a data flow blocks for a car factory
internal class DataflowHandsOn
{
    // Create data flow block that will create cars using the dataflow blocks. You can define each blocks as you fit,
    // you can use the following blocks as a guideline
    // Create frame
    // Put engine
    // Add interior
    // Add paint
    // Delivery/ready

    public static async Task Run()
    {
        TransformBlock<string, string> createFrameBlock = new(t =>
        {
            Console.WriteLine("Creating frame");
            return "Frame created";
        });
        TransformBlock<string, string> putEngineBlock = new(t =>
        {
            Console.WriteLine("Putting engine");
            return "Engine added";
        });
        TransformBlock<string, string> addInteriorBlock = new(t =>
        {
            Console.WriteLine("Adding interior");
            return "Interior added";
        });
        TransformBlock<string, string> addPaintBlock = new(t =>
        {
            Console.WriteLine("Adding paint");
            return "Painted";
        });
        ActionBlock<string> deliverBlock = new(t =>
        {
            Console.WriteLine("Delivered");
        });

        DataflowLinkOptions linkOptions = new() { PropagateCompletion = true };

        createFrameBlock.LinkTo(putEngineBlock, linkOptions);
        putEngineBlock.LinkTo(addInteriorBlock, linkOptions);
        addPaintBlock.LinkTo(deliverBlock, linkOptions);

        createFrameBlock.Post("Tesla");
        createFrameBlock.Complete();

        await deliverBlock.Completion;
    }
}
