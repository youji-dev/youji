namespace Common.Enums
{
    /// <summary>
    /// Flag Enum representing user roles
    /// </summary>
    [Flags]
    public enum Roles
    {
        /// <summary>
        /// User can create and edit his own tickets and see other tickets
        /// </summary>
        Teacher = 0,

        /// <summary>
        /// User can manage all tickets
        /// </summary>
        FacilityManager = 1,

        /// <summary>
        /// User can manage all Tickets and administrate the system
        /// </summary>
        Admin = 2,
    }
}