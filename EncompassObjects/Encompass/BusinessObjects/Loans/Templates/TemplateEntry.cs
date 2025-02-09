// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>Represents a node in the template file structure.</summary>
  /// <remarks>This object can represent either a specific template or a folder within the
  /// template file structure.</remarks>
  public class TemplateEntry : ITemplateEntry
  {
    /// <summary>
    /// Represents a TemplateEntry for the root of the Public domain.
    /// </summary>
    public static readonly TemplateEntry PublicRoot = new TemplateEntry(FileSystemEntry.PublicRoot);
    private FileSystemEntry fsEntry;
    private TemplateEntryProperties properties;
    private TemplateEntry parentEntry;

    internal TemplateEntry(FileSystemEntry fsEntry)
    {
      this.fsEntry = fsEntry;
      this.properties = new TemplateEntryProperties(fsEntry.Properties);
    }

    /// <summary>
    /// Gets the path of the TemplateEntry within either the public or personal domain.
    /// </summary>
    /// <remarks><p>This property will return the portion of the path which excludes the
    /// <c>public:\</c> or <c>personal:\</c> domain indicator,
    /// e.g. <c>\Companywide\ARM Loans\My ARM Template</c>. A path for a folder
    /// will always end with "\" while a path for a template will not.</p>
    /// <p>The retrieve the full path, including the public/personal prefix, use the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.Path" />
    /// property.</p>
    /// </remarks>
    public string DomainPath => this.fsEntry.Path;

    /// <summary>Gets the full path of the TemplateEntry.</summary>
    /// <remarks><p>The path will start with either <c>public:\</c> or <c>personal:\</c>
    /// to denote whether the file is in the public or personal storage area. A path for a folder
    /// will always end with "\" while a path for a template will not.</p>
    /// <p>The retrieve the portion of the path without the public/personal prefix, use the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.DomainPath" /> property. The retrieve a "user-qualified" path for the template,
    /// use the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.UserQualifiedPath" /> property.</p>
    /// </remarks>
    public string Path => this.fsEntry.ToDisplayString();

    /// <summary>
    /// Gets the user-qualified path of the TemplateEntry, which includes the owner's identity for
    /// private templates.
    /// </summary>
    /// <remarks><p>The path will start with either <c>public:\</c> or <c>personal:\</c>
    /// to denote whether the file is in the public or personal storage area. A path for a folder
    /// will always end with "\" while a path for a template will not.</p>
    /// <p>Unlike the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.Path" /> property, this property will include the template owner's User ID
    /// as part of the path when the TemplateEntry represents a file within a user's personal domain.
    /// A user-qualified path for a personal template has the format <c>personal:\&lt;userid&gt;\...</c>.
    /// This path can be used to differentiate between templates with the same <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.Path" />
    /// but belonging to two separate users.
    /// </p>
    /// <p>The retrieve the portion of the path without the public/personal prefix, use the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.DomainPath" /> property.</p>
    /// </remarks>
    public string UserQualifiedPath => this.fsEntry.ToString();

    /// <summary>Returns the name portion of the file or folder.</summary>
    /// <remarks>This property returns only the last portion of the path, which is the name of the
    /// template/folder which is represented by this object.</remarks>
    public string Name => this.fsEntry.Name;

    /// <summary>
    /// Gets the type of TemplateEntry represented by the object.
    /// </summary>
    /// <remarks>This property will return either <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryType.Folder" />
    /// or <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryType.Template" />.</remarks>
    public TemplateEntryType EntryType => (TemplateEntryType) this.fsEntry.Type;

    /// <summary>
    /// Indicates if the TemplateEntry represents a public or personal object.
    /// </summary>
    public bool IsPublic => this.fsEntry.IsPublic;

    /// <summary>Gets the last modification date of the entry.</summary>
    /// <remarks>This property will only return a valid date of the object represents a
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryType.Template" />. For folder entries, this property will return
    /// <c>DateTime.MinValue</c>.</remarks>
    public DateTime LastModified
    {
      get
      {
        return this.fsEntry.Type == FileSystemEntry.Types.Folder ? DateTime.MinValue : this.fsEntry.LastModified;
      }
    }

    /// <summary>
    /// Gets the TemplateEntry for the folder which contains the current object.
    /// </summary>
    /// <remarks>If the current TemplateEntry is the top-most node in the hierarchy,
    /// then this property will return <c>null</c>.</remarks>
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

    /// <summary>Gets the owner of the current TemplateEntry.</summary>
    /// <remarks>If the TemplateEntry resides in the public domain, this property will return
    /// <c>null</c>. Otherwise, this property will return the User ID of the object's owner.</remarks>
    public string Owner => this.fsEntry.Owner;

    /// <summary>
    /// Gets the collection of custom properties attached to the TemplateEntry.
    /// </summary>
    /// <remarks>Most TemplateEntry objects which represent an actual Template will have one or more
    /// properties that allows for convenient display of summary info for the template, e.g. a
    /// Description. The proeprties are name/value pairs and can be different for each template type.
    /// Thus, you should use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryProperties.GetPropertyNames" /> method
    /// to determine the properties available for the specific TemplateEntry.</remarks>
    public TemplateEntryProperties Properties => this.properties;

    /// <summary>Provides a string representation of the object.</summary>
    /// <returns>The method returns the object's <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.Path" />.</returns>
    public override string ToString() => this.Path;

    /// <summary>
    /// Provides an equality comparison between two TemplateEntry objects.
    /// </summary>
    /// <param name="obj">The TemplateEntry object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the two TemplateEntry objects have the same
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.Path" />, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return obj is TemplateEntry templateEntry && string.Compare(templateEntry.UserQualifiedPath, this.UserQualifiedPath, true) == 0;
    }

    /// <summary>Provides a hash code for the TemplateEntry object.</summary>
    public override int GetHashCode() => this.UserQualifiedPath.ToLower().GetHashCode();

    /// <summary>Creates a TemplateEntry from the specified path.</summary>
    /// <param name="userQualifiedPath">The user-qualified path of the template entry.</param>
    /// <returns>The TemplateEntry for this object.</returns>
    /// <remarks>
    /// <p>The <c>userQualifiedPath</c> provided must represent an absolute path and start with
    /// either the <c>public:\</c> or <c>personal:\</c> domain prefix. If the path is for a
    /// personal template, then the path must be a "user-qualified" path and have the format
    /// <c>personal:\&lt;userid&gt;\...</c>. The user-qualified path includes the User ID of the
    /// template's owner in order to fully determine the source of the template.</p>
    /// <p>If the path ends
    /// with a "\" it will be interpreted to represent a folder, otherwise it will represent
    /// a template entry.</p>
    /// </remarks>
    public static TemplateEntry Parse(string userQualifiedPath)
    {
      return new TemplateEntry(FileSystemEntry.Parse(userQualifiedPath));
    }

    /// <summary>Creates a TemplateEntry from the specified path.</summary>
    /// <param name="path">The absolute path of the template entry.</param>
    /// <param name="ownerId">The User ID of the template owner, if the template is
    /// in the personal domain.</param>
    /// <returns>The TemplateEntry for this object.</returns>
    /// <remarks>The <c>path</c> provided must represent an absolute path and start with
    /// either the <c>public:\</c> or <c>personal:\</c> domain prefix. For personal templates,
    /// the path is assumed not to be a "user-qualified" path and, instead, the owner of
    /// the template will be determined by the <c>ownerId</c> parameter.</remarks>
    public static TemplateEntry Parse(string path, string ownerId)
    {
      return new TemplateEntry(FileSystemEntry.Parse(path, ownerId));
    }

    /// <summary>
    /// Combines a TenplateEntry and a string representing a subpath into a new TemplateEntry.
    /// </summary>
    /// <param name="folderEntry">The TemplateEntry which represents the parent folder.</param>
    /// <param name="relativePath">The relative path of the template or folder.</param>
    /// <returns>Returns a TemplateEntry representing the combined path.</returns>
    /// <remarks>The <c>folderEntry</c> must be non-null and have a <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry.EntryType" /> of
    /// Folder. The <c>relativePath</c> must be a string which does not start with the folder
    /// seperator character (\).</remarks>
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

    /// <summary>
    /// Gets a TemplateEntry that represents the root of the personal domain owned by the specified user.
    /// </summary>
    /// <param name="owner">The User ID of the owner.</param>
    /// <returns>Returns a TemplateEntry representing the root of the owner's private domain.</returns>
    public static TemplateEntry GetPersonalRoot(string owner)
    {
      return new TemplateEntry(FileSystemEntry.PrivateRoot(owner));
    }

    internal FileSystemEntry Unwrap() => this.fsEntry;
  }
}
