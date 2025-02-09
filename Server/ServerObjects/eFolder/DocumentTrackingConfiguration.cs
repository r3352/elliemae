// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.DocumentTrackingConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class DocumentTrackingConfiguration
  {
    private const string className = "DocumentTrackingConfiguration�";
    private const string mapCacheName = "DocumentTrackingConfigurationMapCache�";

    private DocumentTrackingConfiguration()
    {
    }

    public static DocumentTrackingSetup GetDocumentTrackingSetup()
    {
      List<DocumentTemplate> documentTemplateList = ClientContext.GetCurrent().Cache.Get<List<DocumentTemplate>>(nameof (DocumentTrackingConfiguration), (Func<List<DocumentTemplate>>) (() => DocumentTrackingAccessor.GetDocumentTemplates().ToList<DocumentTemplate>()), CacheSetting.Low);
      DocumentTrackingSetup setup;
      if (documentTemplateList.Any<DocumentTemplate>())
      {
        Hashtable companySettings = Company.GetCompanySettings("TEMPLATE");
        setup = new DocumentTrackingSetup()
        {
          Version = getSetting("DocumentTrackingVersion", "6.7.0"),
          DoNotCreateInfoDocs = getBoolSetting("DoNotCreateInfoDocs"),
          SaveCopyInfoDocs = getBoolSetting("SaveCopyInfoDocs"),
          ApplyTimeStampToInfoDocs = getBoolSetting("ApplyTimeStampToInfoDocs"),
          UseBackgroundConversion = getBoolSetting("UseBackgroundConversion"),
          IgnoreIntendedFor = getBoolSetting("IgnoreIntendedFor")
        };
        setup.AddRange((IEnumerable<DocumentTemplate>) documentTemplateList);

        string getSetting(string id, string defaultIfEmpty)
        {
          return !(companySettings[(object) id] is string str) || string.IsNullOrEmpty(str) ? defaultIfEmpty : str;
        }

        bool getBoolSetting(string id)
        {
          bool result;
          return bool.TryParse(getSetting(id, "False"), out result) & result;
        }
      }
      else
      {
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings("DocumentList"))
          setup = systemSettings?.ToObject<DocumentTrackingSetup>();
        if (setup != null)
          DocumentTrackingConfiguration.SaveDocumentTrackingSetup(setup);
      }
      return setup;
    }

    public static DocumentTrackingSetup GetDocumentTrackingSetup(UserInfo userInfo)
    {
      DocumentTrackingSetup documentTrackingSetup = DocumentTrackingConfiguration.GetDocumentTrackingSetup();
      if (!userInfo.IsSuperAdministrator() && Company.GetCurrentEdition() == EncompassEdition.Banker)
      {
        DocumentTrackingConfiguration.removeStandardForms(documentTrackingSetup, userInfo);
        DocumentTrackingConfiguration.removeCustomForms(documentTrackingSetup, userInfo);
      }
      return documentTrackingSetup;
    }

    public static void SaveDocumentTrackingSetup(DocumentTrackingSetup setup)
    {
      DocumentTrackingConfiguration.UpdateCompanySettings(setup);
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Remove("DocumentTrackingConfigurationMapCache", (Action) (() => context.Cache.Put<List<DocumentTemplate>>(nameof (DocumentTrackingConfiguration), (Action) (() =>
      {
        DocumentTrackingAccessor.SaveDocumentTrackingSetup(setup);
        DocumentTrackingConfiguration.updateConditionsInDatabase(setup);
      }), (Func<List<DocumentTemplate>>) (() =>
      {
        DocumentTrackingSetup documentTrackingSetup = DocumentTrackingConfiguration.GetDocumentTrackingSetup();
        return documentTrackingSetup == null ? (List<DocumentTemplate>) null : documentTrackingSetup.ToList<DocumentTemplate>();
      }), CacheSetting.Low)), CacheSetting.Low);
    }

    public static void UpsertDocumentTrackingTemplate(DocumentTrackingSetup setup, string guid)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Remove("DocumentTrackingConfigurationMapCache", (Action) (() => context.Cache.Put<List<DocumentTemplate>>(nameof (DocumentTrackingConfiguration), (Action) (() =>
      {
        DocumentTrackingAccessor.UpsertDocumentTrackingTemplate(setup, guid);
        DocumentTrackingConfiguration.upsertDocumentTrackingTemplate(setup, guid);
      }), (Func<List<DocumentTemplate>>) (() => DocumentTrackingConfiguration.GetDocumentTrackingSetup().ToList<DocumentTemplate>()), CacheSetting.Low)), CacheSetting.Low);
    }

    public static Hashtable GetDocumentXRefMap(XRefKeyType keyType)
    {
      if (keyType != XRefKeyType.CustomMilestoneGuid && keyType != XRefKeyType.CustomMilestoneName)
        throw new Exception("Invalid XRefKeyType");
      Hashtable hashtable = ClientContext.GetCurrent().Cache.Get<Hashtable>("DocumentTrackingConfigurationMapCache", (Func<Hashtable>) (() =>
      {
        Hashtable documentXrefMap = new Hashtable();
        Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
        documentXrefMap[(object) XRefKeyType.CustomMilestoneGuid] = (object) insensitiveHashtable1;
        Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
        documentXrefMap[(object) XRefKeyType.CustomMilestoneName] = (object) insensitiveHashtable2;
        foreach (DocumentTemplate documentTemplate in DocumentTrackingAccessor.GetDocumentTemplates())
        {
          string guid = documentTemplate.Guid;
          string name = documentTemplate.Name;
          if (!insensitiveHashtable1.ContainsKey((object) guid))
            insensitiveHashtable1.Add((object) guid, (object) name);
          if (!insensitiveHashtable2.ContainsKey((object) name))
            insensitiveHashtable2.Add((object) name, (object) guid);
        }
        return documentXrefMap;
      }), CacheSetting.Low);
      return hashtable != null ? hashtable[(object) keyType] as Hashtable : new Hashtable();
    }

    private static void removeStandardForms(DocumentTrackingSetup setup, UserInfo userInfo)
    {
      PrintFormList printFormList = PrintFormList.Parse(FormsConfiguration.GetFormConfigurationFile(FormConfigFile.OutFormAndFileMapping), EncompassEdition.Banker);
      List<string> stringList1 = new List<string>();
      string[] usersStdPrintForms = AclGroupStdPrintFormAccessor.GetUsersStdPrintForms(userInfo.Userid);
      List<string> stringList2 = new List<string>();
      foreach (PrintForm printForm in printFormList)
      {
        if (!(printForm.Source == string.Empty))
        {
          if (!stringList1.Contains(printForm.Source))
            stringList1.Add(printForm.Source);
          if (Array.IndexOf<string>(usersStdPrintForms, printForm.FormID) >= 0 && !stringList2.Contains(printForm.Source))
            stringList2.Add(printForm.Source);
        }
      }
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      foreach (DocumentTemplate documentTemplate in setup)
      {
        if (documentTemplate.SourceType == "Standard Form" && stringList1.Contains(documentTemplate.Source) && !stringList2.Contains(documentTemplate.Source))
          documentTemplateList.Add(documentTemplate);
      }
      foreach (DocumentTemplate template in documentTemplateList)
        setup.Remove(template);
    }

    private static void removeCustomForms(DocumentTrackingSetup setup, UserInfo userInfo)
    {
      List<DocumentTemplate> documentTemplateList1 = new List<DocumentTemplate>();
      List<DocumentTemplate> documentTemplateList2 = new List<DocumentTemplate>();
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      foreach (DocumentTemplate documentTemplate in setup)
      {
        if (documentTemplate.SourceType == "Custom Form")
        {
          documentTemplateList1.Add(documentTemplate);
          fileSystemEntryList.Add(FileSystemEntry.Parse(documentTemplate.Source));
        }
        else if (documentTemplate.SourceType == "Borrower Specific Custom Form")
        {
          documentTemplateList2.Add(documentTemplate);
          if (!string.IsNullOrEmpty(documentTemplate.SourceBorrower))
            fileSystemEntryList.Add(FileSystemEntry.Parse(documentTemplate.SourceBorrower));
          if (!string.IsNullOrEmpty(documentTemplate.SourceCoborrower))
            fileSystemEntryList.Add(FileSystemEntry.Parse(documentTemplate.SourceCoborrower));
        }
      }
      AclGroupFileAccessor.ApplyUserAccessRights(userInfo, fileSystemEntryList.ToArray(), AclFileType.CustomPrintForms);
      List<DocumentTemplate> documentTemplateList3 = new List<DocumentTemplate>();
      foreach (FileSystemEntry fileSystemEntry in fileSystemEntryList)
      {
        if (fileSystemEntry.Access == AclResourceAccess.None)
        {
          foreach (DocumentTemplate documentTemplate in documentTemplateList1)
          {
            if (FileSystemEntry.Parse(documentTemplate.Source).Equals((object) fileSystemEntry))
              documentTemplateList3.Add(documentTemplate);
          }
          foreach (DocumentTemplate documentTemplate in documentTemplateList2)
          {
            if (!string.IsNullOrEmpty(documentTemplate.SourceBorrower) && FileSystemEntry.Parse(documentTemplate.SourceBorrower).Equals((object) fileSystemEntry))
            {
              documentTemplate.SourceBorrower = string.Empty;
              if (string.IsNullOrEmpty(documentTemplate.SourceCoborrower))
                documentTemplateList3.Add(documentTemplate);
            }
            if (!string.IsNullOrEmpty(documentTemplate.SourceCoborrower) && FileSystemEntry.Parse(documentTemplate.SourceCoborrower).Equals((object) fileSystemEntry))
            {
              documentTemplate.SourceCoborrower = string.Empty;
              if (string.IsNullOrEmpty(documentTemplate.SourceBorrower))
                documentTemplateList3.Add(documentTemplate);
            }
          }
        }
      }
      foreach (DocumentTemplate template in documentTemplateList3)
      {
        if (setup.Contains(template))
          setup.Remove(template);
      }
    }

    public static void UpdateCompanySettings(DocumentTrackingSetup docTemp)
    {
      Company.SetCompanySettings("TEMPLATE", new Dictionary<string, string>()
      {
        {
          "DocumentTrackingVersion",
          docTemp.Version
        },
        {
          "DoNotCreateInfoDocs",
          docTemp.DoNotCreateInfoDocs.ToString()
        },
        {
          "SaveCopyInfoDocs",
          docTemp.SaveCopyInfoDocs.ToString()
        },
        {
          "ApplyTimeStampToInfoDocs",
          docTemp.ApplyTimeStampToInfoDocs.ToString()
        },
        {
          "UseBackgroundConversion",
          docTemp.UseBackgroundConversion.ToString()
        },
        {
          "IgnoreIntendedFor",
          docTemp.IgnoreIntendedFor.ToString()
        }
      });
    }

    private static void updateConditionsInDatabase(DocumentTrackingSetup setup)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      MergeTable mergeTable = DocumentTrackingConfiguration.GetMergeTable(setup.OfType<DocumentTemplate>());
      dbQueryBuilder.AppendMsMergeTable(mergeTable, true);
      dbQueryBuilder.AppendLine("delete from MRR_RequiredDocs where docGuid not in (select guid from DocumentTemplates)");
      dbQueryBuilder.ExecuteRowQuery();
    }

    private static void upsertDocumentTrackingTemplate(DocumentTrackingSetup setup, string guid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      MergeTable mergeTable = DocumentTrackingConfiguration.GetMergeTable(setup.OfType<DocumentTemplate>().Where<DocumentTemplate>((Func<DocumentTemplate, bool>) (t => t.Guid == guid)));
      dbQueryBuilder.AppendMsMergeTable(mergeTable, false);
      dbQueryBuilder.ExecuteRowQuery();
    }

    private static MergeTable GetMergeTable(IEnumerable<DocumentTemplate> templates)
    {
      return new MergeTable()
      {
        Name = "DocumentTemplates",
        Columns = new List<MergeColumn>()
        {
          new MergeColumn("Guid", MergeIntent.PrimaryKey, DbColumnType.VarChar, 38),
          new MergeColumn("Name", MergeIntent.Upsert, DbColumnType.VarChar, 512)
        },
        Rows = templates.Select<DocumentTemplate, List<object>>((Func<DocumentTemplate, List<object>>) (t => new List<object>()
        {
          (object) t.Guid,
          (object) t.Name
        })).ToList<List<object>>()
      };
    }
  }
}
