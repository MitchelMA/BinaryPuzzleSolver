using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class LineCountStrategy : LineStrategy
{
    protected override bool ProcessLine(ScatteredArray<FieldValues> line)
    {
        var halfLength = line.Length / 2;
        var oneCount = 0;
        var zeroCount = 0;

        foreach (var val in line)
        {
            oneCount += Convert.ToInt32(val == FieldValues.One);
            zeroCount += Convert.ToInt32(val == FieldValues.Zero);
        }

        var openCount = line.Length - oneCount - zeroCount;

        if (openCount == 0 || openCount > halfLength)
            return false;

        if (oneCount != halfLength && zeroCount != halfLength)
            return false;

        var underRepresented = oneCount > zeroCount ? FieldValues.Zero : FieldValues.One;
        
        for (var i = 0; i < line.Length; i++)
            if (line[i] == FieldValues.Open)
                line[i] = underRepresented;

        return true;
    }
}