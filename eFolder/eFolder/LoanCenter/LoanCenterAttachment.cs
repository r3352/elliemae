// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.LoanCenterAttachment
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Microsoft.Web.Services2.Dime;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class LoanCenterAttachment
  {
    protected string id;
    private string title;
    private string filepath;
    private long filesize;

    public LoanCenterAttachment(string filepath, string title)
    {
      FileInfo fileInfo = new FileInfo(filepath);
      this.id = Guid.NewGuid().ToString();
      this.title = title;
      this.filepath = filepath;
      this.filesize = fileInfo.Length;
    }

    public string ID => this.id;

    public string Title => this.title;

    public string Filepath => this.filepath;

    public long Filesize => this.filesize;

    public DimeAttachment DimeAttachment
    {
      get => new DimeAttachment(this.id, ".pdf", TypeFormat.Unchanged, this.filepath);
    }
  }
}
