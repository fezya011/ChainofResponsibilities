using System;
using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public class LoggingHandler : Handler
    {
        private readonly string _stageName;

        public LoggingHandler(string stageName = "Log")
        {
            _stageName = stageName;
        }

        public override void Handle(OrderContext context)
        {
            Console.WriteLine($"[{_stageName}] Состояние заказа: {context}");
            Process(context);
        }
    }
}