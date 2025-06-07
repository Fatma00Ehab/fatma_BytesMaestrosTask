using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Order
    {
        public int Id { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeliveryDateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }




    }
}
