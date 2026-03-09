using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class LastResortStrategy : Strategy
{
    public override bool Run(FieldValues[] field)
    {
        var fieldCopy = new FieldValues[field.Length];
        Array.Copy(field, fieldCopy, fieldCopy.Length);
        
        // Create an array that maps the open values in the field
        var openIndices = fieldCopy
            .Select((val, idx) => (val, idx))
            .Where((tuple) => tuple.val == FieldValues.Open)
            .Select(tuple => tuple.idx).ToArray();
        
        var open = new ScatteredSpan<FieldValues>(fieldCopy, openIndices);
        
        // Outer loop for all open values
        for (var i = 0; i < open.Length; i++)
        {
            Array.Copy(field, fieldCopy, field.Length);
            
            // Inner loop for either 1 or 0 value
            for (var j = 0; j < 2; j++)
            {
                open[i] = (FieldValues)j;
                
                var innerProbeSolver = new Solver(fieldCopy)
                    .AddStrategy<ConsecutivesStrategy>()
                    .AddStrategy<GapStrategy>()
                    .AddStrategy<LineCountStrategy>()
                    .AddStrategy<CompareStrategy>();

                innerProbeSolver.Solve(StrategyIterations.EarlyReturn);

                var containsOpen = fieldCopy.Contains(FieldValues.Open);

                // Success
                if (!containsOpen)
                {
                    Array.Copy(fieldCopy, field, field.Length);
                    return true;
                }
            }
        }

        return false;
    }
}