// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FileSystemEntry
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class FileSystemEntry : ICloneable, IComparable
  {
    private static readonly string sw = Tracing.SwEpass;
    private FileSystemEntry.Types type;
    private string path;
    private string owner;
    private bool parsed;
    private string name;
    private string parentPath;
    private DateTime lastModified = DateTime.MinValue;
    private Hashtable properties = new Hashtable();
    private AclResourceAccess access;
    private bool? hasSubFolders;
    [NonSerialized]
    private FileSystemEntry parentDir;
    public static readonly FileSystemEntry PublicRoot = new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null);

    public FileSystemEntry(
      string parentPath,
      string entryName,
      FileSystemEntry.Types type,
      string owner)
      : this(parentPath, entryName, type, owner, DateTime.MinValue)
    {
    }

    public FileSystemEntry(
      string parentPath,
      string entryName,
      FileSystemEntry.Types type,
      string owner,
      bool? hasSubFolders)
      : this(parentPath, entryName, type, owner, DateTime.MinValue)
    {
      this.hasSubFolders = hasSubFolders;
    }

    public FileSystemEntry(
      string parentPath,
      string entryName,
      FileSystemEntry.Types type,
      string owner,
      DateTime lastModified)
    {
      if (!parentPath.StartsWith("\\"))
        throw new ArgumentException("Invalid path specification");
      if (!parentPath.EndsWith("\\"))
        parentPath += "\\";
      if (owner != null && owner.Length == 0)
        throw new ArgumentException("Invalid object owner");
      if (type != FileSystemEntry.Types.File && type != FileSystemEntry.Types.Folder)
        throw new ArgumentException("Invalid file system type specification");
      this.parentPath = parentPath;
      this.name = entryName;
      this.parsed = true;
      this.path = this.parentPath + this.Name + (type == FileSystemEntry.Types.Folder ? "\\" : "");
      this.type = type;
      this.owner = owner;
      this.lastModified = lastModified;
    }

    public FileSystemEntry(string path, FileSystemEntry.Types type, string owner)
    {
      if (!path.StartsWith("\\"))
        throw new ArgumentException("Invalid path specification");
      if (type == FileSystemEntry.Types.File && path.EndsWith("\\"))
        throw new ArgumentException("Invalid path specification");
      if (type == FileSystemEntry.Types.Folder && !path.EndsWith("\\"))
        path += "\\";
      if (owner != null && owner.Length == 0)
        throw new ArgumentException("Invalid object owner");
      if (type != FileSystemEntry.Types.File && type != FileSystemEntry.Types.Folder)
        throw new ArgumentException("Invalid file system type specification");
      this.path = path;
      this.type = type;
      this.owner = owner;
    }

    public FileSystemEntry(FileSystemEntry e)
    {
      this.type = e.type;
      this.path = e.path;
      this.owner = e.owner;
      this.parsed = e.parsed;
      this.name = e.name;
      this.parentPath = e.parentPath;
      this.lastModified = e.lastModified;
      this.properties = (Hashtable) e.properties.Clone();
    }

    public FileSystemEntry.Types Type => this.type;

    public string Path => this.path;

    public string Name
    {
      get
      {
        this.ensureParsed();
        return this.name;
      }
    }

    public DateTime LastModified => this.lastModified;

    public FileSystemEntry ParentFolder
    {
      get
      {
        this.ensureParsed();
        if (this.parentDir == null && this.parentPath != null)
          this.parentDir = new FileSystemEntry(this.parentPath, FileSystemEntry.Types.Folder, this.Owner);
        return this.parentDir;
      }
    }

    public string Owner => this.owner;

    public bool IsPublic => this.owner == null;

    public bool IsRootFolder => this.path == "\\";

    public Hashtable Properties
    {
      get => this.properties;
      set => this.properties = value;
    }

    public AclResourceAccess Access
    {
      get => this.access;
      set => this.access = value;
    }

    public string GetEncodedPath() => FileSystem.EncodeFilename(this.Path, true);

    public FileSystemEntry Combine(string path)
    {
      if (this.Type != FileSystemEntry.Types.Folder)
        throw new Exception("Operation only valid for Folder entries.");
      return !path.StartsWith("\\") ? FileSystemEntry.Parse(this.ToString() + path) : throw new Exception("Specified path must be relative and cannot start with the \\ character.");
    }

    private void ensureParsed()
    {
      if (this.parsed)
        return;
      this.parsePath();
      this.parsed = true;
    }

    private void parsePath()
    {
      if (this.path == "\\")
      {
        this.name = "";
        this.parentPath = (string) null;
      }
      else
      {
        string str = this.type != FileSystemEntry.Types.Folder ? this.path : this.path.Substring(0, this.path.Length - 1);
        int num = str.LastIndexOf("\\");
        this.name = str.Substring(num + 1, str.Length - num - 1);
        this.parentPath = str.Substring(0, num + 1);
      }
    }

    public override int GetHashCode() => this.ToString().GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is FileSystemEntry fileSystemEntry && string.Compare(fileSystemEntry.ToString(), this.ToString(), true) == 0;
    }

    public override string ToString()
    {
      return this.IsPublic ? "Public:" + this.path : "Personal:\\" + this.owner + this.path;
    }

    public string ToDisplayString()
    {
      return this.IsPublic ? "Public:" + this.path : "Personal:" + this.path;
    }

    public string ToDisplayFormattedString()
    {
      return this.IsPublic ? "Public" + this.path : "Personal" + this.path;
    }

    public static string RemoveRoot(string file)
    {
      file = file.Replace("Public:\\", "");
      return file;
    }

    public static string AddRoot(string file)
    {
      if (file != "" && file != null)
        file = "Public:\\" + file;
      return file;
    }

    public bool? HasSubFolders => this.hasSubFolders;

    public static FileSystemEntry PrivateRoot(string owner)
    {
      return new FileSystemEntry("\\", FileSystemEntry.Types.Folder, owner);
    }

    public static FileSystemEntry Parse(string uri)
    {
      Tracing.Log(FileSystemEntry.sw, TraceLevel.Info, nameof (FileSystemEntry), "Processing FileSystemEntry For URI : " + uri);
      string lower = uri.ToLower();
      string owner = (string) null;
      FileSystemEntry.Types type = uri.EndsWith("\\") ? FileSystemEntry.Types.Folder : FileSystemEntry.Types.File;
      string path;
      if (lower.StartsWith("public:"))
      {
        path = uri.Substring(7);
      }
      else
      {
        if (!lower.StartsWith("personal:"))
          throw new ArgumentException("Invalid FileSystemEntry URI");
        string str = uri.Substring(9);
        int startIndex = str.StartsWith("\\") && str.Length != 1 ? str.IndexOf("\\", 1) : throw new ArgumentException("Invalid FileSystemEntry URI");
        owner = startIndex >= 0 ? str.Substring(1, startIndex - 1) : throw new ArgumentException("Invalid FileSystemEntry URI");
        path = str.Substring(startIndex);
      }
      if (!path.StartsWith("\\"))
        throw new ArgumentException("Invalid FileSystemEntry URI");
      return new FileSystemEntry(path, type, owner);
    }

    public static FileSystemEntry Parse(string uri, string owner)
    {
      string lower = uri.ToLower();
      FileSystemEntry.Types type = uri.EndsWith("\\") ? FileSystemEntry.Types.Folder : FileSystemEntry.Types.File;
      string path;
      if (lower.StartsWith("public:"))
      {
        path = uri.Substring(7);
        owner = (string) null;
      }
      else
      {
        if (!lower.StartsWith("personal:"))
          throw new ArgumentException("Invalid FileSystemEntry URI");
        if (owner == null)
          throw new ArgumentException("Invalid FileSystemEntry URI");
        path = uri.Substring(9);
      }
      if (!path.StartsWith("\\"))
        throw new ArgumentException("Invalid FileSystemEntry URI");
      return new FileSystemEntry(path, type, owner);
    }

    public static bool IsValidPath(string path)
    {
      try
      {
        FileSystemEntry.Parse(path);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool IsValidPath(string path, string userId)
    {
      try
      {
        FileSystemEntry.Parse(path, userId);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public object Clone() => (object) new FileSystemEntry(this);

    public int CompareTo(object obj)
    {
      int num = -1;
      if (obj is FileSystemEntry fileSystemEntry && string.Compare(this.path, fileSystemEntry.path, true) > 0)
        num = 1;
      return num;
    }

    [Flags]
    public enum Types
    {
      None = 0,
      Folder = 1,
      File = 2,
      All = File | Folder, // 0x00000003
    }
  }
}
