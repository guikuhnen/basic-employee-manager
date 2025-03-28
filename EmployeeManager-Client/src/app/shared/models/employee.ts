import { ERoleType } from '../enums/e-role-type';
import { PhoneNumber } from './phone-number';

export class Employee {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  documentNumber: string;
  phoneNumbers: PhoneNumber[];
  managerId: number | null;
  role: ERoleType;
  birthDate: Date;
  active: boolean;

  //#region CUSTOM

  name: string | null;
  password: string;

  //#endregion

  constructor(
    id: number,
    firstName: string,
    lastName: string,
    email: string,
    documentNumber: string,
    phoneNumbers: PhoneNumber[],
    managerId: number,
    role: ERoleType,
    birthDate: Date,
    active: boolean,
    name: string,
    password: string
  ) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.documentNumber = documentNumber;
    this.phoneNumbers = phoneNumbers;
    this.managerId = managerId;
    this.role = role;
    this.birthDate = birthDate;
    this.active = active;
    this.name = name;
    this.password = password;
  }
}
