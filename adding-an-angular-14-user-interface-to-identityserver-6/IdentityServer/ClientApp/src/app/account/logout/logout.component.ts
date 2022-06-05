import {HttpClient} from "@angular/common/http";
import {Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {map} from "rxjs";

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent {
  logoutId = this.route.snapshot.queryParamMap.get('logoutId');

  response$ = this.http.get<{ prompt: boolean, iFrameUrl: string, postLogoutRedirectUri: string }>(`/api/logout?logoutId=${this.logoutId}`);

  constructor(private http: HttpClient, private route: ActivatedRoute) {}

  onClick() {
    this.response$ = this.http.post<{ iFrameUrl: string, postLogoutRedirectUri: string }>(`/api/logout?logoutId=${this.logoutId}`, null).pipe(
      map(value => ({...value, prompt: false}))
    );
  }
}
