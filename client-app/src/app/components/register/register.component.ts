import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomValidators } from 'src/app/helpers/custom-validators';
import { AccountService } from 'src/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  hide = true;
  error: string;
  authenticationForm = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(30)
    ]),
    password: new FormControl('', [
      Validators.required,
      CustomValidators.patternValidator(/\d/, { hasNumber: true }),
      CustomValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
      CustomValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
      CustomValidators.patternValidator(/[$&+,:;=?@#|'<>.^*()%!-]/, { hasSpecialCharacters: true }),
      Validators.minLength(10)
    ]),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl('', [Validators.email, Validators.required]),
  })
  constructor(private router: Router, private accountService: AccountService) {

  }

  get userName() { return this.authenticationForm.get('userName'); }
  get password() { return this.authenticationForm.get('password'); }
  get firstName() { return this.authenticationForm.get('firstName'); }
  get lastName() { return this.authenticationForm.get('lastName'); }
  get email() { return this.authenticationForm.get('email'); }

  ngOnInit(): void {
  }

  onSubmit() {
    this.accountService.register(this.authenticationForm.value)
      .subscribe(result => {
        this.router.navigate(['login'])
      },
        error => console.error(error));
  }

}
