import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
    issuer: 'https://id.graydientcreative.com',
    redirectUri: window.location.origin + '/index.html',
    clientId: 'snackshare',
    scope: 'openid profile email snackshare'

}