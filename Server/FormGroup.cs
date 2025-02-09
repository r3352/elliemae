// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FormGroup
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class FormGroup : IDisposable
  {
    private const string className = "FormGroup�";
    private const string formGroupsFolderName = "FormGroups�";
    private FileSystemEntry fsEntry;
    private DataFile groupFile;
    private bool disposed;
    private FormInfo[] forms;

    public FormGroup(FileSystemEntry fsEntry)
    {
      this.fsEntry = fsEntry;
      this.groupFile = FileStore.CheckOut(FormGroup.GetFilePath(fsEntry));
    }

    public FileSystemEntry FileSystemEntry => this.fsEntry;

    public string PhysicalPath => this.groupFile.Path;

    public bool Exists => this.groupFile.Exists;

    public FormInfo[] Forms
    {
      get
      {
        this.validateInstance();
        if (this.forms == null)
          this.forms = this.getFormsFromXml(this.groupFile, this.readXRefsFromDatabase());
        return this.forms;
      }
    }

    public DataFile GroupFile => this.groupFile;

    public FileSystemEntry[] GetCustomLetterXRefs()
    {
      this.validateInstance();
      Hashtable hashtable = this.readXRefsFromDatabase();
      FileSystemEntry[] customLetterXrefs = new FileSystemEntry[hashtable.Count];
      int num = 0;
      foreach (CustomLetterXRef customLetterXref in (IEnumerable) hashtable.Values)
        customLetterXrefs[num++] = customLetterXref.XRef;
      return customLetterXrefs;
    }

    public void Delete()
    {
      this.validateInstance(false);
      if (this.groupFile.Exists)
        this.groupFile.Delete();
      this.deleteXRefsFromDatabase();
      PrintSelectionXrefStore.DeleteFormGroupXRefs(this.fsEntry);
      this.Dispose();
    }

    public void CheckIn(FormInfo[] newForms) => this.CheckIn(newForms, false);

    public void CheckIn(FormInfo[] newForms, bool keepCheckedOut)
    {
      ArrayList xrefs = (ArrayList) null;
      XmlDocument xmlDocument = this.createXmlDocument(newForms, out xrefs);
      this.saveXRefsToDatabase((CustomLetterXRef[]) xrefs.ToArray(typeof (CustomLetterXRef)));
      this.groupFile.CheckIn(new BinaryObject(xmlDocument.OuterXml, Encoding.Default), true);
      this.forms = newForms;
    }

    public void CheckIn(BinaryObject rawData) => this.groupFile.CheckIn(rawData, false);

    public void UndoCheckout() => this.Dispose();

    public void Dispose()
    {
      try
      {
        this.disposed = true;
        this.groupFile.Dispose();
      }
      catch
      {
      }
    }

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool ensureExists)
    {
      if (this.disposed)
        Err.Raise(TraceLevel.Error, nameof (FormGroup), new ServerException("Object is disposed"));
      if (!ensureExists || this.groupFile.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (FormGroup), new ServerException("Object must exist for this operation"));
    }

    public static string GetFilePath(FileSystemEntry entry)
    {
      return FormGroup.GetFolderPath(entry) + ".xml";
    }

    public static string GetFolderPath(FileSystemEntry entry)
    {
      string encodedPath = entry.GetEncodedPath();
      if (!DataFile.IsValidSubobjectName(encodedPath))
        Err.Raise(TraceLevel.Error, nameof (FormGroup), (ServerException) new ServerArgumentException("Invalid object name: \"" + encodedPath + "\""));
      string path = SystemUtil.CombinePath(entry.Owner != null ? ClientContext.GetCurrent().Settings.GetUserDataFolderPath(entry.Owner, "FormGroups") : ClientContext.GetCurrent().Settings.GetDataFolderPath("FormGroups"), encodedPath);
      if (entry.Path == "\\")
        Directory.CreateDirectory(path);
      return path;
    }

    private XmlDocument createXmlDocument(FormInfo[] forms, out ArrayList xrefs)
    {
      xrefs = new ArrayList();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<Forms version=\"2.8.0\"></Forms>");
      if (forms == null || forms.Length == 0)
        return xmlDocument;
      XmlElement documentElement = xmlDocument.DocumentElement;
      XmlElement element1 = xmlDocument.CreateElement("Element");
      element1.SetAttribute("name", "DTDESC");
      if (this.fsEntry.Properties.ContainsKey((object) "Description"))
        element1.InnerText = this.fsEntry.Properties[(object) "Description"].ToString();
      else
        element1.InnerText = "";
      documentElement.AppendChild((XmlNode) element1);
      for (int index = 0; index < forms.Length; ++index)
      {
        if (forms[index].Type == OutputFormType.CustomLetters)
        {
          CustomLetterXRef xref = this.generateXRef(forms[index].Name);
          if (xref != null)
          {
            XmlElement element2 = xmlDocument.CreateElement("Form");
            element2.SetAttribute("guid", xref.Guid);
            element2.SetAttribute("type", forms[index].Type.ToString());
            documentElement.AppendChild((XmlNode) element2);
            xrefs.Add((object) xref);
          }
        }
        else if (forms[index].Type != OutputFormType.Documents)
        {
          XmlElement element3 = xmlDocument.CreateElement("Form");
          element3.SetAttribute("name", forms[index].Name);
          element3.SetAttribute("type", forms[index].Type.ToString());
          element3.SetAttribute("MergeLocation", forms[index].MergeLocation.ToString());
          XmlElement xml = forms[index].MergeParams.ToXml();
          if (xml != null)
            element3.AppendChild(element3.OwnerDocument.ImportNode((XmlNode) xml, true));
          documentElement.AppendChild((XmlNode) element3);
        }
      }
      return xmlDocument;
    }

    public string FormDescription
    {
      get
      {
        try
        {
          string text = this.groupFile.GetText(Encoding.Default);
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(text);
          XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("Element[@name = \"DTDESC\"]");
          if (xmlNode != null)
            return xmlNode.InnerText;
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (FormGroup), "Error reading form group description " + this.fsEntry.Name + ". Error: " + (object) ex);
        }
        return string.Empty;
      }
    }

    private FormInfo[] getFormsFromXml(XmlDocument xmlDoc, Hashtable xrefs)
    {
      XmlNodeList elementsByTagName = xmlDoc.DocumentElement.GetElementsByTagName("Form");
      ArrayList arrayList = new ArrayList();
      for (int i = 0; i < elementsByTagName.Count; ++i)
      {
        XmlElement xmlElement = (XmlElement) elementsByTagName[i];
        OutputFormType type = (OutputFormType) Enum.Parse(typeof (OutputFormType), xmlElement.GetAttribute("type"), true);
        if (type == OutputFormType.CustomLetters)
        {
          string key = xmlElement.GetAttribute("guid") ?? "";
          CustomLetterXRef xref = (CustomLetterXRef) xrefs[(object) key];
          if (xref != null && (xref.XRef.IsPublic || xref.XRef.Owner == this.fsEntry.Owner) && CustomLetterStore.Exists(CustomLetterType.Generic, xref.XRef))
            arrayList.Add((object) new FormInfo(xref.XRef.ToDisplayString(), type));
        }
        else
        {
          FormInfo formInfo = new FormInfo(xmlElement.GetAttribute("name"), type);
          PrintForm.MergeLocationValues result = PrintForm.MergeLocationValues.Local;
          formInfo.MergeLocation = result;
          if (Enum.TryParse<PrintForm.MergeLocationValues>(xmlElement.GetAttribute("MergeLocation"), true, out result))
            formInfo.MergeLocation = result;
          XmlElement paramsNode = (XmlElement) xmlElement.SelectSingleNode("MergeParams");
          if (paramsNode != null)
            formInfo.MergeParams = new MergeParamValues(paramsNode);
          arrayList.Add((object) formInfo);
        }
      }
      return (FormInfo[]) arrayList.ToArray(typeof (FormInfo));
    }

    private CustomLetterXRef generateXRef(string path)
    {
      try
      {
        return new CustomLetterXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(path, this.fsEntry.Owner));
      }
      catch
      {
        return (CustomLetterXRef) null;
      }
    }

    private FormInfo[] getFormsFromXml(DataFile xmlfile, Hashtable xrefs)
    {
      string text = xmlfile.GetText(Encoding.Default);
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(text);
      return this.getFormsFromXml(xmlDoc, xrefs);
    }

    private void saveXRefsToDatabase(CustomLetterXRef[] xrefs)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("FormGroupXRef");
      dbQueryBuilder.AppendLine("delete from FormGroupXRef where FormGroup like '" + SQL.Escape(this.fsEntry.ToString()) + "'");
      for (int index = 0; index < xrefs.Length; ++index)
      {
        if (xrefs[index] != null)
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "Guid",
              (object) xrefs[index].Guid
            },
            {
              nameof (FormGroup),
              (object) this.fsEntry.ToString()
            },
            {
              "XRef",
              (object) xrefs[index].XRef.ToString()
            }
          }, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private Hashtable readXRefsFromDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef from FormGroupXRef where FormGroup like '" + SQL.Escape(this.fsEntry.ToString()) + "'");
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
          TraceLog.WriteError(nameof (FormGroup), "Error reading form group XRef: " + (object) ex);
        }
      }
      return hashtable;
    }

    private void deleteXRefsFromDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from FormGroupXRef where FormGroup like '" + SQL.Escape(this.fsEntry.ToString()) + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void MoveCustomLetterXRefs(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (source.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine("update FormGroupXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "')");
      }
      else
      {
        dbQueryBuilder.Declare("@sourceLen", "int");
        dbQueryBuilder.SelectVar("@sourceLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("update FormGroupXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%')");
      }
      dbQueryBuilder.Append("delete from FormGroupXRef where FormGroup like 'Public:%' and XRef like 'Personal:%'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteCustomLetterXRefs(FileSystemEntry letterEntry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from FormGroupXRef where (XRef like '" + SQL.Escape(letterEntry.ToString()) + "')");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void MoveFormGroupXRefs(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (source.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine("update FormGroupXRef set FormGroup = " + SQL.Encode((object) target.ToString()) + " where (FormGroup like '" + SQL.Escape(source.ToString()) + "')");
      }
      else
      {
        dbQueryBuilder.Declare("@sourceLen", "int");
        dbQueryBuilder.SelectVar("@sourceLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("update FormGroupXRef set FormGroup = (" + SQL.Encode((object) target.ToString()) + " + substring(FormGroup, @sourceLen + 1, Len(FormGroup) - @sourceLen))  where (FormGroup like '" + SQL.Escape(source.ToString()) + "%')");
      }
      dbQueryBuilder.Append("delete from FormGroupXRef where FormGroup like 'Public:%' and XRef like 'Personal:%'");
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
