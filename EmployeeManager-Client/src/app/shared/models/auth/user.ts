export class User {
    userDocument: string;
    password: string;

    constructor(userDocument: string, password: string) {
        this.userDocument = userDocument;
        this.password = password;
    }
}