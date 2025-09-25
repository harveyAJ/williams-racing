import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DriverSummary } from '../types/driverSummary';

@Injectable({
  providedIn: 'root'
})
export class DriverSummaryService {
  private baseUrl = 'http://localhost:5002';

  constructor(private http: HttpClient) {}

  getSummaries(): Observable<DriverSummary[]> {
    return this.http.get<DriverSummary[]>(`${this.baseUrl}/driver-summary`);
  }
}
