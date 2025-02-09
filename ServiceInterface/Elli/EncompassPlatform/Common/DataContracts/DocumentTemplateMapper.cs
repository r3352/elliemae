// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.DocumentTemplateMapper
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.DataEngine.eFolder;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public class DocumentTemplateMapper
  {
    public void WriteTo(DocumentTemplate source, DocumentTemplateGetContract target)
    {
      if (source == null)
        return;
      target.Guid = source.Guid;
      target.Name = source.Name;
      target.Description = source.Description;
      target.DaysDue = source.DaysTillDue;
      target.DaysTillExpire = source.DaysTillExpire;
      target.IsThirdPartyDocIndicator = source.IsThirdPartyDoc;
      target.IsTPOWebcenterPortalIndicator = source.IsTPOWebcenterPortal;
      target.IsWebCenterIndicator = source.IsWebcenter;
      target.OpeningDocumentIndicator = source.OpeningDocument;
      target.PreClosingDocumentIndicator = source.PreClosingDocument;
      target.ClosingDocumentIndicator = source.ClosingDocument;
      target.Source = source.Source;
      if (source.SignatureType == "eSignable")
        target.SignatureType = 0;
      else if (source.SignatureType == "Wet Sign Only")
        target.SignatureType = 1;
      else if (source.SignatureType == "Informational")
        target.SignatureType = 2;
      target.ConversionType = (int) source.ConversionType;
      if (source.SourceType == "Standard Form")
        target.SourceType = 0;
      else if (source.SourceType == "Custom Form")
        target.SourceType = 1;
      else if (source.SourceType == "Borrower Specific Custom Form")
        target.SourceType = 2;
      else if (source.SourceType == "Settlement Service")
        target.SourceType = 3;
      else if (source.SourceType == "Needed")
        target.SourceType = 4;
      target.SaveOriginalFormat = source.SaveOriginalFormat;
    }
  }
}
