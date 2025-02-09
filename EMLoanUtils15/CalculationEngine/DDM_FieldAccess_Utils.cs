// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.DDM_FieldAccess_Utils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class DDM_FieldAccess_Utils
  {
    public const string IGNORE_VALUE_IN_LOAN_FILE = "Ignore this field�";
    public const string NO_VALUE_IN_LOAN_FILE = "No value in loan file�";
    public const string USE_CALCULATED_VALUE = "Use Calculated Value�";
    private const int FEENAMEINDEX = 1;
    private static readonly List<string[]> DDM_FIELDS_CALCULATED = new List<string[]>()
    {
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801a.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801b.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801c.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801d.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801e.ToString(),
        "",
        "1",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "1",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801f.ToString(),
        "",
        "1",
        "",
        "",
        "",
        "1",
        "1",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "",
        "1",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        "0",
        "1",
        "",
        "",
        "",
        "1",
        "0",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For801g.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For802aTO802d.ToString(),
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For802e.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "1",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For802fTO802h.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For803TO807.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For808TO835.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For902.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "1",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "1",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For901TO912.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For905.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "1",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1002To1009.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "1",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1003.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "1",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1010.ToString(),
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "0",
        "",
        "1",
        "1",
        "1",
        "",
        "",
        "1",
        "0",
        "0",
        "",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1101x.ToString(),
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        ""
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1101bTo1101f.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "",
        "0",
        "",
        "",
        "",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1101a.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "",
        "0",
        "0",
        "0",
        "",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1102c.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1102aTo1102h.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "0",
        "0",
        "",
        "",
        "0",
        "0",
        "0",
        "",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1103.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1104.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1109TO1116.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1202TO1210.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "1",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1302.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1303TO1309.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "1",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "0",
        "0",
        "1",
        "0",
        "0",
        "0",
        "0",
        "0"
      },
      new string[49]
      {
        DDM_FieldAccess_Utils.DDMLineRanges.For1310TO1320.ToString(),
        "",
        "0",
        "",
        "",
        "",
        "0",
        "1",
        "0",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "1",
        "1",
        "1",
        "1",
        "",
        "",
        "0",
        "1",
        "1",
        "0",
        "1",
        "1",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "0",
        "0",
        "1",
        "1",
        "0",
        "",
        "",
        "1",
        "0",
        "0",
        "0",
        "0",
        ""
      }
    };
    private static readonly List<string> DDM_FeeRolodexFields = new List<string>()
    {
      "NEWHUD.X1050",
      "NEWHUD.X1051",
      "NEWHUD.X1052",
      "NEWHUD.X1053",
      "NEWHUD.X1054",
      "NEWHUD.X1055",
      "NEWHUD.X1056",
      "NEWHUD.X1057",
      "NEWHUD.X1058",
      "NEWHUD.X1059",
      "NEWHUD.X1292",
      "NEWHUD.X1300",
      "NEWHUD.X1308",
      "NEWHUD.X1316",
      "NEWHUD.X1324",
      "NEWHUD.X1332",
      "NEWHUD.X1340",
      "NEWHUD.X1348",
      "NEWHUD.X1356",
      "NEWHUD.X1364",
      "NEWHUD.X1372",
      "NEWHUD.X1380",
      "NEWHUD.X1388",
      "NEWHUD.X1396",
      "NEWHUD.X1404",
      "NEWHUD.X1412",
      "NEWHUD.X1060",
      "NEWHUD.X1061"
    };
    private static readonly List<string> DDM_CountyFields = new List<string>()
    {
      "13",
      "MORNET.X26",
      "3901",
      "1949",
      "977",
      "BR0122",
      "BR0222",
      "BR0322",
      "BR0422",
      "BR0522",
      "BR0622",
      "BR0722",
      "BR0822",
      "BR0922",
      "BR1022",
      "CR0122",
      "CR0222",
      "CR0322",
      "CR0422",
      "CR0522",
      "CR0622",
      "CR0722",
      "CR0822",
      "CR0922",
      "CR1022",
      "2944",
      "Closing.LndCnty",
      "Closing.Trst1Cnty",
      "Closing.Trst2Cnty",
      "Closing.LndNtryCmsnCnty"
    };
    private static readonly List<string[]> DDM_AdditionalFieldPerLine = new List<string[]>()
    {
      new string[2]{ "0801a", "388" },
      new string[3]{ "0801e", "389", "1620" },
      new string[4]
      {
        "0801f",
        "NEWHUD.X223",
        "NEWHUD.X224",
        "NEWHUD.X1718"
      },
      new string[2]{ "0801g", "154" },
      new string[2]{ "0801h", "1627" },
      new string[2]{ "0801i", "1838" },
      new string[2]{ "0801j", "1841" },
      new string[2]{ "0801k", "NEWHUD.X732" },
      new string[2]{ "0801l", "NEWHUD.X1235" },
      new string[2]{ "0801m", "NEWHUD.X1243" },
      new string[2]{ "0801n", "NEWHUD.X1251" },
      new string[2]{ "0801o", "NEWHUD.X1259" },
      new string[2]{ "0801p", "NEWHUD.X1267" },
      new string[2]{ "0801q", "NEWHUD.X1275" },
      new string[2]{ "0801r", "NEWHUD.X1283" },
      new string[3]{ "0802a", "NEWHUD.X1141", "NEWHUD.X1225" },
      new string[4]
      {
        "0802b",
        "NEWHUD.X1143",
        "NEWHUD.X1226",
        "NEWHUD.X1170"
      },
      new string[4]
      {
        "0802c",
        "NEWHUD.X1145",
        "NEWHUD.X1146",
        "NEWHUD.X1172"
      },
      new string[4]
      {
        "0802d",
        "NEWHUD.X1147",
        "NEWHUD.X1148",
        "NEWHUD.X1174"
      },
      new string[4]
      {
        "0802e",
        "NEWHUD.X1067",
        "NEWHUD.X1150",
        "NEWHUD.X1227"
      },
      new string[3]{ "0802f", "NEWHUD.X1153", "NEWHUD.X1154" },
      new string[3]{ "0802g", "NEWHUD.X1157", "NEWHUD.X1158" },
      new string[3]{ "0802h", "NEWHUD.X1161", "NEWHUD.X1162" },
      new string[2]{ "0803", "NEWHUD2.X7" },
      new string[2]{ "0808", "NEWHUD.X126" },
      new string[2]{ "0809", "NEWHUD.X127" },
      new string[2]{ "0810", "NEWHUD.X128" },
      new string[2]{ "0811", "NEWHUD.X129" },
      new string[2]{ "0812", "NEWHUD.X130" },
      new string[2]{ "0813", "369" },
      new string[2]{ "0814", "371" },
      new string[2]{ "0815", "348" },
      new string[2]{ "0816", "931" },
      new string[2]{ "0817", "1390" },
      new string[2]{ "0818", "NEWHUD.X1291" },
      new string[2]{ "0819", "NEWHUD.X1299" },
      new string[2]{ "0820", "NEWHUD.X1307" },
      new string[2]{ "0821", "NEWHUD.X1315" },
      new string[2]{ "0822", "NEWHUD.X1323" },
      new string[2]{ "0823", "NEWHUD.X1331" },
      new string[2]{ "0824", "NEWHUD.X1339" },
      new string[2]{ "0825", "NEWHUD.X1347" },
      new string[2]{ "0826", "NEWHUD.X1355" },
      new string[2]{ "0827", "NEWHUD.X1363" },
      new string[2]{ "0828", "NEWHUD.X1371" },
      new string[2]{ "0829", "NEWHUD.X1379" },
      new string[2]{ "0830", "NEWHUD.X1387" },
      new string[2]{ "0831", "NEWHUD.X1395" },
      new string[2]{ "0832", "NEWHUD.X1403" },
      new string[2]{ "0833", "NEWHUD.X1411" },
      new string[2]{ "0834", "410" },
      new string[2]{ "0835", "NEWHUD.X656" },
      new string[5]{ "0901", "SYS.X8", "332", "L244", "L245" },
      new string[29]
      {
        "0902",
        "337",
        "356",
        "1107",
        "1109",
        "1198",
        "1199",
        "1200",
        "1201",
        "1205",
        "1209",
        "1753",
        "1757",
        "1760",
        "1765",
        "1826",
        "2978",
        "3248",
        "3262",
        "3531",
        "3532",
        "3533",
        "3560",
        "3563",
        "3566",
        "3625",
        "SYS.X11",
        "VASUMM.X49",
        "VAVOB.X72"
      },
      new string[4]{ "0903", "L251", "1322", "1750" },
      new string[3]{ "0904", "NEWHUD2.X4397", "231" },
      new string[3]{ "0906", "NEWHUD2.X4399", "NEWHUD2.X4400" },
      new string[7]
      {
        "0907",
        "NEWHUD2.X4401",
        "NEWHUD2.X4402",
        "NEWHUD2.X4435",
        "NEWHUD2.X4415",
        "NEWHUD2.X4416",
        "L259"
      },
      new string[7]
      {
        "0908",
        "NEWHUD2.X4403",
        "NEWHUD2.X4404",
        "NEWHUD2.X4436",
        "NEWHUD2.X4417",
        "NEWHUD2.X4418",
        "1666"
      },
      new string[7]
      {
        "0909",
        "NEWHUD2.X4405",
        "NEWHUD2.X4406",
        "NEWHUD2.X4437",
        "NEWHUD2.X4419",
        "NEWHUD2.X4420",
        "NEWHUD.X583"
      },
      new string[7]
      {
        "0910",
        "NEWHUD2.X4407",
        "NEWHUD2.X4408",
        "NEWHUD2.X4438",
        "NEWHUD2.X4421",
        "NEWHUD2.X4422",
        "NEWHUD.X584"
      },
      new string[7]
      {
        "0911",
        "NEWHUD2.X4409",
        "NEWHUD2.X4410",
        "NEWHUD2.X4439",
        "NEWHUD2.X4423",
        "NEWHUD2.X4424",
        "NEWHUD.X1586"
      },
      new string[7]
      {
        "0912",
        "NEWHUD2.X4411",
        "NEWHUD2.X4412",
        "NEWHUD2.X4440",
        "NEWHUD2.X4425",
        "NEWHUD2.X4426",
        "NEWHUD.X1594"
      },
      new string[4]{ "1002", "1387", "230", "NEWHUD2.X133" },
      new string[26]
      {
        "1003",
        "1107",
        "1109",
        "1198",
        "1199",
        "1200",
        "1201",
        "1205",
        "1209",
        "1753",
        "1757",
        "1760",
        "1765",
        "1826",
        "2978",
        "3248",
        "3262",
        "3531",
        "3532",
        "3533",
        "3625",
        "SYS.X11",
        "VASUMM.X49",
        "VAVOB.X72",
        "1296",
        "232"
      },
      new string[6]
      {
        "1004",
        "1386",
        "231",
        "NEWHUD2.X134",
        "1751",
        "1752"
      },
      new string[4]{ "1005", "L267", "L268", "NEWHUD2.X135" },
      new string[4]{ "1006", "1388", "235", "NEWHUD2.X136" },
      new string[8]
      {
        "1007",
        "1628",
        "1629",
        "1630",
        "NEWHUD2.X124",
        "NEWHUD2.X125",
        "NEWHUD2.X126",
        "NEWHUD2.X137"
      },
      new string[8]
      {
        "1008",
        "660",
        "340",
        "253",
        "NEWHUD2.X127",
        "NEWHUD2.X128",
        "NEWHUD2.X129",
        "NEWHUD2.X138"
      },
      new string[8]
      {
        "1009",
        "661",
        "341",
        "254",
        "NEWHUD2.X130",
        "NEWHUD2.X131",
        "NEWHUD2.X132",
        "NEWHUD2.X139"
      },
      new string[13]
      {
        "1010",
        "NEWHUD.X1706",
        "NEWHUD.X1707",
        "356",
        "1109",
        "1198",
        "1199",
        "1200",
        "1201",
        "3560",
        "3563",
        "3566",
        "NEWHUD2.X140"
      },
      new string[2]{ "1101a", "NEWHUD.X951" },
      new string[2]{ "1101b", "NEWHUD.X960" },
      new string[2]{ "1101c", "NEWHUD.X969" },
      new string[2]{ "1101d", "NEWHUD.X978" },
      new string[2]{ "1101e", "NEWHUD.X987" },
      new string[2]{ "1101f", "NEWHUD.X996" },
      new string[2]{ "1102d", "NEWHUD.X809" },
      new string[2]{ "1102e", "NEWHUD.X811" },
      new string[2]{ "1102f", "NEWHUD.X813" },
      new string[2]{ "1102g", "NEWHUD.X815" },
      new string[2]{ "1102h", "NEWHUD.X817" },
      new string[4]
      {
        "1103",
        "NEWHUD2.X3335",
        "NEWHUD2.X4441",
        "NEWHUD2.X4442"
      },
      new string[5]
      {
        "1104",
        "NEWHUD2.X4443",
        "NEWHUD2.X4444",
        "NEWHUD2.X3361",
        "NEWHUD2.X3362"
      },
      new string[2]{ "1109", "NEWHUD.X208" },
      new string[2]{ "1110", "NEWHUD.X209" },
      new string[2]{ "1111", "1762" },
      new string[2]{ "1112", "1767" },
      new string[2]{ "1113", "1772" },
      new string[2]{ "1114", "1777" },
      new string[2]{ "1115", "NEWHUD.X1602" },
      new string[2]{ "1116", "NEWHUD.X1610" },
      new string[6]
      {
        "1202",
        "2402",
        "2403",
        "2404",
        "1636",
        "NEWHUD2.X121"
      },
      new string[2]{ "1203", "NEWHUD.X947" },
      new string[5]
      {
        "1204",
        "2405",
        "2406",
        "1637",
        "NEWHUD2.X122"
      },
      new string[5]
      {
        "1205",
        "2407",
        "2408",
        "1638",
        "NEWHUD2.X123"
      },
      new string[3]{ "1206", "373", "NEWHUD.X1082" },
      new string[3]{ "1207", "1640", "NEWHUD.X1083" },
      new string[3]{ "1208", "1643", "NEWHUD.X1084" },
      new string[3]{ "1209", "NEWHUD.X1618", "NEWHUD.X1619" },
      new string[3]{ "1210", "NEWHUD.X1625", "NEWHUD.X1626" },
      new string[2]{ "1302", "NEWHUD.X251" },
      new string[2]{ "1303", "650" },
      new string[2]{ "1304", "651" },
      new string[2]{ "1305", "40" },
      new string[2]{ "1306", "43" },
      new string[2]{ "1307", "1782" },
      new string[2]{ "1308", "1787" },
      new string[2]{ "1309", "1792" },
      new string[3]{ "1310", "NEWHUD.X252", "NEWHUD2.X4196" },
      new string[3]{ "1311", "NEWHUD.X253", "NEWHUD2.X4229" },
      new string[3]{ "1312", "NEWHUD.X1632", "NEWHUD2.X4262" },
      new string[3]{ "1313", "NEWHUD.X1640", "NEWHUD2.X4295" },
      new string[3]{ "1314", "NEWHUD.X1648", "NEWHUD2.X4328" },
      new string[3]{ "1315", "NEWHUD.X1656", "NEWHUD2.X4361" },
      new string[3]{ "1316", "NEWHUD2.X4610", "NEWHUD2.X4447" },
      new string[3]{ "1317", "NEWHUD2.X4617", "NEWHUD2.X4480" },
      new string[3]{ "1318", "NEWHUD2.X4624", "NEWHUD2.X4513" },
      new string[3]{ "1319", "NEWHUD2.X4631", "NEWHUD2.X4546" },
      new string[3]{ "1320", "NEWHUD2.X4638", "NEWHUD2.X4579" }
    };
    private static HashSet<string> bannedFieldsForFeeRules;
    public const string FIELD_AFFECT_MODE_MX = "C�";
    public const string FIELD_AFFECT_MODE_NMX = "R�";
    private static readonly Dictionary<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo> FEES_AFFECTED_BY_SYSTBL = new Dictionary<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>()
    {
      {
        "337",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "902",
          Fields = new Dictionary<string, object>()
          {
            {
              "3533",
              (object) null
            },
            {
              "1198",
              (object) null
            },
            {
              "1199",
              (object) null
            }
          }
        }
      },
      {
        "3533",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "902",
          Fields = new Dictionary<string, object>()
          {
            {
              "337",
              (object) null
            },
            {
              "232",
              (object) null
            },
            {
              "1107",
              (object) null
            },
            {
              "1826",
              (object) null
            },
            {
              "1765",
              (object) null
            },
            {
              "1760",
              (object) null
            },
            {
              "1045",
              (object) null
            },
            {
              "3531",
              (object) null
            },
            {
              "3532",
              (object) null
            },
            {
              "1199",
              (object) null
            },
            {
              "1198",
              (object) null
            },
            {
              "1201",
              (object) null
            },
            {
              "1200",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "642",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "903",
          Fields = new Dictionary<string, object>()
          {
            {
              "1322",
              (object) "R"
            },
            {
              "L251",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X591",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "904",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4397",
              (object) "R"
            },
            {
              "231",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "643",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "906",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4399",
              (object) "R"
            },
            {
              "NEWHUD2.X4400",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "L260",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "907",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4401",
              (object) "R"
            },
            {
              "NEWHUD2.X4402",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "1667",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "908",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4403",
              (object) "R"
            },
            {
              "NEWHUD2.X4404",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X592",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "909",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4405",
              (object) "R"
            },
            {
              "NEWHUD2.X4406",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X593",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "910",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4407",
              (object) "R"
            },
            {
              "NEWHUD2.X4408",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X1588",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "911",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4409",
              (object) "R"
            },
            {
              "NEWHUD2.X4410",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X1596",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "912",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X4411",
              (object) "R"
            },
            {
              "NEWHUD2.X4412",
              (object) "R"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X808",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1102c",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X3129",
              (object) null
            }
          }
        }
      },
      {
        "NEWHUD.X572",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1103",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X3327",
              (object) null
            }
          }
        }
      },
      {
        "NEWHUD.X639",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1104",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X3360",
              (object) null
            }
          }
        }
      },
      {
        "1637",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1204",
          Fields = new Dictionary<string, object>()
          {
            {
              "647",
              (object) null
            },
            {
              "2405",
              (object) "C"
            },
            {
              "2406",
              (object) "C"
            }
          }
        }
      },
      {
        "1638",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1205",
          Fields = new Dictionary<string, object>()
          {
            {
              "648",
              (object) null
            },
            {
              "2407",
              (object) "C"
            },
            {
              "2408",
              (object) "C"
            }
          }
        }
      },
      {
        "373",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1206",
          Fields = new Dictionary<string, object>()
          {
            {
              "374",
              (object) null
            }
          }
        }
      },
      {
        "374",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1206",
          Fields = new Dictionary<string, object>()
          {
            {
              "373",
              (object) null
            }
          }
        }
      },
      {
        "1640",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1207",
          Fields = new Dictionary<string, object>()
          {
            {
              "1641",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "1641",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1207",
          Fields = new Dictionary<string, object>()
          {
            {
              "1640",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "1643",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1208",
          Fields = new Dictionary<string, object>()
          {
            {
              "1644",
              (object) null
            }
          }
        }
      },
      {
        "1644",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1208",
          Fields = new Dictionary<string, object>()
          {
            {
              "1643",
              (object) null
            }
          }
        }
      },
      {
        "232",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1003",
          Fields = new Dictionary<string, object>()
          {
            {
              "1198",
              (object) null
            },
            {
              "1199",
              (object) null
            }
          }
        }
      },
      {
        "NEWHUD.X1707",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1010",
          Fields = new Dictionary<string, object>()
          {
            {
              "1198",
              (object) null
            },
            {
              "1199",
              (object) null
            }
          }
        }
      },
      {
        "231",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1004",
          Fields = new Dictionary<string, object>()
          {
            {
              "1752",
              (object) "C"
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "1752",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "1004",
          Fields = new Dictionary<string, object>()
          {
            {
              "231",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "332",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "901",
          Fields = new Dictionary<string, object>()
          {
            {
              "L244",
              (object) null
            },
            {
              "L245",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "L244",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "901",
          Fields = new Dictionary<string, object>()
          {
            {
              "332",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "L245",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "901",
          Fields = new Dictionary<string, object>()
          {
            {
              "332",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD.X599",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "907",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X2371",
              (object) null
            },
            {
              "NEWHUD2.X2372",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD2.X2371",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "907",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD.X599",
              (object) null
            },
            {
              "NEWHUD2.X2372",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      },
      {
        "NEWHUD2.X2372",
        new DDM_FieldAccess_Utils.SysTblInfluenceInfo()
        {
          LineNumber = "907",
          Fields = new Dictionary<string, object>()
          {
            {
              "NEWHUD2.X2371",
              (object) null
            },
            {
              "NEWHUD.X599",
              (object) null
            }
          },
          IsNonSystemTableType = true
        }
      }
    };
    public static Dictionary<string, FieldFormat> FieldDefinitionOverrides = new Dictionary<string, FieldFormat>()
    {
      {
        "2025",
        FieldFormat.DATE
      }
    };

    public static string GetDDMLineRangeForCalculatedFields(string feeLine)
    {
      string strA = "";
      int num = Utils.ParseInt((object) feeLine);
      if (num == -1)
      {
        if (feeLine != "" && feeLine.Length > 1)
        {
          strA = feeLine.Substring(feeLine.Length - 1);
          num = Utils.ParseInt((object) feeLine.Substring(0, feeLine.Length - 1));
        }
        if (num == -1)
          return "";
      }
      if ((num == 801 || num == 802 || num == 1101) && strA == "" || num == 803 && string.Compare(strA, "x", true) == 0)
        return "";
      if (num == 801)
      {
        if (string.Compare(strA, "a", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801a.ToString();
        if (string.Compare(strA, "b", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801b.ToString();
        if (string.Compare(strA, "c", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801c.ToString();
        if (string.Compare(strA, "d", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801d.ToString();
        if (string.Compare(strA, "e", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801e.ToString();
        if (string.Compare(strA, "f", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801f.ToString();
        if ("ghijklmnopqr".IndexOf(strA.ToLower()) > -1)
          return DDM_FieldAccess_Utils.DDMLineRanges.For801g.ToString();
      }
      if (num == 802)
      {
        if ("abcd".IndexOf(strA.ToLower()) > -1)
          return DDM_FieldAccess_Utils.DDMLineRanges.For802aTO802d.ToString();
        if (string.Compare(strA, "e", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For802e.ToString();
        return "fgh".IndexOf(strA.ToLower()) > -1 ? DDM_FieldAccess_Utils.DDMLineRanges.For802fTO802h.ToString() : string.Empty;
      }
      if (num >= 803 && num <= 807)
        return DDM_FieldAccess_Utils.DDMLineRanges.For803TO807.ToString();
      if (num >= 808 && num <= 835)
        return DDM_FieldAccess_Utils.DDMLineRanges.For808TO835.ToString();
      if (num == 902)
        return DDM_FieldAccess_Utils.DDMLineRanges.For902.ToString();
      if (num == 905)
        return DDM_FieldAccess_Utils.DDMLineRanges.For905.ToString();
      if (num == 901 || num >= 903 && num <= 912 && num != 905)
        return DDM_FieldAccess_Utils.DDMLineRanges.For901TO912.ToString();
      if (num >= 1002 && num <= 1006 && num != 1003 || num >= 1007 && num <= 1009)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1002To1009.ToString();
      if (num == 1003)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1003.ToString();
      if (num == 1010)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1010.ToString();
      if (num == 1101)
      {
        if ("a".IndexOf(strA.ToLower()) > -1)
          return DDM_FieldAccess_Utils.DDMLineRanges.For1101a.ToString();
        if ("bcdef".IndexOf(strA.ToLower()) > -1)
          return DDM_FieldAccess_Utils.DDMLineRanges.For1101bTo1101f.ToString();
        if (strA.ToLower().Equals("x"))
          return DDM_FieldAccess_Utils.DDMLineRanges.For1101x.ToString();
      }
      if (num == 1102)
      {
        if ("abdefgh".IndexOf(strA.ToLower()) > -1)
          return DDM_FieldAccess_Utils.DDMLineRanges.For1102aTo1102h.ToString();
        if (string.Compare(strA, "c", true) == 0)
          return DDM_FieldAccess_Utils.DDMLineRanges.For1102c.ToString();
      }
      if (num == 1103)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1103.ToString();
      if (num == 1104)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1104.ToString();
      if (num >= 1109 && num <= 1116)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1109TO1116.ToString();
      if (num >= 1202 && num <= 1210)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1202TO1210.ToString();
      if (num == 1302)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1302.ToString();
      if (num >= 1303 && num <= 1309)
        return DDM_FieldAccess_Utils.DDMLineRanges.For1303TO1309.ToString();
      return num >= 1310 && num <= 1320 ? DDM_FieldAccess_Utils.DDMLineRanges.For1310TO1320.ToString() : "";
    }

    public static bool IsFieldLockIconField(string field)
    {
      if (field.ToLower().StartsWith("cx."))
        return false;
      FieldDefinition field1 = EncompassFields.GetField(field);
      return field1 != null && field1.FieldLockIcon;
    }

    public static bool IsRolodexField(string field)
    {
      if (field.ToLower().StartsWith("cx."))
        return false;
      FieldDefinition field1 = EncompassFields.GetField(field);
      if (field1 == null)
        return false;
      string rolodex = field1.Rolodex;
      return rolodex != null && !string.IsNullOrEmpty(rolodex.Trim());
    }

    public static bool IsRolodexField(string field, out string categoryName)
    {
      categoryName = (string) null;
      if (field.ToLower().StartsWith("cx."))
        return false;
      FieldDefinition field1 = EncompassFields.GetField(field);
      if (field1 == null)
        return false;
      categoryName = string.IsNullOrEmpty(field1.Rolodex) ? (string) null : field1.Rolodex.Trim();
      return !string.IsNullOrEmpty(categoryName);
    }

    public static bool IsValidSubLineNumber(string line)
    {
      List<string> stringList = new List<string>();
      string[] collection = new string[26]
      {
        "801a",
        "801b",
        "801c",
        "801d",
        "801e",
        "801f",
        "801g",
        "801h",
        "801i",
        "801j",
        "801k",
        "801l",
        "801m",
        "801n",
        "801o",
        "801p",
        "801q",
        "801r",
        "802a",
        "802b",
        "802c",
        "802d",
        "802e",
        "802f",
        "802g",
        "802h"
      };
      stringList.AddRange((IEnumerable<string>) collection);
      return !string.IsNullOrEmpty(stringList.Find((Predicate<string>) (x => x == line)));
    }

    public static bool IsAmountFieldLockable(string field)
    {
      return field == "388" || field == "NEWHUD.X1154" || field == "NEWHUD.X1158" || field == "NEWHUD.X1162";
    }

    public static string GetLockableAmountFieldID(string field)
    {
      switch (field)
      {
        case "388":
          return "454";
        case "NEWHUD.X1154":
          return "NEWHUD.X1155";
        case "NEWHUD.X1158":
          return "NEWHUD.X1159";
        case "NEWHUD.X1162":
          return "NEWHUD.X1163";
        default:
          return (string) null;
      }
    }

    private static int splitLineNumberAndColumn(string lineNum, out string col)
    {
      int result = 0;
      col = string.Empty;
      if (int.TryParse(lineNum, out result))
        return result;
      Match match = new Regex("(\\d+)([a-zA-Z]+)").Match(lineNum);
      if (!string.IsNullOrEmpty(match.Groups[1].Value))
      {
        result = Convert.ToInt32(match.Groups[1].Value);
        col = match.Groups[2].Value;
      }
      return result;
    }

    public static List<string[]> GetFeeLineDescriptions()
    {
      List<string[]> lineDescriptions = new List<string[]>();
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        if (GFEItemCollection.GFEItems2010[index].LineNumber >= 801)
          lineDescriptions.Add(new string[2]
          {
            GFEItemCollection.GFEItems2010[index].LineNumber.ToString() + GFEItemCollection.GFEItems2010[index].ComponentID,
            GFEItemCollection.GFEItems2010[index].Description.StartsWith("NEWHUD") || GFEItemCollection.GFEItems2010[index].Description.Length <= 4 ? "User defined Fee" : GFEItemCollection.GFEItems2010[index].Description
          });
      }
      return lineDescriptions;
    }

    static DDM_FieldAccess_Utils() => DDM_FieldAccess_Utils.computeBannedFields();

    private static void computeBannedFields()
    {
      DDM_FieldAccess_Utils.bannedFieldsForFeeRules = new HashSet<string>();
      string[] strArray = new string[6]
      {
        "834",
        "835",
        "1115",
        "1116",
        "1209",
        "1210"
      };
      foreach (string feeline in strArray)
      {
        string[] fieldsForFeeRule = DDM_FieldAccess_Utils.findFieldsForFeeRule(feeline);
        if (fieldsForFeeRule != null)
        {
          int[] numArray = new int[9]
          {
            HUDGFE2010Fields.PTCPOCINDEX_PAIDBY,
            HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT,
            HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT,
            HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED
          };
          foreach (int index in numArray)
            DDM_FieldAccess_Utils.bannedFieldsForFeeRules.Add(fieldsForFeeRule[index]);
        }
      }
    }

    private static string[] findFieldsForFeeRule(string feeline)
    {
      foreach (string[] fieldsForFeeRule in HUDGFE2010Fields.WHOLEPOC_FIELDS)
      {
        string str = fieldsForFeeRule[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] + fieldsForFeeRule[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
        if (str.StartsWith("0"))
          str = str.Substring(1, str.Length - 1);
        if (feeline == str)
          return fieldsForFeeRule;
      }
      return (string[]) null;
    }

    public static List<string[]> GetDDMNonCalculatedFields(string feeLine)
    {
      List<string[]> strArrayList = new List<string[]>();
      List<string[]> calculatedFields = new List<string[]>();
      string strPart = string.Empty;
      string fLine = DDM_FieldAccess_Utils.splitLineNumberAndColumn(feeLine, out strPart).ToString().PadLeft(4, '0');
      if (fLine == "1101" && string.IsNullOrEmpty(strPart))
        strPart = "x";
      else if (fLine == "0802" && "abcd".IndexOf(strPart.ToLower()) > -1)
        strArrayList.Add(new string[1]);
      strArrayList.AddRange(string.IsNullOrEmpty(strPart) ? (IEnumerable<string[]>) HUDGFE2010Fields.WHOLEPOC_FIELDS.FindAll((Predicate<string[]>) (x => x[0] == fLine)) : (IEnumerable<string[]>) HUDGFE2010Fields.WHOLEPOC_FIELDS.FindAll((Predicate<string[]>) (x => x[0] == fLine && x[1] == strPart)));
      string[] source1 = DDM_FieldAccess_Utils.DDM_FIELDS_CALCULATED.Find((Predicate<string[]>) (x => x[0] == DDM_FieldAccess_Utils.GetDDMLineRangeForCalculatedFields(fLine + strPart)));
      foreach (string[] source2 in strArrayList)
      {
        if (!feeLine.Equals("1102") || !source2[1].Equals("c") && !source2[1].Equals(""))
        {
          List<string> stringList = new List<string>();
          string[] source3 = DDM_FieldAccess_Utils.DDM_AdditionalFieldPerLine.Find((Predicate<string[]>) (x => x[0] == fLine + strPart));
          if (source3 != null && ((IEnumerable<string>) source3).Count<string>() >= 2)
          {
            for (int index = 1; index < ((IEnumerable<string>) source3).Count<string>(); ++index)
              stringList.Add(source3[index]);
          }
          for (int index = 0; index < ((IEnumerable<string>) source2).Count<string>(); ++index)
          {
            if (!string.IsNullOrEmpty(source2[index]) && !(source2[index] == fLine) && !(source2[index] == strPart) && ((IEnumerable<string>) source1).Any<string>() && !string.IsNullOrEmpty(source1[index]) && !(source1[index] == "1"))
              stringList.Add(source2[index]);
          }
          calculatedFields.Add(stringList.FindAll((Predicate<string>) (ncField => !DDM_FieldAccess_Utils.bannedFieldsForFeeRules.Contains(ncField))).ToArray());
        }
      }
      return calculatedFields;
    }

    public static bool IsCountyField(string fieldID)
    {
      return DDM_FieldAccess_Utils.DDM_CountyFields.Contains(fieldID);
    }

    public static bool IsFeeManagementField(string fieldID)
    {
      return HUDGFE2010Fields.GetFieldSectionEnum(fieldID) != 0;
    }

    public static FeeSectionEnum GetFeeManagementFieldSectionEnum(string fieldID)
    {
      return HUDGFE2010Fields.GetFieldSectionEnum(fieldID);
    }

    public static bool DoesAffectOtherFieldsBySysTbl(string fieldId)
    {
      return DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.ContainsKey(fieldId);
    }

    public static bool DoesAffectOtherFieldsByNonSysTbl(string fieldId)
    {
      return DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.Where<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>>((Func<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>, bool>) (item => item.Value.IsNonSystemTableType && item.Key.Equals(fieldId))).Any<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>>();
    }

    public static string GetAffectedFieldsBySysTable(string fieldId)
    {
      DDM_FieldAccess_Utils.SysTblInfluenceInfo tblInfluenceInfo = DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.ContainsKey(fieldId) ? DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL[fieldId] : (DDM_FieldAccess_Utils.SysTblInfluenceInfo) null;
      return tblInfluenceInfo != null ? string.Join(", ", (IEnumerable<string>) tblInfluenceInfo.Fields.Keys.ToList<string>()) : string.Empty;
    }

    public static List<string> GetAffectedFieldsListBySysTable(string fieldId)
    {
      DDM_FieldAccess_Utils.SysTblInfluenceInfo tblInfluenceInfo = DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.ContainsKey(fieldId) ? DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL[fieldId] : (DDM_FieldAccess_Utils.SysTblInfluenceInfo) null;
      return tblInfluenceInfo != null ? tblInfluenceInfo.Fields.Keys.ToList<string>() : (List<string>) null;
    }

    public static Dictionary<string, object> GetAffectedFieldsDictBySysTableWithAction(
      string fieldId)
    {
      return (DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.ContainsKey(fieldId) ? DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL[fieldId] : (DDM_FieldAccess_Utils.SysTblInfluenceInfo) null)?.Fields;
    }

    public static Dictionary<string, object> GetAffectedFieldsDictByNonSysTableWithAction(
      string fieldId)
    {
      return DDM_FieldAccess_Utils.GetAffectedFieldsDictBySysTableWithAction(fieldId);
    }

    public static Dictionary<string, string> GetAffectedFieldsDictBySysTable(string fieldId)
    {
      List<string> fieldsListBySysTable = DDM_FieldAccess_Utils.GetAffectedFieldsListBySysTable(fieldId);
      Dictionary<string, string> fieldsDictBySysTable = (Dictionary<string, string>) null;
      if (fieldsListBySysTable != null)
      {
        fieldsDictBySysTable = new Dictionary<string, string>();
        foreach (string key in fieldsListBySysTable)
          fieldsDictBySysTable.Add(key, fieldId);
      }
      return fieldsDictBySysTable;
    }

    public static Dictionary<string, string> GetAffectingFieldsDictBySysTable(string fieldAffected)
    {
      List<\u003C\u003Ef__AnonymousType0<string, string>> list = DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.Where<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>>((Func<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>, bool>) (item => item.Value.Fields.Keys.Contains<string>(fieldAffected))).Select(pair => new
      {
        Affected = fieldAffected,
        Affecting = pair.Key
      }).ToList();
      Dictionary<string, string> fieldsDictBySysTable = (Dictionary<string, string>) null;
      if (list != null)
      {
        fieldsDictBySysTable = new Dictionary<string, string>();
        foreach (var data in list)
        {
          if (!fieldsDictBySysTable.Keys.Contains<string>(data.Affecting))
            fieldsDictBySysTable.Add(data.Affecting, fieldAffected);
        }
      }
      return fieldsDictBySysTable;
    }

    public static bool isSpecialFeeLine(string feeLine)
    {
      switch (feeLine)
      {
        case null:
          return false;
        case "1206":
        case "1207":
          return true;
        default:
          return feeLine == "1208";
      }
    }

    public static bool IsWholePocField(string lineNumber, string fieldId, int index)
    {
      string strPart = string.Empty;
      string fLine = DDM_FieldAccess_Utils.splitLineNumberAndColumn(lineNumber, out strPart).ToString().PadLeft(4, '0');
      if (fLine == "1101" && string.IsNullOrEmpty(strPart))
        strPart = "x";
      return (string.IsNullOrEmpty(strPart) ? HUDGFE2010Fields.WHOLEPOC_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x => x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine && x[index] == fieldId)) : HUDGFE2010Fields.WHOLEPOC_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x => x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine && x[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == strPart && x[index] == fieldId))) != null;
    }

    public static bool IsTaxesField(string lineNumber, string fieldId)
    {
      string strPart = string.Empty;
      string fLine = DDM_FieldAccess_Utils.splitLineNumberAndColumn(lineNumber, out strPart).ToString().PadLeft(4, '0');
      if (fLine == "1101" && string.IsNullOrEmpty(strPart))
        strPart = "x";
      return (string.IsNullOrEmpty(strPart) ? HUDGFE2010Fields.INSURANCE_PROPERTY_TAXES_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x =>
      {
        if (!(x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine))
          return false;
        return x[HUDGFE2010Fields.PTCPOCINDEX_TAXES] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY] == fieldId;
      })) : HUDGFE2010Fields.INSURANCE_PROPERTY_TAXES_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x =>
      {
        if (!(x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine) || !(x[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == strPart))
          return false;
        return x[HUDGFE2010Fields.PTCPOCINDEX_TAXES] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY] == fieldId;
      }))) != null;
    }

    public static string[] ValidateSellerObligated(
      string lineNumber,
      string fieldId,
      DDMFeeRuleScenario currentScenario)
    {
      string strPart = string.Empty;
      string fLine = DDM_FieldAccess_Utils.splitLineNumberAndColumn(lineNumber, out strPart).ToString().PadLeft(4, '0');
      if (fLine == "1101" && string.IsNullOrEmpty(strPart))
        strPart = "x";
      string[] row = string.IsNullOrEmpty(strPart) ? HUDGFE2010Fields.WHOLEPOC_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x =>
      {
        if (!(x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine))
          return false;
        return x[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] == fieldId;
      })) : HUDGFE2010Fields.WHOLEPOC_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x =>
      {
        if (!(x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine) || !(x[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == strPart))
          return false;
        return x[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] == fieldId;
      }));
      if (row != null)
      {
        bool flag = fieldId == row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
        DDMFeeRuleValue ddmFeeRuleValue1 = currentScenario.FeeRuleValues.SingleOrDefault<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]));
        if (!flag)
        {
          if (ddmFeeRuleValue1 != null && ddmFeeRuleValue1.Field_Value == "N")
          {
            DDMFeeRuleValue ddmFeeRuleValue2 = currentScenario.FeeRuleValues.SingleOrDefault<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]));
            if (ddmFeeRuleValue2 != null)
            {
              ddmFeeRuleValue2.Field_Value = string.Empty;
              ddmFeeRuleValue2.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
            }
            return new string[3]
            {
              HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT.ToString(),
              row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT],
              row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]
            };
          }
        }
        else if (ddmFeeRuleValue1 != null && ddmFeeRuleValue1.Field_Value != "Y")
        {
          ddmFeeRuleValue1.Field_Value = "Y";
          ddmFeeRuleValue1.Field_Value_Type = fieldValueType.SpecificValue;
          return new string[3]
          {
            HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED.ToString(),
            row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED],
            row[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]
          };
        }
      }
      return new string[3]{ "0", "0", "0" };
    }

    public static string HandleTaxes(
      string lineNumber,
      string fieldId,
      DDMFeeRuleScenario currentScenario)
    {
      string str1 = "";
      string col = string.Empty;
      string fLine = DDM_FieldAccess_Utils.splitLineNumberAndColumn(lineNumber, out col).ToString().PadLeft(4, '0');
      string[] row = HUDGFE2010Fields.INSURANCE_PROPERTY_TAXES_FIELDS.SingleOrDefault<string[]>((Func<string[], bool>) (x =>
      {
        if (!(x[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == fLine))
          return false;
        return x[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_TAXES] == fieldId || x[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY] == fieldId;
      }));
      if (row != null)
      {
        string str2 = "";
        if (fieldId == row[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE])
          str2 = "INSURANCE";
        else if (fieldId == row[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY])
          str2 = "PROPERTY";
        else if (fieldId == row[HUDGFE2010Fields.PTCPOCINDEX_TAXES])
          str2 = "TAXES";
        if (str2 == string.Empty)
          return (string) null;
        DDMFeeRuleValue ddmFeeRuleValue1 = currentScenario.FeeRuleValues.SingleOrDefault<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == row[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY]));
        DDMFeeRuleValue ddmFeeRuleValue2 = currentScenario.FeeRuleValues.SingleOrDefault<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == row[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE]));
        DDMFeeRuleValue ddmFeeRuleValue3 = currentScenario.FeeRuleValues.SingleOrDefault<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == row[HUDGFE2010Fields.PTCPOCINDEX_TAXES]));
        switch (str2)
        {
          case "INSURANCE":
            if (ddmFeeRuleValue2 != null && ddmFeeRuleValue2.Field_Value == "Y")
            {
              if (ddmFeeRuleValue1 != null && ddmFeeRuleValue1.Field_Value == "Y")
              {
                ddmFeeRuleValue1.Field_Value = string.Empty;
                ddmFeeRuleValue1.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY];
              }
              if (ddmFeeRuleValue3 != null && ddmFeeRuleValue3.Field_Value == "Y")
              {
                ddmFeeRuleValue3.Field_Value = string.Empty;
                ddmFeeRuleValue3.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_TAXES];
                break;
              }
              break;
            }
            break;
          case "PROPERTY":
            if (ddmFeeRuleValue1 != null && ddmFeeRuleValue1.Field_Value == "Y")
            {
              if (ddmFeeRuleValue2 != null && ddmFeeRuleValue2.Field_Value == "Y")
              {
                ddmFeeRuleValue2.Field_Value = string.Empty;
                ddmFeeRuleValue2.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE];
              }
              if (ddmFeeRuleValue3 != null && ddmFeeRuleValue3.Field_Value == "Y")
              {
                ddmFeeRuleValue3.Field_Value = string.Empty;
                ddmFeeRuleValue3.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_TAXES];
                break;
              }
              break;
            }
            break;
          case "TAXES":
            if (ddmFeeRuleValue3 != null && ddmFeeRuleValue3.Field_Value == "Y")
            {
              if (ddmFeeRuleValue2 != null && ddmFeeRuleValue2.Field_Value == "Y")
              {
                ddmFeeRuleValue2.Field_Value = string.Empty;
                ddmFeeRuleValue2.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_INSURANCE];
              }
              if (ddmFeeRuleValue1 != null && ddmFeeRuleValue1.Field_Value == "Y")
              {
                ddmFeeRuleValue1.Field_Value = string.Empty;
                ddmFeeRuleValue1.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                str1 = row[HUDGFE2010Fields.PTCPOCINDEX_PROPERTY];
                break;
              }
              break;
            }
            break;
        }
      }
      return str1;
    }

    public static Dictionary<string, SysTblInfluenceAction> GetAffectingFieldsDictByAnyWithAction(
      string fieldAffected)
    {
      List<\u003C\u003Ef__AnonymousType1<string, string, bool, object>> list = DDM_FieldAccess_Utils.FEES_AFFECTED_BY_SYSTBL.Where<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>>((Func<KeyValuePair<string, DDM_FieldAccess_Utils.SysTblInfluenceInfo>, bool>) (item => item.Value.Fields.Keys.Contains<string>(fieldAffected))).Select(pair => new
      {
        Affected = fieldAffected,
        Affecting = pair.Key,
        IsNonSystemTableType = pair.Value.IsNonSystemTableType,
        Action = pair.Value.Fields[fieldAffected]
      }).ToList();
      Dictionary<string, SysTblInfluenceAction> dictByAnyWithAction = (Dictionary<string, SysTblInfluenceAction>) null;
      if (list != null)
      {
        dictByAnyWithAction = new Dictionary<string, SysTblInfluenceAction>();
        foreach (var data in list)
        {
          if (!dictByAnyWithAction.Keys.Contains<string>(data.Affecting))
            dictByAnyWithAction.Add(data.Affecting, new SysTblInfluenceAction()
            {
              AffectingField = data.Affecting,
              AffectedField = fieldAffected,
              IsNonSystemTableType = data.IsNonSystemTableType,
              Action = Convert.ToString(data.Action)
            });
        }
      }
      return dictByAnyWithAction;
    }

    public static FieldDefinition GetFieldDefinition(string fieldId, ILoanManager loanManager)
    {
      FieldSettings fieldSettings = (FieldSettings) null;
      if (loanManager != null && fieldId.ToLower().StartsWith("cx."))
        fieldSettings = loanManager.GetFieldSettings();
      return DDM_FieldAccess_Utils.GetFieldDefinition(fieldId, fieldSettings);
    }

    public static FieldDefinition GetFieldDefinition(string fieldId, SessionObjects sessionObjects)
    {
      FieldSettings fieldSettings = (FieldSettings) null;
      if (sessionObjects != null && fieldId.ToLower().StartsWith("cx."))
        fieldSettings = sessionObjects.GetCachedFieldSettings();
      return DDM_FieldAccess_Utils.GetFieldDefinition(fieldId, fieldSettings);
    }

    private static FieldDefinition GetFieldDefinition(string fieldId, FieldSettings fieldSettings)
    {
      FieldDefinition fieldDefinition = fieldSettings == null || !fieldId.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldId) : EncompassFields.GetField(fieldId, fieldSettings);
      if (DDM_FieldAccess_Utils.FieldDefinitionOverrides.ContainsKey(fieldId))
        fieldDefinition.Format = DDM_FieldAccess_Utils.FieldDefinitionOverrides[fieldId];
      return fieldDefinition;
    }

    private enum DDMLineRanges
    {
      Nothing,
      For801a,
      For801b,
      For801c,
      For801d,
      For801e,
      For801f,
      For801g,
      For808TO835,
      For802aTO802d,
      For802e,
      For802fTO802h,
      For803TO807,
      For902,
      For905,
      For901TO912,
      For1002To1009,
      For1003,
      For1010,
      For1101x,
      For1101a,
      For1101bTo1101f,
      For1102aTo1102h,
      For1102c,
      For1103,
      For1104,
      For1202TO1210,
      For1109TO1116,
      For1302,
      For1303TO1309,
      For1310TO1320,
    }

    private class SysTblInfluenceInfo
    {
      public string LineNumber { get; set; }

      public Dictionary<string, object> Fields { get; set; }

      public bool IsNonSystemTableType { get; set; }
    }
  }
}
