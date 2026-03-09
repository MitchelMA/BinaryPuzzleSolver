using Solver.Collections;
using Solver.Enums;

namespace Solver.Strategies;

public class CompareStrategy : Strategy
{
    public override bool Run(FieldValues[] field)
    {
        var sideLength = (int)Math.Sqrt(field.Length);

        var rows = new ScatteredMemory<FieldValues>[sideLength];
        var columns = new ScatteredMemory<FieldValues>[sideLength];

        for (var i = 0; i < sideLength; i++)
        {
            rows[i] = GetHorizontal(field, i, sideLength);
            columns[i] = GetVertical(field, i, sideLength);
        }
        
        // Check horizontal against horizontal
        for (var i = 0; i < sideLength; i++)
        {
            for (var j = 0; j < sideLength; j++)
            {
                if (i == j)
                    continue;
                
                var reference = rows[i].Span;
                var toBeCorrected = rows[j].Span;

                var outCome = CorrectLines(reference, toBeCorrected);
                if (!outCome)
                    continue;

                return true;
            }
        }
        
        // Check vertical against vertical
        for (var i = 0; i < sideLength; i++)
        {
            for (var j = 0; j < sideLength; j++)
            {
                if (i == j)
                    continue;
                
                var reference = columns[i].Span;
                var toBeCorrected = columns[j].Span;

                var outCome = CorrectLines(reference, toBeCorrected);
                if (!outCome)
                    continue;

                return true;
            }
        }
        
        // Check for cross-reference: horizontal against vertical
        for (var i = 0; i < sideLength; i++)
        {
            for (var j = 0; j < sideLength; j++)
            {
                var reference = columns[i].Span;
                var toBeCorrected = rows[j].Span;

                var outCome = CorrectLines(reference, toBeCorrected);
                if (!outCome)
                    continue;

                reference = rows[j].Span;
                toBeCorrected = columns[i].Span;
                
                outCome = CorrectLines(reference, toBeCorrected);
                if (!outCome)
                    continue;

                return true;
            }
        }

        return false;
    }

    private static ScatteredMemory<FieldValues> GetHorizontal(FieldValues[] field, int rowIdx, int sideLength)
    {
        return new ScatteredMemory<FieldValues>(
            field,
            (rowIdx * sideLength)..(rowIdx * sideLength + sideLength)
        );
    }


    private static ScatteredMemory<FieldValues> GetVertical(FieldValues[] field, int columnIdx, int sideLength)
    {
        var verticalIndices = VerticalIndicesGenerator(columnIdx, sideLength, field.Length).ToArray();
        return new ScatteredMemory<FieldValues>(field, verticalIndices);
    }
    
    private static IEnumerable<int> VerticalIndicesGenerator(int startIdx, int stepSize, int limit)
    {
        var val = startIdx;
        while (val < limit)
        {
            yield return val;
            val += stepSize;
        }
    }

    private static int ContainsValue(ScatteredSpan<FieldValues> target, FieldValues targetValue)
    {
        var targetValueCount = 0;
        foreach (var val in target)
            targetValueCount += Convert.ToInt32(val == targetValue);

        return targetValueCount;
    }

    private static bool CanBeReferenceLine(ScatteredSpan<FieldValues> target) =>
        ContainsValue(target, FieldValues.Open) is 0;

    private static bool CanBeCorrectedLine(ScatteredSpan<FieldValues> target) =>
        ContainsValue(target, FieldValues.Open) switch
        {
            > 0 and <= 2 => true,
            _ => false
        };

    private static bool TestLines(ScatteredSpan<FieldValues> a, ScatteredSpan<FieldValues> b)
    {
        // Test for a valid case
        if (!(CanBeReferenceLine(a) && CanBeCorrectedLine(b)))
            return false;

        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] == FieldValues.Open || b[i] == FieldValues.Open)
                continue;

            if (a[i] != b[i])
                return false;
        }

        return true;
    }

    private static bool CorrectLines(ScatteredSpan<FieldValues> reference, ScatteredSpan<FieldValues> corrected)
    {
        var outCome = TestLines(reference, corrected);

        if (!outCome)
            return false;
                
        for (var idx = 0; idx < corrected.Length; idx++)
        {
            if (corrected[idx] != FieldValues.Open)
                continue;

            corrected[idx] = (FieldValues)Convert.ToInt32(!Convert.ToBoolean(corrected[idx]));
        }

        return true;
    }
}