import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';  // Import HttpClientModule here
import { Login } from './models/login';
import { Register } from './models/register';
import { JwtAuth } from './models/jwtAuth';
import { AuthenticationService } from './services/authentication.service';
import { AuthenticationInterceptor } from './services/interceptor';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, CommonModule],  // Add HttpClientModule
  providers: [
  
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']  // Corrected typo: 'styleUrl' to 'styleUrls'
})
export class AppComponent {
  title = 'Driver.App';
  loginDto = new Login();
  registerDto = new Register();
  jwtDto = new JwtAuth();

  constructor(private authService: AuthenticationService) {}

  register(registerDto: Register) {
    this.authService.register(registerDto).subscribe();
  }

  login(loginDto: Login) {
    this.authService.login(loginDto).subscribe((jwtDto) => {
      localStorage.setItem('jwtToken', jwtDto.token);
    });
  }

  weather() {
    this.authService.getWeather().subscribe((weatherdata: any) => {
      console.log(weatherdata);
    });
  }
}
