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

  constructor(private cartService: CartService, private orderService: OrderService) {}

  ngOnInit(): void {
    this.cartService.getCart().subscribe(data => {
      this.cartItems = data;
      this.totalPrice = this.calculateTotalPrice();
    });
  }
  calculateTotalPrice(): number {
    return this.cartItems.reduce((sum, item) => sum + item.product.price * item.quantity, 0);
  }

  placeOrder() {
    this.orderService.placeOrder(this.selectedSlot).subscribe(res => {
      alert('Order placed');
    });
  }


}



 