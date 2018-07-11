namespace FluentSsis.Model
{
    using System;

    public class Operation<T> : IOperation<T>
    {
        private Action<T> _child;

        public Operation(Action<T> child)
        {
            _child = child;
        }

        public Action<T> Invoke()
        {
            return x =>
            {
                _child(x);
            };
        }
    }
}