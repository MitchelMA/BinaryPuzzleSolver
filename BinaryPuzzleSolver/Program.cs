using System.CommandLine;
using Solver.Enums;
using Solver.Strategies;

namespace BinaryPuzzleSolver;

internal static class Program
{
    internal static async Task<int> Main(string[] args)
    {
        var fileArgument = new Argument<FileInfo>("filepath", "The binary file to be solved and/or shown");

        var iterationTypeOption = new Option<StrategyIterations>("--iterationKind", () => StrategyIterations.EarlyReturn);
        iterationTypeOption.AddAlias("-i");
        var solveCommand = new Command("solve")
        {
            fileArgument,
            iterationTypeOption
        };
        
        solveCommand.SetHandler(SolveCommandHandler, fileArgument, iterationTypeOption);

        var displayCommand = new Command("display")
        {
            fileArgument
        };
        displayCommand.SetHandler(DisplayCommandHandler, fileArgument);
        
        var rootCommand = new RootCommand("The binary puzzle solver")
        {
            solveCommand,
            displayCommand
        };
        
        return await rootCommand.InvokeAsync(args);
    }

    private static Task<int> SolveCommandHandler(FileInfo fileInfo, StrategyIterations iterationKind)
    {
        var reader = new FieldReader(fileInfo);
        
        try
        {
            var contents = reader.ReadFile();
            var solver = new Solver.Solver(contents)
                .AddStrategy<ConsecutivesStrategy>()
                .AddStrategy<GapStrategy>()
                .AddStrategy<LineCountStrategy>();

            var solvedField = solver.Solve(iterationKind);
            Console.WriteLine(solvedField.Display());
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Failed to solve: {e}");
            return Task.FromResult(1);
        }
        
        return Task.FromResult(0);
    }

    private static Task<int> DisplayCommandHandler(FileInfo fileInfo)
    {
        var reader = new FieldReader(fileInfo);
        
        try
        {
            var displayText = reader.ReadFile().Display();
            Console.WriteLine(displayText);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return Task.FromResult(1);
        }

        return Task.FromResult(0);
    }
}

