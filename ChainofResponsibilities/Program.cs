using System;
using System.Collections.Generic;
using System.Linq;
using ChainOfResponsibilityDemo.Models;
using ChainOfResponsibilityDemo.Handlers;

namespace ChainOfResponsibilityDemo
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Заказ поступил в обработку...");
            Console.WriteLine();
            
            var sampleItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Name = "Laptop", Quantity = 1, UnitPrice = 1000m },
                new OrderItem { ProductId = 2, Name = "Mouse", Quantity = 2, UnitPrice = 25m },
                new OrderItem { ProductId = 3, Name = "Headphones", Quantity = 1, UnitPrice = 100m }
            };
            
            Console.Write("Введите UserId (например, 123 или 13 для подозрительного): ");
            var uidInput = Console.ReadLine();
            if (!int.TryParse(uidInput, out var userId))
            {
                userId = 123;
                Console.WriteLine($"Неверный ввод. Используется значение по умолчанию: {userId}");
            }

            Console.Write("Введите код купона (например, SAVE10 или FLAT50) или нажмите Enter: ");
            var coupon = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(coupon)) coupon = null;
            
            var initialTotal = sampleItems.Sum(i => i.TotalPrice);

            var context = new OrderContext
            {
                Items = sampleItems,
                TotalAmount = initialTotal,
                UserId = userId,
                CouponCode = coupon
            };
            
            var handlersCatalog = new Dictionary<int, Func<Handler>>
            {
                [1] = () => new InventoryCheckHandler(),
                [2] = () => new FraudDetectionHandler(),
                [3] = () => new DiscountCalculationHandler(),
                [4] = () => new TaxCalculationHandler(0.10m),
                [5] = () => new LoggingHandler("Logging")
            };

            Console.WriteLine();
            Console.WriteLine("Доступные обработчики:");
            Console.WriteLine("1 - InventoryCheckHandler (проверка склада)");
            Console.WriteLine("2 - FraudDetectionHandler (проверка мошенничества)");
            Console.WriteLine("3 - DiscountCalculationHandler (применение скидок)");
            Console.WriteLine("4 - TaxCalculationHandler (начисление налогов)");
            Console.WriteLine("5 - LoggingHandler (логирование состояния)");
            Console.WriteLine();
            Console.WriteLine("Введите номера обработчиков в желаемом порядке через запятую (например: 1,2,3,4).");
            Console.Write("Порядок: ");

            var orderInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(orderInput))
            {
                orderInput = "1,2,3,4";
                Console.WriteLine($"Используется порядок по умолчанию: {orderInput}");
            }
            
            var chosenNumbers = orderInput
                .Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (int.TryParse(s.Trim(), out var n)) return n;
                    return -1;
                })
                .Where(n => handlersCatalog.ContainsKey(n))
                .ToList();

            if (!chosenNumbers.Any())
            {
                Console.WriteLine("Не выбран ни один валидный обработчик. Завершение.");
                return;
            }
            var handlerInstances = chosenNumbers.Select(n => handlersCatalog[n]()).ToList();
            
            var first = handlerInstances.First();
            var current = first;
            foreach (var next in handlerInstances.Skip(1))
            {
                current.SetNext(next);
                current = next;
            }
            
            Console.WriteLine();
            Console.WriteLine("=== Запуск цепочки обработчиков ===");
            first.Handle(context);

            Console.WriteLine();
            if (context.IsApproved)
            {
                Console.WriteLine($"Заказ одобрен. Итоговая сумма: {context.TotalAmount:C}");
            }
            else
            {
                Console.WriteLine($"Заказ отклонён: {context.RejectionReason}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}