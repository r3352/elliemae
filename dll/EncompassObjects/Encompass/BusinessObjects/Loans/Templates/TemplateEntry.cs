// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TemplateEntry : ITemplateEntry
  {
    public static readonly TemplateEntry PublicRoot = new TemplateEntry(FileSystemEntry.PublicRoot);
    private FileSystemEntry fsEntry;
    private TemplateEntryProperties properties;
    private TemplateEntry parentEntry;

    internal TemplateEntry(FileSystemEntry fsEntry)
    {
      this.fsEntry = fsEntry;
      this.properties = new TemplateEntryProperties(fsEntry.Properties);
    }

    public string DomainPath => this.fsEntry.Path;

    public string Path => this.fsEntry.ToDisplayString();

    public string UserQualifiedPath => this.fsEntry.ToString();

    public string Name => this.fsEntry.Name;

    public TemplateEntryType EntryType => (TemplateEntryType) this.fsEntry.Type;

    public bool IsPublic => this.fsEntry.IsPublic;

    public DateTime LastModified
    {
      get => this.fsEntry.Type == 1 ? DateTime.MinValue : this.fsEntry.LastModified;
    }

    public TemplateEntry ParentFolder
    {
      get
      {
        if (this.parentEntry != null)
          return this.parentEntry;
        if (this.fsEntry.ParentFolder == null)
          return (TemplateEntry) null;
        this.parentEntry = new TemplateEntry(this.fsEntry.ParentFolder);
        return this.parentEntry;
      }
    }

    public string Owner => this.fsEntry.Owner;

    public TemplateEntryProperties Properties => this.properties;

    public override string ToString() => this.Path;

    public override bool Equals(object obj)
    {
      return obj is TemplateEntry templateEntry && string.Compare(templateEntry.UserQualifiedPath, this.UserQualifiedPath, true) == 0;
    }

    public override int GetHashCode() => this.UserQualifiedPath.ToLower().GetHashCode();

    public static TemplateEntry Parse(string userQualifiedPath)
    {
      return new TemplateEntry(FileSystemEntry.Parse(userQualifiedPath));
    }

    public static TemplateEntry Parse(string path, string ownerId)
    {
      return new TemplateEntry(FileSystemEntry.Parse(path, ownerId));
    }

    public static TemplateEntry Combine(TemplateEntry folderEntry, string relativePath)
    {
      if (folderEntry == null)
        throw new ArgumentNullException(nameof (folderEntry));
      if (folderEntry.EntryType != TemplateEntryType.Folder)
        throw new ArgumentException("The specified folderEntry must represent a folder entry.");
      if (relativePath.StartsWith("\\"))
        throw new ArgumentException("The specified relativePath cannot start with the folder seperator '\\'");
      return TemplateEntry.Parse(folderEntry.UserQualifiedPath + relativePath);
    }

    public static TemplateEntry GetPersonalRoot(string owner)
    {
      return new TemplateEntry(FileSystemEntry.PrivateRoot(owner));
    }

    internal FileSystemEntry Unwrap() => this.fsEntry;
  }
}
