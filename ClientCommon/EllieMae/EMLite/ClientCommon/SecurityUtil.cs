// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.SecurityUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class SecurityUtil
  {
    public static bool CheckUserAccessToFeature(
      Hashtable personaFeatureRights,
      Hashtable userFeatureRights,
      AclFeature feature)
    {
      bool feature1 = false;
      if (Session.UserInfo.IsSuperAdministrator())
        feature1 = true;
      else if (userFeatureRights.Contains((object) feature))
      {
        int num;
        switch ((AclTriState) userFeatureRights[(object) feature])
        {
          case AclTriState.True:
            num = 1;
            break;
          case AclTriState.Unspecified:
            feature1 = (bool) personaFeatureRights[(object) feature];
            goto label_10;
          default:
            num = 0;
            break;
        }
        feature1 = num != 0;
      }
      else if (personaFeatureRights.Contains((object) feature))
        feature1 = (bool) personaFeatureRights[(object) feature];
label_10:
      return feature1;
    }

    public static string[] BankerOnlyTools(bool isUnderwriterSummaryAccessibleForBroker)
    {
      List<string> stringList = new List<string>();
      stringList.Add("Lock Request Form");
      if (!isUnderwriterSummaryAccessibleForBroker)
      {
        stringList.Add("Underwriter Summary");
        stringList.Add("Underwriter Summary Page 2");
      }
      stringList.Add("Funding Worksheet");
      stringList.Add("Funding Balancing Worksheet");
      stringList.Add("Broker Check Calculation");
      stringList.Add("Secondary Registration");
      stringList.Add("Interim Servicing Worksheet");
      stringList.Add("Shipping Detail");
      stringList.Add("Collateral Tracking");
      stringList.Add("Correspondent Purchase Advice Form");
      stringList.Add("Purchase Advice Form");
      stringList.Add("Audit Trail");
      stringList.Add("LO Compensation");
      stringList.Add("TPO Information");
      stringList.Add("Worst Case Pricing");
      stringList.Add("Correspondent Loan Status");
      stringList.Add("Lock Comparison Tool");
      return stringList.ToArray();
    }
  }
}
