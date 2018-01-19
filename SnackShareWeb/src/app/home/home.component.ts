import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { UserService } from '../services/user.service';
import { User } from '../models/user';

@Component({ 
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [ UserService ]
})
export class HomeComponent implements OnInit {
  

  constructor(private oauthService: OAuthService,
    private userService: UserService) {
    
  }

  ngOnInit() {
    console.log('claims', this.oauthService.getIdentityClaims());

    var userClaims = this.oauthService.getIdentityClaims();
    if (!userClaims) {
      return;
    }

    // TODO: seems like there's a more appropriate place to do this...
    var email = userClaims['email'];
    this.userService.getUserByEmail(email).subscribe(result => {
      // success?
    }, error => {
      let newUser = new User();
      newUser.emailAddress = email;
      newUser.firstName = userClaims['family_name'][1];
      newUser.lastName = userClaims['family_name'][0];

      this.userService.createUser(newUser).subscribe(result => {
        alert('User Created!');
      });
    });

  }
}
