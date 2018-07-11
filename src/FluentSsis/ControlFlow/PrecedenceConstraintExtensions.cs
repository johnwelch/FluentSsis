namespace FluentSsis.ControlFlow
{
    using Microsoft.SqlServer.Dts.Runtime;

    public static class PrecedenceConstraintExtensions
    {
        public static bool TryGetValue(this PrecedenceConstraints constraints, string source, string target, out PrecedenceConstraint constraint)
        {
            foreach (var item in constraints)
            {
                if (((IDTSName)item.PrecedenceExecutable).Name == source &&
                    ((IDTSName)item.ConstrainedExecutable).Name == target)
                {
                    constraint = item;
                    return true;
                }
            }

            constraint = null;
            return false;
        }
    }
}
