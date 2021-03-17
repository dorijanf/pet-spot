import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeResponseDto } from 'src/app/generated-models/authorize-response-dto';
import { AccountService } from 'src/services/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  user: AuthorizeResponseDto;
  userName: string;

  constructor(private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
    if (this.accountService.currentUser !== null && this.accountService.currentUser !== undefined) {
      this.accountService.currentUser.subscribe(x =>
        this.user = x
      );
    }
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['login']);
  }

}
