using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the entity of a ticket.
    /// </summary>
    public record class Ticket
    {
        /// <summary>
        /// The id of the ticket.
        /// </summary>
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// The title of the ticket:
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The description of the ticket.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The priority of the ticket
        /// </summary>
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public required Priority Priority { get; set; }

        /// <summary>
        /// The author of the ticket.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The date when the ticket was created
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The state of the ticket.
        /// </summary>
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public required State State { get; set; }

        /// <summary>
        /// An array of all comments on the ticket.
        /// </summary>
        public Collection<TicketComment> Comments { get; set; } = [];

        /// <summary>
        /// An array of all attachements on the ticket.
        /// </summary>
        public Collection<TicketAttachment> Attachments { get; set; } = [];

        /// <summary>
        /// The building where the issue of the ticket was located.
        /// </summary>
        public Building? Building { get; set; }

        /// <summary>
        /// The room where the issue of the ticket was located.
        /// </summary>
        public string? Room { get; set; }

        /// <summary>
        /// The affected object.
        /// </summary>
        public string? Object { get; set; }

        /// <summary>
        /// Filters tickets by a given property. Supports strings, integers, DateTime, and custom class properties.
        /// String|DateTime|Int: Contains
        /// Guid|Boolean: Equals
        /// </summary>
        /// <param name="searchTerm">The string to be compared</param>
        /// <param name="valueProperty">The property to be compared to the string</param>
        /// <param name="classPropertyName" default="Id">The property to be used if the property to be compared is a class</param>
        /// <returns>Expression</returns>
        public static Expression<Func<Ticket, bool>> GetSearchPredicate(string searchTerm, string valueProperty, string classPropertyName = "Id")
        {
            var parameter = Expression.Parameter(typeof(Ticket), "t");
            var property = Expression.Property(parameter, valueProperty);
            var propertyType = property.Type;
            var searchTermExpression = Expression.Constant(searchTerm.ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(string) });
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var toStringMethod = typeof(int).GetMethod("ToString", Type.EmptyTypes);
            var boolToStringMethod = typeof(bool).GetMethod("ToString", Type.EmptyTypes);
            var dateTimeToStringMethod = typeof(DateTime).GetMethod("ToString", Type.EmptyTypes);
            var guidToString = typeof(Guid).GetMethod("ToString", Type.EmptyTypes);
            Expression expression = Expression.Empty();
            if (toLowerMethod != null && containsMethod != null && equalsMethod != null && toStringMethod != null &&
                guidToString != null && dateTimeToStringMethod != null && boolToStringMethod != null)
            {
                if (propertyType == typeof(string))
                {
                    expression = Expression.Call(Expression.Call(property, toLowerMethod), containsMethod, searchTermExpression);
                }

                if (propertyType == typeof(int))
                {
                    expression = Expression.Call(Expression.Call(Expression.Call(property, toStringMethod), toLowerMethod), containsMethod, searchTermExpression);
                }

                if (propertyType == typeof(DateTime))
                {
                    expression = Expression.Call(Expression.Call(Expression.Call(property, dateTimeToStringMethod), toLowerMethod), containsMethod, searchTermExpression);
                }

                if (propertyType == typeof(Guid))
                {
                    expression = Expression.Call(null, equalsMethod, Expression.Call(Expression.Call(property, guidToString), toLowerMethod), searchTermExpression);
                }

                if (propertyType == typeof(bool))
                {
                    expression = Expression.Call(null, equalsMethod, Expression.Call(Expression.Call(property, boolToStringMethod), toLowerMethod), searchTermExpression);
                }

                if (propertyType.IsClass && propertyType != typeof(string) && propertyType != typeof(int) &&
                    propertyType != typeof(bool) && propertyType != typeof(DateTime))
                {
                    Console.WriteLine("Is Class");
                    var stringPropertyExpression = Expression.Property(property, classPropertyName);
                    if (stringPropertyExpression.Type == typeof(Guid))
                    {
                        Console.WriteLine("Is Guid");
                        expression = Expression.Call(null, equalsMethod, Expression.Call(stringPropertyExpression, guidToString), searchTermExpression);
                    }

                    if (stringPropertyExpression.Type == typeof(string))
                    {
                        Console.WriteLine("Is string");
                        expression = Expression.Call(Expression.Call(stringPropertyExpression, toLowerMethod), containsMethod, searchTermExpression);
                    }

                    if (stringPropertyExpression.Type == typeof(int))
                    {
                        Console.WriteLine("Is int");
                        expression = Expression.Call(Expression.Call(stringPropertyExpression, toStringMethod), containsMethod, searchTermExpression);
                    }

                    if (stringPropertyExpression.Type == typeof(bool))
                    {
                        Console.WriteLine("Is bool");
                        expression = Expression.Call(null, equalsMethod, Expression.Call(Expression.Call(stringPropertyExpression, boolToStringMethod), toLowerMethod), searchTermExpression);
                    }

                    if (stringPropertyExpression.Type == typeof(DateTime))
                    {
                        Console.WriteLine("Is DateTime");
                        expression = Expression.Call(Expression.Call(Expression.Call(stringPropertyExpression, dateTimeToStringMethod), toLowerMethod), containsMethod, searchTermExpression);
                    }
                }
            }

            return Expression.Lambda<Func<Ticket, bool>>(expression, parameter);
        }
    }
}