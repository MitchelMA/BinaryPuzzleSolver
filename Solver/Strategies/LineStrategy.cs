using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public abstract class LineStrategy : Strategy
{
    public sealed override bool Run(FieldValues[] field)
    {
        var fieldSideLength = (int)Math.Sqrt(field.Length);

        for (var rowIndex = 0; rowIndex < fieldSideLength; rowIndex++)
        {
            var idxRange = new Range(rowIndex * fieldSideLength, rowIndex * fieldSideLength + fieldSideLength);
            if (ProcessLine(new ScatteredArray<FieldValues>(field, idxRange)))
                return true;
        }

        for (var columnIndex = 0; columnIndex < fieldSideLength; columnIndex++)
        {
            var idxRange =
                VerticalIndexGenerator(columnIndex, fieldSideLength, field.Length)
                .ToArray();
            
            ProcessLine(new ScatteredArray<FieldValues>(field, idxRange));
        }

        return false;
    }
    
    protected abstract bool ProcessLine(ScatteredArray<FieldValues> line);
    
    private IEnumerable<int> VerticalIndexGenerator(int start, int stepSize, int max)
    {
        var lastValue = start;

        while (lastValue < max)
        {
            yield return lastValue;
            lastValue += stepSize;
        }
    }
}