// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.ImageLibraryAttachment
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using Microsoft.Web.Services2.Dime;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class ImageLibraryAttachment
  {
    private string filename;
    private string filepath;
    private long filesize;
    private string type;

    public ImageLibraryAttachment(string filepath, string filename)
    {
      FileInfo fileInfo = new FileInfo(filepath);
      this.filename = filename;
      this.filepath = filepath;
      this.filesize = fileInfo.Length;
      this.type = fileInfo.Extension;
    }

    public string Filename => this.filename;

    public string Filepath => this.filepath;

    public long Filesize => this.filesize;

    public DimeAttachment DimeAttachment
    {
      get => new DimeAttachment(this.filename, this.type, TypeFormat.Unchanged, this.filepath);
    }
  }
}
