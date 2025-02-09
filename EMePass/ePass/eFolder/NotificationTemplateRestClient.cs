// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.NotificationTemplateRestClient
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ePass.Properties;
using EllieMae.EMLite.RemotingServices;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestApiProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.ePass.eFolder
{
  public class NotificationTemplateRestClient
  {
    private const string className = "NotificationTemplateRestClient";
    private const int migrationRollbackMinutes = 10;
    private static HttpClient _httpClient;
    private static string _templatesUrl = "/platform/v1/templates";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static NotificationTemplateRestClient.MigrationResult MigrateEConsentTemplates(
      SessionObjects sessionObjects)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering MigrateEConsentTemplates");
      NotificationTemplateRestClient.MigrationResult migrationResult = new NotificationTemplateRestClient.MigrationResult();
      migrationResult.Success = true;
      string s = sessionObjects.ConfigurationManager.GetCompanySetting("MIGRATION", "eConsentTemplates") ?? "";
      if (s == "")
      {
        RemoteLogger.Write(TraceLevel.Verbose, "eConsent Templates not migrated (or already rolled back). No rollback needed.");
        migrationResult.Message = "eConsentTemplates_NoRollback";
        return migrationResult;
      }
      RemoteLogger.Write(TraceLevel.Verbose, "eConsent Templates were migrated at " + s + ". Rollback initiated");
      DateTime now = DateTime.Now;
      NotificationTemplateRestClient._httpClient = RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(sessionObjects, "application/json");
      NotificationTemplateRestClient.NotificationTemplate[] notificationTemplates = NotificationTemplateRestClient.getNotificationTemplates(sessionObjects.StartupInfo.OAPIGatewayBaseUri + NotificationTemplateRestClient._templatesUrl);
      if (notificationTemplates == null || ((IEnumerable<NotificationTemplateRestClient.NotificationTemplate>) notificationTemplates).Count<NotificationTemplateRestClient.NotificationTemplate>() == 0)
      {
        RemoteLogger.Write(TraceLevel.Verbose, "No Notification Templates found.");
        return migrationResult;
      }
      RemoteLogger.Write(TraceLevel.Verbose, ((IEnumerable<NotificationTemplateRestClient.NotificationTemplate>) notificationTemplates).Count<NotificationTemplateRestClient.NotificationTemplate>().ToString() + " Notification Templates found.");
      DateTime dateTime = DateTime.Parse(s);
      int num = 0;
      foreach (NotificationTemplateRestClient.NotificationTemplate notificationTemplate in notificationTemplates)
      {
        if (notificationTemplate.tags.PackageType == "eConsent")
        {
          if ((dateTime - DateTime.Parse(notificationTemplate.createdDate)).TotalMinutes <= 10.0)
          {
            NotificationTemplateRestClient.deleteNotificationTemplate(sessionObjects.StartupInfo.OAPIGatewayBaseUri + NotificationTemplateRestClient._templatesUrl, notificationTemplate.id);
            ++num;
          }
          else
            RemoteLogger.Write(TraceLevel.Verbose, "template createdDate " + (object) DateTime.Parse(notificationTemplate.createdDate) + " beyond " + (object) 10 + "minute rollback limit.");
        }
      }
      sessionObjects.ConfigurationManager.SetCompanySetting("MIGRATION", "eConsentTemplates", string.Empty);
      TimeSpan timeSpan = DateTime.Now - now;
      migrationResult.Message = "Rolled back " + (object) num + " templates in " + (object) timeSpan.TotalMilliseconds + "ms";
      RemoteLogger.Write(TraceLevel.Verbose, migrationResult.Message);
      return migrationResult;
    }

    private static NotificationTemplateRestClient.MigrationResult runEConsentTemplateMigration(
      SessionObjects sessionObjects)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering MigrateEConsentTemplates");
      NotificationTemplateRestClient.MigrationResult migrationResult = new NotificationTemplateRestClient.MigrationResult();
      migrationResult.Success = true;
      string str = sessionObjects.ConfigurationManager.GetCompanySetting("MIGRATION", "eConsentTemplates") ?? "";
      if (str != "")
      {
        RemoteLogger.Write(TraceLevel.Verbose, "eConsent Templates already migrated at " + str + ". No migration needed.");
        migrationResult.Message = "eConsentTemplatesAlreadyMigrated";
        return migrationResult;
      }
      DateTime now = DateTime.Now;
      HtmlEmailTemplate[] htmlEmailTemplates = sessionObjects.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent);
      if (((IEnumerable<HtmlEmailTemplate>) htmlEmailTemplates).Count<HtmlEmailTemplate>() == 0)
      {
        NotificationTemplateRestClient.createDefaultEConsentTemplate(sessionObjects);
        RemoteLogger.Write(TraceLevel.Verbose, "Default eConsent Template created.");
        sessionObjects.ConfigurationManager.SetCompanySetting("MIGRATION", "eConsentTemplates", DateTime.Now.ToString());
        migrationResult.Message = "eConsentDefaultTemplateMigrated";
        return migrationResult;
      }
      string companySetting = sessionObjects.ConfigurationManager.GetCompanySetting("DefaultCCEmailTemplates", "Consent");
      foreach (HtmlEmailTemplate template in htmlEmailTemplates)
      {
        if (!template.Migrated)
        {
          NotificationTemplateRestClient.CreateNotificationTemplateRequest request = new NotificationTemplateRestClient.CreateNotificationTemplateRequest()
          {
            title = template.Subject,
            channel = "Email",
            content = NotificationTemplateRestClient.migrateHtmlEmailTemplateContent(template.Html),
            contentType = "text/html"
          };
          if (template.Guid == companySetting)
            request.tags = new NotificationTemplateRestClient.CreateNotificationTemplateRequest.Tags()
            {
              PackageType = "eConsent",
              Default = new string[2]
              {
                "Borrowers",
                "NonBorrowingOwners"
              }
            };
          else
            request.tags = new NotificationTemplateRestClient.CreateNotificationTemplateRequest.Tags()
            {
              PackageType = "eConsent"
            };
          string notificationTemplate = NotificationTemplateRestClient.createNotificationTemplate(sessionObjects.StartupInfo.OAPIGatewayBaseUri + NotificationTemplateRestClient._templatesUrl + "?view=entity", request);
          if (notificationTemplate.ToLower() == "created")
          {
            template.Migrated = true;
            sessionObjects.ConfigurationManager.SaveHtmlEmailTemplate(template);
            RemoteLogger.Write(TraceLevel.Verbose, "Migration successful for template " + template.Guid);
          }
          else
            RemoteLogger.Write(TraceLevel.Verbose, "Migration result = " + notificationTemplate + " for template " + template.Guid);
        }
      }
      sessionObjects.ConfigurationManager.SetCompanySetting("MIGRATION", "eConsentTemplates", DateTime.Now.ToString());
      TimeSpan timeSpan = DateTime.Now - now;
      migrationResult.Message = "Migrated " + (object) htmlEmailTemplates.Length + "templates in " + (object) timeSpan.TotalMilliseconds + "ms";
      RemoteLogger.Write(TraceLevel.Verbose, migrationResult.Message);
      return migrationResult;
    }

    private static NotificationTemplateRestClient.NotificationTemplate[] getNotificationTemplates(
      string url)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering getNotificationTemplates");
      NotificationTemplateRestClient.NotificationTemplate[] notificationTemplates = (NotificationTemplateRestClient.NotificationTemplate[]) null;
      try
      {
        HttpResponseMessage result = NotificationTemplateRestClient._httpClient.GetAsync(url).Result;
        if (NotificationTemplateRestClient.isSuccessResponse(result))
          notificationTemplates = JsonConvert.DeserializeObject<NotificationTemplateRestClient.NotificationTemplate[]>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "NotificationTemplateRestClient: Error in getNotificationTemplates. Ex: " + (object) ex);
        throw;
      }
      return notificationTemplates;
    }

    private static bool deleteNotificationTemplate(string url, string templateId)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering deleteNotificationTemplate");
      try
      {
        url = url + "/" + templateId;
        if (!NotificationTemplateRestClient.isSuccessResponse(NotificationTemplateRestClient._httpClient.DeleteAsync(url).Result))
          return false;
        RemoteLogger.Write(TraceLevel.Verbose, "Template " + templateId + " deleted.");
        return true;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "NotificationTemplateRestClient: Error in deleteNotificationTemplate. Ex: " + (object) ex);
        throw;
      }
    }

    private static bool isSuccessResponse(HttpResponseMessage response)
    {
      if (response.IsSuccessStatusCode)
        return true;
      string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
      IEnumerable<string> values;
      response.Headers.TryGetValues("X-Correlation-ID", out values);
      if (values != null)
        message = message + " CorrelationID=" + values.FirstOrDefault<string>();
      throw new HttpException((int) response.StatusCode, message);
    }

    private static string migrateHtmlEmailTemplateContent(string html)
    {
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html);
      foreach (HtmlNode oldChild in htmlDocument.DocumentNode.Descendants("label").ToList<HtmlNode>())
      {
        string attributeValue = oldChild.GetAttributeValue("emid", "");
        if (!string.IsNullOrEmpty(attributeValue))
        {
          switch (attributeValue)
          {
            case "Signature":
              HtmlNode node1 = HtmlNode.CreateNode("{{sender.signature.email}}");
              oldChild.ParentNode.ReplaceChild(node1, oldChild);
              continue;
            case "CurrentUserName":
              HtmlNode node2 = HtmlNode.CreateNode("{{user.name}}");
              oldChild.ParentNode.ReplaceChild(node2, oldChild);
              continue;
            case "CurrentUserEmail":
              HtmlNode node3 = HtmlNode.CreateNode("{{user.email}}");
              oldChild.ParentNode.ReplaceChild(node3, oldChild);
              continue;
            case "CurrentUserPhone":
              HtmlNode node4 = HtmlNode.CreateNode("{{user.phone.work}}");
              oldChild.ParentNode.ReplaceChild(node4, oldChild);
              continue;
            case "Informational Documents":
              HtmlNode node5 = HtmlNode.CreateNode("{{party.documents.reviewer}}");
              oldChild.ParentNode.ReplaceChild(node5, oldChild);
              continue;
            case "Sign and Return Documents":
              HtmlNode node6 = HtmlNode.CreateNode("{{party.documents.signer}}");
              oldChild.ParentNode.ReplaceChild(node6, oldChild);
              continue;
            case "Needed Documents":
              HtmlNode node7 = HtmlNode.CreateNode("{{party.documents.supplier}}");
              oldChild.ParentNode.ReplaceChild(node7, oldChild);
              continue;
            case "AuthenticationUser":
              HtmlNode node8 = HtmlNode.CreateNode("{{party.name}}");
              oldChild.ParentNode.ReplaceChild(node8, oldChild);
              continue;
            case "AuthenticationCode":
              HtmlNode node9 = HtmlNode.CreateNode("{{party.authentication.code}}");
              oldChild.ParentNode.ReplaceChild(node9, oldChild);
              continue;
            case "Recipient Full Name":
              HtmlNode node10 = HtmlNode.CreateNode("{{party.name}}");
              oldChild.ParentNode.ReplaceChild(node10, oldChild);
              continue;
            default:
              HtmlNode node11 = HtmlNode.CreateNode("{{" + attributeValue + "}}");
              oldChild.ParentNode.ReplaceChild(node11, oldChild);
              continue;
          }
        }
      }
      html = htmlDocument.DocumentNode.OuterHtml;
      if (html.Contains("https://webcenter/"))
        html = html.Replace("https://webcenter/", "{{party.url}}");
      if (html.Contains("http://help.icemortgagetechnology.com/encompass/tutorials/360/eSigning.htm"))
        html = html.Replace("http://help.icemortgagetechnology.com/encompass/tutorials/360/eSigning.htm", "http://help.icemortgagetechnology.com/videos/eSigningCC/index.html");
      return html;
    }

    private static string createDefaultEConsentTemplate(SessionObjects sessionObjects)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering createDefaultEConsentTemplate");
      try
      {
        string html = new Regex("emid=\"4002\"", RegexOptions.IgnorePatternWhitespace).Replace(new Regex("&lt;&lt;4002\\s+Borrower\\s+Last\\s+Name&gt;&gt;", RegexOptions.IgnorePatternWhitespace).Replace(Resources.EDMLoanLevelConsentEmailTemplate, "&lt;&lt;Recipient Full Name&gt;&gt;"), "emid=\"Recipient Full Name\"");
        NotificationTemplateRestClient.CreateNotificationTemplateRequest request = new NotificationTemplateRestClient.CreateNotificationTemplateRequest()
        {
          title = "Electronic Signature Consent for Loan Documents",
          channel = "Email",
          tags = new NotificationTemplateRestClient.CreateNotificationTemplateRequest.Tags()
          {
            PackageType = "eConsent",
            Default = new string[2]
            {
              "Borrowers",
              "NonBorrowingOwners"
            }
          },
          content = NotificationTemplateRestClient.migrateHtmlEmailTemplateContent(html),
          contentType = "text/html"
        };
        return NotificationTemplateRestClient.createNotificationTemplate(sessionObjects.StartupInfo.OAPIGatewayBaseUri + NotificationTemplateRestClient._templatesUrl + "?view=entity", request);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "NotificationTemplateRestClient: Error in createDefaultEConsentTemplate. Ex: " + (object) ex);
        return ex.Message;
      }
    }

    private static string createNotificationTemplate(
      string url,
      NotificationTemplateRestClient.CreateNotificationTemplateRequest request)
    {
      RemoteLogger.Write(TraceLevel.Verbose, "Entering createNotificationTemplate");
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json");
        return NotificationTemplateRestClient._httpClient.PostAsync(url, (HttpContent) content).Result.StatusCode.ToString();
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "NotificationTemplateRestClient: Error in createNotificationTemplate. Ex: " + (object) ex);
        return ex.Message;
      }
    }

    public class NotificationTemplate
    {
      public string id { get; set; }

      public string title { get; set; }

      public string channel { get; set; }

      public NotificationTemplateRestClient.NotificationTemplate.Tags tags { get; set; }

      public string createdDate { get; set; }

      public string createdBy { get; set; }

      public string lastModifiedDate { get; set; }

      public string lastModifiedBy { get; set; }

      public class Tags
      {
        public string PackageType { get; set; }
      }
    }

    public class CreateNotificationTemplateRequest
    {
      public string title { get; set; }

      public string channel { get; set; }

      public NotificationTemplateRestClient.CreateNotificationTemplateRequest.Tags tags { get; set; }

      public string content { get; set; }

      public string contentType { get; set; }

      public class Tags
      {
        public string PackageType { get; set; }

        public string[] Default { get; set; }
      }
    }

    public class MigrationResult
    {
      public bool Success { get; set; }

      public string Message { get; set; }
    }
  }
}
