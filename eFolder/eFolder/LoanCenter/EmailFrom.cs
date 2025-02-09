// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.EmailFrom
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class EmailFrom
  {
    internal const string CURRENT_USER = "Current User";
    internal const string FILE_STARTER = "File Starter";
    internal const string LOAN_OFFICER = "Loan Officer";

    public static void GetFromUser(
      LoanDataMgr loanDataMgr,
      EmailFromType userType,
      ref string userid,
      ref string email,
      ref string name)
    {
      userid = string.Empty;
      email = string.Empty;
      name = string.Empty;
      switch (userType)
      {
        case EmailFromType.CurrentUser:
          userid = Session.UserInfo.Userid;
          email = Session.UserInfo.Email;
          name = Session.UserInfo.FullName;
          break;
        case EmailFromType.FileStarter:
          foreach (MilestoneLog allMilestone in loanDataMgr.LoanData.GetLogList().GetAllMilestones())
          {
            if (allMilestone.Stage == "Started")
            {
              userid = allMilestone.LoanAssociateID;
              email = allMilestone.LoanAssociateEmail;
              name = allMilestone.LoanAssociateName;
              break;
            }
          }
          break;
        case EmailFromType.LoanOfficer:
          userid = loanDataMgr.LoanData.GetField("LOID");
          if (string.IsNullOrEmpty(userid))
            break;
          UserInfo user = Session.OrganizationManager.GetUser(userid);
          if (!(user != (UserInfo) null))
            break;
          userid = user.Userid;
          email = user.Email;
          name = user.FullName;
          break;
      }
    }

    public static void SaveFromUser(
      LoanDataMgr loanDataMgr,
      Dictionary<string, string> contactDetails)
    {
      switch (contactDetails["SenderType"])
      {
        case "Current User":
          if (contactDetails.ContainsKey("Email"))
            Session.UserInfo.Email = contactDetails["Email"];
          if (contactDetails.ContainsKey("Name"))
          {
            string[] source = contactDetails["Name"].Split(' ');
            string str = string.Empty;
            if (((IEnumerable<string>) source).Count<string>() > 1)
              str = ((IEnumerable<string>) source).ToList<string>().LastOrDefault<string>();
            Session.UserInfo.FirstName = contactDetails["Name"].Replace(" " + str, string.Empty);
            if (!string.IsNullOrEmpty(str))
              Session.UserInfo.LastName = str;
          }
          Session.User.UpdatePersonalInfo(Session.UserInfo.FirstName, Session.UserInfo.LastName, Session.UserInfo.Email, Session.UserInfo.Phone, Session.UserInfo.CellPhone, Session.UserInfo.Fax);
          break;
        case "File Starter":
          foreach (MilestoneLog allMilestone in loanDataMgr.LoanData.GetLogList().GetAllMilestones())
          {
            if (allMilestone.Stage == "Started")
            {
              string loanAssociateId = allMilestone.LoanAssociateID;
              string email = allMilestone.LoanAssociateEmail;
              string fullName = allMilestone.LoanAssociateName;
              string loanAssociateTitle = allMilestone.LoanAssociateTitle;
              string loanAssociatePhone = allMilestone.LoanAssociatePhone;
              string associateCellPhone = allMilestone.LoanAssociateCellPhone;
              string loanAssociateFax = allMilestone.LoanAssociateFax;
              if (contactDetails.ContainsKey("Name"))
                fullName = contactDetails["Name"];
              if (contactDetails.ContainsKey("Email"))
                email = contactDetails["Email"];
              allMilestone.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, associateCellPhone, loanAssociateFax, loanAssociateTitle);
            }
          }
          break;
        case "Loan Officer":
          string field = loanDataMgr.LoanData.GetField("LOID");
          if (string.IsNullOrEmpty(field))
            break;
          UserInfo user = Session.OrganizationManager.GetUser(field);
          if (!(user != (UserInfo) null))
            break;
          if (contactDetails.ContainsKey("Email"))
            user.Email = contactDetails["Email"];
          if (contactDetails.ContainsKey("Name"))
          {
            string[] source = contactDetails["Name"].Split(' ');
            string str = string.Empty;
            if (((IEnumerable<string>) source).Count<string>() > 1)
              str = ((IEnumerable<string>) source).ToList<string>().LastOrDefault<string>();
            user.FirstName = contactDetails["Name"].Replace(" " + str, string.Empty);
            if (!string.IsNullOrEmpty(str))
              user.LastName = str;
          }
          Session.OrganizationManager.UpdateUser(user);
          break;
      }
    }
  }
}
