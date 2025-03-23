using BinaryPuzzleSolver.Enums;

namespace BinaryPuzzleSolver.Strategies;

public abstract class LineStrategy : Strategy
{
    public sealed override bool Run(FieldValues[][] field)
    {
        for (int i = 0; i < field.Length; i++)
            if (ProcessHorizontal(i, field))
                return true;
        
        for (int i = 0; i < field.Length; i++)
            if (ProcessVertical(i, field))
                return true;

        return false;
    }

    protected abstract bool ProcessHorizontal(int yIndex, FieldValues[][] field);
    protected abstract bool ProcessVertical(int xIndex, FieldValues[][] field);
}