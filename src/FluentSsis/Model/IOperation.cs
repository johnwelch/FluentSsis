namespace FluentSsis.Model
{
    using System;

    public interface IOperation<in T>
    {
        Action<T> Invoke();
    }
}