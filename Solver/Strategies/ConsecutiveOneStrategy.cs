using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class ConsecutiveOneStrategy : LineStrategy
{
    protected override bool ProcessLine(ScatteredArray<FieldValues> line)
    {
        var affectedLine = false;

        for (var i = 0; i < line.Length - 1; i++)
        {
            var current = line[i];
            var next = line[i + 1];
            
            if (current != FieldValues.One || current != next)
                continue;

            if (i > 0 && line[i - 1] == FieldValues.Open)
            {
                line[i - 1] = FieldValues.Zero;
                affectedLine = true;
            }

            if (i < line.Length - 2 && line[i + 2] == FieldValues.Open)
            {
                line[i + 2] = FieldValues.Zero;
                affectedLine = true;
            }
        }

        return affectedLine;
    }
}