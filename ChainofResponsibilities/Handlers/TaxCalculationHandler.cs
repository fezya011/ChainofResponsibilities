using System;
using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public class TaxCalculationHandler : Handler
    {
        private readonly decimal _taxRate;

        public TaxCalculationHandler(decimal taxRate = 0.10m) // дефолт 10%
        {
            _taxRate = taxRate;
        }

        public override void Handle(OrderContext context)
        {
            Console.WriteLine("[Tax] Расчёт налогов...");
            decimal original = context.TotalAmount;
            context.TotalAmount = Decimal.Round(context.TotalAmount * (1 + _taxRate), 2);
            Console.WriteLine($"[Tax] Налог {_taxRate:P0} применён. Новая сумма: {context.TotalAmount:C} (было {original:C}).");
            Process(context);
        }
    }
}