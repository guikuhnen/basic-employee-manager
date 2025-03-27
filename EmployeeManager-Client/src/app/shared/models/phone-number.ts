export class PhoneNumber {
    id: number;
    number: string;
    employeeId: number;
    
    constructor(id: number, number: string, employeeId: number) {
        this.id = id;
        this.number = number;
        this.employeeId = employeeId;
    }
}