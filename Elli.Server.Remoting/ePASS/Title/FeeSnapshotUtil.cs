// Decompiled with JetBrains decompiler
// Type: ePASS.Title.FeeSnapshotUtil
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using ePASS.Title.WebServices;
using System.Collections.Generic;

#nullable disable
namespace ePASS.Title
{
  public class FeeSnapshotUtil
  {
    private static string getFieldValue(IBam bam, string fieldID)
    {
      return string.IsNullOrWhiteSpace(fieldID) ? "" : bam.GetSimpleField(fieldID);
    }

    private static void constructFeeSnapshotItem(
      List<OrderFeeSnapshotItem> feeSnapshots,
      IBam bam,
      string hudLine,
      string nameID,
      string toCompanyID,
      string amountID,
      string sellerAmountID,
      string deedAmountID = null,
      string mortgageAmountID = null,
      string releaseAmountID = null)
    {
      string fieldValue1 = FeeSnapshotUtil.getFieldValue(bam, nameID);
      string fieldValue2 = FeeSnapshotUtil.getFieldValue(bam, toCompanyID);
      string fieldValue3 = FeeSnapshotUtil.getFieldValue(bam, amountID);
      string fieldValue4 = FeeSnapshotUtil.getFieldValue(bam, sellerAmountID);
      string fieldValue5 = FeeSnapshotUtil.getFieldValue(bam, deedAmountID);
      string fieldValue6 = FeeSnapshotUtil.getFieldValue(bam, mortgageAmountID);
      string fieldValue7 = FeeSnapshotUtil.getFieldValue(bam, releaseAmountID);
      if (string.IsNullOrWhiteSpace(fieldValue1) && string.IsNullOrWhiteSpace(fieldValue2) && string.IsNullOrWhiteSpace(fieldValue3) && string.IsNullOrWhiteSpace(fieldValue4) && string.IsNullOrWhiteSpace(fieldValue5) && string.IsNullOrWhiteSpace(fieldValue6) && string.IsNullOrWhiteSpace(fieldValue7))
        return;
      feeSnapshots.Add(new OrderFeeSnapshotItem()
      {
        HudLine = hudLine,
        Name = fieldValue1,
        ToCompany = fieldValue2,
        Amount = fieldValue3,
        SellerAmount = fieldValue4,
        DeedAmount = fieldValue5,
        MortgageAmount = fieldValue6,
        ReleaseAmount = fieldValue7
      });
    }

