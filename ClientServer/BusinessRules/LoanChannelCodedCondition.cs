// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanChannelCodedCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanChannelCodedCondition : CodedCondition
  {
    private string channelOptions = string.Empty;

    public LoanChannelCodedCondition(string channelOptions) => this.channelOptions = channelOptions;

    protected override string GetConditionDefinition()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.channelOptions.Length; ++index)
      {
        switch (this.channelOptions[index])
        {
          case '0':
            stringList.Add("");
            break;
          case '1':
            stringList.Add("Banked - Retail");
            break;
          case '2':
            stringList.Add("Banked - Wholesale");
            break;
          case '3':
            stringList.Add("Brokered");
            break;
          case '4':
            stringList.Add("Correspondent");
            break;
        }
      }
      if (stringList.Count == 0)
        return "False";
      return stringList.Count == 5 ? "True" : "Match([2626], \"" + string.Join("\", \"", stringList.ToArray()) + "\") >= 0";
    }
  }
}
