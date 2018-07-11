using System;
using System.Diagnostics;


namespace FluentSsis
{
    using Microsoft.SqlServer.Dts.Runtime;

    //todo: Switch to log4net
    class EventLogger:IDTSEvents
    {
        public void OnPreValidate(Executable exec, ref bool fireAgain)
        {
            fireAgain = false;
        }

        public void OnPostValidate(Executable exec, ref bool fireAgain)
        {
            fireAgain = false;
        }

        public void OnPreExecute(Executable exec, ref bool fireAgain)
        {
            fireAgain = false;
        }

        public void OnPostExecute(Executable exec, ref bool fireAgain)
        {
            fireAgain = false;
        }

        public void OnWarning(
            DtsObject source,
            int warningCode,
            string subComponent,
            string description,
            string helpFile,
            int helpContext,
            string idofInterfaceWithError)
        {
            Debug.WriteLine($"{nameof(OnWarning)} - {subComponent} : {description}");
        }

        public void OnInformation(
            DtsObject source,
            int informationCode,
            string subComponent,
            string description,
            string helpFile,
            int helpContext,
            string idofInterfaceWithError,
            ref bool fireAgain)
        {
            Debug.WriteLine($"{nameof(OnInformation)} - {subComponent} : {description}");
        }

        public bool OnError(
            DtsObject source,
            int errorCode,
            string subComponent,
            string description,
            string helpFile,
            int helpContext,
            string idofInterfaceWithError)
        {
            throw new InvalidOperationException($"Error:{subComponent}:{description}");
        }

        public void OnTaskFailed(TaskHost taskHost)
        {
            throw new NotImplementedException();
        }

        public void OnProgress(
            TaskHost taskHost,
            string progressDescription,
            int percentComplete,
            int progressCountLow,
            int progressCountHigh,
            string subComponent,
            ref bool fireAgain)
        {
            Debug.WriteLine($"{nameof(OnProgress)} - {subComponent} : {percentComplete}");
        }

        public bool OnQueryCancel()
        {
            return false;
        }

        public void OnBreakpointHit(IDTSBreakpointSite breakpointSite, BreakpointTarget breakpointTarget)
        {
        }

        public void OnExecutionStatusChanged(Executable exec, DTSExecStatus newStatus, ref bool fireAgain)
        {
        }

        public void OnVariableValueChanged(DtsContainer DtsContainer, Variable variable, ref bool fireAgain)
        {
        }

        public void OnCustomEvent(
            TaskHost taskHost,
            string eventName,
            string eventText,
            ref object[] arguments,
            string subComponent,
            ref bool fireAgain)
        {
            Debug.WriteLine($"{nameof(OnCustomEvent)} - {subComponent} : {eventText}");
        }
    }
}
