// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.LoanFolderUtil
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class LoanFolderUtil
  {
    public static LoanFolderInfo[] LoanFolderNames2LoanFolderInfos(string[] folderNames)
    {
      return LoanFolderUtil.LoanFolderNames2LoanFolderInfos(folderNames, true);
    }

    public static LoanFolderInfo[] LoanFolderNames2LoanFolderInfos(
      string[] folderNames,
      bool applySecurity)
    {
      int selectedIndex = -1;
      return LoanFolderUtil.LoanFolderNames2LoanFolderInfos(folderNames, (string) null, out selectedIndex, applySecurity);
    }

    public static LoanFolderInfo[] LoanFolderNames2LoanFolderInfos(
      string[] folderNames,
      string selectedFolder,
      out int selectedIndex)
    {
      return LoanFolderUtil.LoanFolderNames2LoanFolderInfos(folderNames, selectedFolder, out selectedIndex, true);
    }

    public static LoanFolderInfo[] LoanFolderNames2LoanFolderInfos(
      string[] folderNames,
      string selectedFolder,
      out int selectedIndex,
      bool applySecurity)
    {
      selectedIndex = -1;
      if (folderNames == null)
        return (LoanFolderInfo[]) null;
      List<LoanFolderInfo> loanFolderInfoList = new List<LoanFolderInfo>();
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(true, applySecurity);
      foreach (string folderName in folderNames)
      {
        foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
        {
          if (string.Compare(loanFolderInfo.Name, folderName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            loanFolderInfoList.Add(loanFolderInfo);
            if (selectedFolder != null && string.Compare(selectedFolder, loanFolderInfo.Name, StringComparison.OrdinalIgnoreCase) == 0)
              selectedIndex = loanFolderInfoList.Count - 1;
          }
        }
      }
      return loanFolderInfoList.ToArray();
    }
  }
}
