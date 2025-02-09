// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.StandardFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Threading;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class StandardFields : ISerializable
  {
    private const string className = "StandardFields";
    private static readonly string sw = Tracing.SwDataEngine;
    public static string MapFilePath = (string) null;
    private static StandardFields instance = (StandardFields) null;
    private TimeSpan lockTimeout = TimeSpan.FromSeconds(30.0);
    private static object mapFileLock = new object();
    private string xmlData;
    private ReaderWriterLock fieldListLock = new ReaderWriterLock();
    private ReaderWriterLockSlim fieldListLockSlim;
    private Dictionary<StandardFields.FieldType, FieldDefinitionCollection> fieldDefinitions = new Dictionary<StandardFields.FieldType, FieldDefinitionCollection>();
    private string filePath;
    private Version fileVersion;

    static StandardFields()
    {
      StandardFields.MapFilePath = !AssemblyResolver.IsSmartClient ? SystemSettings.LocalAppDir + SystemSettings.MapFileRelPath : AssemblyResolver.GetResourceFileFullPath(SystemSettings.MapFileRelPath);
      StandardFields.instance = new StandardFields(StandardFields.MapFilePath);
    }

    public static void Initialize()
    {
    }

    public static StandardFields Instance => StandardFields.instance;

    public static FieldDefinitionCollection AllVirtualFields
    {
      get => StandardFields.instance.VirtualFields;
    }

    public static FieldDefinitionCollection All => StandardFields.instance.AllFields;

    internal static FieldDefinitionCollection CommonFields => StandardFields.instance.CommonFieldsI;

    internal static FieldDefinitionCollection BorrowerFields
    {
      get => StandardFields.instance.BorrowerFieldsI;
    }

    internal static FieldDefinitionCollection CoborrowerFields
    {
      get => StandardFields.instance.CoborrowerFieldsI;
    }

    public static StandardField GetField(string fieldId) => StandardFields.GetField(fieldId, true);

    public static StandardField GetField(string fieldId, bool resolveInstance)
    {
      return StandardFields.instance.GetFieldI(fieldId, resolveInstance);
    }

    public static bool IsCustomField(string id)
    {
      id = id.ToUpper();
      return id.StartsWith("CX.") || id.StartsWith("CUST") && id.EndsWith("FV");
    }

    public static void AcquireFileLock() => Monitor.Enter(StandardFields.mapFileLock);

    public static void ReleaseFileLock() => Monitor.Exit(StandardFields.mapFileLock);

    public StandardFields()
      : this(StandardFields.MapFilePath)
    {
    }

    public StandardFields(string mapFilePath)
    {
      this.initializeReaderWriterLockSlim();
      this.filePath = mapFilePath;
      this.Reload(true);
    }

    public StandardFields(SerializationInfo info, StreamingContext context)
    {
      this.initializeReaderWriterLockSlim();
      this.xmlData = info.GetString("xml");
      this.filePath = info.GetString("path");
      this.fileVersion = (Version) info.GetValue("ver", typeof (Version));
      XmlDocument xml = new XmlDocument();
      xml.LoadXml(this.xmlData);
      this.readStandardFieldList(xml);
      this.loadCategoryFieldLists();
    }

    private void initializeReaderWriterLockSlim()
    {
      if (!SmartClientUtils.UseReaderWriterLockSlim)
        return;
      if (SmartClientUtils.LockSlimNoRecursionStandardFields)
        this.fieldListLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      else
        this.fieldListLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    }

    public FieldDefinitionCollection VirtualFields => this[StandardFields.FieldType.Virtual];

    public FieldDefinitionCollection AllFields => this[StandardFields.FieldType.All];

    public FieldDefinitionCollection CommonFieldsI => this[StandardFields.FieldType.Common];

    public FieldDefinitionCollection BorrowerFieldsI => this[StandardFields.FieldType.Borrower];

    public FieldDefinitionCollection CoborrowerFieldsI => this[StandardFields.FieldType.CoBorrower];

    private FieldDefinitionCollection this[StandardFields.FieldType fieldType]
    {
      get
      {
        using (this.acquireLock(false))
          return this.fieldDefinitions[fieldType];
      }
    }

    public StandardField GetFieldI(string fieldId) => this.GetFieldI(fieldId, true);

    public StandardField GetFieldI(string fieldId, bool resolveInstance)
    {
      using (this.acquireLock(false))
      {
        FieldDefinition allField1 = this.AllFields[fieldId];
        if (allField1 != null)
          return (StandardField) allField1;
        if (fieldId.IndexOf('#') != -1)
        {
          FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
          if (fieldPairInfo != null)
          {
            fieldId = fieldPairInfo.FieldID;
            FieldDefinition allField2 = this.AllFields[fieldId];
            if (allField2 != null)
              return (StandardField) allField2;
          }
        }
        if (!resolveInstance)
          return (StandardField) null;
        string instanceParentId = StandardField.GetMultiInstanceParentID(fieldId);
        if (instanceParentId == null)
          return (StandardField) null;
        FieldDefinition allField3 = this.AllFields[instanceParentId];
        if (allField3 == null)
          return (StandardField) null;
        StandardField instanceWithId = (StandardField) allField3.CreateInstanceWithID(fieldId);
        if (this.VirtualFields.Contains(fieldId))
        {
          FieldDefinition virtualField = this.VirtualFields[fieldId];
          if (!string.IsNullOrEmpty(virtualField.Rolodex))
            instanceWithId.Rolodex = virtualField.Rolodex;
        }
        return instanceWithId;
      }
    }

    private IDisposable acquireLock(bool writer)
    {
      try
      {
        if (SmartClientUtils.UseReaderWriterLockSlim)
        {
          if (writer)
          {
            if ((!SmartClientUtils.LockSlimNoRecursionStandardFields || !this.fieldListLockSlim.IsWriteLockHeld) && !this.fieldListLockSlim.TryEnterWriteLock(this.lockTimeout))
              throw new ApplicationException("Timeout expires before the write lock request is granted (001).");
          }
          else if ((!SmartClientUtils.LockSlimNoRecursionStandardFields || !this.fieldListLockSlim.IsReadLockHeld) && !this.fieldListLockSlim.TryEnterReadLock(this.lockTimeout))
            throw new ApplicationException("Timeout expires before the read lock request is granted (001).");
          return (IDisposable) new ReaderWriterTracker(this.fieldListLockSlim, writer);
        }
        if (writer)
          this.fieldListLock.AcquireWriterLock(this.lockTimeout);
        else
          this.fieldListLock.AcquireReaderLock(this.lockTimeout);
        return (IDisposable) new ReaderWriterTracker(this.fieldListLock, writer);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to acquire " + (writer ? nameof (writer) : "reader") + " lock on standard field list. Error = " + ex.Message, ex);
      }
    }

    public bool Reload(bool force)
    {
      using (this.acquireLock(true))
      {
        XmlDocument xml = StandardFields.readMapFile(this.filePath);
        Version version = StandardFields.parseVersion(xml);
        if (!force && this.fileVersion != (Version) null && this.fileVersion >= version)
          return false;
        this.readStandardFieldList(xml);
        this.loadCategoryFieldLists();
        this.fileVersion = version;
        return true;
      }
    }

    private static Version parseVersion(XmlDocument xml)
    {
      try
      {
        return new Version(xml.DocumentElement.GetAttribute("Version"));
      }
      catch
      {
        throw new Exception("File definition file has missing or invalid version number.");
      }
    }

    public Version FileVersion
    {
      get
      {
        using (this.acquireLock(false))
          return this.fileVersion;
      }
    }

    public string FilePath => this.filePath;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      using (this.acquireLock(false))
      {
        info.AddValue("xml", (object) this.xmlData);
        info.AddValue("path", (object) this.filePath);
        info.AddValue("ver", (object) this.fileVersion);
      }
    }

    private void readStandardFieldList(XmlDocument xml)
    {
      try
      {
        DateTime now = DateTime.Now;
        FieldDefinitionCollection definitionCollection1 = new FieldDefinitionCollection();
        FieldDefinitionCollection definitionCollection2 = new FieldDefinitionCollection();
        foreach (XmlElement selectNode in xml.SelectNodes("/Fields/Field"))
        {
          StandardField field = new StandardField(selectNode);
          if (field.XPath != "")
            definitionCollection1.Add((FieldDefinition) field);
          else
            definitionCollection2.Add((FieldDefinition) field);
        }
        try
        {
          this.fileVersion = new Version(xml.DocumentElement.GetAttribute("Version"));
        }
        catch
        {
          throw new Exception("Invalid or missing Version attribute on EncompassFields.dat file");
        }
        this.xmlData = xml.OuterXml;
        this.fieldDefinitions[StandardFields.FieldType.All] = definitionCollection1;
        this.fieldDefinitions[StandardFields.FieldType.Virtual] = definitionCollection2;
        TimeSpan timeSpan = DateTime.Now - now;
        Tracing.Log(StandardFields.sw, nameof (StandardFields), TraceLevel.Verbose, "Standard field list loaded in " + (object) timeSpan.TotalMilliseconds + "ms.");
      }
      catch (Exception ex)
      {
        Tracing.Log(StandardFields.sw, nameof (StandardFields), TraceLevel.Error, "Error loading standard field list: " + (object) ex);
        throw ex;
      }
    }

    private void loadCategoryFieldLists()
    {
      FieldDefinitionCollection definitionCollection1 = new FieldDefinitionCollection();
      FieldDefinitionCollection definitionCollection2 = new FieldDefinitionCollection();
      FieldDefinitionCollection definitionCollection3 = new FieldDefinitionCollection();
      foreach (FieldDefinition field in this.fieldDefinitions[StandardFields.FieldType.All])
      {
        if (field is StandardField)
        {
          switch (field.Category)
          {
            case FieldCategory.Common:
              definitionCollection1.Add(field);
              continue;
            case FieldCategory.Borrower:
              definitionCollection2.Add(field);
              continue;
            case FieldCategory.Coborrower:
              definitionCollection3.Add(field);
              continue;
            default:
              continue;
          }
        }
      }
      this.fieldDefinitions[StandardFields.FieldType.Common] = definitionCollection1;
      this.fieldDefinitions[StandardFields.FieldType.Borrower] = definitionCollection2;
      this.fieldDefinitions[StandardFields.FieldType.CoBorrower] = definitionCollection3;
    }

    private static XmlDocument readMapFile(string mapFilePath)
    {
      lock (StandardFields.mapFileLock)
      {
        byte[] numArray = (byte[]) null;
        try
        {
          using (FileStream fileStream = File.OpenRead(mapFilePath))
          {
            int length = (int) fileStream.Length;
            numArray = new byte[length];
            fileStream.Read(numArray, 0, length);
          }
          try
          {
            numArray = FileCompressor.Instance.UnzipBuffer(numArray);
          }
          catch
          {
          }
          byte num = 5;
          for (int index = 0; index < numArray.Length; ++index)
            numArray[index] += num;
          string xml = Encoding.ASCII.GetString(numArray);
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(xml);
          return xmlDocument;
        }
        catch (Exception ex)
        {
          throw new Exception("[StandardFields] Error loading " + mapFilePath + ": " + ex.Message);
        }
      }
    }

    public static Hashtable ReadFieldHelp(string[] fieldIds)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      string str = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "documents\\FieldHelp.xml");
      try
      {
        if (!File.Exists(str))
        {
          Tracing.Log(StandardFields.sw, nameof (StandardFields), TraceLevel.Error, "Error loading field help from file '" + str);
          return (Hashtable) null;
        }
        if (fieldIds == null)
        {
          using (StreamReader streamReader = new StreamReader(str, Encoding.ASCII))
          {
            XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) streamReader).CreateNavigator().Select("/FieldHelpList/FieldHelp");
            while (xpathNodeIterator.MoveNext())
              insensitiveHashtable[(object) xpathNodeIterator.Current.GetAttribute("helpKey", "")] = (object) xpathNodeIterator.Current.Value;
          }
        }
        else
        {
          XElement xelement1 = XDocument.Load(str).Element((XName) "FieldHelpList");
          if (xelement1 != null)
          {
            IEnumerable<XElement> source = xelement1.Elements((XName) "FieldHelp").Where<XElement>((Func<XElement, bool>) (e =>
            {
              XAttribute xattribute = e.Attribute((XName) "helpKey");
              if (xattribute == null)
                return false;
              return ((IEnumerable<string>) fieldIds).Contains<string>(((IEnumerable<string>) xattribute.Value.Split('-')).FirstOrDefault<string>(), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
            }));
            if (source.Any<XElement>())
            {
              foreach (XElement xelement2 in source)
                insensitiveHashtable[(object) xelement2.FirstAttribute.Value] = (object) xelement2.Value;
            }
          }
        }
        return insensitiveHashtable;
      }
      catch (Exception ex)
      {
        Tracing.Log(StandardFields.sw, nameof (StandardFields), TraceLevel.Error, "Error loading field help from file '" + str + "': " + (object) ex);
        return (Hashtable) null;
      }
    }

    private enum FieldType
    {
      All,
      Borrower,
      CoBorrower,
      Common,
      Virtual,
    }
  }
}
