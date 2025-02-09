// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlFieldMerge
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlFieldMerge
  {
    private const string className = "HtmlFieldMerge";
    private static readonly string sw = Tracing.SwInputEngine;
    private IHTMLDocument3 htmlDoc3;
    private string signAndReturnList = string.Empty;
    private string informationalList = string.Empty;
    private string neededList = string.Empty;
    private string authenticationUser = string.Empty;
    private string authenticationCode = string.Empty;

    public HtmlFieldMerge(string html)
    {
      HTMLDocumentClass htmlDocumentClass = new HTMLDocumentClass();
      IHTMLDocument2 htmlDocument2 = (IHTMLDocument2) htmlDocumentClass;
      htmlDocument2.write((object) html);
      htmlDocument2.close();
      this.htmlDoc3 = (IHTMLDocument3) htmlDocumentClass;
    }

    public string SignAndReturnList
    {
      set => this.signAndReturnList = value;
    }

    public string NeededList
    {
      set => this.neededList = value;
    }

    public string InformationalList
    {
      set => this.informationalList = value;
    }

    public string AuthenticationUser
    {
      set => this.authenticationUser = value;
    }

    public string AuthenticationCode
    {
      set => this.authenticationCode = value;
    }

    public string MergeContent(LoanData loanData) => this.MergeContent(loanData, Session.UserInfo);

    public string MergeContent(LoanData loanData, UserInfo userInfo)
    {
      foreach (IHTMLElement elm in this.htmlDoc3.getElementsByTagName("label"))
      {
        string attribute = (string) elm.getAttribute("emid", 2);
        if (!string.IsNullOrEmpty(attribute) && attribute == "Signature")
          this.setFieldSignature(elm, userInfo);
      }
      foreach (IHTMLElement elm in this.htmlDoc3.getElementsByTagName("label"))
      {
        string attribute = (string) elm.getAttribute("emid", 2);
        if (!string.IsNullOrEmpty(attribute))
        {
          switch (attribute)
          {
            case "CurrentUserName":
              this.setFieldText(elm, Session.UserInfo.FullName);
              continue;
            case "CurrentUserEmail":
              this.setFieldText(elm, Session.UserInfo.Email);
              continue;
            case "CurrentUserPhone":
              this.setFieldText(elm, Session.UserInfo.Phone);
              continue;
            case "Informational Documents":
              this.setFieldText(elm, this.informationalList);
              continue;
            case "Sign and Return Documents":
              this.setFieldText(elm, this.signAndReturnList);
              continue;
            case "Needed Documents":
              this.setFieldText(elm, this.neededList);
              continue;
            case "AuthenticationUser":
              this.setFieldText(elm, this.authenticationUser);
              continue;
            case "AuthenticationCode":
              this.setFieldText(elm, this.authenticationCode);
              continue;
            default:
              if (attribute != "Recipient Full Name")
              {
                this.setFieldText(elm, loanData.GetField(attribute));
                continue;
              }
              continue;
          }
        }
      }
      return this.htmlDoc3.documentElement.outerHTML;
    }

    public static string MergeDynamicConsumerConnectContent(
      string html,
      string recipientUrl,
      string recipientName)
    {
      if (FieldRegExMatch.RecipientFullName.IsMatch(html))
      {
        html = FieldRegExMatch.RecipientFullName.Replace(html, recipientName);
        html = FieldRegExMatch.RecipientFullNameStyle.Replace(html, string.Empty);
      }
      StringBuilder stringBuilder = new StringBuilder(html);
      stringBuilder.Replace("https://webcenter/", !string.IsNullOrEmpty(recipientUrl) ? recipientUrl : "https://webcenter/").Replace("[[URL]]", !string.IsNullOrEmpty(recipientUrl) ? recipientUrl : "https://webcenter/").Replace("http://help.icemortgagetechnology.com/encompass/tutorials/360/eSigning.htm", "http://help.icemortgagetechnology.com/videos/eSigningCC/index.html");
      return stringBuilder.ToString();
    }

    private void setFieldText(IHTMLElement elm, string text)
    {
      IHTMLElement htmlElement1 = elm;
      foreach (IHTMLElement htmlElement2 in (IHTMLElementCollection) elm.all)
      {
        if (htmlElement2.innerText == elm.innerText)
          htmlElement1 = htmlElement2;
      }
      htmlElement1.innerText = text;
      elm.outerHTML = elm.innerHTML;
    }

    private void setFieldHtml(IHTMLElement elm, string html)
    {
      IHTMLElement htmlElement = elm;
      while (htmlElement != null)
      {
        try
        {
          htmlElement.insertAdjacentHTML("afterEnd", html);
          break;
        }
        catch (Exception ex)
        {
          Tracing.Log(HtmlFieldMerge.sw, TraceLevel.Error, nameof (HtmlFieldMerge), ex.ToString());
          htmlElement = htmlElement.parentElement;
        }
      }
      this.setFieldText(elm, string.Empty);
    }

    private void setFieldSignature(IHTMLElement elm, UserInfo userInfo)
    {
      if (userInfo != (UserInfo) null && !string.IsNullOrEmpty(userInfo.EmailSignature))
      {
        IHTMLDocument2 htmlDocument2 = (IHTMLDocument2) new HTMLDocumentClass();
        htmlDocument2.write((object) userInfo.EmailSignature);
        htmlDocument2.close();
        string str = htmlDocument2.body.innerText;
        if (str != null)
          str = str.Trim();
        bool flag = !string.IsNullOrEmpty(str);
        if (!flag && ((IHTMLElement2) htmlDocument2.body).getElementsByTagName("img").length > 0)
          flag = true;
        if (flag)
        {
          this.setFieldHtml(elm, htmlDocument2.body.innerHTML);
          return;
        }
      }
      OrgInfo rootOrganization = Session.OrganizationManager.GetRootOrganization();
      if (rootOrganization != null && !string.IsNullOrEmpty(rootOrganization.EmailSignature))
      {
        IHTMLDocument2 htmlDocument2 = (IHTMLDocument2) new HTMLDocumentClass();
        htmlDocument2.write((object) rootOrganization.EmailSignature);
        htmlDocument2.close();
        string str = htmlDocument2.body.innerText;
        if (str != null)
          str = str.Trim();
        bool flag = !string.IsNullOrEmpty(str);
        if (!flag && ((IHTMLElement2) htmlDocument2.body).getElementsByTagName("img").length > 0)
          flag = true;
        if (flag)
        {
          this.setFieldHtml(elm, htmlDocument2.body.innerHTML);
          return;
        }
      }
      this.setFieldText(elm, string.Empty);
    }

    public static void InsertField(
      EncWebFormBrowserControl browser,
      string fieldID,
      string fieldName)
    {
      browser?.InsertField(fieldID, fieldName);
    }
  }
}
