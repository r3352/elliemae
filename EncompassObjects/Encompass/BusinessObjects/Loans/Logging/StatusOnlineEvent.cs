// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Globalization;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single published event to the Status Online web site.
  /// </summary>
  /// <remarks>A published event could be the achievement of a milestone,
  /// the receipt of a supporting document or any other condition avaialble
  /// in the Status Online configuration within Encompass.</remarks>
  /// <example>
  /// The following code displays all of events that have been published to the
  /// Status Online web site for a particular loan, along with the date on
  /// which is was published and the user who published it.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open a loan and lock it for writing
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///       // Iterate over all of the Status Online updates associated with the loan
  ///       foreach (StatusOnlineUpdate update in loan.Log.StatusOnlineUpdates)
  ///       {
  ///          // Each Status Online update can contains multiple published events,
  ///          // e.g. the achievement of a milestone and the receipt of a document
  ///          foreach (StatusOnlineEvent e in update.PublishedEvents)
  ///             Console.WriteLine(e.Description + " published on " + update.Date
  ///                + " by " + update.Creator);
  ///       }
  /// 
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class StatusOnlineEvent : IStatusOnlineEvent
  {
    private string description;
    private DateTime date;

    internal StatusOnlineEvent(string description, string dateStr)
    {
      this.description = description;
      try
      {
        this.date = DateTime.ParseExact(dateStr, new string[2]
        {
          "MM/dd/yyyy",
          "M/d/yy"
        }, (IFormatProvider) null, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
      }
      catch
      {
        this.date = DateTime.MinValue;
      }
    }

    /// <summary>
    /// Gets the description of the item that was published to the Status Online web site.
    /// </summary>
    public string Description => this.description;

    /// <summary>Gets the date of the event that was published.</summary>
    /// <remarks>This date represents the date on which the action described by this
    /// item occurred, not the date on which it was published. For example, if the
    /// item represents the achievement of a milestone, the Date property will be the
    /// date on which that milestone was achieved, not the date on which the Status
    /// Online website was updated.</remarks>
    public DateTime Date => this.date;
  }
}
