// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMFieldExecBatch
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMFieldExecBatch
  {
    private static Dictionary<string, string> _paidByFieldByLine = new Dictionary<string, string>();
    private Dictionary<string, Action<string>> _fieldBatchProcessings = new Dictionary<string, Action<string>>();
    private LoanData _loan;

    static DDMFieldExecBatch()
    {
      foreach (string[] strArray in DDMFieldExecBatch.getPaidByFieldByLine())
        DDMFieldExecBatch._paidByFieldByLine.Add(strArray[0], strArray[1]);
    }

    public DDMFieldExecBatch(LoanData loan)
    {
      this._loan = loan;
      foreach (DDMFieldExecBatch.DDMFieldBatchDef def in this.getDidShop_CalcFeeDetailBatch())
        this._fieldBatchProcessings.Add(def.FieldId, this.calculate2015FeeDetails(def));
    }

    private Action<string> calculate2015FeeDetails(DDMFieldExecBatch.DDMFieldBatchDef def)
    {
      return (Action<string>) (id =>
      {
        string line = def.Line;
        if (string.IsNullOrEmpty(line) || !DDMFieldExecBatch._paidByFieldByLine.ContainsKey(line))
          return;
        string paidByID = DDMFieldExecBatch._paidByFieldByLine[line];
        if (string.IsNullOrEmpty(paidByID))
          return;
        this._loan.Calculator.Calculate2015FeeDetails(id, paidByID, false);
      });
    }

    public void ExecBatchForField(string id)
    {
      if (!this._fieldBatchProcessings.ContainsKey(id))
        return;
      this._fieldBatchProcessings[id](id);
    }

    private static List<string[]> getPaidByFieldByLine()
    {
      return new List<string[]>()
      {
        new string[2]{ "701", "NEWHUD2.X60" },
        new string[2]{ "702", "NEWHUD2.X63" },
        new string[2]{ "704", "NEWHUD2.X66" },
        new string[2]{ "801x", "" },
        new string[2]{ "801a", "SYS.X251" },
        new string[2]{ "801b", "SYS.X261" },
        new string[2]{ "801c", "SYS.X269" },
        new string[2]{ "801d", "SYS.X271" },
        new string[2]{ "801e", "SYS.X265" },
        new string[2]{ "801f", "NEWHUD.X227" },
        new string[2]{ "801g", "SYS.X289" },
        new string[2]{ "801h", "SYS.X291" },
        new string[2]{ "801i", "SYS.X296" },
        new string[2]{ "801j", "SYS.X301" },
        new string[2]{ "801k", "NEWHUD.X748" },
        new string[2]{ "801l", "NEWHUD.X1239" },
        new string[2]{ "801m", "NEWHUD.X1247" },
        new string[2]{ "801n", "NEWHUD.X1255" },
        new string[2]{ "801o", "NEWHUD.X1263" },
        new string[2]{ "801p", "NEWHUD.X1271" },
        new string[2]{ "801q", "NEWHUD.X1279" },
        new string[2]{ "801r", "NEWHUD.X1287" },
        new string[2]{ "802x", "" },
        new string[2]{ "802e", "NEWHUD.X1175" },
        new string[2]{ "802f", "NEWHUD.X1179" },
        new string[2]{ "802g", "NEWHUD.X1183" },
        new string[2]{ "802h", "NEWHUD.X1187" },
        new string[2]{ "802", "NEWHUD.X749" },
        new string[2]{ "803x", "NEWHUD2.X69" },
        new string[2]{ "804", "SYS.X255" },
        new string[2]{ "805", "SYS.X257" },
        new string[2]{ "806", "SYS.X267" },
        new string[2]{ "807", "NEWHUD.X742" },
        new string[2]{ "808", "NEWHUD.X157" },
        new string[2]{ "809", "NEWHUD.X158" },
        new string[2]{ "810", "NEWHUD.X159" },
        new string[2]{ "811", "NEWHUD.X160" },
        new string[2]{ "812", "NEWHUD.X161" },
        new string[2]{ "813", "SYS.X275" },
        new string[2]{ "814", "SYS.X277" },
        new string[2]{ "815", "SYS.X279" },
        new string[2]{ "816", "SYS.X281" },
        new string[2]{ "817", "SYS.X283" },
        new string[2]{ "818", "NEWHUD.X1295" },
        new string[2]{ "819", "NEWHUD.X1303" },
        new string[2]{ "820", "NEWHUD.X1311" },
        new string[2]{ "821", "NEWHUD.X1319" },
        new string[2]{ "822", "NEWHUD.X1327" },
        new string[2]{ "823", "NEWHUD.X1335" },
        new string[2]{ "824", "NEWHUD.X1343" },
        new string[2]{ "825", "NEWHUD.X1351" },
        new string[2]{ "826", "NEWHUD.X1359" },
        new string[2]{ "827", "NEWHUD.X1367" },
        new string[2]{ "828", "NEWHUD.X1375" },
        new string[2]{ "829", "NEWHUD.X1383" },
        new string[2]{ "830", "NEWHUD.X1391" },
        new string[2]{ "831", "NEWHUD.X1399" },
        new string[2]{ "832", "NEWHUD.X1407" },
        new string[2]{ "833", "NEWHUD.X1415" },
        new string[2]{ "834", "SYS.X285" },
        new string[2]{ "835", "NEWHUD.X162" },
        new string[2]{ "901", "SYS.X303" },
        new string[2]{ "902", "SYS.X305" },
        new string[2]{ "903", "SYS.X307" },
        new string[2]{ "904", "NEWHUD.X163" },
        new string[2]{ "905", "SYS.X311" },
        new string[2]{ "906", "SYS.X309" },
        new string[2]{ "907", "SYS.X313" },
        new string[2]{ "908", "SYS.X315" },
        new string[2]{ "909", "NEWHUD.X164" },
        new string[2]{ "910", "NEWHUD.X165" },
        new string[2]{ "911", "NEWHUD.X1590" },
        new string[2]{ "912", "NEWHUD.X1598" },
        new string[2]{ "1002", "SYS.X317" },
        new string[2]{ "1003", "SYS.X319" },
        new string[2]{ "1004", "SYS.X323" },
        new string[2]{ "1005", "SYS.X321" },
        new string[2]{ "1006", "SYS.X325" },
        new string[2]{ "1007", "SYS.X327" },
        new string[2]{ "1008", "SYS.X329" },
        new string[2]{ "1009", "SYS.X331" },
        new string[2]{ "1010", "NEWHUD.X1710" },
        new string[2]{ "1011", "" },
        new string[2]{ "1101x", "" },
        new string[2]{ "1101a", "NEWHUD.X955" },
        new string[2]{ "1101b", "NEWHUD.X964" },
        new string[2]{ "1101c", "NEWHUD.X973" },
        new string[2]{ "1101d", "NEWHUD.X982" },
        new string[2]{ "1101e", "NEWHUD.X991" },
        new string[2]{ "1101f", "NEWHUD.X1000" },
        new string[2]{ "1102", "NEWHUD.X743" },
        new string[2]{ "1102a", "NEWHUD2.X72" },
        new string[2]{ "1102b", "NEWHUD2.X75" },
        new string[2]{ "1102c", "NEWHUD2.X78" },
        new string[2]{ "1102d", "NEWHUD2.X81" },
        new string[2]{ "1102e", "NEWHUD2.X84" },
        new string[2]{ "1102f", "NEWHUD2.X87" },
        new string[2]{ "1102g", "NEWHUD2.X90" },
        new string[2]{ "1102h", "NEWHUD2.X93" },
        new string[2]{ "1103", "NEWHUD.X744" },
        new string[2]{ "1104", "NEWHUD.X745" },
        new string[2]{ "1109", "NEWHUD.X221" },
        new string[2]{ "1110", "NEWHUD.X222" },
        new string[2]{ "1111", "SYS.X347" },
        new string[2]{ "1112", "SYS.X349" },
        new string[2]{ "1113", "SYS.X351" },
        new string[2]{ "1114", "SYS.X353" },
        new string[2]{ "1115", "NEWHUD.X1606" },
        new string[2]{ "1116", "NEWHUD.X1614" },
        new string[2]{ "1201", "" },
        new string[2]{ "1202", "SYS.X355" },
        new string[2]{ "1203", "NEWHUD.X261" },
        new string[2]{ "1204", "SYS.X357" },
        new string[2]{ "1205", "SYS.X359" },
        new string[2]{ "1206", "SYS.X361" },
        new string[2]{ "1207", "SYS.X363" },
        new string[2]{ "1208", "SYS.X365" },
        new string[2]{ "1209", "NEWHUD.X1622" },
        new string[2]{ "1210", "NEWHUD.X1629" },
        new string[2]{ "1302", "NEWHUD.X262" },
        new string[2]{ "1303", "SYS.X374" },
        new string[2]{ "1304", "SYS.X376" },
        new string[2]{ "1305", "SYS.X378" },
        new string[2]{ "1306", "SYS.X380" },
        new string[2]{ "1307", "SYS.X382" },
        new string[2]{ "1308", "SYS.X384" },
        new string[2]{ "1309", "SYS.X386" },
        new string[2]{ "1310", "NEWHUD.X263" },
        new string[2]{ "1311", "NEWHUD.X264" },
        new string[2]{ "1312", "NEWHUD.X1636" },
        new string[2]{ "1313", "NEWHUD.X1644" },
        new string[2]{ "1314", "NEWHUD.X1652" },
        new string[2]{ "1315", "NEWHUD.X1660" },
        new string[2]{ "1316", "NEWHUD2.X4614" },
        new string[2]{ "1317", "NEWHUD2.X4621" },
        new string[2]{ "1318", "NEWHUD2.X4628" },
        new string[2]{ "1319", "NEWHUD2.X4635" },
        new string[2]{ "1320", "NEWHUD2.X4642" }
      };
    }

    private List<DDMFieldExecBatch.DDMFieldBatchDef> getDidShop_CalcFeeDetailBatch()
    {
      return new List<DDMFieldExecBatch.DDMFieldBatchDef>()
      {
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X227", "701"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X260", "702"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X293", "704"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X326", "801a"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X359", "801b"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X392", "801c"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X425", "801d"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X458", "801e"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X491", "801f"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X524", "801g"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X557", "801h"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X590", "801i"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X623", "801j"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X656", "801k"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X689", "801l"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X722", "801m"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X755", "801n"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X788", "801o"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X821", "801p"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X854", "801q"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X887", "801r"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X953", "802e"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X986", "802f"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1019", "802g"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1052", "802h"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1085", "803x"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1118", "804"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1151", "805"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1184", "806"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1217", "807"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1250", "808"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1283", "809"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1316", "810"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1349", "811"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1382", "812"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1415", "813"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1448", "814"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1481", "815"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1514", "816"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1547", "817"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1580", "818"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1613", "819"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1646", "820"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1679", "821"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1712", "822"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1745", "823"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1778", "824"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1811", "825"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1844", "826"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1877", "827"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1910", "828"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1943", "829"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X1976", "830"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2009", "831"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2042", "832"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2075", "833"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2207", "902"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2240", "903"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2273", "904"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2306", "905"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2339", "906"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2372", "907"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2405", "908"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2438", "909"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2471", "910"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2504", "911"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2537", "912"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2570", "1002"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2603", "1003"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2636", "1004"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2669", "1005"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2702", "1006"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2735", "1007"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2768", "1008"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2801", "1009"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2834", "1010"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2867", "1101a"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2900", "1101b"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2933", "1101c"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2966", "1101d"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X2999", "1101e"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3032", "1101f"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3065", "1102a"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3098", "1102b"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3131", "1102c"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3164", "1102d"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3197", "1102e"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3230", "1102f"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3263", "1102g"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3296", "1102h"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3329", "1103"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3362", "1104"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3395", "1109"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3428", "1110"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3461", "1111"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3494", "1112"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3527", "1113"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3560", "1114"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3593", "1115"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3626", "1116"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3659", "1202"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3692", "1203"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3725", "1204"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3758", "1205"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3791", "1206"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3824", "1207"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3857", "1208"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3890", "1209"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3923", "1210"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3956", "1302"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X3989", "1303"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4022", "1304"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4055", "1305"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4088", "1306"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4121", "1307"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4154", "1308"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4187", "1309"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4220", "1310"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4253", "1311"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4286", "1312"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4319", "1313"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4352", "1314"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4385", "1315"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4471", "1316"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4504", "1317"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4537", "1318"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4570", "1319"),
        new DDMFieldExecBatch.DDMFieldBatchDef("NEWHUD2.X4603", "1320")
      };
    }

    private class DDMFieldBatchDef
    {
      public string FieldId { get; set; }

      public string Line { get; set; }

      public DDMFieldBatchDef(string fieldId, string line)
      {
        this.FieldId = fieldId;
        this.Line = line;
      }
    }
  }
}
