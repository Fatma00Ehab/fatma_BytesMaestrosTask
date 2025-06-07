import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service'; 
import { OrderService } from '../../services/order.service'; 

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  cartItems: any[] = [];
  totalPrice: number = 0;
  selectedSlot: string = '';

  constructor(
    private cartService: CartService,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

   loadCart() {
    this.cartService.getCart().subscribe({
      next: (items) => {
        this.cartItems = items;
        this.totalPrice = this.calculateTotalPrice();
      },
      error: (err) => {
        console.error("Cart loading failed:", err);
      }
    });
  }

   calculateTotalPrice(): number {
    return this.cartItems.reduce((sum, item) => 
      sum + (item?.product?.price || 0) * (item?.quantity || 0), 0
    );
  }

  
  

  removeItem(itemId: number) {
  this.cartService.removeCartItem(itemId).subscribe({
    next: () => {
      console.log('Item removed');
      this.loadCart();   
    },
    error: (error) => {
      console.error('Remove failed:', error);
    }
  });
}


  
  clearCart() {
    this.cartService.clearCart().subscribe({
      next: () => this.loadCart(),
      error: (err) => {
        console.error("Clear failed:", err);
      }
    });
  }

   placeOrder() {
    if (!this.selectedSlot) {
      alert("Please select a delivery time slot.");
      return;
    }

    this.orderService.placeOrder(this.selectedSlot).subscribe({
      next: () => {
        alert("Order placed successfully!");
        this.clearCart(); 
      },
      error: (err) => {
        console.error("Order placement failed:", err);
        alert("Failed to place order.");
      }
    });
  }
}
