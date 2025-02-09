// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.ServerUtil
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public static class ServerUtil
  {
    public static string GetScHeaderToken()
    {
      return "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ";
    }

    public static string Truncate(string source, int start, int end)
    {
      return start > end || string.IsNullOrWhiteSpace(source) || source.Length <= end - start ? source : source.Substring(start, end);
    }

    public static bool ValidateExternalUserInfoData(
      ExternalUserInfo externalUserInfo,
      out string validationMessage)
    {
      if ((UserInfo) externalUserInfo == (UserInfo) null)
      {
        validationMessage = "Invalid ExternalUserInfo object";
        return false;
      }
      StringBuilder stringBuilder = new StringBuilder("");
      ClientContext current = ClientContext.GetCurrent();
      DbTableInfo dbTableInfo1 = current.Cache.Get<DbTableInfo>("users_DbTableMetadata", (Func<DbTableInfo>) (() => EllieMae.EMLite.Server.DbAccessManager.GetTable("users")));
      if (!Utils.isValidLength(externalUserInfo.ContactID, dbTableInfo1["userid"].MaxLength, allowNull: false))
        stringBuilder.Append("Userid, ");
      if (!Utils.isValidLength(externalUserInfo.LastName, dbTableInfo1["last_name"].MaxLength))
        stringBuilder.Append("LastName, ");
      if (!Utils.isValidLength(externalUserInfo.FirstName, dbTableInfo1["first_name"].MaxLength))
        stringBuilder.Append("FirstName, ");
      if (!Utils.isValidLength(externalUserInfo.MiddleName, dbTableInfo1["middle_name"].MaxLength))
        stringBuilder.Append("MiddleName, ");
      if (!Utils.isValidLength(externalUserInfo.Suffix, dbTableInfo1["suffix_name"].MaxLength))
        stringBuilder.Append("Suffix, ");
      if (!Utils.isValidLength(externalUserInfo.Email, dbTableInfo1["email"].MaxLength) || !string.IsNullOrWhiteSpace(externalUserInfo.Email) && !Utils.ValidateEmail(externalUserInfo.Email))
        stringBuilder.Append("Email, ");
      if (!Utils.isValidLength(externalUserInfo.Phone, dbTableInfo1["phone"].MaxLength))
        stringBuilder.Append("Phone, ");
      if (!Utils.isValidLength(externalUserInfo.Fax, dbTableInfo1["fax"].MaxLength))
        stringBuilder.Append("Fax, ");
      if (!Utils.isValidLength(externalUserInfo.CellPhone, dbTableInfo1["cell_phone"].MaxLength))
        stringBuilder.Append("CellPhone, ");
      if (!Utils.isValidLength(externalUserInfo.NmlsID, dbTableInfo1["nmlsOriginatorID"].MaxLength))
        stringBuilder.Append("NmlsID, ");
      DbTableInfo dbTableInfo2 = current.Cache.Get<DbTableInfo>("ExternalUsers_DbTableMetadata", (Func<DbTableInfo>) (() => EllieMae.EMLite.Server.DbAccessManager.GetTable("ExternalUsers")));
      if (!Utils.isValidLength(externalUserInfo.Title, dbTableInfo2["Title"].MaxLength))
        stringBuilder.Append("Title, ");
      if (!Utils.isValidLength(externalUserInfo.Address, dbTableInfo2["Address1"].MaxLength))
        stringBuilder.Append("Address, ");
      if (!Utils.isValidLength(externalUserInfo.City, dbTableInfo2["City"].MaxLength))
        stringBuilder.Append("City, ");
      if (!Utils.isValidLength(externalUserInfo.State, dbTableInfo2["State"].MaxLength))
        stringBuilder.Append("State, ");
      if (!Utils.isValidLength(externalUserInfo.Zipcode, dbTableInfo2["Zip"].MaxLength))
        stringBuilder.Append("Zipcode, ");
      if (!Utils.isValidLength(externalUserInfo.SSN, dbTableInfo2["SSN"].MaxLength))
        stringBuilder.Append("SSN, ");
      if (!Utils.isValidLength(externalUserInfo.EmailForRateSheet, dbTableInfo2["Rate_sheet_email"].MaxLength) || !string.IsNullOrWhiteSpace(externalUserInfo.EmailForRateSheet) && !Utils.ValidateEmail(externalUserInfo.EmailForRateSheet))
        stringBuilder.Append("EmailForRateSheet, ");
      if (!Utils.isValidLength(externalUserInfo.FaxForRateSheet, dbTableInfo2["Rate_sheet_fax"].MaxLength))
        stringBuilder.Append("FaxForRateSheet, ");
      if (!Utils.isValidLength(externalUserInfo.EmailForLockInfo, dbTableInfo2["Lock_info_email"].MaxLength) || !string.IsNullOrWhiteSpace(externalUserInfo.EmailForLockInfo) && !Utils.ValidateEmail(externalUserInfo.EmailForLockInfo))
        stringBuilder.Append("EmailForLockInfo, ");
      if (!Utils.isValidLength(externalUserInfo.FaxForLockInfo, dbTableInfo2["Lock_info_fax"].MaxLength))
        stringBuilder.Append("FaxForLockInfo, ");
      if (!Utils.isValidLength(externalUserInfo.EmailForLogin, dbTableInfo2["Login_email"].MaxLength, allowNull: false) || !string.IsNullOrWhiteSpace(externalUserInfo.EmailForLogin) && !Utils.ValidateEmail(externalUserInfo.EmailForLogin))
        stringBuilder.Append("EmailForLogin, ");
      if (!Utils.isValidLength(externalUserInfo.WelcomeEmailUserName, dbTableInfo2["Welcome_Email_By"].MaxLength))
        stringBuilder.Append("WelcomeEmailUserName, ");
      if (!Utils.isValidLength(externalUserInfo.SalesRepID, dbTableInfo2["Sales_rep_userid"].MaxLength, allowNull: false))
        stringBuilder.Append("SalesRepID, ");
      if (!Utils.isValidLength(externalUserInfo.Notes, dbTableInfo2["Note"].MaxLength))
        stringBuilder.Append("Notes, ");
      if (!Utils.isValidLength(externalUserInfo.UpdatedBy, dbTableInfo2["UpdatedBy"].MaxLength))
        stringBuilder.Append("UpdatedBy, ");
      if (stringBuilder.Length > 0)
      {
        stringBuilder.Append(",").Replace(", ,", "");
        validationMessage = "ValidateExternalUserInfoData - Following ExternalUserInfo fields length are invalid - " + stringBuilder.ToString();
        return false;
      }
      validationMessage = "";
      return true;
    }
  }
}
