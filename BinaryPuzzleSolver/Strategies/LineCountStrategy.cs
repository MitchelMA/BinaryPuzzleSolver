using BinaryPuzzleSolver.Enums;

namespace BinaryPuzzleSolver.Strategies;

public class LineCountStrategy : LineStrategy
{
    protected override bool ProcessHorizontal(int yIndex, FieldValues[][] field)
    {
        var horizontal = field[yIndex];
        var openCount = horizontal.Count(x => x == FieldValues.Open);
        if (openCount is > 2 or 0)
            return false;

        var width = horizontal.Length;
        var oneCount = horizontal.Count(x => x == FieldValues.One);
        var zeroCount = width - oneCount - openCount;
        var diff = oneCount - zeroCount;
        if (diff == 0)
            return false;

        var underRepresented = diff > 0 ? FieldValues.Zero : FieldValues.One;
        for (int i = 0; i < width; i++)
            if (horizontal[i] == FieldValues.Open)
                horizontal[i] = underRepresented;

        return true;
    }

    protected override bool ProcessVertical(int xIndex, FieldValues[][] field)
    {
        var vertical = field.Select(x => x[xIndex]).ToArray();
        var openCount = vertical.Count(x => x == FieldValues.Open);
        if (openCount is > 2 or 0)
            return false;

        var height = vertical.Length;
        var oneCount = vertical.Count(x => x == FieldValues.One);
        var zeroCount = height - oneCount - openCount;
        var diff = oneCount - zeroCount;

        if (diff == 0)
            return false;

        var underRepresented = diff > 0 ? FieldValues.Zero : FieldValues.One;
        for (int i = 0; i < height; i++)
            if (vertical[i] == FieldValues.Open)
                field[i][xIndex] = underRepresented;

        return true;
    }
}