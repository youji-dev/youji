export default interface EditUserRequest {
  userId: string;
  newPreferredEmailLcid?: string;
  newRole?: number;
  newAreEmailNotificationsAllowed?: boolean;
}
