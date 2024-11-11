using Common.Enums;

namespace Common.Contracts.Put
{
    /// <summary>
    /// DTO for update operations on users
    /// </summary>
    /// <param name="NewRole">A new role assigement; set to null to leave unchanged</param>
    public record class UserPutDTO(
        Roles? NewRole)
    {
    }
}
