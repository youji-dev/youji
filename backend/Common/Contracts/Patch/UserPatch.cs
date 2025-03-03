﻿using Common.Enums;

namespace Common.Contracts.Patch
{
    /// <summary>
    /// DTO for update operations on users
    /// </summary>
    /// <param name="NewRole">A new role assignment; set to null to leave unchanged</param>
    /// <param name="NewPreferredEmailLcid">A new preferred language code; set to null to leave unchanged</param>
    public record class UserPatch(
        Roles? NewRole,
        string? NewPreferredEmailLcid)
    {
    }
}
