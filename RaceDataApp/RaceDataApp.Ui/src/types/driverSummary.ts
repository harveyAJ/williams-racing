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