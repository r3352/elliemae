// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IAttachmentPageImage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for AttachmentPageImage class.</summary>
  /// <exclude />
  [Guid("2bda4a69-fa14-46eb-b2a2-513c6b185dec")]
  public interface IAttachmentPageImage
  {
    string ImageKey { get; }

    string ZipKey { get; }

    int Width { get; }

    int Height { get; }

    float HorizontalResolution { get; }

    float VerticalResolution { get; }

    PageImageThumbnail Thumbnail { get; }

    PageImageAnnotations Annotations { get; }
  }
}
