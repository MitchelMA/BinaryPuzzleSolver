using System.Text;
using BinaryPuzzleSolver.Enums;

namespace BinaryPuzzleSolver;

public static class FieldDisplayer
{
    public static string Display(this FieldValues[][] values)
    {
        var width = values[0].Length;
        var builder = new StringBuilder();
        foreach (var line in values)
        {
            foreach (var item in line)
            {
                var displayCharacter = item switch
                {
                    FieldValues.Open => "   ",
                    FieldValues.Zero => " 0 ",
                    FieldValues.One => " 1 ",
                    _ => throw new ArgumentOutOfRangeException()
                };
                builder.Append(displayCharacter);
            }
 
            builder.AppendLine();
        }
 
        return builder.ToString();   
    }
}