// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.ImageInfo
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class ImageInfo
  {
    private string filename = string.Empty;
    private int filesize;
    private DateTime uploadedDate = DateTime.MinValue;
    private string uploadedBy = string.Empty;
    private ResourceAccessType accessType;
    private string url = string.Empty;

    public ImageInfo(
      string filename,
      int filesize,
      DateTime uploadedDate,
      string uploadedBy,
      ResourceAccessType accessType,
      string url)
    {
      this.filename = filename;
      this.filesize = filesize;
      this.uploadedDate = uploadedDate;
      this.uploadedBy = uploadedBy;
      this.accessType = accessType;
      this.url = url;
    }

    public string Filename => this.filename.ToLower().Trim();

    public int Filesize => this.filesize / 1024;

    public DateTime UploadedDate
    {
      get
      {
        return this.uploadedDate != DateTime.MinValue ? this.uploadedDate.ToLocalTime() : this.uploadedDate;
      }
    }

    public string UploadedBy => this.uploadedBy;

    public string Url => this.url;
  }
}
