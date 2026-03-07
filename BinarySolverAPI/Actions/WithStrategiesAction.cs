using Solver.Enums;

namespace BinarySolverAPI.Actions;

public struct WithStrategiesAction
{
    public required FieldValues[] Initial { get; init; }
    public required string[] Strategies { get; init; }
}