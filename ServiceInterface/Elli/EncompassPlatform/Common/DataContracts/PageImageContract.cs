// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.PageImageContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "PageImage", Namespace = "http://www.elliemae.com/encompass/platform")]
  public class PageImageContract
  {
    [DataMember]
    public string ImageKey { get; set; }

    [DataMember]
    public string ZipKey { get; set; }

    [DataMember]
    public string NativeKey { get; set; }

    [DataMember]
    public int Width { get; set; }

    [DataMember]
    public int Height { get; set; }

    [DataMember]
    public float HorizontalResolution { get; set; }

    [DataMember]
    public float VerticalResolution { get; set; }

    [DataMember]
    public int Rotation { get; set; }

    [DataMember]
    public long FileSize { get; set; }

    [DataMember]
    public PageThumbnailContract Thumbnail { get; set; }

    [DataMember]
    public List<PageAnnotationContract> Annotations { get; set; }
  }
}
