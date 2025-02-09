// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchUpdate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides a class for encapsulating a batch update to a set of loans.
  /// </summary>
  /// <example>
  ///       The code below demonstrates how to update the broker name and address
  ///       fields on a batch of loans in a single call to the Encompass Server.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.Query;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "admin", "password");
  /// 
  ///       // Generate the list of loan GUIDs that will be updated
  ///       StringList guids = new StringList();
  ///       guids.Add("{55fbe34f-055f-48d0-ade5-2ec5ccfc555a}");
  ///       guids.Add("{78b61507-c4da-4051-9283-a9e6650318eb}");
  ///       guids.Add("{2c680754-816d-4826-a161-bb1b8f2fc51b}");
  /// 
  ///       // We will update the broker company information on the 1003 form
  ///       BatchUpdate batch = new BatchUpdate(guids);
  ///       batch.Fields.Add("315", "Encompass Loan Specialists, Inc.");
  ///       batch.Fields.Add("319", "123 Main Street");
  ///       batch.Fields.Add("313", "Anywhereville");
  ///       batch.Fields.Add("321", "MO");
  ///       batch.Fields.Add("323", "24432");
  /// 
  ///       // Submit the batch to the server
  ///       session.Loans.SubmitBatchUpdate(batch);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class BatchUpdate
  {
    private LoanBatch batch;
    private BatchUpdateFields fields;

    /// <summary>Constructs a new BatchUpdate for a single loan.</summary>
    /// <param name="loanGuid">The GUID of the loan to be updated.</param>
    public BatchUpdate(string loanGuid)
      : this(new StringList((IList) new string[1]
      {
        loanGuid
      }))
    {
    }

    /// <summary>
    /// Constructs a new BatchUpdate from a list of loan GUIDs.
    /// </summary>
    /// <param name="loanGuids">A list of the loan GUIDs to be updated.</param>
    /// <example>
    ///       The code below demonstrates how to update the broker name and address
    ///       fields on a batch of loans in a single call to the Encompass Server.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "password");
    /// 
    ///       // Generate the list of loan GUIDs that will be updated
    ///       StringList guids = new StringList();
    ///       guids.Add("{55fbe34f-055f-48d0-ade5-2ec5ccfc555a}");
    ///       guids.Add("{78b61507-c4da-4051-9283-a9e6650318eb}");
    ///       guids.Add("{2c680754-816d-4826-a161-bb1b8f2fc51b}");
    /// 
    ///       // We will update the broker company information on the 1003 form
    ///       BatchUpdate batch = new BatchUpdate(guids);
    ///       batch.Fields.Add("315", "Encompass Loan Specialists, Inc.");
    ///       batch.Fields.Add("319", "123 Main Street");
    ///       batch.Fields.Add("313", "Anywhereville");
    ///       batch.Fields.Add("321", "MO");
    ///       batch.Fields.Add("323", "24432");
    /// 
    ///       // Submit the batch to the server
    ///       session.Loans.SubmitBatchUpdate(batch);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public BatchUpdate(StringList loanGuids)
    {
      LoanSetBatch loanSetBatch = new LoanSetBatch();
      loanSetBatch.LoanGuids.AddRange((IEnumerable<string>) loanGuids.ToArray());
      this.batch = (LoanBatch) loanSetBatch;
      this.fields = new BatchUpdateFields(this);
    }

    /// <summary>
    /// Constructs a new BatchUpdate using the specified selection criteria for the loans.
    /// </summary>
    /// <param name="selectionCriteria">The criteria which specifies which loans should be updated.
    /// A <c>null</c> value will cause all loans to be updated.</param>
    /// <example>
    ///       The following code locates all loans where credit was provided by a specific
    ///       provider and updates the address info for the provider.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Search for any loan where the credit was provided by "ABC Credit"
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Loan.CreditVendor";
    ///       cri.Value = "ABC Credit";
    ///       cri.MatchType = StringFieldMatchType.Exact;
    /// 
    ///       // We will update the credit company's contact info
    ///       BatchUpdate batch = new BatchUpdate(cri);
    ///       batch.Fields.Add("626", "376 Garden Parkway");
    ///       batch.Fields.Add("627", "Garden City");
    ///       batch.Fields.Add("1245", "NJ");
    ///       batch.Fields.Add("628", "03223");
    /// 
    ///       // Submit the batch to the server
    ///       session.Loans.SubmitBatchUpdate(batch);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public BatchUpdate(QueryCriterion selectionCriteria)
    {
      this.batch = (LoanBatch) new LoanQueryBatch(selectionCriteria.Unwrap());
      this.fields = new BatchUpdateFields(this);
    }

    /// <summary>
    /// Gets the collection of field values to be updated as part of the batch.
    /// </summary>
    /// <example>
    ///       The following code locates all loans where credit was provided by a specific
    ///       provider and updates the address info for the provider.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Search for any loan where the credit was provided by "ABC Credit"
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Loan.CreditVendor";
    ///       cri.Value = "ABC Credit";
    ///       cri.MatchType = StringFieldMatchType.Exact;
    /// 
    ///       // We will update the credit company's contact info
    ///       BatchUpdate batch = new BatchUpdate(cri);
    ///       batch.Fields.Add("626", "376 Garden Parkway");
    ///       batch.Fields.Add("627", "Garden City");
    ///       batch.Fields.Add("1245", "NJ");
    ///       batch.Fields.Add("628", "03223");
    /// 
    ///       // Submit the batch to the server
    ///       session.Loans.SubmitBatchUpdate(batch);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public BatchUpdateFields Fields => this.fields;

    internal LoanBatch Unwrap() => this.batch;
  }
}
