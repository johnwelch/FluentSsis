namespace FluentSsis.DataFlow
{
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

    public static class ManagedComponentWrapperExtensions
    {
        // Doesn't work for non-custom properties.
        //public static CManagedComponentWrapper Named(this CManagedComponentWrapper component, string name)
        //{
        //    component.SetComponentProperty("Name", name);
        //    return component;
        //}
    }
}
