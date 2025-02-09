// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UndefinedField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class UndefinedField : FieldDefinition
  {
    private string description;
    private string rolodex;
    private FieldFormat format;

    public UndefinedField(string fieldId, string description)
      : base(fieldId)
    {
      this.description = description;
    }

    public override FieldFormat Format
    {
      get => this.format;
      set => this.format = FieldFormat.NONE;
    }

    public override bool AllowInReportingDatabase => false;

    public override bool AllowEdit => false;

    public override string Description => this.description;

    public override string Rolodex
    {
      get => this.rolodex;
      set => this.rolodex = value;
    }

    public override string GetValue(LoanData loan) => "";

    public override string GetValue(LoanData loan, string id) => "";

    public override void SetValue(LoanData loan, string value)
    {
      throw new InvalidOperationException("Cannot save value to undefined field");
    }
  }
}
