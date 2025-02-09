// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusSettingsForLoan
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusSettingsForLoan : IXmlSerializable
  {
    private bool noPrompt;
    private DateTime lastPromptDate = DateTime.MinValue.Date;

    public LoanStatusSettingsForLoan()
    {
    }

    public LoanStatusSettingsForLoan(XmlSerializationInfo info)
    {
      this.noPrompt = info.GetBoolean(nameof (NoPrompt));
      this.lastPromptDate = (DateTime) info.GetValue("LastPromptDate", typeof (DateTime));
    }

    public bool NoPrompt => this.noPrompt;

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}
