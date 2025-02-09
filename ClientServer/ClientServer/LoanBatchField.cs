// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanBatchField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanBatchField
  {
    private string fieldId;
    private object fieldValue;

    public LoanBatchField(string fieldId, object fieldValue)
    {
      this.fieldId = fieldId;
      this.fieldValue = fieldValue;
    }

    public string FieldID => this.fieldId;

    public object FieldValue => this.fieldValue;
  }
}
