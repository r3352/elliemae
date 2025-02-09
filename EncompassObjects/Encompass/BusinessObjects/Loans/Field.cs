// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Field
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Abstract base class for fields in the loan.</summary>
  public abstract class Field : IField
  {
    private FieldDescriptor descriptor;
    private string fieldId;

    internal Field(FieldDescriptor descriptor)
    {
      this.descriptor = descriptor;
      this.fieldId = this.descriptor.FieldID;
    }

    internal Field(string fieldId, FieldDescriptor descriptor)
    {
      this.descriptor = descriptor;
      this.fieldId = fieldId;
    }

    /// <summary>
    /// Gets the Ellie Mae ID of the field within the loan file.
    /// </summary>
    public string ID => this.fieldId;

    /// <summary>
    /// Gets the format which should be applied to the field for display to the user.
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
    public LoanFieldFormat Format => this.descriptor.Format;

    /// <summary>
    /// Gets the raw, unformatted value of the underlying field.
    /// </summary>
    public abstract string UnformattedValue { get; }

    /// <summary>Gets the formatted value of the underlying field.</summary>
    /// <returns>This method returns the field value in a formatted form, i.e. in a manner suitable
    /// for display to a user.</returns>
    public string FormattedValue => this.descriptor.FormatValue(this.UnformattedValue);

    /// <summary>
    /// Gets or sets the value of the underlying field in the field's native format.
    /// </summary>
    /// <remarks>If the field is unset, this function will return <c>null</c>, except in
    /// the case of string-valued fields for which the property will return the empty string.
    /// If the field is set, a conversion will occur to its underlying data type (Int32, Decimal, Date or String)
    /// and the resulting value will be returned.
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
    public object Value
    {
      get => this.descriptor.ConvertToNativeType(this.UnformattedValue);
      set => this.setFieldValue(this.descriptor.ValidateInput(string.Concat(value)));
    }

    /// <summary>Abstract method for setting the value of the field.</summary>
    /// <param name="value">The formatted value of the field</param>
    internal abstract void setFieldValue(string value);

    /// <summary>
    /// Gets or sets the value of the field thru the ILoanField interface.
    /// </summary>
    /// <remarks>This method exists primarilly for COM-based clients which cannot marshal values
    /// properly to the object-valued Value property.</remarks>
    string IField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    /// <summary>Determines if the field is empty.</summary>
    /// <returns>Returns <c>true</c> if the field is unset, <c>false</c> otherwise.</returns>
    public bool IsEmpty() => this.UnformattedValue == "";

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> for the field.
    /// </summary>
    /// <returns>Returns the FieldDesciptor for this field</returns>
    public FieldDescriptor Descriptor => this.descriptor;

    /// <summary>
    /// Provides a string representation of the value in the field.
    /// </summary>
    /// <returns>Returns the value of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Field.Value" /> property.</returns>
    public override string ToString() => this.FormattedValue;

    /// <summary>
    /// Provides a decimal representation of the value in the field.
    /// </summary>
    /// <returns>Returns the numeric equivalent of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Field.Value" /> property,
    /// or 0 if the value cannot be converted to a Decimal.</returns>
    public int ToInt() => Utils.ParseInt((object) this.UnformattedValue, 0);

    /// <summary>
    /// Provides a decimal representation of the value in the field.
    /// </summary>
    /// <returns>Returns the numeric equivalent of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Field.Value" /> property,
    /// or 0 if the value cannot be converted to a Decimal.</returns>
    public Decimal ToDecimal() => Utils.ParseDecimal((object) this.UnformattedValue, 0M);

    /// <summary>
    /// Provides a date/time representation of the value in the field.
    /// </summary>
    /// <returns>Returns the numeric equivalent of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Field.Value" /> property,
    /// or <c>DateTime.MinValue</c> if the value cannot be converted to a DateTime.</returns>
    public DateTime ToDate()
    {
      return this.Format == LoanFieldFormat.MONTHDAY ? Utils.ParseMonthDay((object) this.UnformattedValue, DateTime.MinValue) : Utils.ParseDate((object) this.UnformattedValue, DateTime.MinValue);
    }
  }
}
