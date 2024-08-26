import { UserModel } from './user.model';

export interface UserPermissionsModel extends UserModel {
  isAdmin: boolean;
  isOwner: boolean;
}
