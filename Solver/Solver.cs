using Solver.Enums;
using Solver.Strategies;

namespace Solver;

public class Solver
{
    private readonly FieldValues[] _field;
    private readonly HashSet<Strategy> _strategies = new();
    
    public Solver(FieldValues[] field)
    {
        _field = field;
    }

    public Solver AddStrategy<T>()
        where T : Strategy, new()
    {
        return AddStrategy(new T());
    }
    
    public Solver AddStrategy(Strategy addition)
    {
        _strategies.Add(addition);
        return this;
    }

    public FieldValues[] Solve(StrategyIterations iterationType)
    {
        var errorCode = TestLength();

        var errorMessage = errorCode switch
        {
            1 => "The side-lengths are not equal",
            2 => "The side-lengths are not even in length",
            _ => null,
        };

        if (errorMessage is not null)
            throw new ArgumentException(errorMessage);
        
        Action solverKind = iterationType switch
        {
            StrategyIterations.EarlyReturn => EarlyReturnSolver,
            StrategyIterations.IterateEach => IterateEachSolver,
            _ => throw new ArgumentOutOfRangeException(nameof(iterationType), iterationType, null)
        };

        solverKind.Invoke();
        return _field;
    }
    
    private void EarlyReturnSolver()
    {
        var fieldRun = true;
        while (fieldRun)
        {
            fieldRun = false;

            var outcomes = _strategies
                .Select(strat => strat.Run(_field));
            
            foreach (var outcome in outcomes)
            {
                fieldRun |= outcome;
                
                if (outcome)
                    break;
            }
        }
    }

    private void IterateEachSolver()
    {
        var fieldRun = true;
        while (fieldRun)
        {
            fieldRun = _strategies
                .Aggregate(false,
                    (current, strategy) => current | strategy.Run(_field));
        }
    }

    private int TestLength()
    {
        var sideLength = Math.Sqrt(_field.Length);
        
        // Check if the side-lenght is whole
        if (Math.Abs(sideLength - (int)sideLength) > float.Epsilon)
            return 1;
        
        // Test if the side-lengths are even, if not: return false
        if (((int)sideLength & 1) == 1)
            return 2;

        return 0;
    }
}