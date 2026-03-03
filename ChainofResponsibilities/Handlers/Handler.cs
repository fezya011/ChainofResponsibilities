using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public abstract class Handler
    {
        protected Handler? _nextHandler;
        public virtual Handler SetNext(Handler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        public abstract void Handle(OrderContext context);
        
        protected virtual void Process(OrderContext context)
        {
            if (_nextHandler != null)
            {
                _nextHandler.Handle(context);
            }
        }
    }
}