// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Utilities.AutoAssignUtils
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.eFolder.Utilities
{
  public class AutoAssignUtils
  {
    private const string className = "AutoAssignUtils";
    private static readonly string sw = Tracing.SwEFolder;
    private static readonly string ngAutoAssignCompanySetting = Session.ConfigurationManager.GetCompanySetting("ADRSetup", "NGAutoAssign");

    public static bool IsNGAutoAssignEnabled
    {
      get
      {
        if (!Session.LoanDataMgr.UseSkyDrive && (!Session.LoanDataMgr.UseSkyDriveClassic || Session.LoanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments))
          return false;
        return string.IsNullOrEmpty(AutoAssignUtils.ngAutoAssignCompanySetting) || string.Equals(AutoAssignUtils.ngAutoAssignCompanySetting, "TRUE", StringComparison.OrdinalIgnoreCase);
      }
    }

    public DocumentLog[] GetRefreshedLoanDocumentLogs(DocumentLog[] docList)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      try
      {
        LogList logList = Session.LoanDataMgr.LoanData.GetLogList();
        foreach (DocumentLog doc in docList)
          documentLogList.Add(logList?.GetRecordByID(doc.Guid) as DocumentLog);
      }
      catch (Exception ex)
      {
        Tracing.Log(AutoAssignUtils.sw, TraceLevel.Error, nameof (AutoAssignUtils), string.Format("Error in refreshing loan loglist and exception-{0}", (object) ex));
      }
      return documentLogList.ToArray();
    }
  }
}
