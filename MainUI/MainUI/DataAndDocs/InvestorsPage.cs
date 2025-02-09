// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DataAndDocs.InvestorsPage
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI.DataAndDocs
{
  public class InvestorsPage : Form
  {
    private PartnerResponseBody partner;
    private DataDocsServiceHelper dataDocsServiceHelper;
    private List<Loan> loans;
    private bool submissionTypeExists;
    private PartnerPreferencesResponseBody partnerPreferences;
    private IContainer components;
    private WebBrowser webBrowserPartner;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public InvestorsPage(PartnerResponseBody partnerResponseBody, List<Loan> loans)
    {
      this.InitializeComponent();
      this.dataDocsServiceHelper = new DataDocsServiceHelper();
      this.webBrowserPartner.IsWebBrowserContextMenuEnabled = false;
      this.webBrowserPartner.AllowWebBrowserDrop = false;
      this.partner = partnerResponseBody;
      this.Navigate(partnerResponseBody.PartnerWebUIUrl);
      this.loans = loans;
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.DefaultInstance.ServerIdentity.InstanceName, Session.DefaultInstance.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
    }

    private void webBrowserPartner_DocumentCompleted(
      object sender,
      WebBrowserDocumentCompletedEventArgs e)
    {
      if (!this.isValidInvestorPage())
        return;
      this.submissionTypeExists = this.webBrowserPartner.Document.GetElementById("listSubmissionType") != (HtmlElement) null;
      this.ReauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (accessToken =>
      {
        this.partnerPreferences = this.dataDocsServiceHelper.GetPartnerPreferences(this.partner.PartnerID, this.partner.ProductName, this.partner.ActiveVersion, accessToken.TypeAndToken);
        if (this.submissionTypeExists)
        {
          this.RegisterEvents();
          this.PopulateSubmissionType();
          this.SubmissionTypes_SelectedIndexChanged((object) null, (EventArgs) null);
        }
        else
        {
          this.PopulateStackingTemplate();
          this.RegisterEvents();
        }
      }));
    }

    private void RegisterEvents()
    {
      this.webBrowserPartner.Document.GetElementById("btnSubmit").Click += new HtmlElementEventHandler(this.btnSumbit_Click);
      this.webBrowserPartner.Document.GetElementById("btnClose").Click += new HtmlElementEventHandler(this.btnClose_Click);
      if (!this.submissionTypeExists)
        return;
      this.webBrowserPartner.Document.GetElementById("listSubmissionType").AttachEventHandler("onchange", new EventHandler(this.SubmissionTypes_SelectedIndexChanged));
    }

    private bool isValidInvestorPage()
    {
      if (this.webBrowserPartner.Document != (HtmlDocument) null && this.webBrowserPartner.Document.GetElementById("btnSubmit") != (HtmlElement) null)
        return true;
      this.webBrowserPartner.Document.OpenNew(true).Write("<p><span style='color: red; text-align: left;'>There was a problem in loading the Investor Page. Please contact EllieMae support for further details.</span></p>");
      return false;
    }

    private void PopulateSubmissionType()
    {
      if (!this.submissionTypeExists)
        return;
      HtmlElement elementById = this.webBrowserPartner.Document.GetElementById("listSubmissionType");
      try
      {
        elementById.InnerText = "";
        if (this.partnerPreferences == null || this.partnerPreferences.DeliveryArtifacts == null)
          throw new NullReferenceException();
        List<string> list = this.partnerPreferences.DeliveryArtifacts.Select<DelivertArtifact, string>((Func<DelivertArtifact, string>) (deliveryArtifacts => deliveryArtifacts.SubmissionType)).ToList<string>();
        if (list.Count <= 0)
          throw new NullReferenceException();
        if (list.Count > 1)
        {
          HtmlElement element = this.webBrowserPartner.Document.CreateElement("option");
          element.SetAttribute("value", "");
          element.InnerHtml = "<option value=></option>";
          elementById.AppendChild(element);
        }
        else
          elementById.Enabled = false;
        foreach (DelivertArtifact deliveryArtifact in this.partnerPreferences.DeliveryArtifacts)
        {
          HtmlElement element = this.webBrowserPartner.Document.CreateElement("option");
          if (!string.IsNullOrWhiteSpace(deliveryArtifact.DocumentSet.StackingTemplate))
          {
            string str1 = deliveryArtifact.DocumentSet.StackingTemplate.ToLower().Trim();
            element.SetAttribute("value", str1);
            string str2 = "<option value=" + str1 + ">" + deliveryArtifact.SubmissionType + "</option>";
            element.InnerHtml = str2;
          }
          else
          {
            string str = "<option value=false>" + deliveryArtifact.SubmissionType + "</option>";
            element.InnerHtml = str;
          }
          elementById.AppendChild(element);
        }
      }
      catch
      {
        HtmlElement element = this.webBrowserPartner.Document.CreateElement("stError");
        element.InnerHtml = "<br><font id=submissionTypeError color='red' >Error retrieving Submission Type.Please contact ICE Mortgage Technology support for further details.</font>";
        elementById.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, element);
        this.webBrowserPartner.Document.GetElementById("btnSubmit").Enabled = false;
      }
    }

    private void PopulateStackingTemplate()
    {
      HtmlElement elementById = this.webBrowserPartner.Document.GetElementById("lstStackingTemplate");
      if (elementById == (HtmlElement) null)
        return;
      elementById.InnerText = "";
      List<StackingTemplatesResponseBody> stackingtemplatelist = (List<StackingTemplatesResponseBody>) null;
      this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
      {
        this.dataDocsServiceHelper = new DataDocsServiceHelper();
        stackingtemplatelist = this.dataDocsServiceHelper.GetStackingTemplates(accessToken.TypeAndToken);
      }));
      if (stackingtemplatelist != null)
      {
        foreach (StackingTemplatesResponseBody templatesResponseBody in stackingtemplatelist)
        {
          HtmlElement element = this.webBrowserPartner.Document.CreateElement("option");
          element.SetAttribute("value", templatesResponseBody.ID);
          element.InnerHtml = "<option value=" + templatesResponseBody.ID + ">" + templatesResponseBody.Name + "</option>";
          elementById.AppendChild(element);
        }
      }
      else
      {
        HtmlElement element = this.webBrowserPartner.Document.CreateElement("font");
        element.InnerHtml = "<font color='red' >*Error getting stacking Template</font>";
        elementById.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, element);
        this.webBrowserPartner.Document.GetElementById("btnSubmit").SetAttribute("disabled", "true");
      }
    }

    private void PopulateStackingTemplate(
      IReadOnlyCollection<StackingTemplatesResponseBody> stackingTemplateslist,
      HtmlElement elementStackingTemplate,
      bool isStackingTemplateRequired)
    {
      HtmlElement newElement = this.webBrowserPartner.Document.GetElementById("StackingTemplateError");
      if (newElement != (HtmlElement) null)
        newElement.InnerHtml = "";
      this.webBrowserPartner.Document.GetElementById("btnSubmit").Enabled = true;
      if (!isStackingTemplateRequired)
        elementStackingTemplate.Enabled = false;
      else if (stackingTemplateslist == null || stackingTemplateslist.Count == 0)
      {
        if (newElement == (HtmlElement) null)
        {
          newElement = this.webBrowserPartner.Document.CreateElement("Error");
          elementStackingTemplate.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, newElement);
        }
        newElement.InnerHtml = "<br><font id=StackingTemplateError color='red' >Error retrieving stacking template. Please contact ICE Mortgage Technology support for further details.</font>";
        this.webBrowserPartner.Document.GetElementById("btnSubmit").Enabled = false;
      }
      else
      {
        foreach (StackingTemplatesResponseBody templatesResponseBody in (IEnumerable<StackingTemplatesResponseBody>) stackingTemplateslist)
        {
          HtmlElement element = this.webBrowserPartner.Document.CreateElement("option");
          element.SetAttribute("value", templatesResponseBody.ID);
          element.InnerHtml = "<option value=" + templatesResponseBody.ID + ">" + templatesResponseBody.Name + "</option>";
          elementStackingTemplate.AppendChild(element);
        }
      }
    }

    private string GetSubmissionType()
    {
      if (!this.submissionTypeExists)
        return (string) null;
      HtmlElement elementById = this.webBrowserPartner.Document.GetElementById("listSubmissionType");
      string submissionType = string.Empty;
      object domElement = this.webBrowserPartner.Document.GetElementById("listSubmissionType").DomElement;
      // ISSUE: reference to a compiler-generated field
      if (InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (InvestorsPage)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target = InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "selectedIndex", (IEnumerable<System.Type>) null, typeof (InvestorsPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) InvestorsPage.\u003C\u003Eo__16.\u003C\u003Ep__0, domElement);
      int index = target((CallSite) p1, obj);
      if (index != -1)
        submissionType = elementById.Children[index].InnerText;
      return submissionType;
    }

    private void btnSumbit_Click(object sender, HtmlElementEventArgs e)
    {
      if (this.webBrowserPartner.Document == (HtmlDocument) null)
        return;
      try
      {
        this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
        {
          this.dataDocsServiceHelper = new DataDocsServiceHelper();
          List<CustomDataField> customDataFields = !(this.webBrowserPartner.Document.GetElementById("customFields") != (HtmlElement) null) || string.IsNullOrWhiteSpace(this.webBrowserPartner.Document.GetElementById("customFields").InnerText) ? (List<CustomDataField>) null : this.dataDocsServiceHelper.GetCustomDataFields(this.webBrowserPartner.Document.GetElementById("customFields").InnerText);
          string orderId = this.GetOrderID();
          string attribute1 = !(this.webBrowserPartner.Document.GetElementById("LoanFormat") != (HtmlElement) null) || string.IsNullOrWhiteSpace(this.webBrowserPartner.Document.GetElementById("LoanFormat").GetAttribute("value")) ? (string) null : this.webBrowserPartner.Document.GetElementById("LoanFormat").GetAttribute("value");
          string attribute2 = this.webBrowserPartner.Document.GetElementById("lstStackingTemplate") != (HtmlElement) null ? this.webBrowserPartner.Document.GetElementById("lstStackingTemplate").GetAttribute("value") : (string) null;
          string submissionType = this.GetSubmissionType();
          int result1;
          int result2;
          CreateInvestorPackageRequestBody createInvestorPackageRequestBody = new CreateInvestorPackageRequestBody()
          {
            provider = new Provider()
            {
              Id = this.partner.PartnerID,
              Name = this.partner.DisplayName,
              Category = this.partner.Category,
              CredentialStore = this.partnerPreferences == null ? "Company" : this.partnerPreferences.CredentialStore,
              Product = this.partner.ProductName,
              ConfigurationName = this.partner.ConfigurationName,
              ActiveVersion = int.TryParse(this.partner.ActiveVersion, out result1) ? result1 : 0,
              options = new Options()
              {
                OrderID = orderId,
                InvestorFields = this.GetInvestorFields()
              }
            },
            StackingTemplateId = int.TryParse(attribute2, out result2) ? new int?(result2) : new int?(),
            Format = attribute1,
            auditFields = (IList<string>) null,
            customDataFields = customDataFields,
            SubmissionType = submissionType,
            Loans = (IList<Loan>) this.loans
          };
          WebResponse investorPackage = this.dataDocsServiceHelper.CreateInvestorPackage(accessToken.TypeAndToken, createInvestorPackageRequestBody);
          RemoteLogger.Write(TraceLevel.Info, string.Format("Instance Id: {0}, Date / Time: {1}, User id: {2}, Package is submitted to partner: {3}, Package ID: {4}, X-Correlation-ID: {5}", (object) Session.CompanyInfo.ClientID, (object) DateTime.Now, (object) Session.UserID, (object) this.partner.ProviderName, (object) investorPackage.Headers["location"], (object) investorPackage.Headers["X-Correlation-ID"]));
          string[] strArray = investorPackage.Headers["location"].Split('/');
          int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Submission was successful. Transaction Reference {0}", (object) strArray[strArray.Length - 1]), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.Close();
        }));
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to process and package loan. Please contact EllieMae support for further details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private string GetOrderID()
    {
      HtmlElement elementById = this.webBrowserPartner.Document.GetElementById("txtOrder");
      return !(elementById != (HtmlElement) null) ? (string) null : elementById.GetAttribute("value");
    }

    private List<InvestorFields> GetInvestorFields()
    {
      HtmlElement elementById = this.webBrowserPartner.Document.GetElementById("divOptionsContainer");
      List<InvestorFields> investorFieldsList = new List<InvestorFields>();
      if (elementById != (HtmlElement) null)
      {
        HtmlElementCollection elementsByTagName = elementById.GetElementsByTagName("div");
        if (elementsByTagName != null)
        {
          for (int index = 0; index < elementsByTagName.Count; ++index)
          {
            if (!(elementsByTagName[index].Children[1].GetAttribute("id").ToLower() == "txtorder") && elementsByTagName[index].Children.Count == 2 && !(elementsByTagName[index].Children[1].GetAttribute("id").ToLower() == "txtorder"))
              investorFieldsList.Add(this.GetInvestorOption(elementsByTagName[index]));
          }
        }
      }
      return investorFieldsList.Count >= 0 ? investorFieldsList : (List<InvestorFields>) null;
    }

    private InvestorFields GetInvestorOption(HtmlElement optionElement)
    {
      InvestorFields investorOption = new InvestorFields();
      switch (optionElement.Children[1].TagName.ToLower())
      {
        case "input":
        case "select":
        case "textarea":
          investorOption.Key = optionElement.Children[0].OuterText;
          investorOption.Value = optionElement.Children[1].GetAttribute("value");
          break;
        case "form":
          investorOption.Key = optionElement.Children[0].OuterText;
          investorOption.Value = this.GetSelectedOptionFromFormElement(optionElement.Children[1]);
          break;
        default:
          investorOption.Key = optionElement.Children[0].OuterText;
          investorOption.Value = optionElement.Children[1].GetAttribute("value");
          break;
      }
      return investorOption;
    }

    private string GetSelectedOptionFromFormElement(HtmlElement formGroupElement)
    {
      switch (formGroupElement.Children[0].GetAttribute("type").ToLower())
      {
        case "radio":
          IEnumerator enumerator = formGroupElement.Children.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              HtmlElement current = (HtmlElement) enumerator.Current;
              if (current.TagName.ToLower() == "input" && Convert.ToBoolean(current.GetAttribute("checked")))
                return current.GetAttribute("value");
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        case "checkbox":
          List<string> values1 = new List<string>();
          foreach (HtmlElement child in formGroupElement.Children)
          {
            if (child.TagName.ToLower() == "input" && Convert.ToBoolean(child.GetAttribute("checked")))
              values1.Add(child.GetAttribute("value"));
          }
          if (values1.Count == 0)
            return string.Empty;
          if (values1.Count == 1)
            return values1[0];
          if (values1.Count > 1)
            return string.Join(",", (IEnumerable<string>) values1);
          break;
        case "text":
          List<string> values2 = new List<string>();
          foreach (HtmlElement child in formGroupElement.Children)
          {
            if (child.TagName.ToLower() == "input")
              values2.Add(child.GetAttribute("value"));
          }
          if (values2.Count == 0)
            return string.Empty;
          if (values2.Count == 1)
            return values2[0];
          if (values2.Count > 1)
            return string.Join(",", (IEnumerable<string>) values2);
          break;
        default:
          return string.Empty;
      }
      return string.Empty;
    }

    private void Navigate(string address)
    {
      if (string.IsNullOrEmpty(address) || address.Equals("about:blank"))
        return;
      if (!address.StartsWith("http://") && !address.StartsWith("https://"))
        address = "http://" + address;
      try
      {
        this.webBrowserPartner.Navigate(new Uri(address));
      }
      catch (UriFormatException ex)
      {
        this.webBrowserPartner.DocumentText = "";
      }
    }

    private void btnClose_Click(object sender, HtmlElementEventArgs e) => this.Close();

    private void SubmissionTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      HtmlElement elementById1 = this.webBrowserPartner.Document.GetElementById("lstStackingTemplate");
      HtmlElement elementById2 = this.webBrowserPartner.Document.GetElementById("listSubmissionType");
      IReadOnlyCollection<StackingTemplatesResponseBody> stackingTemplates = (IReadOnlyCollection<StackingTemplatesResponseBody>) null;
      elementById1.InnerHtml = "";
      elementById1.Enabled = true;
      object domElement = elementById2.DomElement;
      // ISSUE: reference to a compiler-generated field
      if (InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (InvestorsPage)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target = InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "selectedIndex", (IEnumerable<System.Type>) null, typeof (InvestorsPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) InvestorsPage.\u003C\u003Eo__24.\u003C\u003Ep__0, domElement);
      int index = target((CallSite) p1, obj);
      if (index == -1 || elementById2.Children[index].InnerText == null)
      {
        this.webBrowserPartner.Document.GetElementById("btnSubmit").Enabled = false;
      }
      else
      {
        bool result;
        bool.TryParse(elementById2.GetAttribute("value"), out result);
        if (result)
          this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken => stackingTemplates = (IReadOnlyCollection<StackingTemplatesResponseBody>) this.dataDocsServiceHelper.GetStackingTemplates(accessToken.TypeAndToken)));
        this.PopulateStackingTemplate(stackingTemplates, elementById1, result);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowserPartner = new WebBrowser();
      this.SuspendLayout();
      this.webBrowserPartner.Dock = DockStyle.Fill;
      this.webBrowserPartner.Location = new Point(0, 0);
      this.webBrowserPartner.MinimumSize = new Size(20, 20);
      this.webBrowserPartner.Name = "webBrowserPartner";
      this.webBrowserPartner.Size = new Size(601, 613);
      this.webBrowserPartner.TabIndex = 0;
      this.webBrowserPartner.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.webBrowserPartner_DocumentCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(601, 613);
      this.Controls.Add((Control) this.webBrowserPartner);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (InvestorsPage);
      this.Text = "Investors Page";
      this.ResumeLayout(false);
    }
  }
}
