using System;
using System.Collections.Generic;

namespace ChainOfResponsibilityDemo.Models
{
    public class OrderContext
    {
        public List<OrderItem> Items { get; init; } = new();
        public decimal TotalAmount { get; set; }
        public bool IsApproved { get; set; } = true;
        public string RejectionReason { get; set; } = string.Empty;
        public int UserId { get; init; }
        public string? CouponCode { get; init; }

        public override string ToString()
        {
            return $"UserId={UserId}, Items={Items.Count}, TotalAmount={TotalAmount:C}, IsApproved={IsApproved}, RejectionReason={RejectionReason}, Coupon={CouponCode}";
        }
    }
}