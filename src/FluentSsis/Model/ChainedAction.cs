namespace FluentSsis.Model
{
    using System;

    public class ChainedOperation<T> : IOperation<T>
    {
        private Action<T> _parent;
        private Action<T> _child;

        public ChainedOperation(Action<T> parent, Action<T> child)
        {
            _parent = parent;
            _child = child;
        }

        public Action<T> Invoke()
        {
            return x =>
            {
                _parent(x);
                _child(x);
            };
        }
    }
}