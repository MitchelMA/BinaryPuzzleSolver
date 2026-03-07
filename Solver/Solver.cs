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

    public FieldValues[] Solve(StrategyIterations iterationType)
    {
        Action solverKind = iterationType switch
        {
            StrategyIterations.EarlyReturn => EarlyReturnSolver,
            StrategyIterations.IterateEach => IterateEachSolver,
            _ => throw new ArgumentOutOfRangeException(nameof(iterationType), iterationType, null)
        };

        solverKind.Invoke();
        return _field;
    }
    
    private Solver AddStrategy(Strategy addition)
    {
        _strategies.Add(addition);
        return this;
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
}