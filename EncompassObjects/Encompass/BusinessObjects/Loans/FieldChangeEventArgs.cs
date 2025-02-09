// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldChangeEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Event arguments for the FieldChange loan event.</summary>
  public class FieldChangeEventArgs : EventArgs
  {
    private string fieldId;
    private BorrowerPair pair;
    private string priorValue;
    private string newValue;

    internal FieldChangeEventArgs(Loan loan, FieldChangedEventArgs e)
    {
      this.fieldId = e.FieldID;
      this.pair = loan.BorrowerPairs.GetPair(e.BorrowerPair);
      this.priorValue = e.PriorValue;
      this.newValue = e.NewValue;
    }

    /// <summary>Gets the FieldID</summary>
    public string FieldID => this.fieldId;

    /// <summary>Gets the BorrowerPair</summary>
    public BorrowerPair BorrowerPair => this.pair;

    /// <summary>Gets the PriorValue</summary>
    public string PriorValue => this.priorValue;

    /// <summary>Gets the NewValue</summary>
    public string NewValue => this.newValue;
  }
}
