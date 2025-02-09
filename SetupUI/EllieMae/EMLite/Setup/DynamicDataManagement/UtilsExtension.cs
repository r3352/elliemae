// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.UtilsExtension
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public static class UtilsExtension
  {
    public static int ValidateCalculation(string advancedCode, ref bool needsUpdate)
    {
      if (advancedCode == "")
      {
        needsUpdate = true;
        return 4;
      }
      using (RuntimeContext context = RuntimeContext.Create())
      {
        try
        {
          CustomFieldInfo customFieldInfo = new CustomFieldInfo("CX.TEST", FieldFormat.DECIMAL);
          customFieldInfo.Calculation = advancedCode;
          if (new CustomFieldsInfo(new BinaryObject(new CustomFieldsInfo(false)
          {
            customFieldInfo
          }.ToString(), Encoding.Default).ToString(Encoding.Default)).GetField("CX.TEST").Calculation != customFieldInfo.Calculation)
          {
            needsUpdate = true;
            return 5;
          }
          new CalculationBuilder().CreateImplementation(new CustomCalculation(customFieldInfo.Calculation), context);
          needsUpdate = false;
        }
        catch (CompileException ex)
        {
          needsUpdate = true;
          return 6;
        }
        catch (Exception ex)
        {
          needsUpdate = true;
          return 7;
        }
      }
      return 0;
    }

    public static DDMCriteria GetDataTableValueCriteria(
      CellData cellData,
      FieldDefinition fieldDefinition)
    {
      switch (fieldDefinition.Format)
      {
        case FieldFormat.NONE:
          if (cellData.Criteria != DDMCriteria.Equals)
            return cellData.Criteria;
          return string.IsNullOrEmpty(cellData.Data) ? DDMCriteria.none : DDMCriteria.OP_SpecificValue;
        case FieldFormat.STRING:
        case FieldFormat.YN:
        case FieldFormat.PHONE:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
          if (DDM_FieldAccess_Utils.IsCountyField(fieldDefinition.FieldID) && cellData.Criteria == DDMCriteria.Equals)
            return DDMCriteria.county_SpecificValue;
          if (DDM_FieldAccess_Utils.IsCountyField(fieldDefinition.FieldID) && cellData.Criteria == DDMCriteria.ListOfValues)
            return DDMCriteria.county_FindByCounty;
          if (cellData.Criteria == DDMCriteria.Equals)
            return DDMCriteria.strEquals;
          return cellData.Criteria == DDMCriteria.NotEqual ? DDMCriteria.strNotEqual : cellData.Criteria;
        case FieldFormat.ZIPCODE:
          if (cellData.Criteria == DDMCriteria.Equals)
            return DDMCriteria.zip_SpecificValue;
          return cellData.Criteria == DDMCriteria.ListOfValues ? DDMCriteria.zip_FindByZip : cellData.Criteria;
        case FieldFormat.STATE:
          if (cellData.Criteria == DDMCriteria.ListOfValues)
            return DDMCriteria.st_ListOfValues;
          return cellData.Criteria == DDMCriteria.Equals ? DDMCriteria.strEquals : cellData.Criteria;
        case FieldFormat.SSN:
          if (cellData.Criteria == DDMCriteria.Equals)
            return DDMCriteria.SSN_SpecificValue;
          return cellData.Criteria == DDMCriteria.ListOfValues ? DDMCriteria.SSN_ListofValues : cellData.Criteria;
        default:
          return cellData.Criteria;
      }
    }

    public static string GetDataTableValuesSanitized(CellData cellData)
    {
      return cellData.Criteria == DDMCriteria.Range ? cellData.Data.Replace('-', '|') : cellData.Data;
    }
  }
}
