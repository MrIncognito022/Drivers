import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../models/register';
import { Observable } from 'rxjs';
import { JwtAuth } from '../models/jwtAuth';
import { environment } from '../../environments/environment.development';
import { Login } from '../models/login';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  registerUrl = "AuthManagement/Register";
  loginUrl = "AuthManagement/Login";
  weatherUrl = "WeatherForecast"
  constructor(private http:HttpClient) { }

  public register (user:Register):Observable<JwtAuth>{
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.registerUrl}`, user);
  }

  public login (user:Login):Observable<JwtAuth>{
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.loginUrl}`, user);
  }
  public getWeather ():Observable<any>{
    return this.http.get<any>(`${environment.apiUrl}/${this.weatherUrl}`);
  }
}
