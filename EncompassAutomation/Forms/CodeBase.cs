// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CodeBase
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents the definition of a codebase assembly for an Encompass input form.
  /// </summary>
  /// <remarks>A codebase assembly can be attached to an Encompass input form to provide even grater
  /// extensions of an input form's functionality beyond what would be permitted simply using the
  /// Encompass Form Builder.</remarks>
  public class CodeBase
  {
    /// <summary>
    /// Represents an Empty codebase, where no assembly is specified.
    /// </summary>
    public static readonly CodeBase Empty = new CodeBase();
    private string assemblyPath = "";
    private string assemblyName = "";
    private string assemblyVersion = "";
    private string className = "";

    /// <summary>
    /// Constructor from an existing assembly on the local disk.
    /// </summary>
    /// <param name="assemblyPath">The path to the assembly file.</param>
    /// <param name="assemblyName">The full name of the assembly.</param>
    /// <param name="assemblyVersion">The assembly's version.</param>
    /// <param name="className">The full name of the class within the assembly, including namespace,
    /// which acts as the codebase for the form.</param>
    public CodeBase(
      string assemblyPath,
      string assemblyName,
      string assemblyVersion,
      string className)
    {
      if ((assemblyName ?? "") == "")
        throw new ArgumentException("Assembly name cannot be blank or null");
      if ((className ?? "") == "")
        throw new ArgumentException("Class name cannot be blank or null");
      this.assemblyPath = assemblyPath ?? "";
      this.assemblyName = assemblyName ?? "";
      this.assemblyVersion = assemblyVersion ?? "";
      this.className = className ?? "";
    }

    private CodeBase()
    {
    }

    /// <summary>Gets the name of the codebase assembly.</summary>
    public string AssemblyName => this.assemblyName;

    /// <summary>
    /// Gets the version information for the codebase assembly.
    /// </summary>
    public string AssemblyVersion => this.assemblyVersion;

    /// <summary>
    /// Gets the full name of the class which acts as the form's codebase.
    /// </summary>
    public string ClassName => this.className;

    /// <summary>Returns the local path of the codebase assembly.</summary>
    /// <remarks>This property's value may or may not be valid. It is set at the time the codebase
    /// is initially attached to the form.</remarks>
    public string AssemblyPath => this.assemblyPath;

    /// <summary>
    /// Provides a string representation of the CodeBase object.
    /// </summary>
    /// <returns>Returns the name of the assembly and class.</returns>
    public override string ToString()
    {
      return this.ClassName == "" ? "" : this.AssemblyName + ", " + this.ClassName;
    }
  }
}
