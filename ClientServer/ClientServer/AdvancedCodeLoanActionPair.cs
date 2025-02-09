// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AdvancedCodeLoanActionPair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AdvancedCodeLoanActionPair : LoanActionBase
  {
    private string sourceCode;

    public AdvancedCodeLoanActionPair(string loanActionId, string sourceCode)
      : base(loanActionId)
    {
      this.sourceCode = sourceCode;
    }

    public AdvancedCodeLoanActionPair(XmlSerializationInfo info)
      : base(info)
    {
      this.sourceCode = info.GetString(nameof (SourceCode));
    }

    public string SourceCode => this.sourceCode;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("SourceCode", (object) this.sourceCode);
    }
  }
}
