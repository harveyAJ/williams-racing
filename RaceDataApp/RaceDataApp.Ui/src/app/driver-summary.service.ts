import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DriverSummary {
  driverId: number;
  driverRef: string;
  forename: string;
  surname: string;
  dob: string;
  nationality: string;
  totalPodiums: number;
  totalRaces: number;
}

export type DriverSummaries = DriverSummary[];

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
