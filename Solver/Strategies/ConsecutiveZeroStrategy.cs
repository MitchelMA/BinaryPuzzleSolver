using Solver.Enums;

namespace Solver.Strategies;

public class ConsecutiveZeroStrategy : LineStrategy
{
    protected override bool ProcessHorizontal(int yIndex, FieldValues[][] field)
    {
        var width = field.Length;
        var affectedLine = false;
        for (int i = 0; i < width - 1; i++)
        {
            var current = field[yIndex][i];
            var right = field[yIndex][i + 1];
            
            if (current != FieldValues.Zero)
                continue;

            if (current != right)
                continue;

            if (i > 0 && field[yIndex][i - 1] == FieldValues.Open)
            {
                field[yIndex][i - 1] = FieldValues.One;
                affectedLine = true;
            }

            if (i < width - 2 && field[yIndex][i + 2] == FieldValues.Open)
            {
                field[yIndex][i + 2] = FieldValues.One;
                affectedLine = true;
            }
        }

        return affectedLine;
    }

    protected override bool ProcessVertical(int xIndex, FieldValues[][] field)
    {
        var height = field.Length;
        var affectedLine = false;
        for (int i = 0; i < height - 1; i++)
        {
            var current = field[i][xIndex];
            var below = field[i + 1][xIndex];
            
            if (current != FieldValues.Zero)
                continue;

            if (current != below)
                continue;

            if (i > 0 && field[i - 1][xIndex] == FieldValues.Open)
            {
                field[i - 1][xIndex] = FieldValues.One;
                affectedLine = true;
            }

            if (i < height - 2 && field[i + 2][xIndex] == FieldValues.Open)
            {
                field[i + 2][xIndex] = FieldValues.One;
                affectedLine = true;
            }
        }

        return affectedLine;
    }
}