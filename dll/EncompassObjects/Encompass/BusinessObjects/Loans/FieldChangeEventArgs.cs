// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldChangeEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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

    public string FieldID => this.fieldId;

    public BorrowerPair BorrowerPair => this.pair;

    public string PriorValue => this.priorValue;

    public string NewValue => this.newValue;
  }
}
