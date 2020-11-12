namespace AppDat.Core.Entities
{
    public interface IFieldRangeConstraint<TType> : IConstraint<TType>
    {
        TType MinValue { get; init; }
        TType MaxValue { get; init; }
    }
}
