using System;
using System.Collections.Generic;
using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public class FraudDetectionHandler : Handler
    {
        private readonly HashSet<int> _blacklist;

        public FraudDetectionHandler(IEnumerable<int>? blacklistedUserIds = null)
        {
            _blacklist = blacklistedUserIds != null
                ? new HashSet<int>(blacklistedUserIds)
                : new HashSet<int> { 13, 666 };
        }

        public override void Handle(OrderContext context)
        {
            Console.WriteLine("[Fraud] Проверка безопасности...");
            if (_blacklist.Contains(context.UserId))
            {
                context.IsApproved = false;
                context.RejectionReason = $"[Fraud] Пользователь с id={context.UserId} занесён в чёрный список.";
                Console.WriteLine(context.RejectionReason);
                return;
            }

            Console.WriteLine("[Fraud] Пользователь не подозрителен. Передаём дальше.");
            Process(context);
        }
    }
}