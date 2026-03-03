using System;
using System.Collections.Generic;
using ChainOfResponsibilityDemo.Models;

namespace ChainOfResponsibilityDemo.Handlers
{
    public class InventoryCheckHandler : Handler
    {
        private readonly Dictionary<int, int> _stock;

        public InventoryCheckHandler(Dictionary<int, int>? initialStock = null)
        {
            _stock = initialStock ?? new Dictionary<int, int>
            {
                [1] = 5,   // Laptop
                [2] = 10,  // Mouse
                [3] = 0,   // Headphones (out of stock)
                [4] = 2    // Monitor
            };
        }

        public override void Handle(OrderContext context)
        {
            Console.WriteLine("[Inventory] Проверка склада...");

            foreach (var item in context.Items)
            {
                _stock.TryGetValue(item.ProductId, out var available);
                if (item.Quantity > available)
                {
                    context.IsApproved = false;
                    context.RejectionReason = $"[Inventory] Товар '{item.Name}' (id={item.ProductId}) в количестве {item.Quantity} отсутствует на складе (доступно {available}).";
                    Console.WriteLine(context.RejectionReason);
                    return;
                }
            }

            Console.WriteLine("[Inventory] Все позиции в наличии. Передаём дальше.");
            Process(context);
        }
    }
}