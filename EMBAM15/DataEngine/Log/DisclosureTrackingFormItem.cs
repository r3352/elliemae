// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosureTrackingFormItem
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Xml.AutoMapping;
using System;
using System.Linq.Expressions;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosureTrackingFormItem
  {
    private string formName = "";
    private DisclosureTrackingFormItem.FormType formType = DisclosureTrackingFormItem.FormType.StandardForm;
    private DisclosureTrackingFormItem.FormSignatureType signatureType;
    private string viewableFormFile = "";
    private string guid = "";

    static DisclosureTrackingFormItem()
    {
      XmlAutoMapper.AddProfile<DisclosureTrackingFormItem>(XmlAutoMapper.NewProfile<DisclosureTrackingFormItem>().ForMember<string>((Expression<Func<DisclosureTrackingFormItem, string>>) (form => form.FormName), (Action<XmlAutoMapper.Profile<DisclosureTrackingFormItem>.ProfileOptions<string>>) (opts => opts.Rename("Name"))).ForMember<DisclosureTrackingFormItem.FormType>((Expression<Func<DisclosureTrackingFormItem, DisclosureTrackingFormItem.FormType>>) (form => form.OutputFormType), (Action<XmlAutoMapper.Profile<DisclosureTrackingFormItem>.ProfileOptions<DisclosureTrackingFormItem.FormType>>) (opts => opts.Rename("Type"))).ForMember<string>((Expression<Func<DisclosureTrackingFormItem, string>>) (form => form.OutputFormTypeName), (Action<XmlAutoMapper.Profile<DisclosureTrackingFormItem>.ProfileOptions<string>>) (opts => opts.Ignore())));
    }

    public DisclosureTrackingFormItem()
    {
    }

    public DisclosureTrackingFormItem(string formName, DisclosureTrackingFormItem.FormType formType)
    {
      this.formName = formName;
      this.formType = formType;
    }

    public DisclosureTrackingFormItem(
      string formName,
      DisclosureTrackingFormItem.FormType formType,
      string documentTemplateGuid,
      DisclosureTrackingFormItem.FormSignatureType signatureType)
    {
      this.formName = formName;
      this.formType = formType;
      this.signatureType = signatureType;
      this.guid = documentTemplateGuid;
    }

    public DisclosureTrackingFormItem(
      string formName,
      DisclosureTrackingFormItem.FormType formType,
      string documentTemplateGuid)
    {
      this.formName = formName;
      this.formType = formType;
      this.guid = documentTemplateGuid;
    }

    public string FormName
    {
      get => this.formName;
      set => this.formName = value;
    }

    public DisclosureTrackingFormItem.FormType OutputFormType
    {
      get => this.formType;
      set => this.formType = value;
    }

    public DisclosureTrackingFormItem.FormSignatureType SignatureType
    {
      get => this.signatureType;
      set => this.signatureType = value;
    }

    public string ViewableFormFile
    {
      get => this.viewableFormFile;
      set => this.viewableFormFile = value;
    }

    public string OutputFormTypeName
    {
      get
      {
        switch (this.formType)
        {
          case DisclosureTrackingFormItem.FormType.eDisclosure:
            return "eDisclosure";
          case DisclosureTrackingFormItem.FormType.StandardForm:
            return "Standard Form";
          case DisclosureTrackingFormItem.FormType.CustomForm:
            return "Custom Form";
          case DisclosureTrackingFormItem.FormType.Needed:
            return "Needed";
          case DisclosureTrackingFormItem.FormType.ClosingDocsOrder:
            return "Closing Document";
          case DisclosureTrackingFormItem.FormType.eFolder:
            return "eFolder";
          case DisclosureTrackingFormItem.FormType.CoverSheet:
            return "Cover Sheet";
          default:
            return "Unknown";
        }
      }
    }

    public string DocumentTemplateGuid => this.guid;

    public enum FormType
    {
      None,
      eDisclosure,
      StandardForm,
      CustomForm,
      Needed,
      ClosingDocsOrder,
      eFolder,
      CoverSheet,
    }

    public enum FormSignatureType
    {
      None,
      Informational,
      eSignature,
      WetSignature,
    }
  }
}
