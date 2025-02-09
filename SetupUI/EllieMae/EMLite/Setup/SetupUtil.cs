// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SetupUtil
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SetupUtil
  {
    public static string ApiSessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static UserInfo GetNewUserInfoFromDialog(
      Sessions.Session session,
      IWin32Window owner,
      bool bEdit,
      string userid,
      int orgId,
      bool bReadOnly)
    {
      AddEditUserCEDialog editUserCeDialog = !bEdit ? new AddEditUserCEDialog(session, (IWin32Window) null, (string) null, orgId, false) : new AddEditUserCEDialog(session, (IWin32Window) null, userid, orgId, bReadOnly);
      if (editUserCeDialog.ShowDialog(owner) == DialogResult.Cancel && !editUserCeDialog.DataSaved)
        return (UserInfo) null;
      if (session.UserID == userid)
      {
        int num = (int) Utils.Dialog(owner, "You are editing your own user information. Please log out and log in again to have a consistent environment.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return editUserCeDialog.getUserInfo();
    }

    public static IEnumerable<EllieMae.EMLite.Workflow.Milestone> GetMilestones(
      Sessions.Session session,
      bool archived)
    {
      return archived ? ((MilestoneTemplatesBpmManager) session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList() : ((MilestoneTemplatesBpmManager) session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
    }
  }
}
