// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ActivityContactInfo
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer.Calendar;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ActivityContactInfo : ContactInfo
  {
    public CampaignActivity Activity;

    private ActivityContactInfo()
    {
    }

    public ActivityContactInfo(CampaignActivity activity, CategoryType categoryType)
      : base(activity.ContactProperties["Contact.FirstName"].ToString() + " " + activity.ContactProperties["Contact.LastName"], activity.ContactId.ToString(), categoryType)
    {
      this.Activity = activity;
    }
  }
}
