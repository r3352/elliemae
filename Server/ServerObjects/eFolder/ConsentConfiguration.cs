// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.ConsentConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Properties;
using System;
using System.IO;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class ConsentConfiguration
  {
    private const string className = "ConsentConfiguration�";

    private ConsentConfiguration()
    {
    }

    public static ConsentSettings GetConsentSettings(UserInfo userInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      ConsentSettings consentSettings = new ConsentSettings();
      EDisclosureSetup setup = EDisclosureConfiguration.GetSetup();
      consentSettings.Model = !(setup.ConsentModelType == "Package level consent") ? ConsentModelType.Group : ConsentModelType.Package;
      consentSettings.UseBranchAddress = setup.UseBranchAddress;
      consentSettings.FulfillmentFee = Company.GetCompanySetting((IClientContext) current, "eDisclosures", "FulfillmentFee");
      consentSettings.Verbiage = ConsentConfiguration.buildConsentVerbiage(current, userInfo, consentSettings.UseBranchAddress, consentSettings.FulfillmentFee);
      return consentSettings;
    }

    private static string buildConsentVerbiage(
      ClientContext context,
      UserInfo userInfo,
      bool useBranchAddress,
      string fulfillmentFee)
    {
      OrgInfo orgInfo = (OrgInfo) null;
      if (useBranchAddress)
        orgInfo = OrganizationStore.GetFirstAvaliableOrganization(userInfo.OrgId);
      if (orgInfo == null)
      {
        CompanyInfo companyInfo = Company.GetCompanyInfo((IClientContext) context);
        orgInfo = new OrgInfo();
        orgInfo.CompanyName = companyInfo.Name;
        orgInfo.CompanyAddress.Street1 = companyInfo.Address;
        orgInfo.CompanyAddress.City = companyInfo.City;
        orgInfo.CompanyAddress.State = companyInfo.State;
        orgInfo.CompanyAddress.Zip = companyInfo.Zip;
        orgInfo.CompanyPhone = companyInfo.Phone;
        orgInfo.CompanyFax = companyInfo.Fax;
      }
      string str = ConsentConfiguration.concatString(ConsentConfiguration.concatString(ConsentConfiguration.concatString(ConsentConfiguration.concatString(ConsentConfiguration.concatString(string.Empty, string.Empty, orgInfo.CompanyAddress.Street1), " ", orgInfo.CompanyAddress.Street2), ", ", orgInfo.CompanyAddress.City), ", ", orgInfo.CompanyAddress.State), " ", orgInfo.CompanyAddress.Zip);
      string newValue1 = string.Empty;
      if (!string.IsNullOrWhiteSpace(orgInfo.CompanyPhone))
        newValue1 = HttpUtility.HtmlEncode("Phone: " + orgInfo.CompanyPhone.Trim());
      if (!string.IsNullOrWhiteSpace(str))
      {
        if (newValue1 != string.Empty)
          newValue1 += "<br />";
        newValue1 += HttpUtility.HtmlEncode("Mailing Address: " + str);
      }
      if (newValue1 == string.Empty)
        newValue1 = "Please Contact Your Loan Officer";
      string newValue2;
      string newValue3;
      if (string.IsNullOrEmpty(fulfillmentFee))
      {
        newValue2 = "You will not be required to pay a fee for receiving paper copies of the Loan Documents.";
        newValue3 = "You will not be required to pay a fee for withdrawing consent and receiving paper copies of the Loan Documents.";
      }
      else
      {
        newValue2 = "You will be required to pay a fee of $" + fulfillmentFee + " for such paper copies.";
        newValue3 = newValue2;
      }
      string path = Path.Combine(context.Settings.AppDataDir, "Consent.htm");
      return (!File.Exists(path) ? Resources.Consent : File.ReadAllText(path)).Replace("[EFFECTIVE_DATE]", DateTime.Today.ToLongDateString()).Replace("[ORIGINATOR_CONTACT]", newValue1).Replace("[ORIGINATOR_FULFILLMENT_FEE_RECEIVE_PAPER_COPY_TEXT]", newValue2).Replace("[ORIGINATOR_FULFILLMENT_FEE_WITHDRAW_CONSENT_RECEIVE_PAPER_COPY_TEXT]", newValue3);
    }

    private static string concatString(string original, string concat, string append)
    {
      if (string.IsNullOrEmpty(original))
        concat = string.Empty;
      return !string.IsNullOrWhiteSpace(append) ? original + concat + append.Trim() : original;
    }
  }
}
