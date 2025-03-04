using Common.Enums;

namespace Common.Contracts.Patch
{
    /// <summary>
    /// DTO for update operations on users
    /// </summary>
    /// <param name="NewRole">A new role assignment</param>
    /// <param name="NewPreferredEmailLcid">A new preferred language code</param>
    /// <param name="NewAreEmailNotificationsAllowed">New setting for whether the user wants to receive e-mails</param>
    public record class UserPatch(
        Roles? NewRole,
        string? NewPreferredEmailLcid,
        bool? NewAreEmailNotificationsAllowed)
    {
    }
}
