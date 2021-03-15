import { Component, OnInit } from '@angular/core';
import { Authentication } from 'src/app/helpers/authentication';

@Component({
  selector: 'app-left-panel',
  templateUrl: './left-panel.component.html',
  styleUrls: ['./left-panel.component.css']
})
export class LeftPanelComponent implements OnInit {
  isAuthenticated: boolean;
  constructor(private authentication: Authentication) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authentication.isAuthenticated();
  }

}
