using BinaryPuzzleSolver.Enums;

namespace BinaryPuzzleSolver.Strategies;

public class LineCountStrategy : LineStrategy
{
    protected override bool ProcessHorizontal(int yIndex, FieldValues[][] field)
    {
        var horizontal = field[yIndex];
        var openCount = horizontal.Count(x => x == FieldValues.Open);
        if (openCount > 1)
            return false;

        var width = horizontal.Length;
        var oneCount = horizontal.Count(x => x == FieldValues.One);

        for (int i = 0; i < width; i++)
        {
            if (horizontal[i] != FieldValues.Open)
                continue;
            
            horizontal[i] = (oneCount == width / 2) ? FieldValues.Zero : FieldValues.One;
        }

        return true;
    }

    protected override bool ProcessVertical(int xIndex, FieldValues[][] field)
    {
        var vertical = field.Select(x => x[xIndex]).ToArray();
        var openCount = vertical.Count(x => x == FieldValues.Open);
        if (openCount > 1)
            return false;

        var height = vertical.Length;
        var oneCount = vertical.Count(x => x == FieldValues.One);

        for (int i = 0; i < height; i++)
        {
            if (vertical[i] != FieldValues.Open)
                continue;

            field[i][xIndex] = (oneCount == height / 2) ? FieldValues.Zero : FieldValues.One;
        }

        return true;
    }
}