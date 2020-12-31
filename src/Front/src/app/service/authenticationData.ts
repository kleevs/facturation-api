import { AuthenticationModel } from '../model/authentication';

export class AuthenticationData {
  constructor(public current: { name: string; id: number; data: AuthenticationModel[] }) {
  }
}