using Solver.Enums;

namespace BinarySolverAPI.Actions;

public readonly struct FullFledgedBinaryAction
{
    public required FieldValues[] Initial { get; init; }
}