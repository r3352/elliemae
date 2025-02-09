// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.SDCDocument
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic
{
  [Serializable]
  public class SDCDocument
  {
    private const int CONVERSION_DPI = 150;

    public SDCDocument()
    {
    }

    public SDCDocument(int version, string conversionStatus, int pageCount)
    {
      this.Version = version;
      this.ConversionStatus = conversionStatus;
      this.PageCount = pageCount;
      this.Dpi = 150;
      this.InitializePagesList();
    }

    private void InitializePagesList()
    {
      for (int index = 1; index <= this.PageCount; ++index)
        this.Pages.Add(new EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.Pages() { Id = index });
    }

    public int Version { get; set; }

    public int PageCount { get; set; }

    public int Dpi { get; set; }

    public string ConversionStatus { get; set; }

    public int Rotation { get; set; }

    public List<EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.Pages> Pages { get; set; } = new List<EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.Pages>();

    public List<Annotation> Annotations { get; set; }

    public List<Bookmark> Bookmarks { get; set; }

    public bool TextAvailable { get; set; }

    public long ImagesCreatedDateTime { get; set; }

    public long ContentLength { get; set; }

    public string ConversionErrorCode { get; set; }

    public bool Overlay { get; set; }

    public List<EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic.Events> Events { get; set; }

    public List<SourceObject> SourceObjects { get; set; }

    public string ToCamelCaseJsonString()
    {
      return JsonConvert.SerializeObject((object) this, new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver(),
        DefaultValueHandling = DefaultValueHandling.Ignore
      });
    }
  }
}
