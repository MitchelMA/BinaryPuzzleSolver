using BinaryPuzzleSolver.Enums;
using BinaryPuzzleSolver.Strategies;

namespace BinaryPuzzleSolver;

public class Solver
{
    private FieldValues[][] _field;
    private List<Strategy> _strategies = new();
    
    public Solver(FieldValues[][] field)
    {
        _field = field;
    }
    
    public override string ToString()
    {
        return _field.Display();
    }

    public Solver AddStrategy(Strategy addition)
    {
        _strategies.Add(addition);
        return this;
    }

    public FieldValues[][] Solve(StrategyIterations iterationType)
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

    private void EarlyReturnSolver()
    {
        var fieldRun = true;
        while (fieldRun)
        {
            fieldRun = false;
            
            foreach (var strategy in _strategies)
            {
                var outcome = strategy.Run(_field);
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
            fieldRun = false;
            
            foreach (var strategy in _strategies)
                fieldRun |= strategy.Run(_field);
        }
    }
}