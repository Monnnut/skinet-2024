import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  //track if something is loding
  loading = false;
  busyRequestCount = 0;

  busy() {
    this.busyRequestCount++;
    this.loading = true;
  }
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.loading = false;
    }
  }
}
