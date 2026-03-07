using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class ConsecutiveZeroStrategy : LineStrategy
{
    protected override bool ProcessLine(ScatteredArray<FieldValues> line)
    {
        var affectedLine = false;

        for (var i = 0; i < line.Length - 1; i++)
        {
            var current = line[i];
            var next = line[i + 1];
            
            if (current != FieldValues.Zero || current != next)
                continue;

            if (i > 0 && line[i - 1] == FieldValues.Open)
            {
                line[i - 1] = FieldValues.One;
                affectedLine = true;
            }

            if (i < line.Length - 2 && line[i + 2] == FieldValues.Open)
            {
                line[i + 2] = FieldValues.One;
                affectedLine = true;
            }
        }

        return affectedLine;
    }
}