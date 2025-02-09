// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.FileContacts
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  public class FileContacts
  {
    public Dictionary<string, int> ContactTypes;
    protected LoanData loanData;

    public FileContacts(LoanData loanData)
    {
      this.loanData = loanData;
      this.ContactTypes = new Dictionary<string, int>();
      this.ContactTypes.Add("Borrower", 0);
      this.ContactTypes.Add("Coborrower", 0);
      this.ContactTypes.Add("Loan Officer", 0);
      this.ContactTypes.Add("Loan Processor", 0);
      this.ContactTypes.Add("Lender", 0);
      this.ContactTypes.Add("Appraiser", 0);
      this.ContactTypes.Add("Escrow Company", 0);
      this.ContactTypes.Add("Title Insurance", 0);
      this.ContactTypes.Add("Buyer's Attorney", 0);
      this.ContactTypes.Add("Seller's Attorney", 0);
      this.ContactTypes.Add("Buyer's Agent", 0);
      this.ContactTypes.Add("Seller's Agent", 0);
      this.ContactTypes.Add("Seller", 0);
      this.ContactTypes.Add("Seller2", 0);
      this.ContactTypes.Add("Seller3", 0);
      this.ContactTypes.Add("Seller4", 0);
      this.ContactTypes.Add("Builder", 0);
      this.ContactTypes.Add("Hazard Insurance", 0);
      this.ContactTypes.Add("Mortgage Insurance", 0);
      this.ContactTypes.Add("Surveyor", 0);
      this.ContactTypes.Add("Flood Insurance", 0);
      this.ContactTypes.Add("Credit Company", 0);
      this.ContactTypes.Add("Underwriter", 0);
      this.ContactTypes.Add("Servicing", 0);
      this.ContactTypes.Add("Doc Signing", 0);
      this.ContactTypes.Add("Warehouse", 0);
      this.ContactTypes.Add("Financial Planner", 0);
      this.ContactTypes.Add("Investor", 0);
      this.ContactTypes.Add("Assign To", 0);
      this.ContactTypes.Add("Broker", 0);
      this.ContactTypes.Add("Notary", 0);
      this.ContactTypes.Add("Settlement Agent", 0);
      this.ContactTypes.Add("Docs Prepared By", 0);
      this.ContactTypes.Add("Custom Category #1", 0);
      this.ContactTypes.Add("Custom Category #2", 0);
      this.ContactTypes.Add("Custom Category #3", 0);
      this.ContactTypes.Add("Custom Category #4", 0);
    }

    protected virtual Dictionary<string, string> getFieldIds(string contactType)
    {
      Dictionary<string, string> fieldIds = new Dictionary<string, string>();
      switch (contactType)
      {
        case "Appraiser":
          fieldIds.Add("Email", "89");
          fieldIds.Add("Name", "618");
          break;
        case "Assign To":
          fieldIds.Add("Email", "VEND.X288");
          fieldIds.Add("Name", "VEND.X286");
          break;
        case "Borrower":
          fieldIds.Add("Email", "1240");
          fieldIds.Add("Name", "4000;4001;4002;4003");
          break;
        case "Broker":
          fieldIds.Add("Email", "VEND.X305");
          fieldIds.Add("Name", "VEND.X302");
          break;
        case "Builder":
          fieldIds.Add("Email", "94");
          fieldIds.Add("Name", "714");
          break;
        case "Buyer's Agent":
          fieldIds.Add("Email", "VEND.X141");
          fieldIds.Add("Name", "VEND.X139");
          break;
        case "Buyer's Attorney":
          fieldIds.Add("Email", "VEND.X119");
          fieldIds.Add("Name", "VEND.X117");
          break;
        case "Coborrower":
          fieldIds.Add("Email", "1268");
          fieldIds.Add("Name", "4004;4005;4006;4007");
          break;
        case "Credit Company":
          fieldIds.Add("Email", "90");
          fieldIds.Add("Name", "625");
          break;
        case "Custom Category #1":
          fieldIds.Add("Email", "VEND.X63");
          fieldIds.Add("Name", "VEND.X55");
          break;
        case "Custom Category #2":
          fieldIds.Add("Email", "VEND.X73");
          fieldIds.Add("Name", "VEND.X65");
          break;
        case "Custom Category #3":
          fieldIds.Add("Email", "VEND.X83");
          fieldIds.Add("Name", "VEND.X75");
          break;
        case "Custom Category #4":
          fieldIds.Add("Email", "VEND.X10");
          fieldIds.Add("Name", "VEND.X2");
          break;
        case "Doc Signing":
          fieldIds.Add("Email", "VEND.X197");
          fieldIds.Add("Name", "VEND.X195");
          break;
        case "Docs Prepared By":
          fieldIds.Add("Email", "VEND.X319");
          fieldIds.Add("Name", "VEND.X317");
          break;
        case "Escrow Company":
          fieldIds.Add("Email", "87");
          fieldIds.Add("Name", "611");
          break;
        case "Financial Planner":
          fieldIds.Add("Email", "VEND.X53");
          fieldIds.Add("Name", "VEND.X45");
          break;
        case "Flood Insurance":
          fieldIds.Add("Email", "VEND.X21");
          fieldIds.Add("Name", "VEND.X13");
          break;
        case "Hazard Insurance":
          fieldIds.Add("Email", "VEND.X164");
          fieldIds.Add("Name", "VEND.X162");
          break;
        case "Investor":
          fieldIds.Add("Email", "VEND.X273");
          fieldIds.Add("Name", "VEND.X271");
          break;
        case "Lender":
          fieldIds.Add("Email", "95");
          fieldIds.Add("Name", "1256");
          break;
        case "Loan Officer":
          fieldIds.Add("Email", "1407");
          fieldIds.Add("Name", "317");
          break;
        case "Loan Processor":
          fieldIds.Add("Email", "1409");
          fieldIds.Add("Name", "362");
          break;
        case "Mortgage Insurance":
          fieldIds.Add("Email", "93");
          fieldIds.Add("Name", "707");
          break;
        case "Notary":
          fieldIds.Add("Email", "VEND.X432");
          fieldIds.Add("Name", "VEND.X429");
          break;
        case "Seller":
          fieldIds.Add("Email", "92");
          fieldIds.Add("Name", "638");
          break;
        case "Seller's Agent":
          fieldIds.Add("Email", "VEND.X152");
          fieldIds.Add("Name", "VEND.X150");
          break;
        case "Seller's Attorney":
          fieldIds.Add("Email", "VEND.X130");
          fieldIds.Add("Name", "VEND.X128");
          break;
        case "Seller2":
          fieldIds.Add("Email", "VEND.X419");
          fieldIds.Add("Name", "VEND.X412");
          break;
        case "Seller3":
          fieldIds.Add("Email", "Seller3.Email");
          fieldIds.Add("Name", "Seller3.Name");
          break;
        case "Seller4":
          fieldIds.Add("Email", "Seller4.Email");
          fieldIds.Add("Name", "Seller4.Name");
          break;
        case "Servicing":
          fieldIds.Add("Email", "VEND.X186");
          fieldIds.Add("Name", "VEND.X184");
          break;
        case "Settlement Agent":
          fieldIds.Add("Email", "VEND.X670");
          fieldIds.Add("Name", "VEND.X668");
          break;
        case "Surveyor":
          fieldIds.Add("Email", "VEND.X43");
          fieldIds.Add("Name", "VEND.X35");
          break;
        case "Title Insurance":
          fieldIds.Add("Email", "88");
          fieldIds.Add("Name", "416");
          break;
        case "Underwriter":
          fieldIds.Add("Email", "1411");
          fieldIds.Add("Name", "984");
          break;
        case "Warehouse":
          fieldIds.Add("Email", "VEND.X208");
          fieldIds.Add("Name", "VEND.X206");
          break;
      }
      return fieldIds;
    }

    public virtual Dictionary<string, string> GetContactDetails(string contactType)
    {
      Dictionary<string, string> contactDetails = new Dictionary<string, string>();
      contactDetails.Add("Name", string.Empty);
      contactDetails.Add("Email", string.Empty);
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
              return contactDetails;
            }
            if (hashtable.ContainsKey((object) allMilestone.RoleID) && ((RoleSummaryInfo) hashtable[(object) allMilestone.RoleID]).RoleName == contactType)
            {
              contactDetails["Name"] = allMilestone.LoanAssociateName;
              contactDetails["Email"] = allMilestone.LoanAssociateEmail;
              return contactDetails;
            }
            if (allMilestone.RoleName == contactType)
            {
              contactDetails["Name"] = allMilestone.LoanAssociateName;
              contactDetails["Email"] = allMilestone.LoanAssociateEmail;
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
      return contactDetails;
    }

    public List<Tuple<string, string, string>> GetRoles()
    {
      List<Tuple<string, string, string>> roles = new List<Tuple<string, string, string>>();
      Hashtable hashtable1 = new Hashtable();
      RoleInfo[] allRoles = Session.LoanDataMgr.SystemConfiguration.AllRoles;
      for (int index = 0; index < allRoles.Length; ++index)
      {
        if (!hashtable1.ContainsKey((object) allRoles[index].RoleID))
          hashtable1.Add((object) allRoles[index].RoleID, (object) allRoles[index]);
      }
      LogList logList = this.loanData.GetLogList();
      if (logList != null)
      {
        Hashtable hashtable2 = new Hashtable();
        foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
        {
          if (allMilestone.RoleID != -1 || !((allMilestone.RoleName ?? "") == ""))
          {
            hashtable2[(object) allMilestone.RoleID] = (object) allMilestone.RoleName;
            string str = string.Compare(allMilestone.Stage, "Started", true) != 0 ? (!hashtable1.ContainsKey((object) allMilestone.RoleID) ? allMilestone.RoleName + " (" + allMilestone.Stage + ")" : ((RoleSummaryInfo) hashtable1[(object) allMilestone.RoleID]).RoleName + " (" + allMilestone.Stage + ")") : "File Starter";
            if (allMilestone.LoanAssociateID != null && allMilestone.LoanAssociateType != LoanAssociateType.Group)
              roles.Add(new Tuple<string, string, string>(str, allMilestone.LoanAssociateName, allMilestone.LoanAssociateEmail));
          }
        }
        ArrayList arrayList = new ArrayList();
        foreach (RoleInfo roleInfo in (IEnumerable) hashtable1.Values)
        {
          if (roleInfo.RoleID != RoleInfo.FileStarter.RoleID && !hashtable2.Contains((object) roleInfo.RoleID))
            arrayList.Add((object) roleInfo);
        }
        arrayList.Sort();
        foreach (RoleInfo roleInfo in arrayList)
        {
          string str = "Role - " + roleInfo.RoleName;
          LoanAssociateLog loanAssociateLog = (LoanAssociateLog) null;
          LoanAssociateLog[] assignedAssociates = logList.GetAssignedAssociates(roleInfo.RoleID);
          if (assignedAssociates != null && assignedAssociates.Length != 0)
          {
            loanAssociateLog = assignedAssociates[0];
          }
          else
          {
            LoanAssociateLog[] allLoanAssociates = logList.GetAllLoanAssociates();
            if (allLoanAssociates != null)
            {
              for (int index = 0; index < allLoanAssociates.Length; ++index)
              {
                if (allLoanAssociates[index].RoleID == roleInfo.RoleID)
                {
                  loanAssociateLog = allLoanAssociates[index];
                  break;
                }
              }
            }
          }
          if (loanAssociateLog == null)
          {
            MilestoneFreeRoleLog rec = new MilestoneFreeRoleLog();
            rec.RoleID = roleInfo.RoleID;
            rec.RoleName = roleInfo.RoleName;
            rec.MarkAsClean();
            logList.AddRecord((LogRecordBase) rec, false);
            loanAssociateLog = (LoanAssociateLog) rec;
          }
          if (loanAssociateLog.LoanAssociateID != null && loanAssociateLog.LoanAssociateType != LoanAssociateType.Group)
            roles.Add(new Tuple<string, string, string>(str, loanAssociateLog.LoanAssociateName, loanAssociateLog.LoanAssociateEmail));
        }
      }
      return roles;
    }

    public bool UpdateContactDetails(Dictionary<string, string> contactDetails)
    {
      try
      {
        if (contactDetails["ContactType"] == "Originator")
        {
          UserInfo user = Session.OrganizationManager.GetUser(this.loanData.GetSimpleField("3239"));
          if (user != (UserInfo) null)
          {
            if (contactDetails.ContainsKey("Email"))
              user.Email = contactDetails["Email"];
            if (contactDetails.ContainsKey("Name"))
            {
              if (((IEnumerable<string>) contactDetails["Name"].Split(' ')).Count<string>() > 1)
              {
                string str = ((IEnumerable<string>) contactDetails["Name"].Split(' ')).ToList<string>().LastOrDefault<string>();
                user.LastName = str;
                user.FirstName = contactDetails["Name"].Replace(" " + str, string.Empty);
              }
              else
                user.FirstName = contactDetails["Name"];
            }
            if (contactDetails.ContainsKey("CellPhoneNumber"))
              user.CellPhone = contactDetails["CellPhoneNumber"];
            Session.OrganizationManager.UpdateUser(user);
          }
        }
        else if (contactDetails["ContactType"] == "Loan Officer" || contactDetails["ContactType"] == "Loan Processor" || contactDetails["ContactType"] == "File Starter")
        {
          Hashtable hashtable = new Hashtable();
          LogList logList = this.loanData.GetLogList();
          if (logList == null)
            return false;
          foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
          {
            if (allMilestone.RoleID != -1 || !((allMilestone.RoleName ?? "") == ""))
            {
              if (allMilestone.Stage == "Started" && contactDetails["ContactType"] == "File Starter")
              {
                string loanAssociateId = allMilestone.LoanAssociateID;
                string email = allMilestone.LoanAssociateEmail;
                string fullName = allMilestone.LoanAssociateName;
                string loanAssociateTitle = allMilestone.LoanAssociateTitle;
                string loanAssociatePhone = allMilestone.LoanAssociatePhone;
                string cellPhone = allMilestone.LoanAssociateCellPhone;
                string loanAssociateFax = allMilestone.LoanAssociateFax;
                if (contactDetails.ContainsKey("Name"))
                  fullName = contactDetails["Name"];
                if (contactDetails.ContainsKey("Email"))
                  email = contactDetails["Email"];
                if (contactDetails.ContainsKey("CellPhoneNumber"))
                  cellPhone = contactDetails["CellPhoneNumber"];
                allMilestone.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, cellPhone, loanAssociateFax, loanAssociateTitle);
              }
              else if (hashtable.ContainsKey((object) allMilestone.RoleID) && ((RoleSummaryInfo) hashtable[(object) allMilestone.RoleID]).RoleName == contactDetails["ContactType"])
              {
                string loanAssociateId = allMilestone.LoanAssociateID;
                string email = allMilestone.LoanAssociateEmail;
                string fullName = allMilestone.LoanAssociateName;
                string loanAssociateTitle = allMilestone.LoanAssociateTitle;
                string loanAssociatePhone = allMilestone.LoanAssociatePhone;
                string cellPhone = allMilestone.LoanAssociateCellPhone;
                string loanAssociateFax = allMilestone.LoanAssociateFax;
                if (contactDetails.ContainsKey("Name"))
                  fullName = contactDetails["Name"];
                if (contactDetails.ContainsKey("Email"))
                  email = contactDetails["Email"];
                if (contactDetails.ContainsKey("CellPhoneNumber"))
                  cellPhone = contactDetails["CellPhoneNumber"];
                allMilestone.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, cellPhone, loanAssociateFax, loanAssociateTitle);
              }
              else if (allMilestone.RoleName == contactDetails["ContactType"])
              {
                string loanAssociateId = allMilestone.LoanAssociateID;
                string email = allMilestone.LoanAssociateEmail;
                string fullName = allMilestone.LoanAssociateName;
                string loanAssociateTitle = allMilestone.LoanAssociateTitle;
                string loanAssociatePhone = allMilestone.LoanAssociatePhone;
                string cellPhone = allMilestone.LoanAssociateCellPhone;
                string loanAssociateFax = allMilestone.LoanAssociateFax;
                if (contactDetails.ContainsKey("Name"))
                  fullName = contactDetails["Name"];
                if (contactDetails.ContainsKey("Email"))
                  email = contactDetails["Email"];
                if (contactDetails.ContainsKey("CellPhoneNumber"))
                  cellPhone = contactDetails["CellPhoneNumber"];
                allMilestone.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, cellPhone, loanAssociateFax, loanAssociateTitle);
              }
            }
          }
          foreach (MilestoneFreeRoleLog milestoneFreeRole in this.loanData.GetLogList().GetAllMilestoneFreeRoles())
          {
            LoanAssociateLog loanAssociateLog = (LoanAssociateLog) milestoneFreeRole;
            if (milestoneFreeRole.RoleName == contactDetails["ContactType"])
            {
              string loanAssociateId = loanAssociateLog.LoanAssociateID;
              string email = loanAssociateLog.LoanAssociateEmail;
              string fullName = loanAssociateLog.LoanAssociateName;
              string loanAssociateTitle = loanAssociateLog.LoanAssociateTitle;
              string loanAssociatePhone = loanAssociateLog.LoanAssociatePhone;
              string cellPhone = loanAssociateLog.LoanAssociateCellPhone;
              string loanAssociateFax = loanAssociateLog.LoanAssociateFax;
              if (contactDetails.ContainsKey("Name"))
                fullName = contactDetails["Name"];
              if (contactDetails.ContainsKey("Email"))
                email = contactDetails["Email"];
              if (contactDetails.ContainsKey("CellPhoneNumber"))
                cellPhone = contactDetails["CellPhoneNumber"];
              loanAssociateLog.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, cellPhone, loanAssociateFax, loanAssociateTitle);
            }
          }
        }
        else if (contactDetails["ContactType"] == "Non-Borrowing Owner")
        {
          if (contactDetails.ContainsKey("NboIndex"))
          {
            int num = Utils.ParseInt((object) contactDetails["NboIndex"]);
            string str = "NBOC" + (num <= 99 ? num.ToString("00") : num.ToString("000"));
            if (contactDetails.ContainsKey("Name"))
            {
              if (((IEnumerable<string>) contactDetails["Name"].Split(' ')).Count<string>() > 1)
              {
                string val = ((IEnumerable<string>) contactDetails["Name"].Split(' ')).ToList<string>().LastOrDefault<string>();
                this.loanData.SetField(str + "01", contactDetails["Name"].Replace(" " + val, string.Empty));
                this.loanData.SetField(str + "03", val);
              }
              else
                this.loanData.SetField(str + "01", contactDetails["Name"]);
            }
            if (contactDetails.ContainsKey("Email"))
            {
              string contactDetail = contactDetails["Email"];
              this.loanData.SetField(str + "11", contactDetails["Email"]);
            }
            if (contactDetails.ContainsKey("CellPhoneNumber"))
              this.loanData.SetField(str + "14", contactDetails["CellPhoneNumber"]);
          }
        }
        else
        {
          int pairIndex = this.loanData.GetPairIndex(this.loanData.PairId);
          BorrowerPair pair = (BorrowerPair) null;
          if (contactDetails.ContainsKey("BorrowerId"))
            pair = ((IEnumerable<BorrowerPair>) this.loanData.GetBorrowerPairs()).ToList<BorrowerPair>().FirstOrDefault<BorrowerPair>((Func<BorrowerPair, bool>) (x => x.Borrower.Id == contactDetails["BorrowerId"] || x.CoBorrower.Id == contactDetails["BorrowerId"]));
          try
          {
            if (pair != null)
              this.loanData.SetBorrowerPair(pair);
            Dictionary<string, string> fieldIds = this.getFieldIds(contactDetails["ContactType"]);
            if (contactDetails.ContainsKey("Email"))
              this.loanData.SetField(fieldIds["Email"], contactDetails["Email"]);
            if (contactDetails.ContainsKey("Name"))
            {
              if (contactDetails["ContactType"] == "Borrower" || contactDetails["ContactType"] == "Coborrower")
              {
                string[] strArray = fieldIds["Name"].Split(';');
                if (((IEnumerable<string>) contactDetails["Name"].Split(' ')).Count<string>() > 1)
                {
                  string val = ((IEnumerable<string>) contactDetails["Name"].Split(' ')).ToList<string>().LastOrDefault<string>();
                  this.loanData.SetField(strArray[0], contactDetails["Name"].Replace(" " + val, string.Empty));
                  this.loanData.SetField(strArray[1], val);
                }
                else
                  this.loanData.SetField(strArray[0], contactDetails["Name"]);
              }
              else
                this.loanData.SetField(fieldIds["Name"], contactDetails["Name"]);
            }
            if (contactDetails.ContainsKey("CellPhoneNumber"))
              this.loanData.SetField(fieldIds["CellPhoneNumber"], contactDetails["CellPhoneNumber"]);
          }
          finally
          {
            if (pair != null && pair.Id != pairIndex.ToString())
              this.loanData.SetBorrowerPair(pairIndex);
          }
        }
        Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
