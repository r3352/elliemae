// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.QueryCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Provides a base class for all query criterion classes.
  /// </summary>
  public abstract class QueryCriterion : IQueryCriterion
  {
    internal QueryCriterion()
    {
    }

    /// <summary>
    /// Combines the current query criterion with the one provided using boolean AND
    /// logic to produce a composite criterion.
    /// </summary>
    /// <param name="criterion">The query criterion that will act as the right hand
    /// side of the boolean operation.</param>
    /// <returns>A new query criterion object representing the logical AND of
    /// the current and specified criteria.</returns>
    /// <example>
    /// The following example demonstrates how to combine criterion objects using
    /// the And() and Or() methods into a complex query.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // We first want to look for Business Partners who are appraisers
    ///       NumericFieldCriterion catCri = new NumericFieldCriterion();
    ///       catCri.FieldName = "Contact.CategoryID";
    ///       catCri.Value = session.Contacts.BizCategories.GetItemByName("Appraiser").ID;
    /// 
    ///       // Now we want the Fees to be either greater than $350 or less than $250
    ///       NumericFieldCriterion feeCriHigh = new NumericFieldCriterion();
    ///       feeCriHigh.FieldName = "Contact.Fees";
    ///       feeCriHigh.Value = 350;
    ///       feeCriHigh.MatchType = OrdinalFieldMatchType.GreaterThan;
    /// 
    ///       NumericFieldCriterion feeCriLow = new NumericFieldCriterion();
    ///       feeCriLow.FieldName = "Contact.Fees";
    ///       feeCriLow.Value = 250;
    ///       feeCriLow.MatchType = OrdinalFieldMatchType.LessThan;
    /// 
    ///       // Join the fee criteria with OR logic
    ///       QueryCriterion feeCri = feeCriHigh.Or(feeCriLow);
    /// 
    ///       // Next, join the the fee criteria with the category criteria
    ///       QueryCriterion allCri = catCri.And(feeCri);
    /// 
    ///       // Query for the matching contacts and then print their names
    ///       ContactList contacts = session.Contacts.Query(allCri, ContactLoanMatchType.None, ContactType.Biz);
    /// 
    ///       // Print the names of the matching contacts
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          Console.WriteLine(contacts[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public QueryCriterion And(QueryCriterion criterion)
    {
      return (QueryCriterion) new BooleanLogicCriterion(BinaryOperator.And, this, criterion);
    }

    /// <summary>
    /// Combines the current query criterion with the one provided using boolean OR
    /// logic to produce a composite criterion.
    /// </summary>
    /// <param name="criterion">The query criterion that will act as the right hand
    /// side of the boolean operation.</param>
    /// <returns>A new query criterion object representing the logical OR of
    /// the current and specified criteria.</returns>
    /// <example>
    /// The following example demonstrates how to combine criterion objects using
    /// the And() and Or() methods into a complex query.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // We first want to look for Business Partners who are appraisers
    ///       NumericFieldCriterion catCri = new NumericFieldCriterion();
    ///       catCri.FieldName = "Contact.CategoryID";
    ///       catCri.Value = session.Contacts.BizCategories.GetItemByName("Appraiser").ID;
    /// 
    ///       // Now we want the Fees to be either greater than $350 or less than $250
    ///       NumericFieldCriterion feeCriHigh = new NumericFieldCriterion();
    ///       feeCriHigh.FieldName = "Contact.Fees";
    ///       feeCriHigh.Value = 350;
    ///       feeCriHigh.MatchType = OrdinalFieldMatchType.GreaterThan;
    /// 
    ///       NumericFieldCriterion feeCriLow = new NumericFieldCriterion();
    ///       feeCriLow.FieldName = "Contact.Fees";
    ///       feeCriLow.Value = 250;
    ///       feeCriLow.MatchType = OrdinalFieldMatchType.LessThan;
    /// 
    ///       // Join the fee criteria with OR logic
    ///       QueryCriterion feeCri = feeCriHigh.Or(feeCriLow);
    /// 
    ///       // Next, join the the fee criteria with the category criteria
    ///       QueryCriterion allCri = catCri.And(feeCri);
    /// 
    ///       // Query for the matching contacts and then print their names
    ///       ContactList contacts = session.Contacts.Query(allCri, ContactLoanMatchType.None, ContactType.Biz);
    /// 
    ///       // Print the names of the matching contacts
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          Console.WriteLine(contacts[i].ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public QueryCriterion Or(QueryCriterion criterion)
    {
      return (QueryCriterion) new BooleanLogicCriterion(BinaryOperator.Or, this, criterion);
    }

    /// <summary>
    /// Creates an exact duplicate of the object by performing a deep copy.
    /// </summary>
    /// <returns>Returns a deep copy of the current criterion object.</returns>
    public abstract QueryCriterion Clone();

    /// <summary>
    /// This method is meant only to be used by the Encompass application.
    /// </summary>
    /// <returns></returns>
    public abstract EllieMae.EMLite.ClientServer.Query.QueryCriterion Unwrap();
  }
}
