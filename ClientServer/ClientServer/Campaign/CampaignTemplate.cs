// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private string templateName = string.Empty;
    private string templateDescription = string.Empty;
    private string templateVersion = string.Empty;
    private XmlStringTable campaignFields = new XmlStringTable();
    private CampaignTemplate.ContactQueryTemplate addQueryTemplate;
    private CampaignTemplate.ContactQueryTemplate deleteQueryTemplate;
    private CampaignTemplate.CampaignStepTemplateList campaignStepTemplates = new CampaignTemplate.CampaignStepTemplateList();

    public static explicit operator CampaignTemplate(BinaryObject binaryObject)
    {
      return (CampaignTemplate) BinaryConvertibleObject.Parse(binaryObject, typeof (CampaignTemplate));
    }

    public string TemplateName
    {
      get => this.templateName;
      set => this.templateName = value == null ? string.Empty : value.Trim();
    }

    public string Description
    {
      get => this.templateDescription;
      set => this.templateDescription = value == null ? string.Empty : value.Trim();
    }

    public string TemplateVersion
    {
      get => this.templateVersion;
      set => this.templateVersion = value == null ? string.Empty : value.Trim();
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "TemplateName", (object) this.TemplateName);
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public CampaignTemplate()
    {
    }

    public CampaignTemplate(CampaignInfo campaignInfo)
    {
      this.TemplateName = campaignInfo.CampaignName;
      this.Description = campaignInfo.CampaignDesc;
      this.TemplateVersion = campaignInfo.CampaignVersion;
      this.campaignFields.Add("CampaignName", (object) campaignInfo.CampaignName);
      this.campaignFields.Add("CampaignDesc", (object) campaignInfo.CampaignDesc);
      this.campaignFields.Add("CampaignType", (object) campaignInfo.CampaignType.ToString());
      this.campaignFields.Add("ContactType", (object) new ContactTypeNameProvider().GetName((object) campaignInfo.ContactType));
      this.campaignFields.Add("Status", (object) new CampaignStatusNameProvider().GetName((object) campaignInfo.Status));
      this.campaignFields.Add("FrequencyType", (object) new CampaignFrequencyNameProvider().GetName((object) campaignInfo.FrequencyType));
      this.campaignFields.Add("FrequencyInterval", (object) campaignInfo.FrequencyInterval.ToString());
      this.campaignFields.Add(nameof (TemplateVersion), (object) campaignInfo.CampaignVersion);
      if (campaignInfo.AddQueryInfo != null)
        this.addQueryTemplate = new CampaignTemplate.ContactQueryTemplate(campaignInfo.AddQueryInfo);
      if (campaignInfo.DeleteQueryInfo != null)
        this.deleteQueryTemplate = new CampaignTemplate.ContactQueryTemplate(campaignInfo.DeleteQueryInfo);
      foreach (CampaignStepInfo campaignStep in campaignInfo.CampaignSteps)
        this.campaignStepTemplates.Add((object) new CampaignTemplate.CampaignStepTemplate(campaignStep));
    }

    public CampaignTemplate(XmlSerializationInfo info)
    {
      this.templateName = info.GetString(nameof (TemplateName));
      this.templateDescription = info.GetString("TemplateDescription");
      try
      {
        this.templateVersion = info.GetString(nameof (TemplateVersion));
      }
      catch
      {
      }
      this.campaignFields = (XmlStringTable) info.GetValue("CampaignFields", typeof (XmlStringTable));
      this.addQueryTemplate = (CampaignTemplate.ContactQueryTemplate) info.GetValue("AddQueryFields", typeof (CampaignTemplate.ContactQueryTemplate));
      this.deleteQueryTemplate = (CampaignTemplate.ContactQueryTemplate) info.GetValue("DeleteQueryFields", typeof (CampaignTemplate.ContactQueryTemplate));
      this.campaignStepTemplates = (CampaignTemplate.CampaignStepTemplateList) info.GetValue("CampaignStepList", typeof (CampaignTemplate.CampaignStepTemplateList));
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("TemplateName", (object) this.templateName);
      info.AddValue("TemplateDescription", (object) this.templateDescription);
      info.AddValue("TemplateVersion", (object) this.templateVersion);
      info.AddValue("CampaignFields", (object) this.campaignFields);
      info.AddValue("AddQueryFields", (object) this.addQueryTemplate);
      info.AddValue("DeleteQueryFields", (object) this.deleteQueryTemplate);
      info.AddValue("CampaignStepList", (object) this.campaignStepTemplates);
    }

    public string GetField(string fieldKey)
    {
      return (string) this.campaignFields[fieldKey] ?? string.Empty;
    }

    public CampaignTemplate.ContactQueryTemplate AddQueryTemplate => this.addQueryTemplate;

    public CampaignTemplate.ContactQueryTemplate DeleteQueryTemplate => this.deleteQueryTemplate;

    public CampaignTemplate.CampaignStepTemplateList CampaignStepTemplates
    {
      get => this.campaignStepTemplates;
    }

    [Serializable]
    public class ContactQueryTemplate : XmlStringTable
    {
      public ContactQueryTemplate()
      {
      }

      public ContactQueryTemplate(ContactQueryInfo contactQueryInfo)
      {
        this.Add("ContactType", (object) new ContactTypeNameProvider().GetName((object) contactQueryInfo.ContactType));
        this.Add("QueryType", (object) contactQueryInfo.QueryType.ToString());
        this.Add("QueryName", (object) contactQueryInfo.QueryName);
        this.Add("QueryDesc", (object) contactQueryInfo.QueryDesc);
        this.Add("XmlQueryString", (object) contactQueryInfo.XmlQueryString);
        this.Add("PrimaryOnly", (object) contactQueryInfo.PrimaryOnly.ToString());
      }

      public ContactQueryTemplate(XmlSerializationInfo info)
      {
        this.Add("ContactType", (object) info.GetString("ContactType"));
        this.Add("QueryType", (object) info.GetString("QueryType"));
        this.Add("QueryName", (object) info.GetString("QueryName"));
        this.Add("QueryDesc", (object) info.GetString("QueryDesc"));
        this.Add("XmlQueryString", (object) info.GetString("XmlQueryString"));
        this.Add("PrimaryOnly", (object) info.GetString("PrimaryOnly"));
      }

      public string GetField(string fieldKey) => (string) this[fieldKey] ?? string.Empty;
    }

    public class CampaignStepTemplateList : XmlArrayList
    {
      public CampaignStepTemplateList()
      {
      }

      public CampaignStepTemplateList(XmlSerializationInfo info)
        : base(info, typeof (CampaignTemplate.CampaignStepTemplate))
      {
      }
    }

    [Serializable]
    public class CampaignStepTemplate : XmlStringTable
    {
      public CampaignStepTemplate()
      {
      }

      public CampaignStepTemplate(CampaignStepInfo campaignStepInfo)
      {
        this.Add("StepNumber", (object) campaignStepInfo.StepNumber.ToString());
        this.Add("StepName", (object) campaignStepInfo.StepName);
        this.Add("StepDesc", (object) campaignStepInfo.StepDesc);
        this.Add("StepInterval", (object) campaignStepInfo.StepInterval.ToString());
        this.Add("ActivityType", (object) new ActivityTypeNameProvider().GetName((object) campaignStepInfo.ActivityType));
        this.Add("DocumentId", (object) campaignStepInfo.DocumentId);
        this.Add("Subject", (object) campaignStepInfo.Subject);
        this.Add("Comments", (object) campaignStepInfo.Comments);
        this.Add("TaskPriority", (object) new TaskPriorityNameProvider().GetName((object) campaignStepInfo.TaskPriority));
        this.Add("BarColor", (object) campaignStepInfo.BarColor.ToArgb());
        this.Add("TasksDueCount", (object) campaignStepInfo.TasksDueCount.ToString());
        this.Add("LastActivityDate", (object) campaignStepInfo.LastActivityDate.ToUniversalTime());
      }

      public CampaignStepTemplate(XmlSerializationInfo info)
      {
        this.Add("StepNumber", (object) info.GetString("StepNumber"));
        this.Add("StepName", (object) info.GetString("StepName"));
        this.Add("StepDesc", (object) info.GetString("StepDesc"));
        this.Add("StepInterval", (object) info.GetString("StepInterval"));
        this.Add("ActivityType", (object) info.GetString("ActivityType"));
        this.Add("DocumentId", (object) info.GetString("DocumentId"));
        this.Add("Subject", (object) info.GetString("Subject"));
        this.Add("Comments", (object) info.GetString("Comments"));
        this.Add("TaskPriority", (object) info.GetString("TaskPriority"));
        this.Add("BarColor", (object) info.GetString("BarColor"));
        this.Add("TasksDueCount", (object) info.GetString("TasksDueCount"));
        this.Add("LastActivityDate", (object) info.GetString("LastActivityDate"));
      }

      public string GetField(string fieldKey) => (string) this[fieldKey] ?? string.Empty;
    }
  }
}
