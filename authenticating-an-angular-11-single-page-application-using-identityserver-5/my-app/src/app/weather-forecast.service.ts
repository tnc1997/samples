import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { WeatherForecast } from './weather-forecast';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {
  constructor(private http: HttpClient) { }

  get(): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(`${environment.apiUrl}/WeatherForecast`);
  }
}
