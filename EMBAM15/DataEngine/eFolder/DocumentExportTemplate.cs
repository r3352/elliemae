// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentExportTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentExportTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private static b jed;
    private string templateName = string.Empty;
    private string description = string.Empty;
    private bool isDefault;
    private bool exportAsZip;
    private AnnotationExportType annotationExportType;
    private bool passwordProtect;
    private bool isEncrypted;
    private string password;
    private bool shouldEncryptPassword = true;
    private bool exportLocationSet;
    private string exportLocation = string.Empty;
    private ExportFileNameFieldType fileNameField1 = ExportFileNameFieldType.LoanNumber;
    private string fileNameText1 = string.Empty;
    private ExportFileNameFieldType fileNameField2;
    private string fileNameText2 = string.Empty;
    private ExportFileNameFieldType fileNameField3;
    private string fileNameText3 = string.Empty;

    public string TemplateName
    {
      get
      {
        if (this.templateName == null)
          this.templateName = string.Empty;
        return this.templateName;
      }
      set => this.templateName = value;
    }

    public string Description
    {
      get
      {
        if (this.description == null)
          this.description = string.Empty;
        return this.description;
      }
      set => this.description = value;
    }

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public bool ExportAsZip
    {
      get => this.exportAsZip;
      set => this.exportAsZip = value;
    }

    public string StackingOrderName { get; set; }

    public int DocumentStackingTemplateID { get; set; }

    public AnnotationExportType AnnotationExportType
    {
      get => this.annotationExportType;
      set => this.annotationExportType = value;
    }

    public bool PasswordProtect
    {
      get => this.passwordProtect;
      set => this.passwordProtect = value;
    }

    public bool IsEncrypted
    {
      get => this.isEncrypted;
      set => this.isEncrypted = value;
    }

    public string Password
    {
      get
      {
        if (!this.isEncrypted)
          return this.password;
        try
        {
          return DocumentExportTemplate.decryptString(this.password);
        }
        catch
        {
          return "";
        }
      }
      set
      {
        if (this.shouldEncryptPassword)
        {
          this.password = DocumentExportTemplate.encryptString(value);
          this.isEncrypted = true;
        }
        else
          this.password = value;
      }
    }

    public string EntrytedPassword => this.password;

    public bool ShouldEncryptPassword
    {
      set => this.shouldEncryptPassword = value;
    }

    private static void jedConfig()
    {
      a.a("z2r1xy8k5mp4ccpl");
      DocumentExportTemplate.jed = DocumentExportTemplate.CreateJed();
    }

    private static b CreateJed() => a.b("z5cty6u5dj3bd8");

    private static string encryptString(string plainText)
    {
      if (DocumentExportTemplate.jed == null)
        DocumentExportTemplate.jedConfig();
      lock (DocumentExportTemplate.jed)
      {
        DocumentExportTemplate.jed.b();
        return Convert.ToBase64String(DocumentExportTemplate.jed.b(plainText));
      }
    }

    private static string decryptString(string cipherText)
    {
      if (DocumentExportTemplate.jed == null)
        DocumentExportTemplate.jedConfig();
      lock (DocumentExportTemplate.jed)
      {
        DocumentExportTemplate.jed.b();
        return DocumentExportTemplate.jed.a((Stream) new MemoryStream(Convert.FromBase64String(cipherText)));
      }
    }

    public bool ExportLocationSet
    {
      get => this.exportLocationSet;
      set => this.exportLocationSet = value;
    }

    public string ExportLocation
    {
      get => this.exportLocation;
      set => this.exportLocation = value;
    }

    public ExportFileNameFieldType FileNameField1
    {
      get => this.fileNameField1;
      set => this.fileNameField1 = value;
    }

    public string FileNameText1
    {
      get => this.fileNameText1;
      set => this.fileNameText1 = value;
    }

    public ExportFileNameFieldType FileNameField2
    {
      get => this.fileNameField2;
      set => this.fileNameField2 = value;
    }

    public string FileNameText2
    {
      get => this.fileNameText2;
      set => this.fileNameText2 = value;
    }

    public ExportFileNameFieldType FileNameField3
    {
      get => this.fileNameField3;
      set => this.fileNameField3 = value;
    }

    public string FileNameText3
    {
      get => this.fileNameText3;
      set => this.fileNameText3 = value;
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public DocumentExportTemplate()
    {
    }

    public DocumentExportTemplate(XmlSerializationInfo info)
    {
      if (info == null)
        return;
      XmlStringTable xmlStringTable = (XmlStringTable) info.GetValue("0", typeof (XmlStringTable));
      this.templateName = string.Concat(xmlStringTable["DTNAME"]);
      this.description = string.Concat(xmlStringTable["DTDESC"]);
      this.isDefault = (string) xmlStringTable["DEFAULT"] == "YES";
      this.exportAsZip = (string) xmlStringTable["EXPORTASZIP"] == "YES";
      this.StackingOrderName = string.Concat(xmlStringTable["STACKINGNAME"]);
      this.DocumentStackingTemplateID = xmlStringTable[nameof (DocumentStackingTemplateID)] != null ? Convert.ToInt32(xmlStringTable[nameof (DocumentStackingTemplateID)].ToString()) : 0;
      switch ((string) xmlStringTable["ANNOTATIONEXPORT"])
      {
        case "ALL":
          this.annotationExportType = AnnotationExportType.All;
          break;
        case "PERSONAL":
          this.annotationExportType = AnnotationExportType.Personal;
          break;
        case "PUBLIC":
          this.annotationExportType = AnnotationExportType.Public;
          break;
        default:
          this.annotationExportType = AnnotationExportType.None;
          break;
      }
      this.isEncrypted = xmlStringTable["ISENCRYPTED"] != null;
      this.passwordProtect = (string) xmlStringTable["PASSWORDPROTECT"] == "YES";
      this.password = string.Concat(xmlStringTable["PASSWORD"]);
      this.exportLocationSet = (string) xmlStringTable["EXPORTLOCATIONSET"] == "YES";
      this.exportLocation = string.Concat(xmlStringTable["EXPORTLOCATION"]);
      this.fileNameField1 = (ExportFileNameFieldType) Enum.Parse(typeof (ExportFileNameFieldType), (string) xmlStringTable["FILENAMEFIELD1"]);
      this.fileNameText1 = string.Concat(xmlStringTable["FILENAMETEXT1"]);
      this.fileNameField2 = (ExportFileNameFieldType) Enum.Parse(typeof (ExportFileNameFieldType), (string) xmlStringTable["FILENAMEFIELD2"]);
      this.fileNameText2 = string.Concat(xmlStringTable["FILENAMETEXT2"]);
      this.fileNameField3 = (ExportFileNameFieldType) Enum.Parse(typeof (ExportFileNameFieldType), (string) xmlStringTable["FILENAMEFIELD3"]);
      this.fileNameText3 = string.Concat(xmlStringTable["FILENAMETEXT3"]);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      XmlStringTable xmlStringTable = new XmlStringTable();
      xmlStringTable["DTNAME"] = (object) this.templateName;
      xmlStringTable["DTDESC"] = (object) this.description;
      if (this.isDefault)
        xmlStringTable["DEFAULT"] = (object) "YES";
      else
        xmlStringTable["DEFAULT"] = (object) "";
      if (this.exportAsZip)
        xmlStringTable["EXPORTASZIP"] = (object) "YES";
      else
        xmlStringTable["EXPORTASZIP"] = (object) "";
      xmlStringTable["STACKINGNAME"] = (object) this.StackingOrderName;
      xmlStringTable["DocumentStackingTemplateID"] = (object) this.DocumentStackingTemplateID.ToString();
      switch (this.annotationExportType)
      {
        case AnnotationExportType.All:
          xmlStringTable["ANNOTATIONEXPORT"] = (object) "ALL";
          break;
        case AnnotationExportType.Personal:
          xmlStringTable["ANNOTATIONEXPORT"] = (object) "PERSONAL";
          break;
        case AnnotationExportType.Public:
          xmlStringTable["ANNOTATIONEXPORT"] = (object) "PUBLIC";
          break;
        default:
          xmlStringTable["ANNOTATIONEXPORT"] = (object) "NONE";
          break;
      }
      xmlStringTable["ISENCRYPTED"] = (object) "YES";
      if (this.passwordProtect)
        xmlStringTable["PASSWORDPROTECT"] = (object) "YES";
      else
        xmlStringTable["PASSWORDPROTECT"] = (object) "";
      xmlStringTable["PASSWORD"] = (object) this.password;
      if (this.exportLocationSet)
        xmlStringTable["EXPORTLOCATIONSET"] = (object) "YES";
      else
        xmlStringTable["EXPORTLOCATIONSET"] = (object) "";
      xmlStringTable["EXPORTLOCATION"] = (object) this.exportLocation;
      xmlStringTable["FILENAMEFIELD1"] = (object) Enum.GetName(typeof (ExportFileNameFieldType), (object) this.fileNameField1);
      xmlStringTable["FILENAMETEXT1"] = (object) this.fileNameText1;
      xmlStringTable["FILENAMEFIELD2"] = (object) Enum.GetName(typeof (ExportFileNameFieldType), (object) this.fileNameField2);
      xmlStringTable["FILENAMETEXT2"] = (object) this.fileNameText2;
      xmlStringTable["FILENAMEFIELD3"] = (object) Enum.GetName(typeof (ExportFileNameFieldType), (object) this.fileNameField3);
      xmlStringTable["FILENAMETEXT3"] = (object) this.fileNameText3;
      info.AddValue("0", (object) xmlStringTable);
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public static explicit operator DocumentExportTemplate(BinaryObject obj)
    {
      return (DocumentExportTemplate) BinaryConvertibleObject.Parse(obj, typeof (DocumentExportTemplate));
    }
  }
}
