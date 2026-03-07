using Solver.Enums;

namespace Solver.Strategies;

public class GapStrategy : LineStrategy
{
    protected override bool ProcessHorizontal(int yIndex, FieldValues[][] field)
    {
        var width = field.Length;
        var affectedLine = false;

        for (int i = 0; i < width - 2; i++)
        {
            var current = field[yIndex][i];
            var gap = field[yIndex][i + 1];
            var gapSkipped = field[yIndex][i + 2];

            if (current == FieldValues.Open || 
                gapSkipped == FieldValues.Open ||
                gap != FieldValues.Open ||
                current != gapSkipped)
                continue;

            field[yIndex][i + 1] = (FieldValues)Convert.ToInt32(!Convert.ToBoolean(current));
            affectedLine = true;
        }

        return affectedLine;
    }

    protected override bool ProcessVertical(int xIndex, FieldValues[][] field)
    {
        var height = field.Length;
        var affectedLine = false;

        for (int i = 0; i < height - 2; i++)
        {
            var current = field[i][xIndex];
            var gap = field[i + 1][xIndex];
            var gapSkipped = field[i + 2][xIndex];
            
            if (current == FieldValues.Open ||
                gapSkipped == FieldValues.Open ||
                gap != FieldValues.Open ||
                current != gapSkipped)
                continue;

            field[i + 1][xIndex] = (FieldValues)Convert.ToInt32(!Convert.ToBoolean(current));
            affectedLine = true;
        }

        return affectedLine;
    }
}