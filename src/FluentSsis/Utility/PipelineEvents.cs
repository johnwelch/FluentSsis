namespace FluentSsis.Utility
{
    using System;
    using System.IO;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Handles error reporting for pipeline.
    /// Thanks to Matt Masson for
    /// <see cref="https://blogs.msdn.microsoft.com/mattm/2009/08/03/debugging-a-comexception-during-package-generation/">publishing this example</see>.
    /// </summary>
    internal class PipelineEvents : IDTSComponentEvents
    {
        private readonly TextWriter _output;

        public PipelineEvents()
            : this(Console.Out)
        {
        }

        public PipelineEvents(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _output = writer;
        }

        public void FireWarning(int warningCode, string subComponent, string description, string helpFile, int helpContext)
        {
            LogEvent("Warning", subComponent, description, warningCode);
        }

        public void FireInformation(int informationCode, string subComponent, string description, string helpFile, int helpContext, ref bool fireAgain)
        {
            LogEvent("Information", subComponent, description, informationCode);
        }

        public bool FireError(int errorCode, string subComponent, string description, string helpFile, int helpContext)
        {
            LogEvent("Error", subComponent, description, errorCode);
            return true;
        }

        public bool FireQueryCancel()
        {
            return true;
        }

        public void FireBreakpointHit(BreakpointTarget breakpointTarget)
        {
            // Ignored on purpose
        }

        public void FireProgress(string progressDescription, int percentComplete, int progressCountLow, int progressCountHigh, string subComponent, ref bool fireAgain)
        {
            // Ignored on purpose
        }

        public void FireCustomEvent(string eventName, string eventText, ref object[] arguments, string subComponent, ref bool fireAgain)
        {
            LogEvent($"CUSTOM:{eventName}", subComponent, eventText, 0);
        }

        private void LogEvent(string eventName, string subcomponent, string description, int code)
        {
            _output.WriteLine($"[{eventName}] {code} - {subcomponent}: {description} ");
        }
    }
}
