export default interface CreateAdvancedTicketSearchRequest {
  /**
   * The string to be used to filter the tickets. Can be empty. All tickets will be returned in the specified order.
  */
  searchTerm: string;
  /**
   * The ticket property to be searched with the searchTerm. Default: Title 
   * Can be of types String|Int|Boolean|DateTime|Guid or another class.
  */
  property: string | null;
  /**
   * Has to be specified if property is a class. This specifies the property to be searched with the searchTerm in this class.
   * Note: further nesting is not possible, so the type of this property has to be String|Int|Boolean|DateTime|Guid, not a class.   
  */
  classPropertyName: string | null;
  /**
   * Which property of the ticket the results should be ordered by.
  */
  orderByColumn: string,
  /**
   * Which direction the results should be ordered in.
   * True: descending - False: ascending
  */
  orderDesc: boolean,
  /**
   * Pagination: the amount of entries to be skipped
  */
  skip: number,
  /**
   * Pagination: The amount of entries to be returned after the skipped entries
  */
  take: number
}



