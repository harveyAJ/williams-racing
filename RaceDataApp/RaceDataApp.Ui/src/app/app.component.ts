import { Component, Injectable } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { DriverSummaryService } from './driver-summary.service';
import { DriverSummary, DriverSummaries } from '../types/driverSummary';

@Injectable()
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Race Data';
  summaries: DriverSummaries = [];
  loading = false;

  constructor(private driverSummaryService: DriverSummaryService) {}

  loadSummaries() {
    this.loading = true;
    this.driverSummaryService.getSummaries().subscribe({
      next: (data) => {
        this.summaries = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }
}
