using System.Text;
using Solver.Enums;

namespace BinaryPuzzleSolver;

public static class FieldDisplayer
{
    public static string Display(this FieldValues[] values)
    {
        var count = values.Length;
        var sideLength = Math.Sqrt(count);
        var builder = new StringBuilder();

        for (var i = 0; i < count; i++)
        {
            var displayCharacter = values[i] switch
            {
                FieldValues.Open => "   ",
                FieldValues.Zero => " 0 ",
                FieldValues.One  => " 1 ",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            if (i % sideLength == 0 && i != 0)
                builder.AppendLine();
            
            builder.Append(displayCharacter);
        }

        return builder.ToString();
    }
}