using Common.Enums;

namespace Common.Contracts.Patch
{
    /// <summary>
    /// DTO for update operations on users
    /// </summary>
    /// <param name="NewRole">A new role assignment</param>
    /// <param name="NewPreferredEmailLcid">A new preffered language code</param>
    /// <param name="NewAreEmailNotificationsAllowed">New setting for whether the user wants to recieve e-mails</param>
    public record class UserPatch(
        Roles? NewRole,
        string? NewPreferredEmailLcid,
        bool? NewAreEmailNotificationsAllowed)
    {
    }
}
