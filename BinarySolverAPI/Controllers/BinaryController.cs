using System.Reflection;
using System.Text;
using BinarySolverAPI.Actions;
using Microsoft.AspNetCore.Mvc;
using Solver.Strategies;
using Solver.Enums;

namespace BinarySolverAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BinaryController : Controller
{
    private static readonly Assembly SolverAssembly = Assembly.GetAssembly(typeof(Solver.Solver))!;
    private static readonly string StrategyNamespace = $"{typeof(Strategy).Namespace}.";
    private static string[]? _allowedStrategyNames = null;
    
    [HttpPost("")]
    [HttpPost("[action]")]
    public IActionResult Index([FromBody] FullFledgedBinaryAction action)
    {
        var solver = new Solver.Solver(action.Initial)
            .AddStrategy<ConsecutivesStrategy>()
            .AddStrategy<GapStrategy>()
            .AddStrategy<LineCountStrategy>()
            .AddStrategy<CompareStrategy>()
            .AddStrategy<LastResortStrategy>();

        return HandleSolver(solver);
    }

    [HttpPost("[action]")]
    public IActionResult WithStrategies([FromBody] WithStrategiesAction action)
    {
        if (action.Strategies.Length <= 0)
            return BadRequest("At least one (1) strategy is required");
        
        var solver = new Solver.Solver(action.Initial);

        var strategyTypes = action.Strategies.Select(s => (s, GetStrategyType(s))).ToArray();
        var possibleFailure = BuildWrongStrategyMessage(strategyTypes);

        if (possibleFailure is not null)
            return possibleFailure;

        var isolatedStrategies = strategyTypes.Select(t => Activator.CreateInstance(t.Item2!) as Strategy);

        foreach (var strat in isolatedStrategies)
            solver.AddStrategy(strat!);

        return HandleSolver(solver);
    }

    private IActionResult HandleSolver(Solver.Solver solver)
    {
        FieldValues[] solved;
        try
        {
            solved = solver.Solve(StrategyIterations.EarlyReturn);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(solved);
    }

    private BadRequestObjectResult? BuildWrongStrategyMessage((string, Type?)[] types)
    {
        var failed = types.Where(t => t.Item2 is null).ToArray();
        
        if (failed.Length <= 0)
            return null;

        _allowedStrategyNames ??= SolverAssembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => typeof(Strategy).IsAssignableFrom(t))
            .Select(t => t.Name)
            .ToArray();
        
        var sb = new StringBuilder();
        sb
            .AppendJoin("\r\n", failed.Select(tuple => $"'{tuple.Item1}' is not a valid Strategy"))
            .AppendLine("\r\n")
            .AppendLine("Valid strategy-names are as follows:")
            .AppendJoin("\r\n", _allowedStrategyNames.Select(n => $" - {n}"));
            
        return BadRequest(sb.ToString());
    }
    
    private static Type? GetStrategyType(string strategyName) =>
        SolverAssembly.GetType(StrategyNamespace + strategyName);
}