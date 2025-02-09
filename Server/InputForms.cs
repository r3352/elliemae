// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.InputForms
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class InputForms
  {
    private const string className = "InputForms�";

    private InputForms()
    {
    }

    public static InputFormInfo[] GetAllFormInfos()
    {
      ClientContext current = ClientContext.GetCurrent();
      ArrayList formsFromDatabase = (ArrayList) current.Cache.Get(nameof (InputForms));
      if (formsFromDatabase == null)
      {
        try
        {
          using (current.Cache.Lock(nameof (InputForms), timeout: ServerGlobals.LockTimeoutDuringGetCache))
          {
            if (ServerGlobals.CacheRegetFromCache)
              formsFromDatabase = (ArrayList) current.Cache.Get(nameof (InputForms));
            if (formsFromDatabase == null)
            {
              formsFromDatabase = InputForms.getAllFormsFromDatabase();
              current.Cache.Put(nameof (InputForms), (object) formsFromDatabase, CacheSetting.Low);
            }
          }
        }
        catch (TimeoutException ex)
        {
          try
          {
            TraceLog.WriteWarning(nameof (InputForms), "Timeout expired while acquiring lock on InputForms");
          }
          catch
          {
          }
          if (ServerGlobals.CacheRegetFromDB)
            formsFromDatabase = InputForms.getAllFormsFromDatabase();
          else
            throw;
        }
        catch (ApplicationException ex)
        {
          if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
          {
            try
            {
              TraceLog.WriteWarning(nameof (InputForms), "Timeout expired while acquiring lock on InputForms");
            }
            catch
            {
            }
            if (ServerGlobals.CacheRegetFromDB)
              formsFromDatabase = InputForms.getAllFormsFromDatabase();
            else
              throw;
          }
          else
            throw;
        }
      }
      return (InputFormInfo[]) formsFromDatabase.ToArray(typeof (InputFormInfo));
    }

    public static InputFormInfo[] GetFormInfos(InputFormType formType)
    {
      ArrayList arrayList = new ArrayList((ICollection) InputForms.GetAllFormInfos());
      if (formType != InputFormType.All)
      {
        for (int index = arrayList.Count - 1; index >= 0; --index)
        {
          if ((((InputFormInfo) arrayList[index]).Type & formType) == InputFormType.None)
            arrayList.RemoveAt(index);
        }
      }
      return (InputFormInfo[]) arrayList.ToArray(typeof (InputFormInfo));
    }

    public static InputFormInfo[] GetFormInfos(InputFormType formType, InputFormCategory category)
    {
      ArrayList arrayList = new ArrayList((ICollection) InputForms.GetAllFormInfos());
      for (int index = arrayList.Count - 1; index >= 0; --index)
      {
        InputFormInfo inputFormInfo = (InputFormInfo) arrayList[index];
        if ((inputFormInfo.Type & formType) == InputFormType.None || (inputFormInfo.Category & category) == InputFormCategory.None)
          arrayList.RemoveAt(index);
      }
      return (InputFormInfo[]) arrayList.ToArray(typeof (InputFormInfo));
    }

    public static InputFormInfo[] GetFormInfos(string[] formIds)
    {
      ArrayList arrayList1 = new ArrayList((ICollection) InputForms.GetAllFormInfos());
      Hashtable hashtable = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
      foreach (InputFormInfo inputFormInfo in arrayList1)
        hashtable[(object) inputFormInfo.FormID] = (object) inputFormInfo;
      ArrayList arrayList2 = new ArrayList();
      foreach (string formId in formIds)
      {
        InputFormInfo inputFormInfo = (InputFormInfo) hashtable[(object) formId];
        if (inputFormInfo != (InputFormInfo) null && !arrayList2.Contains((object) inputFormInfo))
          arrayList2.Add((object) inputFormInfo);
      }
      return (InputFormInfo[]) arrayList2.ToArray(typeof (InputFormInfo));
    }

    public static InputFormInfo GetFormInfo(string formId)
    {
      ArrayList arrayList = (ArrayList) ClientContext.GetCurrent().Cache.Get(nameof (InputForms));
      if (arrayList == null)
        return InputForms.getFormFromDatabase(formId);
      foreach (InputFormInfo formInfo in arrayList)
      {
        if (string.Compare(formInfo.FormID, formId, true) == 0)
          return formInfo;
      }
      return (InputFormInfo) null;
    }

    public static InputFormInfo GetFormInfoByName(string formName)
    {
      foreach (InputFormInfo allFormInfo in InputForms.GetAllFormInfos())
      {
        if (string.Compare(allFormInfo.Name, formName, true) == 0)
          return allFormInfo;
      }
      return (InputFormInfo) null;
    }

    public static BinaryObject GetCustomForm(string formId)
    {
      InputFormInfo formInfo = InputForms.GetFormInfo(formId);
      if (formInfo == (InputFormInfo) null)
        return (BinaryObject) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(InputForms.getCustomFormPath(formInfo.Name)))
        return latestVersion.Exists ? latestVersion.GetData() : (BinaryObject) null;
    }

    public static void SaveCustomForm(InputFormInfo formInfo, BinaryObject formData)
    {
      if (formInfo.Type != InputFormType.Custom)
        Err.Raise(TraceLevel.Error, nameof (InputForms), (ServerException) new ServerArgumentException("The specified form is not a custom form"));
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (InputForms)))
      {
        using (DataFile dataFile = FileStore.CheckOut(InputForms.getCustomFormPath(formInfo.Name), MutexAccess.Write))
          dataFile.CheckIn(formData);
        InputForms.updateFormInDatabase(formInfo);
        current.Cache.Remove(nameof (InputForms));
      }
    }

    public static void RenameForm(string formId, string newMName)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (InputForms)))
      {
        InputFormInfo formInfo = InputForms.GetFormInfo(formId);
        if (formInfo == (InputFormInfo) null)
          Err.Raise(TraceLevel.Warning, nameof (InputForms), (ServerException) new ObjectNotFoundException("The form '" + formId + "' was not found", ObjectType.CustomForm, (object) formId));
        InputFormInfo inputFormInfo = new InputFormInfo(newMName);
        InputFormInfo formInfoByName = InputForms.GetFormInfoByName(inputFormInfo.Name);
        if (formInfoByName != (InputFormInfo) null && string.Compare(formInfoByName.FormID, formId, true) != 0)
          Err.Raise(TraceLevel.Warning, nameof (InputForms), (ServerException) new DuplicateObjectException("A form already exists with the name '" + inputFormInfo.Name + "'", ObjectType.CustomForm, (object) inputFormInfo.Name));
        InputForms.updateFormNameInDatabase(formId, newMName);
        if (InputForms.GetFormInfo(formId).Type == InputFormType.Custom && formInfo.Name.ToLower() != inputFormInfo.Name.ToLower())
          File.Move(InputForms.getCustomFormPath(formInfo.Name), InputForms.getCustomFormPath(inputFormInfo.Name));
        current.Cache.Remove(nameof (InputForms));
      }
    }

    public static void DeleteCustomForm(string formId)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (InputForms)))
      {
        InputFormInfo formInfo = InputForms.GetFormInfo(formId);
        if (formInfo == (InputFormInfo) null)
          Err.Raise(TraceLevel.Warning, nameof (InputForms), (ServerException) new ObjectNotFoundException("The form '" + formId + "' was not found", ObjectType.CustomForm, (object) formId));
        if (formInfo.Type != InputFormType.Custom)
          Err.Raise(TraceLevel.Error, nameof (InputForms), (ServerException) new SecurityException("The form '" + formId + "' cannot be deleted from the system."));
        InputForms.deleteFormFromDatabase(formId);
        try
        {
          FileStore.Delete(InputForms.getCustomFormPath(formInfo.Name));
        }
        catch
        {
        }
        current.Cache.Remove(nameof (InputForms));
      }
    }

    public static DateTime GetCustomFormModificationDate(string formId)
    {
      InputFormInfo formInfo = InputForms.GetFormInfo(formId);
      if (formInfo == (InputFormInfo) null)
        return DateTime.MinValue;
      using (DataFile latestVersion = FileStore.GetLatestVersion(InputForms.getCustomFormPath(formInfo.Name)))
        return !latestVersion.Exists ? DateTime.MinValue : latestVersion.LastModified;
    }

    public static void SetFormOrder(InputFormInfo[] forms)
    {
      ClientContext current = ClientContext.GetCurrent();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update InputForms set FormOrder = -1 * FormOrder where FormOrder > 0");
      string empty = string.Empty;
      for (int index = 0; index < forms.Length; ++index)
      {
        string str = forms[index].IsDefault ? "1" : "0";
        dbQueryBuilder.AppendLine("update InputForms set FormOrder = " + (object) (index + 1) + ", IsDefault = " + str + " where FormID = " + SQL.EncodeString(forms[index].FormID, 38));
      }
      dbQueryBuilder.AppendLine("update InputForms set FormOrder = -1 * FormOrder + " + (object) forms.Length + " where FormOrder < 0");
      using (current.Cache.Lock(nameof (InputForms)))
      {
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove(nameof (InputForms));
      }
    }

    public static string[] GetCustomFormAssemblyNames()
    {
      ArrayList arrayList = new ArrayList();
      foreach (string file in Directory.GetFiles(InputForms.getFormAssemblyRoot(), "*.dll"))
        arrayList.Add((object) Path.GetFileNameWithoutExtension(file));
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static BinaryObject GetCustomFormAssembly(string assemblyName)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(InputForms.getFormAssemblyPath(assemblyName)))
        return latestVersion.Exists ? latestVersion.GetData() : (BinaryObject) null;
    }

    public static void SaveCustomFormAssembly(string assemblyName, BinaryObject assemblyData)
    {
      using (DataFile dataFile = FileStore.CheckOut(InputForms.getFormAssemblyPath(assemblyName), MutexAccess.Write))
        dataFile.CheckIn(assemblyData);
    }

    public static Version GetCustomFormAssemblyFileVersion(string assemblyName)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(InputForms.getFormAssemblyPath(assemblyName)))
        return !latestVersion.Exists ? new Version(0, 0) : latestVersion.FileVersion;
    }

    private static string getFormAssemblyRoot()
    {
      return ClientContext.GetCurrent().Settings.GetDataFolderPath(nameof (InputForms));
    }

    private static string getCustomFormPath(string formName)
    {
      return Path.Combine(InputForms.getFormAssemblyRoot(), FileSystem.EncodeFilename(formName, false) + ".emfrm");
    }

    private static string getFormAssemblyPath(string assemblyName)
    {
      return Path.Combine(InputForms.getFormAssemblyRoot(), assemblyName + ".dll");
    }

    private static InputFormInfo getFormFromDatabase(string formId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(nameof (InputForms)), new DbValue("FormID", (object) formId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count > 0 ? InputForms.DataRowToInputFormInfo(dataRowCollection[0]) : (InputFormInfo) null;
    }

    [PgReady]
    private static ArrayList getAllFormsFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        string text = "Select * From InputForms" + " order by FormOrder ";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine(text);
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        EncompassEdition currentEdition = Company.GetCurrentEdition();
        ArrayList formsFromDatabase = new ArrayList();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          InputFormInfo inputFormInfo = InputForms.DataRowToInputFormInfo(row);
          if (inputFormInfo.Type != InputFormType.Custom || currentEdition == EncompassEdition.Banker)
            formsFromDatabase.Add((object) inputFormInfo);
        }
        return formsFromDatabase;
      }
      string text1 = "Select * From InputForms" + " order by FormOrder ";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(text1);
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      EncompassEdition currentEdition1 = Company.GetCurrentEdition();
      ArrayList formsFromDatabase1 = new ArrayList();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
      {
        InputFormInfo inputFormInfo = InputForms.DataRowToInputFormInfo(row);
        if (inputFormInfo.Type != InputFormType.Custom || currentEdition1 == EncompassEdition.Banker)
          formsFromDatabase1.Add((object) inputFormInfo);
      }
      return formsFromDatabase1;
    }

    [PgReady]
    public static InputFormInfo DataRowToInputFormInfo(DataRow r)
    {
      return ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres ? new InputFormInfo(r["FormID"].ToString(), r["Name"].ToString(), (InputFormType) r["Type"], (InputFormCategory) r["Category"], (int) r["FormOrder"], SQL.DecodeBoolean(r["IsDefault"]), SQL.DecodeBoolean(r["CanPickField"])) : new InputFormInfo(r["FormID"].ToString(), r["Name"].ToString(), (InputFormType) r["Type"], (InputFormCategory) r["Category"], (int) r["FormOrder"], (bool) r["IsDefault"], (bool) r["CanPickField"]);
    }

    private static void updateFormInDatabase(InputFormInfo form)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (InputForms));
      DbValueList dbValueList = InputForms.createDbValueList(form);
      DbValue dbValue = new DbValue("FormID", (object) form.FormID);
      dbQueryBuilder.Declare("@formOrder", "int");
      dbQueryBuilder.SelectVar("@formOrder", (object) "Max(FormOrder) + 1 from InputForms", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.IfExists(table, dbValue);
      dbQueryBuilder.Update(table, dbValueList, dbValue);
      dbQueryBuilder.Else();
      dbValueList.Add(dbValue);
      dbValueList.Add("FormOrder", (object) "@formOrder", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList createDbValueList(InputFormInfo form)
    {
      return new DbValueList()
      {
        {
          "Name",
          (object) form.MnemonicName
        },
        {
          "Type",
          (object) (int) form.Type
        },
        {
          "Category",
          (object) (int) form.Category
        }
      };
    }

    private static void deleteFormFromDatabase(string formId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (InputForms));
      dbQueryBuilder.DeleteFrom(table, new DbValue("FormID", (object) formId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void updateFormNameInDatabase(string formId, string formName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (InputForms));
      dbQueryBuilder.Update(table, new DbValueList()
      {
        {
          "Name",
          (object) formName
        }
      }, new DbValue("FormID", (object) formId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Dictionary<string, string> GetFormBinary(string inputFormId)
    {
      Dictionary<string, string> formBinary = new Dictionary<string, string>();
      string path1 = Path.Combine(InputForms.getFormAssemblyRoot(), inputFormId + ".EMFRM");
      string str = Path.Combine(SystemSettings.TempFolderRoot, "Forms\\" + FileSystem.EncodeFilename(inputFormId, false));
      if (!File.Exists(path1))
        return (Dictionary<string, string>) null;
      using (ZipInputStream zipInputStream = new ZipInputStream((Stream) File.OpenRead(path1)))
      {
        ZipEntry nextEntry;
        while ((nextEntry = zipInputStream.GetNextEntry()) != null)
        {
          string path2 = str + "\\" + Path.GetDirectoryName(nextEntry.Name);
          string fileName = Path.GetFileName(nextEntry.Name);
          if (path2.Length > 0 && !Directory.Exists(path2))
            Directory.CreateDirectory(path2);
          if (fileName != string.Empty)
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              byte[] buffer = new byte[2048];
              while (true)
              {
                int count;
                try
                {
                  count = zipInputStream.Read(buffer, 0, buffer.Length);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                  count = 0;
                }
                if (count > 0)
                  memoryStream.Write(buffer, 0, count);
                else
                  break;
              }
              formBinary.Add(nextEntry.Name, Encoding.Default.GetString(memoryStream.ToArray()));
            }
          }
        }
        zipInputStream.Close();
      }
      return formBinary;
    }
  }
}
