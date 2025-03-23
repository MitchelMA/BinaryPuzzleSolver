using System.CommandLine;
using BinaryPuzzleSolver.Enums;
using BinaryPuzzleSolver.Strategies;

namespace BinaryPuzzleSolver;

internal static class Program
{
    internal static async Task<int> Main(string[] args)
    {
        var fileArgument = new Argument<FileInfo>("filepath", "The binary file to be solved and/or shown");

        var iterationTypeOption = new Option<StrategyIterations>("--iterationKind", () => StrategyIterations.EarlyReturn);
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

    private static async Task<int> SolveCommandHandler(FileInfo fileInfo, StrategyIterations iterationKind)
    {
        var reader = new FieldReader(fileInfo);
        var contents = reader.ReadFile();
        var solver = new Solver(contents);

        solver.AddStrategy(new ZeroStrategy());


        var solvedField = solver.Solve(iterationKind);
        Console.WriteLine(solvedField.Display());
        
        return await Task.FromResult(0);
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

