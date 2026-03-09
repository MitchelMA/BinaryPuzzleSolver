using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class ConsecutivesStrategy : LineStrategy
{
    protected override bool ProcessLine(ScatteredSpan<FieldValues> line)
    {
        var affectedLine = false;

        for (var i = 0; i < line.Length - 1; i++)
        {
            var current = line[i];
            var next = line[i + 1];
            
            if (current == FieldValues.Open)
                continue;
            
            if (current != next)
                continue;

            var oppositeValue = (FieldValues)Convert.ToInt32(!Convert.ToBoolean(current));
            
            if (i > 0 && line[i - 1] == FieldValues.Open)
            {
                line[i - 1] = oppositeValue;
                affectedLine = true;
            }

            if (i < line.Length - 2 && line[i + 2] == FieldValues.Open)
            {
                line[i + 2] = oppositeValue;
                affectedLine = true;
            }
        }

        return affectedLine;
    }
}