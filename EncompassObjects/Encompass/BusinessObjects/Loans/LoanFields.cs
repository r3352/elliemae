// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents the collection of all fields on a Loan.</summary>
  /// <example>
  /// The following code demonstrates how to use the LoanFields object to access
  /// various fields on a Loan.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class LoanManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open an existing loan using the GUID from the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  /// 
  ///       // Print our the property address for the loan. Since there's only a
  ///       // single value for each field, just use the field ID directly with
  ///       // the LoanFields indexer property.
  ///       Console.WriteLine("Street: " + loan.Fields["11"].Value);
  ///       Console.WriteLine("City:   " + loan.Fields["12"].Value);
  ///       Console.WriteLine("County: " + loan.Fields["13"].Value);
  ///       Console.WriteLine("State:  " + loan.Fields["14"].Value);
  /// 
  ///       // Now print out the names of the "primary" Borrower pair.
  ///       // Since this pair is primary, we can use the field IDs directly
  ///       // with the indexer property.
  ///       Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].Value);
  ///       Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].Value);
  ///       Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].Value);
  ///       Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].Value);
  /// 
  ///       // Now output the name of all of the other borrower pairs.
  ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
  ///       {
  ///          BorrowerPair pair = loan.BorrowerPairs[i];
  /// 
  ///          if (pair != loan.BorrowerPairs.Current)
  ///          {
  ///             // Because these pairs are not the Current (i.e. primary) pair, we
  ///             // must use the GetValueForBorrowerPair() function to get the field
  ///             // value for the specified pair.
  ///             Console.WriteLine("Borrower Pair " + i + ":");
  ///             Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].GetValueForBorrowerPair(pair));
  ///             Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].GetValueForBorrowerPair(pair));
  ///             Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].GetValueForBorrowerPair(pair));
  ///             Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].GetValueForBorrowerPair(pair));
  ///          }
  ///       }
  /// 
  ///       // Dump the liability verification records for the loan
  ///       for (int i = 1; i <= loan.Liabilities.Count; i++)
  ///       {
  ///          // Use the LoanField object's GetFieldAt() method to get the field value for
  ///          // a specified liablity.
  ///          Console.WriteLine("Liability " + i + ":");
  ///          Console.WriteLine("Holder:   " + loan.Fields.GetFieldAt("FL02", i).Value);
  ///          Console.WriteLine("Account:  " + loan.Fields.GetFieldAt("FL10", i).Value);
  /// 
  ///          // Alternatively, you can construct the entire loan field ID manually
  ///          // for the specific subitem.
  ///          Console.WriteLine("Balance:  " + loan.Fields["FL" + i.ToString("00") + "13"].Value);
  ///       }
  /// 
  ///       // Close the loan to release its resources
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class LoanFields : ILoanFields
  {
    private Loan loan;
    private Hashtable fields = new Hashtable();

    internal LoanFields(Loan loan) => this.loan = loan;

    /// <summary>
    /// Gets the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanField">LoanField</see> based on the
    /// field ID provided.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to use the LoanFields object to access
    /// various fields on a Loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Print our the property address for the loan. Since there's only a
    ///       // single value for each field, just use the field ID directly with
    ///       // the LoanFields indexer property.
    ///       Console.WriteLine("Street: " + loan.Fields["11"].Value);
    ///       Console.WriteLine("City:   " + loan.Fields["12"].Value);
    ///       Console.WriteLine("County: " + loan.Fields["13"].Value);
    ///       Console.WriteLine("State:  " + loan.Fields["14"].Value);
    /// 
    ///       // Now print out the names of the "primary" Borrower pair.
    ///       // Since this pair is primary, we can use the field IDs directly
    ///       // with the indexer property.
    ///       Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].Value);
    ///       Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].Value);
    ///       Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].Value);
    ///       Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].Value);
    /// 
    ///       // Now output the name of all of the other borrower pairs.
    ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
    ///       {
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          if (pair != loan.BorrowerPairs.Current)
    ///          {
    ///             // Because these pairs are not the Current (i.e. primary) pair, we
    ///             // must use the GetValueForBorrowerPair() function to get the field
    ///             // value for the specified pair.
    ///             Console.WriteLine("Borrower Pair " + i + ":");
    ///             Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].GetValueForBorrowerPair(pair));
    ///          }
    ///       }
    /// 
    ///       // Dump the liability verification records for the loan
    ///       for (int i = 1; i <= loan.Liabilities.Count; i++)
    ///       {
    ///          // Use the LoanField object's GetFieldAt() method to get the field value for
    ///          // a specified liablity.
    ///          Console.WriteLine("Liability " + i + ":");
    ///          Console.WriteLine("Holder:   " + loan.Fields.GetFieldAt("FL02", i).Value);
    ///          Console.WriteLine("Account:  " + loan.Fields.GetFieldAt("FL10", i).Value);
    /// 
    ///          // Alternatively, you can construct the entire loan field ID manually
    ///          // for the specific subitem.
    ///          Console.WriteLine("Balance:  " + loan.Fields["FL" + i.ToString("00") + "13"].Value);
    ///       }
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanField this[string fieldId]
    {
      get
      {
        lock (this.loan)
        {
          LoanField field = (LoanField) this.fields[(object) (fieldId ?? "")];
          if (field != null)
            return field;
          FieldDefinition fieldDefinition = this.loan.Unwrap().LoanData.GetFieldDefinition(fieldId);
          LoanField loanField = fieldDefinition != null && fieldDefinition != FieldDefinition.Empty ? new LoanField(this.loan, new FieldDescriptor(fieldDefinition)) : throw new ArgumentException("The Field ID '" + fieldId + "' is invalid.", nameof (fieldId));
          this.fields.Add((object) fieldId, (object) loanField);
          return loanField;
        }
      }
    }

    /// <summary>
    /// Returns a parameterized <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanField">LoanField</see> for the
    /// subitem with the specified index.
    /// </summary>
    /// <param name="fieldId">The base Field ID of the desired field.</param>
    /// <param name="itemIndex">The index of the subitem.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanField">LoanField</see> for the request field.</returns>
    /// <remarks>This method is used to retrieve field data for loan fields which
    /// have multiple instances, e.g. the name of the depositor for the various
    /// desposits recorded in the loan.</remarks>
    /// <example>
    /// The following code demonstrates how to use the LoanFields object to access
    /// various fields on a Loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Print our the property address for the loan. Since there's only a
    ///       // single value for each field, just use the field ID directly with
    ///       // the LoanFields indexer property.
    ///       Console.WriteLine("Street: " + loan.Fields["11"].Value);
    ///       Console.WriteLine("City:   " + loan.Fields["12"].Value);
    ///       Console.WriteLine("County: " + loan.Fields["13"].Value);
    ///       Console.WriteLine("State:  " + loan.Fields["14"].Value);
    /// 
    ///       // Now print out the names of the "primary" Borrower pair.
    ///       // Since this pair is primary, we can use the field IDs directly
    ///       // with the indexer property.
    ///       Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].Value);
    ///       Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].Value);
    ///       Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].Value);
    ///       Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].Value);
    /// 
    ///       // Now output the name of all of the other borrower pairs.
    ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
    ///       {
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          if (pair != loan.BorrowerPairs.Current)
    ///          {
    ///             // Because these pairs are not the Current (i.e. primary) pair, we
    ///             // must use the GetValueForBorrowerPair() function to get the field
    ///             // value for the specified pair.
    ///             Console.WriteLine("Borrower Pair " + i + ":");
    ///             Console.WriteLine("Borrower First Name:   " + loan.Fields["36"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("Borrower Last Name:    " + loan.Fields["37"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("CoBorrower First Name: " + loan.Fields["68"].GetValueForBorrowerPair(pair));
    ///             Console.WriteLine("CoBorrower Last Name:  " + loan.Fields["69"].GetValueForBorrowerPair(pair));
    ///          }
    ///       }
    /// 
    ///       // Dump the liability verification records for the loan
    ///       for (int i = 1; i <= loan.Liabilities.Count; i++)
    ///       {
    ///          // Use the LoanField object's GetFieldAt() method to get the field value for
    ///          // a specified liablity.
    ///          Console.WriteLine("Liability " + i + ":");
    ///          Console.WriteLine("Holder:   " + loan.Fields.GetFieldAt("FL02", i).Value);
    ///          Console.WriteLine("Account:  " + loan.Fields.GetFieldAt("FL10", i).Value);
    /// 
    ///          // Alternatively, you can construct the entire loan field ID manually
    ///          // for the specific subitem.
    ///          Console.WriteLine("Balance:  " + loan.Fields["FL" + i.ToString("00") + "13"].Value);
    ///       }
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanField GetFieldAt(string fieldId, int itemIndex)
    {
      return this[this.generateFieldID(fieldId ?? "", itemIndex)];
    }

    private string generateFieldID(string baseId, int itemIndex)
    {
      for (int index = 0; index < baseId.Length; ++index)
      {
        if (!char.IsLetter(baseId[index]))
          return baseId.Substring(0, index) + itemIndex.ToString("00") + baseId.Substring(index);
      }
      return baseId;
    }
  }
}
