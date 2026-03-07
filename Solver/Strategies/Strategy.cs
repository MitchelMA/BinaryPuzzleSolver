using Solver.Enums;

namespace Solver.Strategies;

public abstract class Strategy
{
    public abstract bool Run(FieldValues[][] field);
}