    public static OrderFeeSnapshotItem[] BuildFeeSnapshot(IBam bam)
    {
      List<OrderFeeSnapshotItem> feeSnapshots = new List<OrderFeeSnapshotItem>();
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101", "", "NEWHUD.X202", "", "", "", "", "");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101a", "NEWHUD.X951", "NEWHUD.X1070", "NEWHUD.X952", "NEWHUD.X953");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101b", "NEWHUD.X960", "NEWHUD.X1071", "NEWHUD.X961", "NEWHUD.X962");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101c", "NEWHUD.X969", "NEWHUD.X1072", "NEWHUD.X970", "NEWHUD.X971");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101d", "NEWHUD.X978", "NEWHUD.X1073", "NEWHUD.X979", "NEWHUD.X980");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101e", "NEWHUD.X987", "NEWHUD.X1074", "NEWHUD.X988", "NEWHUD.X989");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1101f", "NEWHUD.X996", "NEWHUD.X1075", "NEWHUD.X997", "NEWHUD.X998");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102a", "", "NEWHUD.X203", "NEWHUD2.X11", "NEWHUD2.X12");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102b", "", "NEWHUD2.X13", "NEWHUD2.X14", "NEWHUD2.X15");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102c", "", "NEWHUD2.X16", "NEWHUD.X808", "NEWHUD2.X17");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102d", "NEWHUD.X809", "NEWHUD2.X55", "NEWHUD.X810", "NEWHUD2.X18");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102e", "NEWHUD.X811", "NEWHUD2.X56", "NEWHUD.X812", "NEWHUD2.X19");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102f", "NEWHUD.X813", "NEWHUD2.X57", "NEWHUD.X814", "NEWHUD2.X20");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102g", "NEWHUD.X815", "NEWHUD2.X58", "NEWHUD.X816", "NEWHUD2.X21");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1102h", "NEWHUD.X817", "NEWHUD2.X59", "NEWHUD.X818", "NEWHUD2.X22");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1103", "", "NEWHUD.X204", "NEWHUD.X572", "NEWHUD.X783");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1104", "", "NEWHUD.X205", "NEWHUD.X639", "NEWHUD.X784");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1105", "", "", "646", "");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1106", "", "", "1634", "");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1107", "", "NEWHUD.X206", "NEWHUD.X640", "");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1108", "", "NEWHUD.X207", "NEWHUD.X641", "");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1109", "NEWHUD.X208", "NEWHUD.X1076", "NEWHUD.X215", "NEWHUD.X218");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1110", "NEWHUD.X209", "NEWHUD.X1077", "NEWHUD.X216", "NEWHUD.X219");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1111", "1762", "NEWHUD.X1078", "1763", "1764");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1112", "1767", "NEWHUD.X1079", "1768", "1769");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1113", "1772", "NEWHUD.X1080", "1773", "1774");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1114", "1777", "NEWHUD.X1081", "1778", "1779");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1115", "NEWHUD.X1602", "NEWHUD.X1603", "NEWHUD.X1604", "NEWHUD.X1605");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1116", "NEWHUD.X1610", "NEWHUD.X1611", "NEWHUD.X1612", "NEWHUD.X1613");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1202", "", "", "390", "587", "2402", "2403", "2404");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1203", "", "NEWHUD.X947", "NEWHUD.X731", "NEWHUD.X787");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1204", "", "", "647", "593", "2405", "2406");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1205", "", "", "648", "594", "2407", "2408");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1206", "373", "NEWHUD.X1082", "374", "576");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1207", "1640", "NEWHUD.X1083", "1641", "1642");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1208", "1643", "NEWHUD.X1084", "1644", "1645");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1209", "NEWHUD.X1618", "NEWHUD.X1619", "NEWHUD.X1620", "NEWHUD.X1621");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1210", "NEWHUD.X1625", "NEWHUD.X1626", "NEWHUD.X1627", "NEWHUD.X1628");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1301", "", "", "NEWHUD.X603", "NEWHUD.X800");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1302", "NEWHUD.X251", "NEWHUD.X1085", "NEWHUD.X254", "NEWHUD.X258");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1303", "650", "NEWHUD.X1086", "644", "590");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1304", "651", "NEWHUD.X1087", "645", "591");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1305", "40", "NEWHUD.X1088", "41", "42");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1306", "43", "NEWHUD.X1089", "44", "55");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1307", "1782", "NEWHUD.X1090", "1783", "1784");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1308", "1787", "NEWHUD.X1091", "1788", "1789");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1309", "1792", "NEWHUD.X1092", "1793", "1794");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1310", "NEWHUD.X252", "NEWHUD.X1093", "NEWHUD.X255", "NEWHUD.X259");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1311", "NEWHUD.X253", "NEWHUD.X1094", "NEWHUD.X256", "NEWHUD.X260");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1312", "NEWHUD.X1632", "NEWHUD.X1633", "NEWHUD.X1634", "NEWHUD.X1635");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1313", "NEWHUD.X1640", "NEWHUD.X1641", "NEWHUD.X1642", "NEWHUD.X1643");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1314", "NEWHUD.X1648", "NEWHUD.X1649", "NEWHUD.X1650", "NEWHUD.X1651");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1315", "NEWHUD.X1656", "NEWHUD.X1657", "NEWHUD.X1658", "NEWHUD.X1659");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1316", "NEWHUD2.X4610", "NEWHUD2.X4611", "NEWHUD2.X4612", "NEWHUD2.X4613");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1317", "NEWHUD2.X4617", "NEWHUD2.X4618", "NEWHUD2.X4619", "NEWHUD2.X4620");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1318", "NEWHUD2.X4624", "NEWHUD2.X4625", "NEWHUD2.X4626", "NEWHUD2.X4627");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1319", "NEWHUD2.X4631", "NEWHUD2.X4632", "NEWHUD2.X4633", "NEWHUD2.X4634");
      FeeSnapshotUtil.constructFeeSnapshotItem(feeSnapshots, bam, "1320", "NEWHUD2.X4638", "NEWHUD2.X4639", "NEWHUD2.X4640", "NEWHUD2.X4641");
      return feeSnapshots.ToArray();
    }
  }
}
