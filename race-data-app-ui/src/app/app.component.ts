import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DriverSummaryService, DriverSummary } from './driver-summary.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'race-data-app-ui';
  summaries: DriverSummary[] = [];
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
