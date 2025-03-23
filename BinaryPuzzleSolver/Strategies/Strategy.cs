using BinaryPuzzleSolver.Enums;

namespace BinaryPuzzleSolver.Strategies;

public abstract class Strategy
{
    public abstract bool Run(FieldValues[][] field);
}