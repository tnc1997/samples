import { Component, OnInit } from '@angular/core';
import { Observable} from 'rxjs';

import { WeatherForecast } from '../weather-forecast';
import { WeatherForecastService } from '../weather-forecast.service';

@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.css']
})
export class WeatherForecastComponent implements OnInit {
  weatherForecasts$: Observable<WeatherForecast[]>;

  constructor(private weatherForecastService: WeatherForecastService) { }

  ngOnInit(): void {
    this.weatherForecasts$ = this.weatherForecastService.get();
  }
}
