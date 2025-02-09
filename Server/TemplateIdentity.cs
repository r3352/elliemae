// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TemplateIdentity
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TemplateIdentity : ICloneable
  {
    private TemplateSettingsType type;
    private string physicalPath;
    private IEqualityComparer hashCodeProvider;
    private FileSystemEntry fsEntry;

    public TemplateIdentity(TemplateSettingsType type, FileSystemEntry fsEntry)
    {
      this.type = type;
      this.fsEntry = fsEntry;
      this.physicalPath = TemplateSettings.GetFilePath(type, fsEntry);
    }

    public TemplateIdentity(TemplateIdentity source)
    {
      this.type = source.type;
      this.physicalPath = source.physicalPath;
      this.fsEntry = (FileSystemEntry) source.fsEntry.Clone();
    }

    public TemplateIdentity(string physicalPath)
    {
      this.type = ~TemplateSettingsType.CustomLetter;
      this.physicalPath = physicalPath;
      this.fsEntry = (FileSystemEntry) null;
    }

    public TemplateSettingsType Type => this.type;

    public string PhysicalPath => this.physicalPath;

    public FileSystemEntry FileSystemEntry => this.fsEntry;

    public override int GetHashCode()
    {
      if (this.hashCodeProvider == null)
        this.hashCodeProvider = (IEqualityComparer) StringComparer.InvariantCultureIgnoreCase;
      return this.hashCodeProvider.GetHashCode((object) this.physicalPath);
    }

    public override bool Equals(object obj)
    {
      return obj is TemplateIdentity templateIdentity && string.Compare(templateIdentity.physicalPath, this.physicalPath, true) == 0;
    }

    public object Clone() => (object) new TemplateIdentity(this);

    public override string ToString() => this.physicalPath;
  }
}
