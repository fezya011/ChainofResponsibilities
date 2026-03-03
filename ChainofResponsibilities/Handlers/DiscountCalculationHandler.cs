using System;
using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public class DiscountCalculationHandler : Handler
    {
        public override void Handle(OrderContext context)
        {
            Console.WriteLine("[Discount] Применение скидок (если есть купон)...");
            if (string.IsNullOrWhiteSpace(context.CouponCode))
            {
                Console.WriteLine("[Discount] Купон не указан. Передаём дальше.");
                Process(context);
                return;
            }

            var coupon = context.CouponCode!.Trim().ToUpperInvariant();
            decimal original = context.TotalAmount;
            
            if (coupon == "SAVE10")
            {
                context.TotalAmount = Decimal.Round(context.TotalAmount * 0.90m, 2);
                Console.WriteLine($"[Discount] Применён купон SAVE10. Скидка 10%. Новая сумма: {context.TotalAmount:C} (было {original:C}).");
            }
            else if (coupon == "FLAT50")
            {
                context.TotalAmount = Decimal.Round(Math.Max(0, context.TotalAmount - 50m), 2);
                Console.WriteLine($"[Discount] Применён купон FLAT50. Списано 50. Новая сумма: {context.TotalAmount:C} (было {original:C}).");
            }
            else
            {
                Console.WriteLine($"[Discount] Купон '{context.CouponCode}' не распознан. Игнорируем.");
            }
            
            Process(context);
        }
    }
}