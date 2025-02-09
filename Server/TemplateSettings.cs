// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TemplateSettings
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TemplateSettings : IDisposable
  {
    private const string className = "TemplateSettings�";
    private TemplateIdentity id;
    private ICacheLock<TemplateCacheData> innerLock;
    private bool readOnly;
    private DataFile templateFile;
    private TemplateCacheData cacheData;
    private BinaryObject data;

    public TemplateSettings(ICacheLock<TemplateCacheData> innerLock)
    {
      this.innerLock = innerLock;
      this.id = (TemplateIdentity) innerLock.Identifier;
      if (innerLock.Value == null)
        return;
      this.cacheData = innerLock.Value;
    }

    public TemplateSettings(
      DataFile templateFile,
      TemplateCacheData cacheData,
      TemplateIdentity id)
    {
      this.id = id;
      this.templateFile = templateFile;
      this.cacheData = cacheData;
      this.readOnly = true;
    }

    public TemplateIdentity Identity => this.id;

    public bool Exists
    {
      get
      {
        if (this.templateFile == null && this.cacheData != null)
          return true;
        this.ensureDataFileLoaded();
        return this.templateFile.Exists;
      }
    }

    public Hashtable Properties
    {
      get
      {
        if (!this.readOnly || this.cacheData == null)
          this.validateInstance();
        return this.cacheData.Properties;
      }
    }

    public void ChangeName(string newName)
    {
      this.validateInstance();
      this.ensureDataFileLoaded();
      using (BinaryObject data = this.templateFile.GetData())
      {
        BinaryConvertibleObject templateObject = TemplateSettingsTypeConverter.ConvertToTemplateObject(this.id.Type, data);
        ((ITemplateSetting) templateObject).TemplateName = newName;
        this.templateFile.CheckIn((BinaryObject) templateObject, true);
      }
    }

    public BinaryObject Data
    {
      get
      {
        if (!this.readOnly)
          this.validateInstance();
        this.ensureDataFileLoaded();
        if (this.data == null)
        {
          BinaryObject data = this.templateFile.GetData();
          this.data = this.ResolveXRefs(data);
          if (this.data != data)
            data.Dispose();
        }
        return this.data;
      }
    }

    public BinaryObject CopyData()
    {
      if (!this.readOnly)
        this.validateInstance();
      this.ensureDataFileLoaded();
      return TemplateSettings.allowSimpleCopy(this.id.Type) ? this.templateFile.GetData() : this.Data.Clone();
    }

    public FileSystemEntry[] GetTemplateXRefs()
    {
      if (!this.readOnly)
        this.validateInstance();
      Hashtable hashtable = this.readXRefsFromDatabase();
      FileSystemEntry[] templateXrefs = new FileSystemEntry[hashtable.Count];
      int num = 0;
      foreach (TemplateXRef templateXref in (IEnumerable) hashtable.Values)
        templateXrefs[num++] = templateXref.XRef;
      return templateXrefs;
    }

    public void Delete()
    {
      this.validateInstance(false);
      this.ensureDataFileLoaded();
      if (this.templateFile.Exists)
        this.templateFile.Delete();
      this.deleteXRefsFromDatabase();
      this.innerLock.CheckIn((TemplateCacheData) null);
      this.Dispose();
    }

    public void CheckIn(BinaryObject data) => this.CheckIn(data, false);

    public void CheckIn(BinaryObject newData, bool keepCheckedOut)
    {
      this.validateInstance(false);
      this.ensureDataFileLoaded();
      BinaryConvertibleObject templateObject = TemplateSettingsTypeConverter.ConvertToTemplateObject(this.id.Type, newData);
      ITemplateSetting template = (ITemplateSetting) templateObject;
      TemplateCacheData newValue = new TemplateCacheData();
      newValue.Properties = template.GetProperties();
      this.updateTemplateXRefs((object) template);
      this.templateFile.Filename = FileSystem.EncodeFilename(template.TemplateName, false) + ".xml";
      this.templateFile.CheckIn((BinaryObject) templateObject, keepCheckedOut);
      this.innerLock.CheckIn(newValue, keepCheckedOut);
      this.data = newData;
      this.cacheData = newValue;
    }

    public void CheckInCopy(BinaryObject newData)
    {
      this.validateInstance(false);
      this.ensureDataFileLoaded();
      if (!TemplateSettings.allowSimpleCopy(this.id.Type))
      {
        this.CheckIn(newData);
      }
      else
      {
        this.templateFile.CheckIn((BinaryObject) TemplateSettingsTypeConverter.ConvertToTemplateObject(this.id.Type, newData));
        this.innerLock.CheckIn();
      }
    }

    public void Dispose()
    {
      if (this.templateFile != null)
        this.templateFile.Dispose();
      if (this.innerLock == null)
        return;
      this.innerLock.UndoCheckout();
      this.innerLock = (ICacheLock<TemplateCacheData>) null;
    }

    private static bool allowSimpleCopy(TemplateSettingsType type)
    {
      return type != TemplateSettingsType.LoanProgram && type != TemplateSettingsType.LoanTemplate && type != TemplateSettingsType.Campaign;
    }

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool requireExists)
    {
      if (this.innerLock == null)
        Err.Raise(TraceLevel.Error, nameof (TemplateSettings), new ServerException("Attempt to access disposed loan object"));
      if (!requireExists)
        return;
      if (!this.Exists)
        Err.Raise(TraceLevel.Error, nameof (TemplateSettings), new ServerException("Object does not exist"));
      if (this.cacheData != null)
        return;
      this.initializeCacheData();
    }

    private void ensureDataFileLoaded()
    {
      if (this.templateFile != null)
        return;
      this.templateFile = FileStore.CheckOut(this.id.PhysicalPath);
    }

    public static TemplateCacheData LoadTemplateCacheData(
      DataFile templateFile,
      TemplateIdentity id)
    {
      if (!templateFile.Exists)
        return (TemplateCacheData) null;
      using (BinaryObject data = templateFile.GetData())
        return new TemplateCacheData()
        {
          Properties = ((ITemplateSetting) TemplateSettingsTypeConverter.ConvertToTemplateObject(id.Type, data)).GetProperties()
        };
    }

    private void initializeCacheData()
    {
      this.ensureDataFileLoaded();
      using (BinaryObject data = this.templateFile.GetData())
      {
        this.cacheData = new TemplateCacheData();
        this.cacheData.Properties = ((ITemplateSetting) TemplateSettingsTypeConverter.ConvertToTemplateObject(this.id.Type, data)).GetProperties();
        this.innerLock.CheckIn(this.cacheData, true);
      }
    }

    private void updateTemplateXRefs(object template)
    {
      switch (template)
      {
        case EllieMae.EMLite.DataEngine.LoanTemplate _:
          this.updateLoanTemplateXRefs((EllieMae.EMLite.DataEngine.LoanTemplate) template);
          break;
        case LoanProgram _:
          this.updateLoanProgramXRefs((LoanProgram) template);
          break;
        case DocumentSetTemplate _:
          this.updateDocumentSetXRefs((DocumentSetTemplate) template);
          break;
        case StackingOrderSetTemplate _:
          this.updateStackingOrderSetXRefs((StackingOrderSetTemplate) template);
          break;
        case CampaignTemplate _:
          this.updateCampaignTemplateXRefs((CampaignTemplate) template);
          break;
        case TaskSetTemplate _:
          this.updateTaskSetXRefs((TaskSetTemplate) template);
          break;
        case ConditionalLetterPrintOption _:
          this.updateConditionLetterXrefs((ConditionalLetterPrintOption) template);
          break;
      }
    }

    private void updateConditionLetterXrefs(ConditionalLetterPrintOption letterOption)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [ConditionalLetterXRef] WHERE [LetterName] = " + SQL.Encode((object) letterOption.TemplateName));
      try
      {
        if (letterOption.StartingPages != string.Empty)
        {
          CustomLetterXRef customLetterXref = new CustomLetterXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(letterOption.StartingPages));
          letterOption.StartingPages = customLetterXref.Guid;
          dbQueryBuilder.AppendLine("INSERT INTO ConditionalLetterXRef([Guid], [LetterName], [XRef]) values(" + SQL.Encode((object) customLetterXref.Guid) + "," + SQL.Encode((object) letterOption.TemplateName) + "," + SQL.Encode((object) customLetterXref.XRef.ToString()) + ")");
        }
      }
      catch (Exception ex)
      {
        letterOption.StartingPages = string.Empty;
        TraceLog.WriteError(nameof (TemplateSettings), "Error converting starting pages to guid XRef: " + ex.Message);
      }
      try
      {
        if (letterOption.EndingPages != string.Empty)
        {
          CustomLetterXRef customLetterXref = new CustomLetterXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(letterOption.EndingPages));
          letterOption.EndingPages = customLetterXref.Guid;
          dbQueryBuilder.AppendLine("INSERT INTO ConditionalLetterXRef([Guid], [LetterName], [XRef]) values(" + SQL.Encode((object) customLetterXref.Guid) + "," + SQL.Encode((object) letterOption.TemplateName) + "," + SQL.Encode((object) customLetterXref.XRef.ToString()) + ")");
        }
      }
      catch (Exception ex)
      {
        letterOption.EndingPages = string.Empty;
        TraceLog.WriteError(nameof (TemplateSettings), "Error converting ending pages to guid XRef: " + ex.Message);
      }
      try
      {
        TraceLog.WriteDebug(nameof (TemplateSettings), "Insert Conditional Approval Letter Xref. SQL: " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        letterOption.StartingPages = string.Empty;
        letterOption.EndingPages = string.Empty;
        TraceLog.WriteError(nameof (TemplateSettings), "Error converting ending pages to guid XRef: " + ex.Message);
      }
    }

    private void updateLoanTemplateXRefs(EllieMae.EMLite.DataEngine.LoanTemplate lt)
    {
      this.saveXRefsToDatabase((TemplateXRef[]) new ArrayList()
      {
        (object) this.replaceLoanTemplateXRef(lt, "DOCSET", TemplateSettingsType.DocumentSet),
        (object) this.replaceLoanTemplateXRef(lt, "FORMLIST", TemplateSettingsType.FormList),
        (object) this.replaceLoanTemplateXRef(lt, "PROVIDERLIST", TemplateSettingsType.SettlementServiceProviders),
        (object) this.replaceLoanTemplateXRef(lt, "AFFILIATELIST", TemplateSettingsType.AffiliatedBusinessArrangements),
        (object) this.replaceLoanTemplateXRef(lt, "MISCDATA", TemplateSettingsType.MiscData),
        (object) this.replaceLoanTemplateXRef(lt, "PROGRAM", TemplateSettingsType.LoanProgram),
        (object) this.replaceLoanTemplateXRef(lt, "COST", TemplateSettingsType.ClosingCost),
        (object) this.replaceLoanTemplateXRef(lt, "TASKSET", TemplateSettingsType.TaskSet)
      }.ToArray(typeof (TemplateXRef)));
    }

    private TemplateXRef replaceLoanTemplateXRef(
      EllieMae.EMLite.DataEngine.LoanTemplate lt,
      string fieldName,
      TemplateSettingsType templateType)
    {
      string uri = lt.GetField(fieldName) ?? "";
      if (uri == "")
        return (TemplateXRef) null;
      try
      {
        TemplateXRef templateXref = new TemplateXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(uri, this.id.FileSystemEntry.Owner), templateType);
        lt.SetCurrentField(fieldName, templateXref.Guid);
        return templateXref;
      }
      catch
      {
        lt.SetCurrentField(fieldName, "");
        return (TemplateXRef) null;
      }
    }

    private void updateLoanProgramXRefs(LoanProgram lp)
    {
      string uri = lp.GetSimpleField("LP97") ?? "";
      if (uri == "")
      {
        this.saveXRefsToDatabase(new TemplateXRef[0]);
      }
      else
      {
        try
        {
          FileSystemEntry xref = FileSystemEntry.Parse(uri, this.id.FileSystemEntry.Owner);
          TemplateXRef templateXref = new TemplateXRef(Guid.NewGuid().ToString(), xref, TemplateSettingsType.ClosingCost);
          lp.SetCurrentField("LP97", templateXref.Guid);
          this.saveXRefsToDatabase(new TemplateXRef[1]
          {
            templateXref
          });
        }
        catch
        {
          lp.SetCurrentField("LP97", "");
          this.saveXRefsToDatabase(new TemplateXRef[0]);
        }
      }
    }

    private void updateCampaignTemplateXRefs(CampaignTemplate campaignTemplate)
    {
      List<TemplateXRef> templateXrefList = new List<TemplateXRef>();
      foreach (CampaignTemplate.CampaignStepTemplate campaignStepTemplate in (ArrayList) campaignTemplate.CampaignStepTemplates)
      {
        string field = campaignStepTemplate.GetField("DocumentId");
        if (string.Empty != field)
        {
          string guid = Guid.NewGuid().ToString();
          FileSystemEntry xref = FileSystemEntry.Parse(field);
          TemplateXRef templateXref = new TemplateXRef(guid, xref, TemplateSettingsType.CustomLetter);
          templateXrefList.Add(templateXref);
          campaignStepTemplate["DocumentId"] = (object) guid;
        }
      }
      if (0 >= templateXrefList.Count)
        return;
      this.saveXRefsToDatabase(templateXrefList.ToArray());
    }

    private void saveXRefsToDatabase(TemplateXRef[] xrefs)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TemplateXRef");
      DbValue dbValue1 = new DbValue("Source", (object) this.id.FileSystemEntry.ToString());
      DbValue dbValue2 = new DbValue("SourceType", (object) (int) this.id.Type);
      dbQueryBuilder.AppendLine("delete from TemplateXRef where SourceType = " + (object) (int) this.id.Type + " and Source like '" + SQL.Escape(this.id.FileSystemEntry.ToString()) + "'");
      for (int index = 0; index < xrefs.Length; ++index)
      {
        if (xrefs[index] != null)
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "Guid",
              (object) xrefs[index].Guid
            },
            dbValue1,
            dbValue2,
            {
              "XRef",
              (object) xrefs[index].XRef.ToString()
            },
            {
              "XRefType",
              (object) (int) xrefs[index].XRefType
            }
          }, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private Hashtable readXRefsFromDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef, XRefType from TemplateXRef where SourceType = " + (object) (int) this.id.Type + " and Source like '" + SQL.Escape(this.id.FileSystemEntry.ToString()) + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable hashtable = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        try
        {
          TemplateXRef templateXref = new TemplateXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()), (TemplateSettingsType) dataRow["XRefType"]);
          hashtable.Add((object) templateXref.Guid, (object) templateXref);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (TemplateSettings), "Error reading template XRef: " + (object) ex);
        }
      }
      return hashtable;
    }

    private void deleteXRefsFromDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (this.id.Type == TemplateSettingsType.ConditionalLetter)
        dbQueryBuilder.AppendLine("DELETE FROM [ConditionalLetterXRef] WHERE [LetterName] = " + SQL.Encode((object) this.id.FileSystemEntry.Name));
      dbQueryBuilder.AppendLine("delete from TemplateXRef where SourceType = " + (object) (int) this.id.Type + " and Source like '" + SQL.Escape(this.id.FileSystemEntry.ToString()) + "'");
      dbQueryBuilder.AppendLine("delete from TemplateXRef where XRefType = " + (object) (int) this.id.Type + " and XRef like '" + SQL.Escape(this.id.FileSystemEntry.ToString()) + "'");
      if (this.id.Type == TemplateSettingsType.StackingOrder)
      {
        dbQueryBuilder.AppendLine("delete from company_settings where Category = '" + this.id.Type.ToString() + "' and Value like '" + SQL.Escape(this.id.FileSystemEntry.ToString()) + "'");
      }
      else
      {
        UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
        UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues((string) null, this.id.Type.ToString(), (string) null);
        settingsAccessor.Delete(this.id.FileSystemEntry.ToString(), pk);
      }
      if (this.id.Type == TemplateSettingsType.LoanDuplicationTemplate)
        dbQueryBuilder.AppendLine("DELETE FROM [AclF_LoanDuplicationConfig] where [templateName] = '" + SQL.Escape(this.id.FileSystemEntry.Name) + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public BinaryObject ResolveXRefs(BinaryObject data)
    {
      if (this.id.Type == TemplateSettingsType.LoanTemplate)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveLoanTemplateXRefs((EllieMae.EMLite.DataEngine.LoanTemplate) data);
      if (this.id.Type == TemplateSettingsType.LoanProgram)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveLoanProgramXRefs((LoanProgram) data);
      if (this.id.Type == TemplateSettingsType.DocumentSet)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveDocumentSetXRefs((DocumentSetTemplate) data);
      if (this.id.Type == TemplateSettingsType.StackingOrder)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveStackingOrderSetXRefs((StackingOrderSetTemplate) data);
      if (this.id.Type == TemplateSettingsType.Campaign)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveCampaignTemplateXRefs((CampaignTemplate) data);
      if (this.id.Type == TemplateSettingsType.TaskSet)
        return (BinaryObject) (BinaryConvertibleObject) this.resolveTaskSetXRefs((TaskSetTemplate) data);
      return this.id.Type == TemplateSettingsType.ConditionalLetter ? (BinaryObject) (BinaryConvertibleObject) this.resolveConditionLetterXrefs((ConditionalLetterPrintOption) data) : data;
    }

    private ConditionalLetterPrintOption resolveConditionLetterXrefs(
      ConditionalLetterPrintOption letterOption)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef from ConditionalLetterXRef where LetterName = " + SQL.Encode((object) letterOption.TemplateName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable hashtable = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        try
        {
          CustomLetterXRef customLetterXref = new CustomLetterXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()));
          hashtable.Add((object) customLetterXref.Guid, (object) customLetterXref);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (TemplateSettings), "resolveConditionLetterXrefs:Error reading Conditional Approval Letter Xref: " + (object) ex);
          return letterOption;
        }
      }
      if (hashtable != null)
      {
        try
        {
          if (hashtable.ContainsKey((object) letterOption.StartingPages))
          {
            CustomLetterXRef customLetterXref = (CustomLetterXRef) hashtable[(object) letterOption.StartingPages];
            letterOption.StartingPages = customLetterXref.XRef.ToString();
          }
          else
            letterOption.StartingPages = string.Empty;
          if (hashtable.ContainsKey((object) letterOption.EndingPages))
          {
            CustomLetterXRef customLetterXref = (CustomLetterXRef) hashtable[(object) letterOption.EndingPages];
            letterOption.EndingPages = customLetterXref.XRef.ToString();
          }
          else
            letterOption.EndingPages = string.Empty;
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (TemplateSettings), "resolveConditionLetterXrefs:Error reading Conditional Approval Letter Xref: " + (object) ex);
        }
      }
      else
      {
        letterOption.StartingPages = string.Empty;
        letterOption.EndingPages = string.Empty;
      }
      return letterOption;
    }

    private DocumentSetTemplate resolveDocumentSetXRefs(DocumentSetTemplate ds)
    {
      Hashtable milestoneGuidToName = WorkflowBpmDbAccessor.GetCompleteMilestoneGUIDToName();
      Hashtable documentXrefMap = DocumentTrackingConfiguration.GetDocumentXRefMap(XRefKeyType.CustomMilestoneGuid);
      TemplateSettings.migrateDocumentSetMilestonNames(ds);
      TemplateSettings.translateDocumentSetXRefs(ds, milestoneGuidToName, documentXrefMap);
      return ds;
    }

    private TaskSetTemplate resolveTaskSetXRefs(TaskSetTemplate ds)
    {
      Hashtable milestoneGuidToName = WorkflowBpmDbAccessor.GetCompleteMilestoneGUIDToName();
      Hashtable taskXrefMap = MilestoneTaskAccessor.GetTaskXRefMap(XRefKeyType.CustomMilestoneGuid);
      TemplateSettings.translateTaskSetXRefs(ds, milestoneGuidToName, taskXrefMap);
      return ds;
    }

    private void updateDocumentSetXRefs(DocumentSetTemplate ds)
    {
      Hashtable milestoneNameToGuid = WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID();
      Hashtable documentXrefMap = DocumentTrackingConfiguration.GetDocumentXRefMap(XRefKeyType.CustomMilestoneName);
      TemplateSettings.translateDocumentSetXRefs(ds, milestoneNameToGuid, documentXrefMap);
    }

    private void updateTaskSetXRefs(TaskSetTemplate ds)
    {
      Hashtable milestoneNameToGuid = WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID();
      Hashtable taskXrefMap = MilestoneTaskAccessor.GetTaskXRefMap(XRefKeyType.CustomMilestoneName);
      TemplateSettings.translateTaskSetXRefs(ds, milestoneNameToGuid, taskXrefMap);
    }

    private void updateStackingOrderSetXRefs(StackingOrderSetTemplate ds)
    {
      Hashtable documentXrefMap = DocumentTrackingConfiguration.GetDocumentXRefMap(XRefKeyType.CustomMilestoneName);
      TemplateSettings.translateStackingOrderSetXRefs(ds, documentXrefMap);
    }

    private StackingOrderSetTemplate resolveStackingOrderSetXRefs(StackingOrderSetTemplate ds)
    {
      Hashtable documentXrefMap = DocumentTrackingConfiguration.GetDocumentXRefMap(XRefKeyType.CustomMilestoneGuid);
      TemplateSettings.translateStackingOrderSetXRefs(ds, documentXrefMap);
      return ds;
    }

    private static void migrateDocumentSetMilestonNames(DocumentSetTemplate ds)
    {
      string[] strArray = new string[ds.DocList.Count];
      ds.DocList.Keys.CopyTo((Array) strArray, 0);
      for (int index = 0; index < strArray.Length; ++index)
      {
        switch (strArray[index].ToLower())
        {
          case "approval":
            ds.RenameMilestoneInDocuments(strArray[index], "4");
            break;
          case "closing":
            ds.RenameMilestoneInDocuments(strArray[index], "7");
            break;
          case "completion":
            ds.RenameMilestoneInDocuments(strArray[index], "7");
            break;
          case "docs signing":
            ds.RenameMilestoneInDocuments(strArray[index], "5");
            break;
          case "funding":
            ds.RenameMilestoneInDocuments(strArray[index], "6");
            break;
          case "processing":
            ds.RenameMilestoneInDocuments(strArray[index], "2");
            break;
          case "started":
            ds.RenameMilestoneInDocuments(strArray[index], "1");
            break;
          case "submittal":
            ds.RenameMilestoneInDocuments(strArray[index], "3");
            break;
        }
      }
    }

    private static void translateDocumentSetXRefs(
      DocumentSetTemplate ds,
      Hashtable msXRefs,
      Hashtable docXRefs)
    {
      foreach (ArrayList arrayList in (IEnumerable) ds.DocList.Values)
      {
        for (int index = arrayList.Count - 1; index >= 0; --index)
        {
          string docXref = (string) docXRefs[arrayList[index]];
          if (docXref == null)
            arrayList.RemoveAt(index);
          else
            arrayList[index] = (object) docXref;
        }
      }
      string[] strArray = new string[ds.DocList.Count];
      ds.DocList.Keys.CopyTo((Array) strArray, 0);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != "")
        {
          string newName = (string) msXRefs[(object) strArray[index]] ?? "";
          ds.RenameMilestoneInDocuments(strArray[index], newName);
        }
      }
    }

    private static void translateTaskSetXRefs(
      TaskSetTemplate ds,
      Hashtable msXRefs,
      Hashtable taskXRefs)
    {
      foreach (ArrayList arrayList in (IEnumerable) ds.DocList.Values)
      {
        for (int index = arrayList.Count - 1; index >= 0; --index)
        {
          string taskXref = (string) taskXRefs[arrayList[index]];
          if (taskXref == null)
            arrayList.RemoveAt(index);
          else
            arrayList[index] = (object) taskXref;
        }
      }
      string[] strArray = new string[ds.DocList.Count];
      ds.DocList.Keys.CopyTo((Array) strArray, 0);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != "")
        {
          string newName = (string) msXRefs[(object) strArray[index]] ?? "";
          if (newName == "")
          {
            foreach (string str in (IEnumerable) msXRefs.Values)
            {
              if (str.ToLower().Equals(strArray[index].ToLower()))
              {
                newName = str;
                break;
              }
            }
          }
          ds.RenameMilestoneInTasks(strArray[index], newName);
        }
      }
    }

    private static void translateStackingOrderSetXRefs(
      StackingOrderSetTemplate ds,
      Hashtable docXRefs)
    {
      if (docXRefs.Contains((object) "VOD"))
        docXRefs.Remove((object) "VOD");
      if (docXRefs.Contains((object) "VOE"))
        docXRefs.Remove((object) "VOE");
      if (docXRefs.Contains((object) "VOL"))
        docXRefs.Remove((object) "VOL");
      if (docXRefs.Contains((object) "VOM"))
        docXRefs.Remove((object) "VOM");
      if (docXRefs.Contains((object) "VOR"))
        docXRefs.Remove((object) "VOR");
      docXRefs.Add((object) "VOD", (object) "VOD");
      docXRefs.Add((object) "VOE", (object) "VOE");
      docXRefs.Add((object) "VOL", (object) "VOL");
      docXRefs.Add((object) "VOM", (object) "VOM");
      docXRefs.Add((object) "VOR", (object) "VOR");
      for (int index = ds.DocNames.Count - 1; index >= 0; --index)
      {
        string docXref = (string) docXRefs[ds.DocNames[index]];
        if (docXref != null)
          ds.DocNames[index] = (object) docXref;
      }
    }

    private EllieMae.EMLite.DataEngine.LoanTemplate resolveLoanTemplateXRefs(EllieMae.EMLite.DataEngine.LoanTemplate lt)
    {
      Hashtable xrefs = this.readXRefsFromDatabase();
      this.restoreLoanTemplateXRef(lt, "DOCSET", xrefs, TemplateSettingsType.DocumentSet);
      this.restoreLoanTemplateXRef(lt, "FORMLIST", xrefs, TemplateSettingsType.FormList);
      this.restoreLoanTemplateXRef(lt, "PROVIDERLIST", xrefs, TemplateSettingsType.SettlementServiceProviders);
      this.restoreLoanTemplateXRef(lt, "AFFILIATELIST", xrefs, TemplateSettingsType.AffiliatedBusinessArrangements);
      this.restoreLoanTemplateXRef(lt, "MISCDATA", xrefs, TemplateSettingsType.MiscData);
      this.restoreLoanTemplateXRef(lt, "PROGRAM", xrefs, TemplateSettingsType.LoanProgram);
      this.restoreLoanTemplateXRef(lt, "COST", xrefs, TemplateSettingsType.ClosingCost);
      this.restoreLoanTemplateXRef(lt, "TASKSET", xrefs, TemplateSettingsType.TaskSet);
      return lt;
    }

    private void restoreLoanTemplateXRef(
      EllieMae.EMLite.DataEngine.LoanTemplate lt,
      string fieldName,
      Hashtable xrefs,
      TemplateSettingsType xrefType)
    {
      string str = lt.GetField(fieldName) ?? "";
      if (str == "")
        return;
      TemplateXRef xref = (TemplateXRef) xrefs[(object) str];
      if (xrefType == TemplateSettingsType.TaskSet && str.IndexOf("\\") > -1)
        lt.SetCurrentField(fieldName, str);
      else if (xref == null || xref.XRefType != xrefType)
        lt.SetCurrentField(fieldName, "");
      else if (xref.XRef.IsPublic || xref.XRef.Owner == this.id.FileSystemEntry.Owner)
        lt.SetCurrentField(fieldName, xref.XRef.ToDisplayString());
      else
        lt.SetCurrentField(fieldName, "");
    }

    private LoanProgram resolveLoanProgramXRefs(LoanProgram lp)
    {
      Hashtable hashtable = this.readXRefsFromDatabase();
      string key = lp.GetSimpleField("LP97") ?? "";
      if (key == "")
        return lp;
      TemplateXRef templateXref = (TemplateXRef) hashtable[(object) key];
      if (templateXref == null || templateXref.XRefType != TemplateSettingsType.ClosingCost)
        lp.SetCurrentField("LP97", "");
      else if (this.id.FileSystemEntry.IsPublic && !templateXref.XRef.IsPublic)
        lp.SetCurrentField("LP97", "");
      else if (!File.Exists(TemplateSettings.GetFilePath(templateXref.XRefType, templateXref.XRef)))
        lp.SetCurrentField("LP97", "");
      else
        lp.SetCurrentField("LP97", templateXref.XRef.ToDisplayString());
      return lp;
    }

    private CampaignTemplate resolveCampaignTemplateXRefs(CampaignTemplate campaignTemplate)
    {
      Hashtable hashtable = this.readXRefsFromDatabase();
      foreach (CampaignTemplate.CampaignStepTemplate campaignStepTemplate in (ArrayList) campaignTemplate.CampaignStepTemplates)
      {
        string field = campaignStepTemplate.GetField("DocumentId");
        if (string.Empty != field && hashtable.Contains((object) field))
        {
          TemplateXRef templateXref = (TemplateXRef) hashtable[(object) field];
          campaignStepTemplate["DocumentId"] = (object) templateXref.XRef.ToDisplayString();
        }
      }
      return campaignTemplate;
    }

    public static void MoveTemplateXRefs(
      TemplateSettingsType sourceType,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      if (TemplateSettingsType.DashboardTemplate == sourceType)
        Dashboard.UpdateTemplatePath(source, target);
      else if (TemplateSettingsType.DashboardViewTemplate == sourceType)
      {
        Dashboard.UpdateViewTemplatePath(source, target);
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (source.Type == FileSystemEntry.Types.File)
        {
          dbQueryBuilder.AppendLine("update TemplateXRef set Source = " + SQL.Encode((object) target.ToString()) + " where (Source like '" + SQL.Escape(source.ToString()) + "') and (SourceType = " + (object) (int) sourceType + ")");
          dbQueryBuilder.AppendLine("update TemplateXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "') and (XRefType = " + (object) (int) sourceType + ")");
          if (sourceType == TemplateSettingsType.StackingOrder)
          {
            dbQueryBuilder.AppendLine("update company_settings set Value = " + SQL.Encode((object) target.ToString()) + " where (Category = '" + (object) sourceType + "') and (Value like '" + SQL.Escape(source.ToString()) + "')");
          }
          else
          {
            UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
            UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues((string) null, sourceType.ToString(), (string) null);
            settingsAccessor.UpdateValues(source.ToString(), target.ToString(), pk);
          }
        }
        else
        {
          dbQueryBuilder.Declare("@sourceLen", "int");
          dbQueryBuilder.SelectVar("@sourceLen", (object) source.ToString().Length);
          dbQueryBuilder.AppendLine("update TemplateXRef set Source = (" + SQL.Encode((object) target.ToString()) + " + substring(Source, @sourceLen + 1, Len(Source) - @sourceLen))  where (Source like '" + SQL.Escape(source.ToString()) + "%') and (SourceType = " + (object) (int) sourceType + ")");
          dbQueryBuilder.AppendLine("update TemplateXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%') and (XRefType = " + (object) (int) sourceType + ")");
          UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
          UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues((string) null, sourceType.ToString(), (string) null);
          settingsAccessor.ReplacePrefixInValues(source.ToString(), target.ToString(), pk);
        }
        dbQueryBuilder.AppendLine("delete from TemplateXRef where Source like 'Public:%' and XRef like 'Personal:%'");
        if (sourceType == TemplateSettingsType.LoanDuplicationTemplate)
          dbQueryBuilder.AppendLine("update [AclF_LoanDuplicationConfig] set [templateName] = " + SQL.Encode((object) target.Name) + " WHERE [templateName] = " + SQL.Encode((object) source.Name));
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static string GetFilePath(TemplateSettingsType type, FileSystemEntry entry)
    {
      return TemplateSettings.GetFolderPath(type, entry) + ".xml";
    }

    private static string getTemplateRoot(TemplateSettingsType type, string userId)
    {
      ClientContext current = ClientContext.GetCurrent();
      string templateRoot;
      if (type == TemplateSettingsType.CustomLetter)
      {
        if (userId == null)
          return current.Settings.GetDataFolderPath("CustomLetters");
        templateRoot = current.Settings.GetUserDataFolderPath(userId, "CustomLetters");
      }
      else
        templateRoot = userId != null ? current.Settings.GetUserDataFolderPath(userId, "TemplateSettings\\" + type.ToString()) : current.Settings.GetDataFolderPath("TemplateSettings\\" + type.ToString());
      return templateRoot;
    }

    public static string GetFolderPath(TemplateSettingsType type, FileSystemEntry entry)
    {
      string encodedPath = entry.GetEncodedPath();
      if (!DataFile.IsValidSubobjectName(encodedPath))
        Err.Raise(TraceLevel.Error, nameof (TemplateSettings), (ServerException) new ServerArgumentException("Invalid object name: \"" + encodedPath + "\""));
      return SystemUtil.CombinePath(TemplateSettings.getTemplateRoot(type, entry.Owner), encodedPath);
    }

    public static bool IsExists(TemplateSettingsType type, FileSystemEntry entry)
    {
      string encodedPath = entry.GetEncodedPath();
      if (!DataFile.IsValidSubobjectName(encodedPath))
        return false;
      return File.Exists(SystemUtil.CombinePath(TemplateSettings.getTemplateRoot(type, entry.Owner), encodedPath) + ".xml");
    }

    public static bool IsValidName(string name) => FileSystem.IsValidName(name);

    public static string DecodeViewId(string viewId, bool validate = true)
    {
      string name = UrlSafeBase64Encoding.Decode(viewId);
      return validate && !TemplateSettings.IsValidName(name) ? (string) null : name;
    }

    public static string EncodeForViewId(string viewName) => UrlSafeBase64Encoding.Encode(viewName);
  }
}
