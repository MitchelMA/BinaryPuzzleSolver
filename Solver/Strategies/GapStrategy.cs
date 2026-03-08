using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class GapStrategy : LineStrategy
{
    protected override bool ProcessLine(ScatteredSpan<FieldValues> line)
    {
        var affectedLine = false;
        
        for (var i = 0; i < line.Length - 2; i++)
        {
            var current = line[i];
            var gap = line[i + 1];
            var gapSkipped = line[i + 2];

            if (current == FieldValues.Open ||
                gapSkipped == FieldValues.Open ||
                gap != FieldValues.Open ||
                current != gapSkipped)
                continue;

            line[i + 1] = (FieldValues)Convert.ToInt32(!Convert.ToBoolean(current));
            affectedLine = true;
        }

        return affectedLine;
    }
}