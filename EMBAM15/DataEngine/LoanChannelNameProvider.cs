// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanChannelNameProvider
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanChannelNameProvider : IEnumNameProvider
  {
    private static Dictionary<string, LoanChannel> nameToChannelMap = new Dictionary<string, LoanChannel>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<LoanChannel, string> channelToNameMap = new Dictionary<LoanChannel, string>();

    static LoanChannelNameProvider()
    {
      LoanChannelNameProvider.nameToChannelMap.Add("", LoanChannel.None);
      LoanChannelNameProvider.nameToChannelMap.Add("Banked - Retail", LoanChannel.BankedRetail);
      LoanChannelNameProvider.nameToChannelMap.Add("Banked - Wholesale", LoanChannel.BankedWholesale);
      LoanChannelNameProvider.nameToChannelMap.Add("Brokered", LoanChannel.Brokered);
      LoanChannelNameProvider.nameToChannelMap.Add("Correspondent", LoanChannel.Correspondent);
      LoanChannelNameProvider.channelToNameMap.Add(LoanChannel.None, "");
      LoanChannelNameProvider.channelToNameMap.Add(LoanChannel.BankedRetail, "Banked - Retail");
      LoanChannelNameProvider.channelToNameMap.Add(LoanChannel.BankedWholesale, "Banked - Wholesale");
      LoanChannelNameProvider.channelToNameMap.Add(LoanChannel.Brokered, "Brokered");
      LoanChannelNameProvider.channelToNameMap.Add(LoanChannel.Correspondent, "Correspondent");
    }

    public string GetName(object value)
    {
      LoanChannel key = (LoanChannel) value;
      return LoanChannelNameProvider.channelToNameMap.ContainsKey(key) ? LoanChannelNameProvider.channelToNameMap[key] : (string) null;
    }

    public string[] GetNames()
    {
      return new string[4]
      {
        LoanChannelNameProvider.channelToNameMap[LoanChannel.BankedRetail],
        LoanChannelNameProvider.channelToNameMap[LoanChannel.BankedWholesale],
        LoanChannelNameProvider.channelToNameMap[LoanChannel.Brokered],
        LoanChannelNameProvider.channelToNameMap[LoanChannel.Correspondent]
      };
    }

    public object GetValue(string name)
    {
      return LoanChannelNameProvider.nameToChannelMap.ContainsKey(name) ? (object) LoanChannelNameProvider.nameToChannelMap[name] : (object) LoanChannel.None;
    }
  }
}
