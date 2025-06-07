import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-time-slots',
  templateUrl: './time-slots.component.html',
  styleUrls: ['./time-slots.component.css']
})
export class TimeSlotsComponent implements OnInit {
  slots: any[] = [];
  groupedSlots: { [date: string]: any[] } = {};
  selectedDate: string | null = null;
  selectedSlot: any = null;
  loading = true;

  constructor(
    private orderService: OrderService,
    private cartService: CartService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.cartService.getCart().subscribe(() => {
      this.orderService.getAvailableSlots().subscribe({
        next: (slots) => {
          this.groupedSlots = this.groupSlotsByDate(slots);
          this.selectedDate = Object.keys(this.groupedSlots)[0];
          this.loading = false;
        },
        error: (err) => {
          console.error('Failed to get slots', err);
          this.loading = false;
        }
      });
    });
  }

  groupSlotsByDate(slots: any[]): { [date: string]: any[] } {
    const grouped: { [date: string]: any[] } = {};
    slots.forEach(slot => {
      const date = new Date(slot.slot).toISOString().split('T')[0];
      if (!grouped[date]) grouped[date] = [];
      grouped[date].push(slot);
    });
    return grouped;
  }

  getAvailableDates(): string[] {
    return Object.keys(this.groupedSlots);
  }

  selectSlot(slot: any) {
    this.selectedSlot = slot;
  }

  placeOrder() {
    if (!this.selectedSlot) return;

    this.orderService.placeOrder(this.selectedSlot.slot).subscribe({
      next: () => {
        alert('Order placed successfully!');
        this.router.navigate(['/products']);
      },
      error: (err) => {
        console.error('Order failed:', err);
        alert('Order failed. Please try again.');
      }
    });



  }
}
