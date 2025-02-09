// Decompiled with JetBrains decompiler
// Type: ImportGenesisData.GenesisImportParameters
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace ImportGenesisData
{
  public class GenesisImportParameters
  {
    private string mapFile = string.Empty;
    private string emliteFolder = string.Empty;
    private GenesisImportParameters.FileParameter[] fileParameters;

    public string MapFile => this.mapFile;

    public string EmliteFolder => this.emliteFolder;

    public GenesisImportParameters.FileParameter[] FileParameters => this.fileParameters;

    public GenesisImportParameters(
      string mapFile,
      string emliteFolder,
      GenesisImportParameters.FileParameter[] fileParameters)
    {
      this.mapFile = mapFile;
      this.emliteFolder = emliteFolder;
      this.fileParameters = fileParameters;
    }

    public class FileParameter
    {
      private string fileName = string.Empty;
      private string firstName = string.Empty;
      private string lastName = string.Empty;

      public string FileName => this.fileName;

      public string FirstName => this.firstName;

      public string LastName => this.lastName;

      public FileParameter(string fileName, string firstName, string lastName)
      {
        this.fileName = fileName;
        this.firstName = firstName;
        this.lastName = lastName;
      }
    }
  }
}
