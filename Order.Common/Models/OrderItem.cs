using System;

namespace OrderCommon
{
    public class OrderItem
    {
        public DateTime Date { get; set; }

        public int Id { get; set; }

        public int Random { get; set; }

        public string OrderText { get; set; }
        
        public int Status { get; set; }
    }
}
