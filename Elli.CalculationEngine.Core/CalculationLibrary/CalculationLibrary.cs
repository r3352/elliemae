// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.CalculationLibrary
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  public class CalculationLibrary : ICalculationLibrary
  {
    public string Name { get; set; }

    public string ServerPath { get; set; }

    public string LocalPath { get; set; }

    public string AssemblyLibraryPath { get; set; }

    public string AssemblyOutputPath { get; set; }

    public List<string> EntityTypes { get; set; }

    public string RootEntityType { get; set; }

    public string RootRelationship { get; set; }

    public string ObjectModelRoot { get; set; }

    public string ObjectModelNamespace { get; set; }

    public string ObjectModelAssemblyName { get; set; }
  }
}
