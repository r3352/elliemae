// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.SettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public abstract class SettingDefinition : IComparable
  {
    private string category;
    private string name;
    private string displayName;
    private string description;
    private bool requiresRestart;
    private bool displayEnabled = true;
    private SettingTargetSystem appliesTo;

    public SettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      bool requiresRestart,
      bool displayEnabled)
    {
      int length = path.IndexOf(".");
      if (length <= 0 || length == path.Length - 1)
        throw new ArgumentException("Invalid path specification", nameof (path));
      this.category = path.Substring(0, length);
      this.name = path.Substring(length + 1);
      this.displayName = displayName;
      this.description = description;
      this.requiresRestart = requiresRestart;
      this.displayEnabled = displayEnabled;
      this.appliesTo = appliesTo;
    }

    public string Category => this.category;

    public string Name => this.name;

    public string Path => this.category + "." + this.name;

    public string DisplayName => this.displayName;

    public bool DisplayEnabled => this.displayEnabled;

    public string Description => this.description;

    public bool RequiresRestart => this.requiresRestart;

    public SettingTargetSystem AppliesTo => this.appliesTo;

    public bool DoesApplyTo(SettingTargetSystem system) => (this.appliesTo & system) == system;

    public virtual string ToString(object value) => value.ToString();

    public int CompareTo(object obj)
    {
      return this.DisplayName.CompareTo(((SettingDefinition) obj).DisplayName);
    }

    public abstract object DefaultValue { get; }

    public abstract object Parse(string value);
  }
}
