export default interface EditUserRequest {
    userId: string,
    preferredEmailLcid: string,
    newRole?: number
  }