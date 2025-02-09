// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.OTPFileContacts
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  public class OTPFileContacts(LoanData loanData) : FileContacts(loanData)
  {
    protected override Dictionary<string, string> getFieldIds(string contactType)
    {
      Dictionary<string, string> fieldIds = base.getFieldIds(contactType);
      if (fieldIds != null)
      {
        switch (contactType)
        {
          case "Appraiser":
            fieldIds.Add("CellPhoneNumber", "VEND.X526");
            fieldIds.Add("WorkPhoneNumber", "622");
            break;
          case "Assign To":
            fieldIds.Add("CellPhoneNumber", "VEND.X514");
            fieldIds.Add("WorkPhoneNumber", "VEND.X287");
            break;
          case "Borrower":
            fieldIds.Add("CellPhoneNumber", "1490");
            fieldIds.Add("HomePhoneNumber", "66");
            fieldIds.Add("WorkPhoneNumber", "FE0117");
            break;
          case "Broker":
            fieldIds.Add("CellPhoneNumber", "VEND.X515");
            fieldIds.Add("WorkPhoneNumber", "VEND.X303");
            break;
          case "Builder":
            fieldIds.Add("CellPhoneNumber", "VEND.X503");
            fieldIds.Add("WorkPhoneNumber", "718");
            break;
          case "Buyer's Agent":
            fieldIds.Add("CellPhoneNumber", "VEND.X500");
            fieldIds.Add("WorkPhoneNumber", "VEND.X140");
            break;
          case "Buyer's Attorney":
            fieldIds.Add("CellPhoneNumber", "VEND.X498");
            fieldIds.Add("WorkPhoneNumber", "VEND.X118");
            break;
          case "Coborrower":
            fieldIds.Add("CellPhoneNumber", "1480");
            fieldIds.Add("HomePhoneNumber", "98");
            fieldIds.Add("WorkPhoneNumber", "FE0217");
            break;
          case "Credit Company":
            fieldIds.Add("CellPhoneNumber", "VEND.X523");
            fieldIds.Add("WorkPhoneNumber", "629");
            break;
          case "Custom Category #1":
            fieldIds.Add("CellPhoneNumber", "VEND.X509");
            fieldIds.Add("WorkPhoneNumber", "VEND.X61");
            break;
          case "Custom Category #2":
            fieldIds.Add("CellPhoneNumber", "VEND.X510");
            fieldIds.Add("WorkPhoneNumber", "VEND.X71");
            break;
          case "Custom Category #3":
            fieldIds.Add("CellPhoneNumber", "VEND.X511");
            fieldIds.Add("WorkPhoneNumber", "VEND.X81");
            break;
          case "Custom Category #4":
            fieldIds.Add("CellPhoneNumber", "VEND.X512");
            fieldIds.Add("WorkPhoneNumber", "VEND.X8");
            break;
          case "Doc Signing":
            fieldIds.Add("CellPhoneNumber", "VEND.X506");
            fieldIds.Add("WorkPhoneNumber", "VEND.X196");
            break;
          case "Docs Prepared By":
            fieldIds.Add("CellPhoneNumber", "VEND.X516");
            fieldIds.Add("WorkPhoneNumber", "VEND.X318");
            break;
          case "Escrow Company":
            fieldIds.Add("CellPhoneNumber", "VEND.X518");
            fieldIds.Add("WorkPhoneNumber", "615");
            break;
          case "Financial Planner":
            fieldIds.Add("CellPhoneNumber", "VEND.X508");
            fieldIds.Add("WorkPhoneNumber", "VEND.X51");
            break;
          case "Flood Insurance":
            fieldIds.Add("CellPhoneNumber", "VEND.X524");
            fieldIds.Add("WorkPhoneNumber", "VEND.X19");
            break;
          case "Hazard Insurance":
            fieldIds.Add("CellPhoneNumber", "VEND.X517");
            fieldIds.Add("WorkPhoneNumber", "VEND.X163");
            break;
          case "Investor":
            fieldIds.Add("CellPhoneNumber", "VEND.X513");
            fieldIds.Add("WorkPhoneNumber", "VEND.X272");
            break;
          case "Lender":
            fieldIds.Add("CellPhoneNumber", "VEND.X497");
            fieldIds.Add("WorkPhoneNumber", "1262");
            break;
          case "Loan Officer":
            fieldIds.Add("CellPhoneNumber", "2854");
            fieldIds.Add("WorkPhoneNumber", "1406");
            break;
          case "Loan Processor":
            fieldIds.Add("CellPhoneNumber", "2855");
            fieldIds.Add("WorkPhoneNumber", "1408");
            break;
          case "Mortgage Insurance":
            fieldIds.Add("CellPhoneNumber", "VEND.X525");
            fieldIds.Add("WorkPhoneNumber", "711");
            break;
          case "Notary":
            fieldIds.Add("CellPhoneNumber", "VEND.X520");
            fieldIds.Add("WorkPhoneNumber", "VEND.X430");
            break;
          case "Seller":
            fieldIds.Add("CellPhoneNumber", "VEND.X502");
            fieldIds.Add("HomePhoneNumber", "704");
            fieldIds.Add("WorkPhoneNumber", "VEND.X220");
            break;
          case "Seller's Agent":
            fieldIds.Add("CellPhoneNumber", "VEND.X501");
            fieldIds.Add("WorkPhoneNumber", "VEND.X151");
            break;
          case "Seller's Attorney":
            fieldIds.Add("CellPhoneNumber", "VEND.X499");
            fieldIds.Add("WorkPhoneNumber", "VEND.X129");
            break;
          case "Seller2":
            fieldIds.Add("CellPhoneNumber", "VEND.X521");
            fieldIds.Add("HomePhoneNumber", "VEND.X417");
            fieldIds.Add("WorkPhoneNumber", "VEND.X421");
            break;
          case "Seller3":
            fieldIds.Add("CellPhoneNumber", "Seller3.Cell");
            fieldIds.Add("HomePhoneNumber", "Seller3.Phone");
            fieldIds.Add("WorkPhoneNumber", "Seller3.BusPh");
            break;
          case "Seller4":
            fieldIds.Add("CellPhoneNumber", "Seller4.Cell");
            fieldIds.Add("HomePhoneNumber", "Seller4.Phone");
            fieldIds.Add("WorkPhoneNumber", "Seller4.BusPh");
            break;
          case "Servicing":
            fieldIds.Add("CellPhoneNumber", "VEND.X505");
            fieldIds.Add("WorkPhoneNumber", "VEND.X185");
            break;
          case "Settlement Agent":
            fieldIds.Add("CellPhoneNumber", "VEND.X672");
            fieldIds.Add("WorkPhoneNumber", "VEND.X669");
            break;
          case "Surveyor":
            fieldIds.Add("CellPhoneNumber", "VEND.X504");
            fieldIds.Add("WorkPhoneNumber", "VEND.X41");
            break;
          case "Title Insurance":
            fieldIds.Add("CellPhoneNumber", "VEND.X519");
            fieldIds.Add("WorkPhoneNumber", "417");
            break;
          case "Underwriter":
            fieldIds.Add("CellPhoneNumber", "VEND.X522");
            fieldIds.Add("WorkPhoneNumber", "1410");
            break;
          case "Warehouse":
            fieldIds.Add("CellPhoneNumber", "VEND.X507");
            fieldIds.Add("WorkPhoneNumber", "VEND.X207");
            break;
        }
      }
      return fieldIds;
    }

    public override Dictionary<string, string> GetContactDetails(string contactType)
    {
      Dictionary<string, string> contactDetails = new Dictionary<string, string>();
      contactDetails.Add("Name", string.Empty);
      contactDetails.Add("Email", string.Empty);
      contactDetails.Add("CellPhoneNumber", string.Empty);
      contactDetails.Add("HomePhoneNumber", string.Empty);
      contactDetails.Add("WorkPhoneNumber", string.Empty);
      if (contactType == "Loan Officer" || contactType == "Loan Processor" || contactType == "File Starter" || contactType == "Underwriter")
      {
        Hashtable hashtable = new Hashtable();
        LogList logList = this.loanData.GetLogList();
        if (logList == null)
          return (Dictionary<string, string>) null;
        foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
        {
          if (allMilestone.RoleID != -1 || !((allMilestone.RoleName ?? "") == ""))
          {
            if (allMilestone.Stage == "Started" && contactType == "File Starter")
            {
              contactDetails["Name"] = allMilestone.LoanAssociateName;
              contactDetails["Email"] = allMilestone.LoanAssociateEmail;
              contactDetails["CellPhoneNumber"] = allMilestone.LoanAssociateCellPhone;
              contactDetails["WorkPhoneNumber"] = allMilestone.LoanAssociatePhone;
              return contactDetails;
            }
            if (hashtable.ContainsKey((object) allMilestone.RoleID) && ((RoleSummaryInfo) hashtable[(object) allMilestone.RoleID]).RoleName == contactType)
            {
              contactDetails["Name"] = allMilestone.LoanAssociateName;
              contactDetails["Email"] = allMilestone.LoanAssociateEmail;
              contactDetails["CellPhoneNumber"] = allMilestone.LoanAssociateCellPhone;
              contactDetails["WorkPhoneNumber"] = allMilestone.LoanAssociatePhone;
              return contactDetails;
            }
            if (allMilestone.RoleName == contactType)
            {
              contactDetails["Name"] = allMilestone.LoanAssociateName;
              contactDetails["Email"] = allMilestone.LoanAssociateEmail;
              contactDetails["CellPhoneNumber"] = allMilestone.LoanAssociateCellPhone;
              contactDetails["WorkPhoneNumber"] = allMilestone.LoanAssociatePhone;
              return contactDetails;
            }
          }
        }
        foreach (MilestoneFreeRoleLog milestoneFreeRole in this.loanData.GetLogList().GetAllMilestoneFreeRoles())
        {
          LoanAssociateLog loanAssociateLog = (LoanAssociateLog) milestoneFreeRole;
          if (milestoneFreeRole.RoleName == contactType)
          {
            contactDetails["Name"] = loanAssociateLog.LoanAssociateName;
            contactDetails["Email"] = loanAssociateLog.LoanAssociateEmail;
            contactDetails["CellPhoneNumber"] = loanAssociateLog.LoanAssociateCellPhone;
            contactDetails["WorkPhoneNumber"] = loanAssociateLog.LoanAssociatePhone;
            return contactDetails;
          }
        }
        return contactDetails;
      }
      Dictionary<string, string> fieldIds = this.getFieldIds(contactType);
      string simpleField = this.loanData.GetSimpleField(fieldIds["Email"]);
      string str1 = string.Empty;
      string str2 = fieldIds["Name"];
      char[] chArray = new char[1]{ ';' };
      foreach (string id in ((IEnumerable<string>) str2.Split(chArray)).ToList<string>())
        str1 = string.Join(" ", ((IEnumerable<string>) new string[2]
        {
          str1,
          this.loanData.GetSimpleField(id)
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      contactDetails["Name"] = str1;
      contactDetails["Email"] = simpleField;
      if (fieldIds.ContainsKey("CellPhoneNumber"))
        contactDetails["CellPhoneNumber"] = this.loanData.GetSimpleField(fieldIds["CellPhoneNumber"]);
      if (fieldIds.ContainsKey("HomePhoneNumber"))
        contactDetails["HomePhoneNumber"] = this.loanData.GetSimpleField(fieldIds["HomePhoneNumber"]);
      if (fieldIds.ContainsKey("WorkPhoneNumber"))
        contactDetails["WorkPhoneNumber"] = this.loanData.GetSimpleField(fieldIds["WorkPhoneNumber"]);
      return contactDetails;
    }
  }
}
