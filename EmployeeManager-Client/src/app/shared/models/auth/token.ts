export class Token {
  authenticated: boolean;
  created: string;
  expiration: string;
  accessToken: string;
  refreshToken: string;

  constructor(authenticated: boolean, created: string, expiration: string, accessToken: string, refreshToken: string) {
    this.authenticated = authenticated;
    this.created = created;
    this.expiration = expiration;
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;    
  }
}
