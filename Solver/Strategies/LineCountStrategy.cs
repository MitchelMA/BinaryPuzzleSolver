using Solver.Enums;

namespace Solver.Strategies;

public class LineCountStrategy : LineStrategy
{
    protected override bool ProcessHorizontal(int yIndex, FieldValues[][] field)
    {
        var horizontal = field[yIndex];
        var width = horizontal.Length;
        var halfWidth = width / 2;
        var oneCount = horizontal.Count(x => x == FieldValues.One);
        var zeroCount = horizontal.Count(x => x == FieldValues.Zero);
        var openCount = width - oneCount - zeroCount;

        if (openCount == 0 || openCount > halfWidth)
            return false;

        if (oneCount != halfWidth && zeroCount != halfWidth)
            return false;

        var underRepresented = oneCount > zeroCount ? FieldValues.Zero : FieldValues.One;
        for (int i = 0; i < width; i++)
            if (horizontal[i] == FieldValues.Open)
                horizontal[i] = underRepresented;

        return true;
    }

    protected override bool ProcessVertical(int xIndex, FieldValues[][] field)
    {
        var vertical = field.Select(x => x[xIndex]).ToArray();
        var height = vertical.Length;
        var halfHeight = height / 2;
        var oneCount = vertical.Count(x => x == FieldValues.One);
        var zeroCount = vertical.Count(x => x == FieldValues.Zero);
        var openCount = height - oneCount - zeroCount;

        if (openCount == 0 || openCount > halfHeight)
            return false;

        if (oneCount != halfHeight && zeroCount != halfHeight)
            return false;

        var underRepresented = oneCount > zeroCount ? FieldValues.Zero : FieldValues.One;
        for (int i = 0; i < height; i++)
            if (vertical[i] == FieldValues.Open)
                field[i][xIndex] = underRepresented;

        return true;
    }
}