namespace backend.Models.Dtos
{
    public class DeliverySlotRequest
    {
        public List<CartItemDto> Cart { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
