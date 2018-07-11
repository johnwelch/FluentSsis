namespace FluentSsis.Factories
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    public class ConstraintFactory
    {
        public Action<IDTSSequence> Success(string fromExecutable, string toExecutable)
        {
            return sequence =>
            {
                var constraint = sequence.PrecedenceConstraints.Add(sequence.Executables[fromExecutable], sequence.Executables[toExecutable]);
                constraint.Value = DTSExecResult.Success;
            };
        }

        public Action<IDTSSequence> Failure(string fromExecutable, string toExecutable)
        {
            return sequence =>
            {
                var constraint = sequence.PrecedenceConstraints.Add(sequence.Executables[fromExecutable], sequence.Executables[toExecutable]);
                constraint.Value = DTSExecResult.Failure;
            };
        }

    }
}
