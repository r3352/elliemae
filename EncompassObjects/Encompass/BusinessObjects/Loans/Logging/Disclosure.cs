// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single Disclosure Tracking record associated with a Loan.
  /// </summary>
  /// <remarks>The inherited Date property of a Disclosure represents the
  /// date on which the disclosure was made.
  /// <p>Disclosure instances become invalid
  /// when the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Refresh">Refresh</see> method is
  /// invoked on the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object. Attempting
  /// to access this object after invoking Refresh() will result in an
  /// exception.</p>
  /// </remarks>
  /// <example>
  ///     The following code access disclosures in a loan.
  ///     <code>
  ///       <![CDATA[
  /// using System;
  /// using EllieMae.Encompass.Client;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server. We will need to be logged
  ///       // in as an Administrator to modify the user accounts.
  ///       Session session = new Session();
  ///       session.Start("EMServer", "myuser", "myuserpwd");
  /// 
  ///       //Open a Loan by providing loan guid
  ///       var loan = session.Loans.Open("{b8bee82b-d1cd-48ce-b97f-365a9e60defb}");
  /// 
  ///       if (loan.Log.Disclosures != null && loan.Log.Disclosures.Count > 0)
  /// 
  ///       {
  ///           Console.WriteLine(@"ReceivedDate: " + loan.Log.Disclosures[0].ReceivedDate);
  ///           Console.WriteLine(@"Date: " + loan.Log.Disclosures[0].Date);
  ///           Console.WriteLine(@"DateAdded: " + loan.Log.Disclosures[0].DateAdded);
  ///           Console.WriteLine(@"DisclosedBy: " + loan.Log.Disclosures[0].DisclosedBy);
  ///       }
  /// 
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///     </code>
  ///   </example>
  public class Disclosure : DisclosureBase, IDisclosure
  {
    internal Disclosure(Loan loan, DisclosureTrackingLog discItem)
      : base(loan, (DisclosureTrackingBase) discItem)
    {
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.Disclosure" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.Disclosure;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosureType" /> for the disclosure.
    /// </summary>
    public StandardDisclosureType DisclosureType
    {
      get
      {
        this.EnsureValid();
        StandardDisclosureType disclosureType = StandardDisclosureType.None;
        if (this.DiscItem.DisclosedForGFE)
          disclosureType |= StandardDisclosureType.GFE;
        if (this.DiscItem.DisclosedForTIL)
          disclosureType |= StandardDisclosureType.TIL;
        if (this.DiscItem.DisclosedForSafeHarbor)
          disclosureType |= StandardDisclosureType.SAFEHARBOR;
        return disclosureType;
      }
    }

    /// <summary>
    /// Gets or sets the flag indicating disclosure is invalid.
    /// </summary>
    /// <remarks>An invalid disclosure is one that was never actually made and will not be considered for
    /// the purposes of determining the compliance timeline of the loan.</remarks>
    public bool EnabledForCompliance
    {
      get
      {
        this.EnsureValid();
        return this.DiscItem.IsDisclosed;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.IsDisclosed = value;
      }
    }

    /// <summary>Gets or Sets the Delivery Method</summary>
    public DeliveryMethod DeliveryMethod
    {
      get
      {
        this.EnsureValid();
        return (DeliveryMethod) this.DiscItem.DisclosureMethod;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.DisclosureMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    /// <summary>
    /// Gets or sets the date the disclosure was received by the borrower.
    /// </summary>
    /// <remarks>If the disclosure has not been received, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a disclosure as not having been received, set this
    /// property to null.
    /// When the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure.DeliveryMethod" /> for the disclosure is set to Mail, Fax, or InPerson,
    /// the ReceivedDate will be set automatically based on compliance regulations. For Mail deliveries,
    /// the ReceivedDate is always three postal days from the date sent. For Fax and InPerson, the
    /// date received will match the disclosure date. Attempting to set this property when the DeliveryMethod
    /// is any of those values will result in an exception.
    /// </remarks>
    public override object ReceivedDate
    {
      get
      {
        this.EnsureValid();
        return !this.discItem.Received ? (object) null : (object) this.discItem.ReceivedDate;
      }
      set
      {
        this.EnsureEditable();
        if (this.DeliveryMethod == DeliveryMethod.Fax || this.DeliveryMethod == DeliveryMethod.Mail || this.DeliveryMethod == DeliveryMethod.InPerson)
          throw new Exception("The received date cannot be changed and is set by regulation for the specified delivery method.");
        if (value == null)
        {
          this.discItem.ReceivedDate = DateTime.MinValue;
        }
        else
        {
          DateTime dateTime = value is DateTime ? Convert.ToDateTime(value) : throw new ArgumentException("The specified value must be a DateTime.");
          if (dateTime.Date < this.discItem.Date.Date)
            throw new ArgumentException("The specified date is prior to the disclosure date.");
          this.discItem.ReceivedDate = dateTime.Date;
        }
      }
    }

    internal override bool IseDiclosure => this.DeliveryMethod == DeliveryMethod.eDisclosure;

    private DisclosureTrackingLog DiscItem => (DisclosureTrackingLog) this.discItem;
  }
}
