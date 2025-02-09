// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanTemplateSelection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanTemplateSelection
  {
    public FileSystemEntry TemplateEntry;
    public bool AppendData;

    public LoanTemplateSelection(FileSystemEntry template)
      : this(template, false)
    {
    }

    public LoanTemplateSelection(FileSystemEntry template, bool append)
    {
      this.TemplateEntry = template != null ? template : throw new ArgumentNullException(nameof (template));
      this.AppendData = append;
    }
  }
}
