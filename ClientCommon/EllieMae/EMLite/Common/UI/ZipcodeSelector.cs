// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ZipcodeSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ZipcodeSelector
  {
    private static ZipcodeUserDefinedList zipcodeUserDefinedList = Session.ConfigurationManager.GetZipcodeUserDefinedList();

    public static ZipCodeInfo GetZipCodeInfo(string zipcode, ZipCodeInfo[] zips)
    {
      return ZipcodeSelector.GetZipCodeInfo(zipcode, zips, false);
    }

    public static ZipCodeInfo GetZipCodeInfo(
      string zipcode,
      ZipCodeInfo[] zips,
      bool getFirstMatched)
    {
      if (zips == null)
        return (ZipCodeInfo) null;
      if (zips.Length == 1 || getFirstMatched && zips.Length != 0)
        return zips[0];
      using (ZipcodeSelectorDialog zipcodeSelectorDialog = new ZipcodeSelectorDialog(zipcode, zips))
      {
        if (zipcodeSelectorDialog.ShowDialog() == DialogResult.OK)
          return zipcodeSelectorDialog.SelectedZipCodeInfo;
      }
      return (ZipCodeInfo) null;
    }

    public static ZipCodeInfo GetZipCodeInfoWithUserDefined(string zipcode)
    {
      if (zipcode == string.Empty)
        return (ZipCodeInfo) null;
      List<ZipCodeInfo> zipCodeInfoList = new List<ZipCodeInfo>();
      ZipCodeInfo[] multipleZipInfoAt = ZipCodeUtils.GetMultipleZipInfoAt(zipcode);
      if (multipleZipInfoAt != null && multipleZipInfoAt.Length != 0)
        zipCodeInfoList.AddRange((IEnumerable<ZipCodeInfo>) multipleZipInfoAt);
      if (ZipcodeSelector.zipcodeUserDefinedList == null)
      {
        ZipcodeSelector.zipcodeUserDefinedList = Session.ConfigurationManager.GetZipcodeUserDefinedList();
        if (ZipcodeSelector.zipcodeUserDefinedList == null)
          ZipcodeSelector.zipcodeUserDefinedList = new ZipcodeUserDefinedList();
      }
      ZipCodeInfo[] zipcodeInfo = ZipcodeSelector.zipcodeUserDefinedList.GetZipcodeInfo(zipcode);
      if (zipcodeInfo != null)
      {
        if (zipcodeInfo.Length == 1)
          return zipcodeInfo[0];
        if (zipcodeInfo.Length != 0)
          zipCodeInfoList.AddRange((IEnumerable<ZipCodeInfo>) zipcodeInfo);
      }
      return zipCodeInfoList != null && zipCodeInfoList.Count > 0 ? ZipcodeSelector.GetZipCodeInfo(zipcode, zipCodeInfoList.ToArray()) : (ZipCodeInfo) null;
    }
  }
}
