using PersistenceLayer.DataAccess.Entities;
using System.Linq.Expressions;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the ticket entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class TicketRepository(DataContext context) : Repository<Ticket, Guid>(context)
    {
        /// <summary>
        /// Get the ids of all users involved in a ticket
        /// </summary>
        /// <param name="ticket">The ticket to get users from</param>
        /// <param name="exclude">A list of user ids to exclude</param>
        /// <returns>A list containing all user ids</returns>
        public IEnumerable<string> GetInvolvedUsersIds(Ticket ticket, IEnumerable<string>? exclude = null)
        {
            List<string> userIds = [ticket.Author];

            string[] commentAuthorIds = ticket.Comments.Select(c => c.Author).ToArray();
            userIds.AddRange(commentAuthorIds);

            exclude ??= [];
            userIds.RemoveAll(id => exclude.Contains(id));

            return userIds;
        }

        /// <summary>
        /// Filters tickets by a given property. Supports strings, integers, DateTime, and custom class properties.
        /// <list type="bullet">
        ///     <item>string | DateTime | int: Contains</item>
        ///     <item>Guid | bool: Equals</item>
        /// </list>
        /// </summary>
        /// <param name="searchTerm">The string to be compared</param>
        /// <param name="propertyName">The property to be compared to the string</param>
        /// <param name="innerPropertyName">The property to be used if the property to be compared is a class</param>
        /// <param name="caseInsensitive">Whether to do a case-insensitive comparision</param>
        /// <returns>Expression</returns>
        public IQueryable<Ticket> GetByPropertyValue(string searchTerm, string propertyName, string? innerPropertyName = null, bool caseInsensitive = true)
        {
            Type? propertyType = typeof(Ticket).GetProperty(propertyName)?.PropertyType;

            propertyType = innerPropertyName is null ? propertyType : propertyType?.GetProperty(innerPropertyName)?.PropertyType;

            if (propertyType is null)
                throw new InvalidOperationException($"Property '{propertyName}' or '{propertyName}.{innerPropertyName}' does not exist on Ticket");

            var ticketParameter = Expression.Parameter(typeof(Ticket), "ticket");
            Expression valueAccessor = Expression.Property(ticketParameter, propertyName);

            if (innerPropertyName is not null)
            {
                valueAccessor = Expression.Property(valueAccessor, innerPropertyName);
            }

            if (propertyType != typeof(string))
            {
                var toStringMethod = propertyType.GetMethod("ToString", Type.EmptyTypes)
                    ?? throw new NotSupportedException($"Type of property '{propertyName}' does not have a 'ToString' method");

                valueAccessor = Expression.Call(valueAccessor, toStringMethod);
            }

            if (caseInsensitive)
            {
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)
                    ?? throw new InvalidOperationException("Could not find 'ToLower' method in string type");

                valueAccessor = Expression.Call(valueAccessor, toLowerMethod);
            }

            Type[] typesUsingContains = [typeof(string), typeof(DateTime), typeof(int)];

            var stringContainsMethod = typeof(string).GetMethod("Contains", [typeof(string)])
                ?? throw new InvalidOperationException("Could not find 'Contains' method in string type");

            var stringEqualsMethod = typeof(string).GetMethod("Equals", [typeof(string), typeof(string)])
                ?? throw new InvalidOperationException("Could not find 'Equals' method in string type");

            var compareMethod = propertyType switch
            {
                Type a when typesUsingContains.Contains(a) => stringContainsMethod,
                _ => stringEqualsMethod,
            };

            var searchTermAccessor = Expression.Constant(caseInsensitive ? searchTerm.ToLower() : searchTerm);

            var compareExpression = Expression.Call(valueAccessor, compareMethod, searchTermAccessor);

            return this.GetAll().Where(Expression.Lambda<Func<Ticket, bool>>(compareExpression, ticketParameter));
        }
    }
}
