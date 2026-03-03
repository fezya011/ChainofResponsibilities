using System;

namespace ChainOfResponsibilityDemo.Models
{
    public class OrderItem
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; init; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}