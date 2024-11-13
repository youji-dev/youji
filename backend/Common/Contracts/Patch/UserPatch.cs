using Common.Enums;

namespace Common.Contracts.Patch
{
    /// <summary>
    /// DTO for update operations on users
    /// </summary>
    /// <param name="NewRole">A new role assigement; set to null to leave unchanged</param>
    public record class UserPatch(
        Roles? NewRole)
    {
    }
}
