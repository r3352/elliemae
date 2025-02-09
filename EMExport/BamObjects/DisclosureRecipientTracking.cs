// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.DisclosureRecipientTracking
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class DisclosureRecipientTracking
  {
    private DateTime _acceptConsentDate;
    private DateTime _eSignedDate;
    private DateTime _wetSignedDate;
    private DateTime _rejectConsentDate;
    private DateTime _viewConsentDate;
    private DateTime _viewMessageDate;
    private DateTime _authenticatedDate;
    private DateTime _viewESignedDate;
    private DateTime _viewWetSignedDate;
    private DateTime _informationalViewedDate;
    private DateTime _informationalCompletedDate;

    public DateTime AcceptConsentDate
    {
      get => this._acceptConsentDate;
      set => this._acceptConsentDate = value;
    }

    public DateTime ESignedDate
    {
      get => this._eSignedDate;
      set => this._eSignedDate = value;
    }

    public DateTime WetSignedDate
    {
      get => this._wetSignedDate;
      set => this._wetSignedDate = value;
    }

    public DateTime RejectConsentDate
    {
      get => this._rejectConsentDate;
      set => this._rejectConsentDate = value;
    }

    public DateTime ViewConsentDate
    {
      get => this._viewConsentDate;
      set => this._viewConsentDate = value;
    }

    public DateTime ViewMessageDate
    {
      get => this._viewMessageDate;
      set => this._viewMessageDate = value;
    }

    public DateTime AuthenticatedDate
    {
      get => this._authenticatedDate;
      set => this._authenticatedDate = value;
    }

    public string AuthenticatedIP { get; set; }

    public string AcceptConsentIP { get; set; }

    public string RejectConsentIP { get; set; }

    public string ESignedIP { get; set; }

    public string LoanLevelConsent { get; set; }

    public DateTime ViewESignedDate
    {
      get => this._viewESignedDate;
      set => this._viewESignedDate = value;
    }

    public DateTime ViewWetSignedDate
    {
      get => this._viewWetSignedDate;
      set => this._viewWetSignedDate = value;
    }

    public DateTime InformationalViewedDate
    {
      get => this._informationalViewedDate;
      set => this._informationalViewedDate = value;
    }

    public DateTime InformationalCompletedDate
    {
      get => this._informationalCompletedDate;
      set => this._informationalCompletedDate = value;
    }

    public string InformationalViewedIP { get; set; }

    public string InformationalCompletedIP { get; set; }

    public DisclosureRecipientTracking(
      DateTime acceptConsentDate,
      DateTime eSignedDate,
      DateTime wetSignedDate,
      DateTime rejectConsentDate,
      DateTime viewConsentDate,
      DateTime viewMessageDate,
      DateTime authenticatedDate,
      string authenticatedIP,
      string acceptConsentIP,
      string rejectConsentIP,
      string eSignedIP,
      string loanLevelConsent,
      DateTime viewESignedDate,
      DateTime viewWetSignedDate,
      DateTime informationalViewedDate = default (DateTime),
      string informationalViewedIP = "",
      DateTime informationalCompletedDate = default (DateTime),
      string informationalCompletedIP = "")
    {
      this.AcceptConsentDate = acceptConsentDate;
      this.ESignedDate = eSignedDate;
      this.WetSignedDate = wetSignedDate;
      this.RejectConsentDate = rejectConsentDate;
      this.ViewConsentDate = viewConsentDate;
      this.ViewMessageDate = viewMessageDate;
      this.AuthenticatedDate = authenticatedDate;
      this.AuthenticatedIP = authenticatedIP;
      this.AcceptConsentIP = acceptConsentIP;
      this.RejectConsentIP = rejectConsentIP;
      this.ESignedIP = eSignedIP;
      this.LoanLevelConsent = loanLevelConsent;
      this.ViewESignedDate = viewESignedDate;
      this.ViewWetSignedDate = viewWetSignedDate;
      this.InformationalViewedDate = informationalViewedDate;
      this.InformationalViewedIP = informationalViewedIP;
      this.InformationalCompletedDate = informationalCompletedDate;
      this.InformationalCompletedIP = informationalCompletedIP;
    }
  }
}
