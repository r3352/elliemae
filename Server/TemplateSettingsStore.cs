// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TemplateSettingsStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TemplateSettingsStore
  {
    private const string className = "TemplateSettingsStore�";

    public static TemplateSettings CheckOut(
      TemplateSettingsType type,
      FileSystemEntry entry,
      bool toAcquireLock = true)
    {
      TemplateIdentity identifier = new TemplateIdentity(type, entry);
      ICacheLock<TemplateCacheData> innerLock = ClientContext.GetCurrent().Cache.CheckOut<TemplateCacheData>(nameof (TemplateSettingsStore), identifier.ToString(), (object) identifier, toAcquireLock: toAcquireLock);
      try
      {
        return new TemplateSettings(innerLock);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (TemplateSettingsStore), ex);
        return (TemplateSettings) null;
      }
    }

    public static TemplateSettings GetLatestVersion(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      return TemplateSettingsStore.getLatestVersionUsingID(new TemplateIdentity(type, entry));
    }

    private static TemplateSettings getLatestVersionUsingID(TemplateIdentity id)
    {
      DataFile templateFile = FileStore.GetLatestVersion(id.PhysicalPath);
      TemplateCacheData cacheData = ClientContext.GetCurrent().Cache.Get<TemplateCacheData>(nameof (TemplateSettingsStore), id.ToString(), (Func<TemplateCacheData>) (() => TemplateSettings.LoadTemplateCacheData(templateFile, id)), CacheSetting.Low);
      return new TemplateSettings(templateFile, cacheData, id);
    }

    public static BinaryObject GetTemplate(TemplateSettingsType type, FileSystemEntry entry)
    {
      using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entry))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.Data;
    }

    public static FileSystemEntry[] GetAllPublicSystemEntries(
      TemplateSettingsType type,
      bool includeProperties)
    {
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(TemplateSettings.GetFolderPath(type, new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null)), FileSystemEntry.Types.All, (string) null, "\\", false, true, true);
      if (includeProperties)
        TemplateSettingsStore.populateTemplateProperties(type, directoryEntries);
      return directoryEntries;
    }

    public static bool Exists(TemplateSettingsType type, FileSystemEntry entry)
    {
      if (entry.Path == "\\")
        return true;
      return entry.Type == FileSystemEntry.Types.File ? File.Exists(TemplateSettings.GetFilePath(type, entry)) : Directory.Exists(TemplateSettings.GetFolderPath(type, entry));
    }

    public static bool ExistsOfAnyType(TemplateSettingsType type, FileSystemEntry entry)
    {
      return entry.Path == "\\" || File.Exists(TemplateSettings.GetFilePath(type, entry)) || Directory.Exists(TemplateSettings.GetFolderPath(type, entry));
    }

    public static void Delete(TemplateSettingsType type, FileSystemEntry entry)
    {
      if (entry.Type == FileSystemEntry.Types.File)
      {
        using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(type, entry))
        {
          if (entry.IsPublic)
            AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), entry);
          templateSettings.Delete();
        }
      }
      else
      {
        string folderPath = TemplateSettings.GetFolderPath(type, entry);
        if (!Directory.Exists(folderPath))
          return;
        if (entry.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), entry);
        Directory.Delete(folderPath, false);
      }
    }

    public static void CreateFolder(TemplateSettingsType type, FileSystemEntry entry)
    {
      if (File.Exists(TemplateSettings.GetFilePath(type, entry)))
        Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new DuplicateObjectException("Template with this name already exists", ObjectType.Template, (object) entry));
      Directory.CreateDirectory(TemplateSettings.GetFolderPath(type, entry));
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      TemplateSettingsType type,
      FileSystemEntry parentEntry)
    {
      return TemplateSettingsStore.GetDirectoryEntries(type, parentEntry, false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      TemplateSettingsType type,
      FileSystemEntry parentEntry,
      bool recurse)
    {
      string folderPath = TemplateSettings.GetFolderPath(type, parentEntry);
      if (!Directory.Exists(folderPath))
        Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.Template, (object) parentEntry));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, parentEntry.Owner, parentEntry.Path, false, true, recurse);
      TemplateSettingsStore.populateTemplateProperties(type, directoryEntries);
      return directoryEntries;
    }

    public static FileSystemEntry[] GetTemplatePropertiesAndRights(
      UserInfo user,
      TemplateSettingsType type,
      FileSystemEntry[] fsEntries,
      bool includeProperties,
      bool includeRights)
    {
      if (includeRights)
        fsEntries = AclGroupFileAccessor.ApplyUserAccessRights(user, fsEntries, type == TemplateSettingsType.ConditionalLetter ? AclFileType.None : TemplateSettingsStore.templateTypeToAclFileType(type));
      if (includeProperties)
        TemplateSettingsStore.populateTemplateProperties(type, fsEntries);
      return fsEntries;
    }

    public static FileSystemEntry[] FindFileEntries(
      TemplateSettingsType type,
      FileSystemEntry parentEntry,
      string propertyName,
      object propertyValue,
      bool recurse)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(type, parentEntry, recurse);
      ArrayList arrayList = new ArrayList();
      foreach (FileSystemEntry fileSystemEntry in directoryEntries)
      {
        if (object.Equals(fileSystemEntry.Properties[(object) propertyName], propertyValue))
          arrayList.Add((object) fileSystemEntry);
      }
      return (FileSystemEntry[]) arrayList.ToArray(typeof (FileSystemEntry));
    }

    public static FileSystemEntry FindEntryByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentEntry,
      string guid,
      bool recurse)
    {
      foreach (FileSystemEntry directoryEntry in TemplateSettingsStore.GetDirectoryEntries(type, parentEntry, recurse))
      {
        if (object.Equals(directoryEntry.Properties[(object) "Guid"], (object) guid))
          return directoryEntry;
      }
      return (FileSystemEntry) null;
    }

    public static BinaryObject GetTemplateByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentEntry,
      string guid,
      bool recurse)
    {
      FileSystemEntry entryByGuid = TemplateSettingsStore.FindEntryByGuid(type, parentEntry, guid, recurse);
      if (entryByGuid == null)
        return (BinaryObject) null;
      using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entryByGuid))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.Data;
    }

    private static AclFileType templateTypeToAclFileType(TemplateSettingsType type)
    {
      switch (type)
      {
        case TemplateSettingsType.LoanProgram:
          return AclFileType.LoanProgram;
        case TemplateSettingsType.ClosingCost:
          return AclFileType.ClosingCost;
        case TemplateSettingsType.MiscData:
          return AclFileType.MiscData;
        case TemplateSettingsType.FormList:
          return AclFileType.FormList;
        case TemplateSettingsType.DocumentSet:
          return AclFileType.DocumentSet;
        case TemplateSettingsType.LoanTemplate:
          return AclFileType.LoanTemplate;
        case TemplateSettingsType.UnderwritingConditionSet:
          return AclFileType.UnderwritingConditionSet;
        case TemplateSettingsType.PostClosingConditionSet:
          return AclFileType.PostClosingConditionSet;
        case TemplateSettingsType.Campaign:
          return AclFileType.CampaignTemplate;
        case TemplateSettingsType.DashboardTemplate:
          return AclFileType.DashboardTemplate;
        case TemplateSettingsType.DashboardViewTemplate:
          return AclFileType.DashboardViewTemplate;
        case TemplateSettingsType.TaskSet:
          return AclFileType.TaskSet;
        case TemplateSettingsType.ConditionalLetter:
          return AclFileType.ConditionalApprovalLetter;
        case TemplateSettingsType.SettlementServiceProviders:
          return AclFileType.SettlementServiceProviders;
        case TemplateSettingsType.AffiliatedBusinessArrangements:
          return AclFileType.AffiliatedBusinessArrangements;
        case TemplateSettingsType.ChangeOfCircumstanceOptions:
          return AclFileType.ChangeOfCircumstanceOptions;
        default:
          return AclFileType.None;
      }
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      UserInfo user,
      TemplateSettingsType type,
      FileSystemEntry parentEntry,
      FileSystemEntry.Types fileTypes,
      bool recurse,
      bool includeProperties,
      bool checkSubFolders = false)
    {
      string folderPath = TemplateSettings.GetFolderPath(type, parentEntry);
      if (!Directory.Exists(folderPath))
        Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.Template, (object) parentEntry));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, fileTypes, parentEntry.Owner, parentEntry.Path, type == TemplateSettingsType.CustomLetter, true, recurse, checkSubFolders);
      FileSystemEntry[] fs = AclGroupFileAccessor.ApplyUserAccessRights(user, directoryEntries, type == TemplateSettingsType.ConditionalLetter ? AclFileType.None : TemplateSettingsStore.templateTypeToAclFileType(type));
      if (includeProperties)
        TemplateSettingsStore.populateTemplateProperties(type, fs);
      return fs;
    }

    public static FileSystemEntry[] GetAllPublicFileEntries(
      TemplateSettingsType type,
      bool includeProperties)
    {
      return TemplateSettingsStore.GetAllFileEntries(type, (string) null, includeProperties);
    }

    public static FileSystemEntry[] GetAllFileEntries(
      TemplateSettingsType type,
      string userId,
      bool includeProperties)
    {
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(TemplateSettings.GetFolderPath(type, new FileSystemEntry("\\", FileSystemEntry.Types.Folder, userId)), FileSystemEntry.Types.File, userId, "\\", false, true, true);
      if (includeProperties)
        TemplateSettingsStore.populateTemplateProperties(type, directoryEntries);
      return directoryEntries;
    }

    public static Hashtable GetLoanTemplateComponents(FileSystemEntry templateEntry, string userId)
    {
      EllieMae.EMLite.DataEngine.LoanTemplate template = (EllieMae.EMLite.DataEngine.LoanTemplate) TemplateSettingsStore.GetTemplate(TemplateSettingsType.LoanTemplate, templateEntry);
      if (template == null)
        throw new ObjectNotFoundException("Invalid template specification", ObjectType.Template, (object) templateEntry);
      Hashtable templateComponents = new Hashtable();
      templateComponents[(object) "LOANTEMPLATEFILE"] = (object) templateEntry.Path;
      if ((template.GetField("DOCSET") ?? "") != "")
      {
        templateComponents[(object) "DOCSET"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.DocumentSet, FileSystemEntry.Parse(template.GetField("DOCSET"), userId));
        templateComponents[(object) "DOCSETFILE"] = (object) template.GetField("DOCSET");
      }
      if ((template.GetField("FORMLIST") ?? "") != "")
      {
        templateComponents[(object) "FORMLIST"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.FormList, FileSystemEntry.Parse(template.GetField("FORMLIST"), userId));
        templateComponents[(object) "FORMLISTFILE"] = (object) template.GetField("FORMLIST");
      }
      if ((template.GetField("MISCDATA") ?? "") != "")
      {
        templateComponents[(object) "MISCDATA"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.MiscData, FileSystemEntry.Parse(template.GetField("MISCDATA"), userId));
        templateComponents[(object) "MISCDATAFILE"] = (object) template.GetField("MISCDATA");
      }
      if ((template.GetField("PROGRAM") ?? "") != "")
      {
        templateComponents[(object) "PROGRAM"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.LoanProgram, FileSystemEntry.Parse(template.GetField("PROGRAM"), userId));
        templateComponents[(object) "PROGRAMFILE"] = (object) template.GetField("PROGRAM");
      }
      if ((template.GetField("COST") ?? "") != "")
      {
        templateComponents[(object) "COST"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.ClosingCost, FileSystemEntry.Parse(template.GetField("COST"), userId));
        templateComponents[(object) "COSTFILE"] = (object) template.GetField("COST");
      }
      LoanProgram loanProgram = (LoanProgram) templateComponents[(object) TemplateSettingsType.LoanProgram];
      if (loanProgram != null && (loanProgram.GetSimpleField("LP97") ?? "") != "")
      {
        templateComponents[(object) "LPCOST"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.ClosingCost, FileSystemEntry.Parse(loanProgram.GetSimpleField("LP97"), userId));
        templateComponents[(object) "LPCOSTFILE"] = (object) template.GetField("LPCOST");
      }
      if ((template.GetField("TASKSET") ?? "") != "")
      {
        templateComponents[(object) "TASKSET"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.TaskSet, FileSystemEntry.Parse(template.GetField("TASKSET"), userId));
        templateComponents[(object) "TASKSETFILE"] = (object) template.GetField("TASKSET");
      }
      if ((template.GetField("PROVIDERLIST") ?? "") != "")
      {
        templateComponents[(object) "PROVIDERLIST"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.SettlementServiceProviders, FileSystemEntry.Parse(template.GetField("PROVIDERLIST"), userId));
        templateComponents[(object) "PROVIDERLISTFILE"] = (object) template.GetField("PROVIDERLIST");
      }
      if ((template.GetField("AFFILIATELIST") ?? "") != "")
      {
        templateComponents[(object) "AFFILIATELIST"] = (object) TemplateSettingsStore.GetTemplate(TemplateSettingsType.AffiliatedBusinessArrangements, FileSystemEntry.Parse(template.GetField("AFFILIATELIST"), userId));
        templateComponents[(object) "AFFILIATELISTFILE"] = (object) template.GetField("AFFILIATELIST");
      }
      if ((template.GetField("MILETEMP") ?? "") != "")
        templateComponents[(object) "MILETEMP"] = (object) template.GetField("MILETEMP");
      return templateComponents;
    }

    public static void Move(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        target = TemplateSettingsStore.moveFile(type, source, target);
      else
        TemplateSettingsStore.moveFolder(type, source, target);
      TemplateSettings.MoveTemplateXRefs(type, source, target);
    }

    private static FileSystemEntry moveFile(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      if (TemplateSettingsStore.ExistsOfAnyType(type, target) && string.Compare(source.Name, target.Name, false) == 0)
        Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.Template, (object) target));
      string filePath1 = TemplateSettings.GetFilePath(type, source);
      string filePath2 = TemplateSettings.GetFilePath(type, target);
      if (!TemplateSettingsStore.Exists(type, source))
        Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new ObjectNotFoundException("Source object not found", ObjectType.Template, (object) source));
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("TemplateSettingsStore_" + filePath2))
      {
        using (current.Cache.Lock("TemplateSettingsStore_" + filePath1))
        {
          if (source.IsPublic)
            AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), new FileSystemEntry(source.Path, source.Type, source.Owner));
          File.Move(filePath1, filePath2);
          ClientContext.GetCurrent().Cache.Remove("TemplateSettingsStore_" + new TemplateIdentity(filePath1).ToString());
          if (source.Name != target.Name)
          {
            bool toAcquireLock = !SmartClientUtils.LockSlimNoRecursion;
            TemplateSettingsStore.fixTemplateName(type, target, toAcquireLock);
          }
          return target;
        }
      }
    }

    private static void moveFolder(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      string folderPath = TemplateSettings.GetFolderPath(type, source);
      Directory.CreateDirectory(TemplateSettings.GetFolderPath(type, target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          TemplateSettingsStore.moveFile(type, directoryEntries[index], target);
        else
          TemplateSettingsStore.moveFolder(type, directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
      try
      {
        if (source.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), source);
        Directory.Delete(folderPath, false);
      }
      catch
      {
      }
    }

    public static void Copy(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      bool forNewRESPA)
    {
      TemplateSettingsStore.Copy(type, source, target, forNewRESPA ? RespaVersions.Respa2015 : RespaVersions.Respa2010);
    }

    public static void Copy(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion)
    {
      if (source.Type == FileSystemEntry.Types.File)
        TemplateSettingsStore.copyFile(type, source, target, respaVersion);
      else
        TemplateSettingsStore.copyFolder(type, source, target);
    }

    private static void copyFile(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      TemplateSettingsStore.copyFile(type, source, target, true);
    }

    private static void copyFile(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      bool forNewRESPA)
    {
      TemplateSettingsStore.copyFile(type, source, target, forNewRESPA ? RespaVersions.Respa2015 : RespaVersions.Respa2010);
    }

    private static void copyFile(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      BinaryObject binaryObject = (BinaryObject) null;
      using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, source))
      {
        if (!latestVersion.Exists)
          Err.Raise(TraceLevel.Warning, nameof (TemplateSettingsStore), (ServerException) new ObjectNotFoundException("Source object not found", ObjectType.Template, (object) source));
        binaryObject = latestVersion.CopyData();
        if (type == TemplateSettingsType.ConditionalLetter)
          binaryObject = latestVersion.ResolveXRefs(binaryObject);
        BinaryConvertibleObject templateObject = TemplateSettingsTypeConverter.ConvertToTemplateObject(type, binaryObject);
        ((ITemplateSetting) templateObject).TemplateName = target.Name;
        if (type != TemplateSettingsType.ClosingCost)
        {
          if (type != TemplateSettingsType.DashboardTemplate)
          {
            if (type == TemplateSettingsType.DashboardViewTemplate)
              ((DashboardViewTemplate) templateObject).ViewGuid = Guid.NewGuid().ToString();
          }
          else
            ((DashboardTemplate) templateObject).Guid = Guid.NewGuid().ToString();
        }
        else
        {
          ClosingCost closingCost = (ClosingCost) templateObject;
          switch (respaVersion)
          {
            case RespaVersions.Respa2009:
              closingCost.RESPAVersion = "2009";
              break;
            case RespaVersions.Respa2010:
              closingCost.RESPAVersion = "2010";
              break;
            default:
              closingCost.RESPAVersion = "2015";
              break;
          }
        }
        binaryObject = (BinaryObject) templateObject;
      }
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(type, target))
        templateSettings.CheckInCopy(binaryObject);
      binaryObject.Dispose();
    }

    private static void copyFolder(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      string folderPath = TemplateSettings.GetFolderPath(type, source);
      Directory.CreateDirectory(TemplateSettings.GetFolderPath(type, target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          TemplateSettingsStore.copyFile(type, directoryEntries[index], target);
        else
          TemplateSettingsStore.copyFolder(type, directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    private static void populateTemplateProperties(TemplateSettingsType type, FileSystemEntry[] fs)
    {
      for (int index = 0; index < fs.Length; ++index)
      {
        FileSystemEntry f = fs[index];
        if (f.Type == FileSystemEntry.Types.File)
          TemplateSettingsStore.SetTemplatePropertiesUsingCache(type, f);
      }
    }

    private static void SetTemplatePropertiesUsingCache(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      using (TemplateSettings latestVersionUsingId = TemplateSettingsStore.getLatestVersionUsingID(new TemplateIdentity(type, entry)))
      {
        if (!latestVersionUsingId.Exists)
          TraceLog.WriteError(nameof (TemplateSettingsStore), "Template '" + (object) latestVersionUsingId.Identity + "' reported as non-existent");
        else
          entry.Properties = latestVersionUsingId.Properties;
      }
    }

    private static void fixTemplateName(
      TemplateSettingsType type,
      FileSystemEntry entry,
      bool toAcquireLock = true)
    {
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(type, entry, toAcquireLock))
      {
        if (!templateSettings.Exists)
          return;
        templateSettings.ChangeName(entry.Name);
      }
    }
  }
}
