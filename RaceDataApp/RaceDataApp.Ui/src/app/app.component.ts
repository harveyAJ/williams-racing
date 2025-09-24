import { Component, Injectable } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WeatherForecasts } from '../types/weatherForecast';
import { DriverSummaryService, DriverSummary, DriverSummaries } from './driver-summary.service';

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
  //forecasts: WeatherForecasts = [];
  summaries: DriverSummaries = [];
  loading = false;

  // constructor(private http: HttpClient) {
  //   http.get<WeatherForecasts>('api/weatherforecast').subscribe({
  //     next: result => this.forecasts = result,
  //     error: console.error
  //   });
  // }

  //constructor(private http: HttpClient) {}

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
