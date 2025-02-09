// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents a single field of the loan file.</summary>
  /// <example>
  /// The following code demonstrates how the properties of a LoanField are
  /// affected by locking and calculations.
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
  ///       // Set the base loan amount and appraised value of the home
  ///       loan.Fields["1109"].Value = "250000";    // Loan amount, excluding fees
  ///       loan.Fields["136"].Value = "370000";     // Purchase price
  ///       loan.Fields["356"].Value = "375000";     // Appraised amount
  ///       loan.Recalculate();
  /// 
  ///       // Get the LTV field for the loan (field ID 353)
  ///       LoanField ltv = loan.Fields["353"];
  /// 
  ///       // Print out the properties of the field
  ///       dumpLoanField(ltv);
  /// 
  ///       // Generally, the LTV field would be unlocked, so we'll lock it will no
  ///       // longer be affected by calculations.
  ///       ltv.Locked = true;
  ///       ltv.Value = "0.75";
  ///       dumpLoanField(ltv);
  /// 
  ///       // Reclaculate and dump it again. The Value will not have change but the
  ///       // OriginalValue field will reflect the correctly calculated LTV.
  ///       loan.Recalculate();
  ///       dumpLoanField(ltv);
  /// 
  ///       // Unlock the field and recalculate. The calculated LTV will now overwrite the
  ///       // value we explicitly set above.
  ///       ltv.Locked = false;
  ///       loan.Recalculate();
  ///       dumpLoanField(ltv);
  /// 
  ///       // Close the loan to release its resources
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// 
  ///    // Dumps the properties of a loan field
  ///    private static void dumpLoanField(LoanField field)
  ///    {
  ///       Console.WriteLine("Formatted Value:   " + field.Value);
  ///       Console.WriteLine("Unformatted Value: " + field.UnformattedValue);
  ///       Console.WriteLine("Original Value:    " + field.OriginalValue);
  ///       Console.WriteLine("Format:            " + field.Format);
  ///       Console.WriteLine("Locked:            " + field.Locked);
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class LoanField : Field, ILoanField
  {
    private Loan loan;

    internal LoanField(Loan loan, FieldDescriptor descriptor)
      : base(descriptor)
    {
      this.loan = loan;
    }

    /// <summary>
    /// Gets the raw, unformatted value of the underlying field.
    /// </summary>
    /// <example>
    /// The following code demonstrates how the properties of a LoanField are
    /// affected by locking and calculations.
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
    ///       // Set the base loan amount and appraised value of the home
    ///       loan.Fields["1109"].Value = "250000";    // Loan amount, excluding fees
    ///       loan.Fields["136"].Value = "370000";     // Purchase price
    ///       loan.Fields["356"].Value = "375000";     // Appraised amount
    ///       loan.Recalculate();
    /// 
    ///       // Get the LTV field for the loan (field ID 353)
    ///       LoanField ltv = loan.Fields["353"];
    /// 
    ///       // Print out the properties of the field
    ///       dumpLoanField(ltv);
    /// 
    ///       // Generally, the LTV field would be unlocked, so we'll lock it will no
    ///       // longer be affected by calculations.
    ///       ltv.Locked = true;
    ///       ltv.Value = "0.75";
    ///       dumpLoanField(ltv);
    /// 
    ///       // Reclaculate and dump it again. The Value will not have change but the
    ///       // OriginalValue field will reflect the correctly calculated LTV.
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Unlock the field and recalculate. The calculated LTV will now overwrite the
    ///       // value we explicitly set above.
    ///       ltv.Locked = false;
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Dumps the properties of a loan field
    ///    private static void dumpLoanField(LoanField field)
    ///    {
    ///       Console.WriteLine("Formatted Value:   " + field.Value);
    ///       Console.WriteLine("Unformatted Value: " + field.UnformattedValue);
    ///       Console.WriteLine("Original Value:    " + field.OriginalValue);
    ///       Console.WriteLine("Format:            " + field.Format);
    ///       Console.WriteLine("Locked:            " + field.Locked);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public override string UnformattedValue
    {
      get => this.Descriptor.UnformatValue(this.loan.LoanData.GetSimpleField(this.ID));
    }

    /// <summary>Sets the value if a field in the loan.</summary>
    /// <param name="value">The new value for the field.</param>
    internal override void setFieldValue(string value)
    {
      if (this.Descriptor.RequiresExclusiveLock)
        this.loan.EnsureExclusive();
      if (this.loan.LoanAccessExceptionsEnabled && this.ReadOnly)
        throw new InvalidOperationException("Edit access to the field '" + this.Descriptor.FieldID + "' is denied");
      if (this.loan.CalculationsEnabled)
        this.loan.LoanData.SetField(this.ID, value);
      else
        this.loan.LoanData.SetCurrentField(this.ID, value);
    }

    /// <summary>
    /// Gets or sets the value of the field thru the ILoanField interface.
    /// </summary>
    /// <remarks>This method exists primarilly for COM-based clients which cannot marshal values
    /// properly to the object-valued Value property.</remarks>
    string ILoanField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    /// <summary>
    /// Gets the calculated value of the field when a field is locked.
    /// </summary>
    /// <remarks>When a field is locked, calculations will no longer affect
    /// the Value of the field. Instead, the calculated value will be placed in
    /// the OriginalValue property of the field.</remarks>
    /// <example>
    /// The following code demonstrates how the properties of a LoanField are
    /// affected by locking and calculations.
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
    ///       // Set the base loan amount and appraised value of the home
    ///       loan.Fields["1109"].Value = "250000";    // Loan amount, excluding fees
    ///       loan.Fields["136"].Value = "370000";     // Purchase price
    ///       loan.Fields["356"].Value = "375000";     // Appraised amount
    ///       loan.Recalculate();
    /// 
    ///       // Get the LTV field for the loan (field ID 353)
    ///       LoanField ltv = loan.Fields["353"];
    /// 
    ///       // Print out the properties of the field
    ///       dumpLoanField(ltv);
    /// 
    ///       // Generally, the LTV field would be unlocked, so we'll lock it will no
    ///       // longer be affected by calculations.
    ///       ltv.Locked = true;
    ///       ltv.Value = "0.75";
    ///       dumpLoanField(ltv);
    /// 
    ///       // Reclaculate and dump it again. The Value will not have change but the
    ///       // OriginalValue field will reflect the correctly calculated LTV.
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Unlock the field and recalculate. The calculated LTV will now overwrite the
    ///       // value we explicitly set above.
    ///       ltv.Locked = false;
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Dumps the properties of a loan field
    ///    private static void dumpLoanField(LoanField field)
    ///    {
    ///       Console.WriteLine("Formatted Value:   " + field.Value);
    ///       Console.WriteLine("Unformatted Value: " + field.UnformattedValue);
    ///       Console.WriteLine("Original Value:    " + field.OriginalValue);
    ///       Console.WriteLine("Format:            " + field.Format);
    ///       Console.WriteLine("Locked:            " + field.Locked);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string OriginalValue => this.loan.LoanData.GetOrgField(this.ID);

    /// <summary>
    /// Gets or sets a flag indicating if the field is locked.
    /// </summary>
    /// <remarks><p>Locking a field provides a way of overriding the value
    /// obtained through automatic calculation. If this property is set to <c>true</c>,
    /// the field will never be overwritten by the Encompass Calculation Engine.
    /// Setting this property to <c>false</c> allows the field's value to be overwritten
    /// as needed to ensure consistency with its dependent fields.</p>
    /// <p>Locking or unlocking a field requires that you have an exclusive lock on the loan.</p>
    /// </remarks>
    /// <example>
    /// The following code demonstrates how the properties of a LoanField are
    /// affected by locking and calculations.
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
    ///       // Set the base loan amount and appraised value of the home
    ///       loan.Fields["1109"].Value = "250000";    // Loan amount, excluding fees
    ///       loan.Fields["136"].Value = "370000";     // Purchase price
    ///       loan.Fields["356"].Value = "375000";     // Appraised amount
    ///       loan.Recalculate();
    /// 
    ///       // Get the LTV field for the loan (field ID 353)
    ///       LoanField ltv = loan.Fields["353"];
    /// 
    ///       // Print out the properties of the field
    ///       dumpLoanField(ltv);
    /// 
    ///       // Generally, the LTV field would be unlocked, so we'll lock it will no
    ///       // longer be affected by calculations.
    ///       ltv.Locked = true;
    ///       ltv.Value = "0.75";
    ///       dumpLoanField(ltv);
    /// 
    ///       // Reclaculate and dump it again. The Value will not have change but the
    ///       // OriginalValue field will reflect the correctly calculated LTV.
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Unlock the field and recalculate. The calculated LTV will now overwrite the
    ///       // value we explicitly set above.
    ///       ltv.Locked = false;
    ///       loan.Recalculate();
    ///       dumpLoanField(ltv);
    /// 
    ///       // Close the loan to release its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Dumps the properties of a loan field
    ///    private static void dumpLoanField(LoanField field)
    ///    {
    ///       Console.WriteLine("Formatted Value:   " + field.Value);
    ///       Console.WriteLine("Unformatted Value: " + field.UnformattedValue);
    ///       Console.WriteLine("Original Value:    " + field.OriginalValue);
    ///       Console.WriteLine("Format:            " + field.Format);
    ///       Console.WriteLine("Locked:            " + field.Locked);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool Locked
    {
      get => this.loan.LoanData.IsLocked(this.ID);
      set
      {
        this.loan.EnsureExclusive();
        if (value)
          this.loan.LoanData.AddLock(this.ID);
        else
          this.loan.LoanData.RemoveLock(this.ID);
      }
    }

    /// <summary>Indicates if the current loan field is read-only.</summary>
    /// <remarks>A field may be read-only either because the underlying <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" />
    /// is read-only or because of a Loan Access Rule. Field Access Rules are not enforced by the API
    /// and thus will not factor in to this proeprty's value.</remarks>
    public bool ReadOnly
    {
      get
      {
        if (this.Descriptor.ReadOnly || this.loan.LoanData.IsFieldReadOnly(this.Descriptor.FieldID))
          return true;
        if (this.loan.LoanData.ContentAccess == LoanContentAccess.FullAccess)
          return false;
        if ((this.loan.LoanData.ContentAccess & LoanContentAccess.FormFields) == LoanContentAccess.None)
          return true;
        return this.loan.LoanData.EditableFields != null && !this.loan.LoanData.EditableFields.ContainsKey((object) this.Descriptor.FieldID);
      }
    }

    /// <summary>
    /// Gets the value of a field for a specific <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>
    /// other than the current one.
    /// </summary>
    /// <param name="pair">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see> for which to
    /// retrieve the value.</param>
    /// <returns>The value of the field for the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>
    /// </returns>
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
    public string GetValueForBorrowerPair(BorrowerPair pair)
    {
      return this.loan.LoanData.GetSimpleField(this.ID, pair.Unwrap());
    }

    /// <summary>
    /// Sets the value of a field for a specific <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see>
    /// other than the current one.
    /// </summary>
    /// <param name="pair">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPair</see> for which to
    /// retrieve the value.</param>
    /// <param name="value">The value to be assigned to the specified field</param>
    /// <example>
    /// The following code demonstrates how to read and write the value of a loan field.
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
    ///       // Lock the loan for changes
    ///       loan.Lock();
    /// 
    ///       // Retrieve the BorrowerPair for borrower "John Smith"
    ///       for (int i = 0; i < loan.BorrowerPairs.Count; i++)
    ///       {
    ///          BorrowerPair pair = loan.BorrowerPairs[i];
    /// 
    ///          if ((pair.Borrower.FirstName == "John") && (pair.Borrower.LastName == "Smith"))
    ///          {
    ///             // Fix the first name for this borrower
    ///             loan.Fields["36"].SetValueForBorrowerPair(pair, "Jonathan");
    ///          }
    ///       }
    /// 
    ///       // Save the changes and close the loan to release the resources
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void SetValueForBorrowerPair(BorrowerPair pair, string value)
    {
      string str = (string) null;
      EllieMae.EMLite.DataEngine.BorrowerPair[] borrowerPairArray = (EllieMae.EMLite.DataEngine.BorrowerPair[]) null;
      if (pair != (BorrowerPair) null && pair.Borrower != (Borrower) null && !string.IsNullOrEmpty(pair.Borrower.ID) && this.loan.LoanData.CurrentBorrowerPair.Id != pair.Borrower.ID)
      {
        str = this.loan.LoanData.CurrentBorrowerPair.Id;
        borrowerPairArray = this.loan.LoanData.GetBorrowerPairs();
        if (borrowerPairArray.Length > 1)
        {
          for (int index = 0; index < borrowerPairArray.Length; ++index)
          {
            if (borrowerPairArray[index].Borrower.Id == pair.Borrower.ID)
            {
              this.loan.LoanData.SetBorrowerPair(borrowerPairArray[index]);
              break;
            }
          }
        }
      }
      this.loan.LoanData.SetCurrentField(this.ID, this.Descriptor.ValidateInput(value ?? ""), pair.Unwrap());
      if (this.loan.CalculationsEnabled)
        this.loan.Recalculate();
      if (string.IsNullOrEmpty(str) || borrowerPairArray == null)
        return;
      for (int index = 0; index < borrowerPairArray.Length; ++index)
      {
        if (borrowerPairArray[index].Borrower.Id == str)
        {
          this.loan.LoanData.SetBorrowerPair(borrowerPairArray[index]);
          break;
        }
      }
    }
  }
}
