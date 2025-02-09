// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.NewHUD2015FeeDetailCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class NewHUD2015FeeDetailCalculation : CalculationBase
  {
    private const string className = "NewHUD2015FeeDetailCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine Calc_2015LineItem801a;
    internal Routine Calc_2015LineItem801e;
    internal Routine Calc_2015LineItem801f;
    internal Routine Calc_2015LineItem802e;
    internal Routine Calc_2015LineItem802f;
    internal Routine Calc_2015LineItem802g;
    internal Routine Calc_2015LineItem802h;
    internal Routine Calc_2015LineItem819;
    internal Routine Calc_2015LineItem901;
    internal Routine Calc_2015LineItem902;
    internal Routine Calc_2015LineItem903;
    internal Routine Calc_2015LineItem904;
    internal Routine Calc_2015LineItem905;
    internal Routine Calc_2015LineItem906;
    internal Routine Calc_2015LineItem907;
    internal Routine Calc_2015LineItem908;
    internal Routine Calc_2015LineItem909;
    internal Routine Calc_2015LineItem910;
    internal Routine Calc_2015LineItem911;
    internal Routine Calc_2015LineItem912;
    internal Routine Calc_2015LineItem1002;
    internal Routine Calc_2015LineItem1003;
    internal Routine Calc_2015LineItem1004;
    internal Routine Calc_2015LineItem1005;
    internal Routine Calc_2015LineItem1006;
    internal Routine Calc_2015LineItem1007;
    internal Routine Calc_2015LineItem1008;
    internal Routine Calc_2015LineItem1009;
    internal Routine Calc_2015LineItem1010;
    internal Routine Calc_2015LineItem1011;
    internal Routine Calc_2015LineItem1102c;
    internal Routine Calc_2015LineItem1103;
    internal Routine Calc_2015LineItem1104;
    internal Routine Calc_2015LineItem1202;
    internal Routine Calc_2015LineItem1204;
    internal Routine Calc_2015LineItem1205;
    internal Routine Calc_2015LineItem1206;
    internal Routine Calc_2015LineItem1207;
    internal Routine Calc_2015LineItem1208;
    internal Routine Calc_2015LineItem2001;
    internal Routine Calc_2015LineItem2002;
    internal Routine Calc_2015LineItem2003;
    internal Routine Calc_2015LineItem2004;
    internal Routine Calc_2015TitleFees;
    internal Routine Calc_2015CityStateTaxFees;
    internal Routine Calc_2015AllFeeDetails;
    internal Routine Calc_LastDisclosedLE;
    internal Routine Calc_LastDisclosedCD;
    internal Routine CalcOnlyOneFeeDetail;
    internal Routine Populate_2015LineItem904TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem907TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem908TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem909TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem910TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem911TaxesAndInsuranceIndicators;
    internal Routine Populate_2015LineItem912TaxesAndInsuranceIndicators;
    internal Routine Update2010ItemizationFrom2015Itemization;
    internal Routine ClearTaxStampIndicatorFor1204and1205;
    private List<string> excludedItemizationFields = new List<string>()
    {
      "NEWHUD2.X206",
      "NEWHUD2.X231",
      "NEWHUD2.X239",
      "NEWHUD2.X264",
      "NEWHUD2.X272",
      "NEWHUD2.X297",
      "NEWHUD2.X305",
      "NEWHUD2.X330",
      "NEWHUD2.X338",
      "NEWHUD2.X363",
      "NEWHUD2.X371",
      "NEWHUD2.X396",
      "NEWHUD2.X404",
      "NEWHUD2.X429",
      "NEWHUD2.X437",
      "NEWHUD2.X462",
      "NEWHUD2.X467",
      "NEWHUD2.X470",
      "NEWHUD2.X471",
      "NEWHUD2.X485",
      "NEWHUD2.X495",
      "NEWHUD2.X503",
      "NEWHUD2.X528",
      "NEWHUD2.X536",
      "NEWHUD2.X561",
      "NEWHUD2.X569",
      "NEWHUD2.X594",
      "NEWHUD2.X602",
      "NEWHUD2.X627",
      "NEWHUD2.X635",
      "NEWHUD2.X660",
      "NEWHUD2.X668",
      "NEWHUD2.X693",
      "NEWHUD2.X701",
      "NEWHUD2.X726",
      "NEWHUD2.X734",
      "NEWHUD2.X759",
      "NEWHUD2.X767",
      "NEWHUD2.X792",
      "NEWHUD2.X800",
      "NEWHUD2.X825",
      "NEWHUD2.X833",
      "NEWHUD2.X858",
      "NEWHUD2.X866",
      "NEWHUD2.X891",
      "NEWHUD2.X894",
      "NEWHUD2.X898",
      "NEWHUD2.X899",
      "NEWHUD2.X900",
      "NEWHUD2.X905",
      "NEWHUD2.X906",
      "NEWHUD2.X913",
      "NEWHUD2.X932",
      "NEWHUD2.X957",
      "NEWHUD2.X965",
      "NEWHUD2.X990",
      "NEWHUD2.X998",
      "NEWHUD2.X1023",
      "NEWHUD2.X1031",
      "NEWHUD2.X1056",
      "NEWHUD2.X1064",
      "NEWHUD2.X1089",
      "NEWHUD2.X1097",
      "NEWHUD2.X1122",
      "NEWHUD2.X1130",
      "NEWHUD2.X1155",
      "NEWHUD2.X1163",
      "NEWHUD2.X1188",
      "NEWHUD2.X1196",
      "NEWHUD2.X1221",
      "NEWHUD2.X1229",
      "NEWHUD2.X1254",
      "NEWHUD2.X1262",
      "NEWHUD2.X1287",
      "NEWHUD2.X1295",
      "NEWHUD2.X1320",
      "NEWHUD2.X1328",
      "NEWHUD2.X1353",
      "NEWHUD2.X1361",
      "NEWHUD2.X1386",
      "NEWHUD2.X1394",
      "NEWHUD2.X1419",
      "NEWHUD2.X1427",
      "NEWHUD2.X1452",
      "NEWHUD2.X1460",
      "NEWHUD2.X1485",
      "NEWHUD2.X1493",
      "NEWHUD2.X1518",
      "NEWHUD2.X1526",
      "NEWHUD2.X1551",
      "NEWHUD2.X1559",
      "NEWHUD2.X1584",
      "NEWHUD2.X1592",
      "NEWHUD2.X1617",
      "NEWHUD2.X1625",
      "NEWHUD2.X1650",
      "NEWHUD2.X1658",
      "NEWHUD2.X1683",
      "NEWHUD2.X1691",
      "NEWHUD2.X1716",
      "NEWHUD2.X1724",
      "NEWHUD2.X1749",
      "NEWHUD2.X1757",
      "NEWHUD2.X1782",
      "NEWHUD2.X1790",
      "NEWHUD2.X1815",
      "NEWHUD2.X1823",
      "NEWHUD2.X1848",
      "NEWHUD2.X1856",
      "NEWHUD2.X1881",
      "NEWHUD2.X1889",
      "NEWHUD2.X1914",
      "NEWHUD2.X1922",
      "NEWHUD2.X1947",
      "NEWHUD2.X1955",
      "NEWHUD2.X1980",
      "NEWHUD2.X1988",
      "NEWHUD2.X2013",
      "NEWHUD2.X2021",
      "NEWHUD2.X2046",
      "NEWHUD2.X2054",
      "NEWHUD2.X2079",
      "NEWHUD2.X2084",
      "NEWHUD2.X2087",
      "NEWHUD2.X2107",
      "NEWHUD2.X2112",
      "NEWHUD2.X2113",
      "NEWHUD2.X2117",
      "NEWHUD2.X2120",
      "NEWHUD2.X2140",
      "NEWHUD2.X2145",
      "NEWHUD2.X2146",
      "NEWHUD2.X2153",
      "NEWHUD2.X2173",
      "NEWHUD2.X2178",
      "NEWHUD2.X2186",
      "NEWHUD2.X2211",
      "NEWHUD2.X2244",
      "NEWHUD2.X2252",
      "NEWHUD2.X2277",
      "NEWHUD2.X2285",
      "NEWHUD2.X2310",
      "NEWHUD2.X2343",
      "NEWHUD2.X2351",
      "NEWHUD2.X2376",
      "NEWHUD2.X2384",
      "NEWHUD2.X2409",
      "NEWHUD2.X2417",
      "NEWHUD2.X2442",
      "NEWHUD2.X2450",
      "NEWHUD2.X2475",
      "NEWHUD2.X2476",
      "NEWHUD2.X2483",
      "NEWHUD2.X2508",
      "NEWHUD2.X2509",
      "NEWHUD2.X2516",
      "NEWHUD2.X2541",
      "NEWHUD2.X2542",
      "NEWHUD2.X2549",
      "NEWHUD2.X2553",
      "NEWHUD2.X2555",
      "NEWHUD2.X2556",
      "NEWHUD2.X2559",
      "NEWHUD2.X2562",
      "NEWHUD2.X2565",
      "NEWHUD2.X2574",
      "NEWHUD2.X2582",
      "NEWHUD2.X2586",
      "NEWHUD2.X2588",
      "NEWHUD2.X2589",
      "NEWHUD2.X2592",
      "NEWHUD2.X2595",
      "NEWHUD2.X2598",
      "NEWHUD2.X2607",
      "NEWHUD2.X2615",
      "NEWHUD2.X2619",
      "NEWHUD2.X2621",
      "NEWHUD2.X2622",
      "NEWHUD2.X2625",
      "NEWHUD2.X2628",
      "NEWHUD2.X2631",
      "NEWHUD2.X2640",
      "NEWHUD2.X2648",
      "NEWHUD2.X2652",
      "NEWHUD2.X2654",
      "NEWHUD2.X2655",
      "NEWHUD2.X2658",
      "NEWHUD2.X2661",
      "NEWHUD2.X2664",
      "NEWHUD2.X2673",
      "NEWHUD2.X2681",
      "NEWHUD2.X2685",
      "NEWHUD2.X2687",
      "NEWHUD2.X2688",
      "NEWHUD2.X2691",
      "NEWHUD2.X2694",
      "NEWHUD2.X2697",
      "NEWHUD2.X2706",
      "NEWHUD2.X2714",
      "NEWHUD2.X2718",
      "NEWHUD2.X2720",
      "NEWHUD2.X2721",
      "NEWHUD2.X2724",
      "NEWHUD2.X2727",
      "NEWHUD2.X2730",
      "NEWHUD2.X2739",
      "NEWHUD2.X2747",
      "NEWHUD2.X2751",
      "NEWHUD2.X2753",
      "NEWHUD2.X2754",
      "NEWHUD2.X2757",
      "NEWHUD2.X2760",
      "NEWHUD2.X2763",
      "NEWHUD2.X2772",
      "NEWHUD2.X2780",
      "NEWHUD2.X2784",
      "NEWHUD2.X2786",
      "NEWHUD2.X2787",
      "NEWHUD2.X2790",
      "NEWHUD2.X2793",
      "NEWHUD2.X2796",
      "NEWHUD2.X2805",
      "NEWHUD2.X2812",
      "NEWHUD2.X2813",
      "NEWHUD2.X2817",
      "NEWHUD2.X2819",
      "NEWHUD2.X2820",
      "NEWHUD2.X2823",
      "NEWHUD2.X2826",
      "NEWHUD2.X2829",
      "NEWHUD2.X2838",
      "NEWHUD2.X2846",
      "NEWHUD2.X2871",
      "NEWHUD2.X2879",
      "NEWHUD2.X2904",
      "NEWHUD2.X2912",
      "NEWHUD2.X2937",
      "NEWHUD2.X2945",
      "NEWHUD2.X2970",
      "NEWHUD2.X2978",
      "NEWHUD2.X3003",
      "NEWHUD2.X3011",
      "NEWHUD2.X3036",
      "NEWHUD2.X3044",
      "NEWHUD2.X3069",
      "NEWHUD2.X3077",
      "NEWHUD2.X3102",
      "NEWHUD2.X3110",
      "NEWHUD2.X3135",
      "NEWHUD2.X3143",
      "NEWHUD2.X3168",
      "NEWHUD2.X3176",
      "NEWHUD2.X3201",
      "NEWHUD2.X3209",
      "NEWHUD2.X3234",
      "NEWHUD2.X3242",
      "NEWHUD2.X3267",
      "NEWHUD2.X3275",
      "NEWHUD2.X3300",
      "NEWHUD2.X3308",
      "NEWHUD2.X3333",
      "NEWHUD2.X3341",
      "NEWHUD2.X3366",
      "NEWHUD2.X3374",
      "NEWHUD2.X3399",
      "NEWHUD2.X3407",
      "NEWHUD2.X3432",
      "NEWHUD2.X3440",
      "NEWHUD2.X3465",
      "NEWHUD2.X3473",
      "NEWHUD2.X3498",
      "NEWHUD2.X3506",
      "NEWHUD2.X3531",
      "NEWHUD2.X3539",
      "NEWHUD2.X3564",
      "NEWHUD2.X3569",
      "NEWHUD2.X3572",
      "NEWHUD2.X3597",
      "NEWHUD2.X3598",
      "NEWHUD2.X3602",
      "NEWHUD2.X3605",
      "NEWHUD2.X3630",
      "NEWHUD2.X3631",
      "NEWHUD2.X3635",
      "NEWHUD2.X3638",
      "NEWHUD2.X3663",
      "NEWHUD2.X3664",
      "NEWHUD2.X3671",
      "NEWHUD2.X3696",
      "NEWHUD2.X3701",
      "NEWHUD2.X3702",
      "NEWHUD2.X3704",
      "NEWHUD2.X3729",
      "NEWHUD2.X3734",
      "NEWHUD2.X3735",
      "NEWHUD2.X3737",
      "NEWHUD2.X3762",
      "NEWHUD2.X3767",
      "NEWHUD2.X3770",
      "NEWHUD2.X3795",
      "NEWHUD2.X3800",
      "NEWHUD2.X3803",
      "NEWHUD2.X3828",
      "NEWHUD2.X3833",
      "NEWHUD2.X3836",
      "NEWHUD2.X3861",
      "NEWHUD2.X3866",
      "NEWHUD2.X3869",
      "NEWHUD2.X3894",
      "NEWHUD2.X3899",
      "NEWHUD2.X3902",
      "NEWHUD2.X3927",
      "NEWHUD2.X3935",
      "NEWHUD2.X3960",
      "NEWHUD2.X3968",
      "NEWHUD2.X3993",
      "NEWHUD2.X4001",
      "NEWHUD2.X4026",
      "NEWHUD2.X4034",
      "NEWHUD2.X4059",
      "NEWHUD2.X4067",
      "NEWHUD2.X4092",
      "NEWHUD2.X4100",
      "NEWHUD2.X4125",
      "NEWHUD2.X4133",
      "NEWHUD2.X4158",
      "NEWHUD2.X4166",
      "NEWHUD2.X4191",
      "NEWHUD2.X4199",
      "NEWHUD2.X4224",
      "NEWHUD2.X4225",
      "NEWHUD2.X4232",
      "NEWHUD2.X4257",
      "NEWHUD2.X4258",
      "NEWHUD2.X4265",
      "NEWHUD2.X4290",
      "NEWHUD2.X4291",
      "NEWHUD2.X4298",
      "NEWHUD2.X4323",
      "NEWHUD2.X4324",
      "NEWHUD2.X4331",
      "NEWHUD2.X4356",
      "NEWHUD2.X4357",
      "NEWHUD2.X4364",
      "NEWHUD2.X4389",
      "NEWHUD2.X4390",
      "NEWHUD2.X4449",
      "NEWHUD2.X4450",
      "NEWHUD2.X4475",
      "NEWHUD2.X4476",
      "NEWHUD2.X4477",
      "NEWHUD2.X4482",
      "NEWHUD2.X4483",
      "NEWHUD2.X4508",
      "NEWHUD2.X4509",
      "NEWHUD2.X4510",
      "NEWHUD2.X4515",
      "NEWHUD2.X4516",
      "NEWHUD2.X4541",
      "NEWHUD2.X4542",
      "NEWHUD2.X4543",
      "NEWHUD2.X4548",
      "NEWHUD2.X4549",
      "NEWHUD2.X4574",
      "NEWHUD2.X4575",
      "NEWHUD2.X4576",
      "NEWHUD2.X4581",
      "NEWHUD2.X4582",
      "NEWHUD2.X4607",
      "NEWHUD2.X4608",
      "NEWHUD2.X4609"
    };

    internal NewHUD2015FeeDetailCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.Calc_2015LineItem801a = this.RoutineX(new Routine(this.calculate_2015LineItem801a));
      this.Calc_2015LineItem801e = this.RoutineX(new Routine(this.calculate_2015LineItem801e));
      this.Calc_2015LineItem801f = this.RoutineX(new Routine(this.calculate_2015LineItem801f));
      this.Calc_2015LineItem802e = this.RoutineX(new Routine(this.calculate_2015LineItem802e));
      this.Calc_2015LineItem802f = this.RoutineX(new Routine(this.calculate_2015LineItem802f));
      this.Calc_2015LineItem802g = this.RoutineX(new Routine(this.calculate_2015LineItem802g));
      this.Calc_2015LineItem802h = this.RoutineX(new Routine(this.calculate_2015LineItem802h));
      this.Calc_2015LineItem819 = this.RoutineX(new Routine(this.calculate_2015LineItem819));
      this.Calc_2015LineItem901 = this.RoutineX(new Routine(this.calculate_2015LineItem901));
      this.Calc_2015LineItem902 = this.RoutineX(new Routine(this.calculate_2015LineItem902));
      this.Calc_2015LineItem903 = this.RoutineX(new Routine(this.calculate_2015LineItem903));
      this.Calc_2015LineItem904 = this.RoutineX(new Routine(this.calculate_2015LineItem904));
      this.Calc_2015LineItem905 = this.RoutineX(new Routine(this.calculate_2015LineItem905));
      this.Calc_2015LineItem906 = this.RoutineX(new Routine(this.calculate_2015LineItem906));
      this.Calc_2015LineItem907 = this.RoutineX(new Routine(this.calculate_2015LineItem907));
      this.Calc_2015LineItem908 = this.RoutineX(new Routine(this.calculate_2015LineItem908));
      this.Calc_2015LineItem909 = this.RoutineX(new Routine(this.calculate_2015LineItem909));
      this.Calc_2015LineItem910 = this.RoutineX(new Routine(this.calculate_2015LineItem910));
      this.Calc_2015LineItem911 = this.RoutineX(new Routine(this.calculate_2015LineItem911));
      this.Calc_2015LineItem912 = this.RoutineX(new Routine(this.calculate_2015LineItem912));
      this.Calc_2015LineItem1002 = this.RoutineX(new Routine(this.calculate_2015LineItem1002));
      this.Calc_2015LineItem1003 = this.RoutineX(new Routine(this.calculate_2015LineItem1003));
      this.Calc_2015LineItem1004 = this.RoutineX(new Routine(this.calculate_2015LineItem1004));
      this.Calc_2015LineItem1005 = this.RoutineX(new Routine(this.calculate_2015LineItem1005));
      this.Calc_2015LineItem1006 = this.RoutineX(new Routine(this.calculate_2015LineItem1006));
      this.Calc_2015LineItem1007 = this.RoutineX(new Routine(this.calculate_2015LineItem1007));
      this.Calc_2015LineItem1008 = this.RoutineX(new Routine(this.calculate_2015LineItem1008));
      this.Calc_2015LineItem1009 = this.RoutineX(new Routine(this.calculate_2015LineItem1009));
      this.Calc_2015LineItem1010 = this.RoutineX(new Routine(this.calculate_2015LineItem1010));
      this.Calc_2015LineItem1011 = this.RoutineX(new Routine(this.calculate_2015LineItem1011));
      this.Calc_2015LineItem1102c = this.RoutineX(new Routine(this.calculate_2015LineItem1102c));
      this.Calc_2015LineItem1103 = this.RoutineX(new Routine(this.calculate_2015LineItem1103));
      this.Calc_2015LineItem1104 = this.RoutineX(new Routine(this.calculate_2015LineItem1104));
      this.Calc_2015LineItem1202 = this.RoutineX(new Routine(this.calculate_2015LineItem1202));
      this.Calc_2015LineItem1204 = this.RoutineX(new Routine(this.calculate_2015LineItem1204));
      this.Calc_2015LineItem1205 = this.RoutineX(new Routine(this.calculate_2015LineItem1205));
      this.Calc_2015LineItem1206 = this.RoutineX(new Routine(this.calculate_2015LineItem1206));
      this.Calc_2015LineItem1207 = this.RoutineX(new Routine(this.calculate_2015LineItem1207));
      this.Calc_2015LineItem1208 = this.RoutineX(new Routine(this.calculate_2015LineItem1208));
      this.Calc_2015LineItem2001 = this.RoutineX(new Routine(this.calculate_2015LineItem2001));
      this.Calc_2015LineItem2002 = this.RoutineX(new Routine(this.calculate_2015LineItem2002));
      this.Calc_2015LineItem2003 = this.RoutineX(new Routine(this.calculate_2015LineItem2003));
      this.Calc_2015LineItem2004 = this.RoutineX(new Routine(this.calculate_2015LineItem2004));
      this.Calc_2015TitleFees = this.RoutineX(new Routine(this.calculate_2015TitleFees));
      this.Calc_2015CityStateTaxFees = this.RoutineX(new Routine(this.calculate_2015CityStateTaxFees));
      this.Calc_2015AllFeeDetails = this.RoutineX(new Routine(this.calculateAllFeeDetails));
      this.Calc_LastDisclosedLE = this.RoutineX(new Routine(this.calculate_LastDisclosedLE));
      this.Calc_LastDisclosedCD = this.RoutineX(new Routine(this.calculate_LastDisclosedCD));
      this.CalcOnlyOneFeeDetail = this.RoutineX(new Routine(this.calculateOnlyOneFeeDetail));
      this.Populate_2015LineItem904TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem904TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem907TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem907TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem908TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem908TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem909TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem909TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem910TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem910TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem911TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem911TaxesAndInsuranceIndicators));
      this.Populate_2015LineItem912TaxesAndInsuranceIndicators = this.RoutineX(new Routine(this.populate_2015LineItem912TaxesAndInsuranceIndicators));
      this.Update2010ItemizationFrom2015Itemization = this.RoutineX(new Routine(this.update2010ItemizationFrom2015Itemization));
      this.ClearTaxStampIndicatorFor1204and1205 = this.RoutineX(new Routine(this.clearTaxStampIndicatorFor1204and1205));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      this.AddFieldHandler("647", this.RoutineX(new Routine(this.calculate_2015LineItem1204)));
      this.AddFieldHandler("648", this.RoutineX(new Routine(this.calculate_2015LineItem1205)));
      Routine routine1 = this.RoutineX(new Routine(this.populate_2015LineItem904TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4413", routine1);
      this.AddFieldHandler("NEWHUD2.X4414", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.populate_2015LineItem907TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4415", routine2);
      this.AddFieldHandler("NEWHUD2.X4416", routine2);
      this.AddFieldHandler("NEWHUD2.X4435", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.populate_2015LineItem908TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4417", routine3);
      this.AddFieldHandler("NEWHUD2.X4418", routine3);
      this.AddFieldHandler("NEWHUD2.X4436", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.populate_2015LineItem909TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4419", routine4);
      this.AddFieldHandler("NEWHUD2.X4420", routine4);
      this.AddFieldHandler("NEWHUD2.X4437", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.populate_2015LineItem910TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4421", routine5);
      this.AddFieldHandler("NEWHUD2.X4422", routine5);
      this.AddFieldHandler("NEWHUD2.X4438", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.populate_2015LineItem911TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4423", routine6);
      this.AddFieldHandler("NEWHUD2.X4424", routine6);
      this.AddFieldHandler("NEWHUD2.X4439", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.populate_2015LineItem912TaxesAndInsuranceIndicators));
      this.AddFieldHandler("NEWHUD2.X4425", routine7);
      this.AddFieldHandler("NEWHUD2.X4426", routine7);
      this.AddFieldHandler("NEWHUD2.X4440", routine7);
    }

    private void clearTaxStampIndicatorFor1204and1205(string id, string val)
    {
      if (this.loan.Use2015RESPA)
        return;
      this.SetVal("4855", "");
      this.SetVal("4856", "");
    }

    private void calculateAllFeeDetails(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if (!(pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0802") || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == ""))
          this.CalcFeeDetail(id, pocFields);
      }
    }

    private void calculateOnlyOneFeeDetail(string id, string val)
    {
      if (!(id == "NEWHUD2.X955") && !(id == "NEWHUD.X1152"))
        return;
      this.CalcFeeDetail(id, HUDGFE2010Fields.WHOLEPOC_FIELDS[23]);
    }

    internal bool CalcFeeDetail(string fieldID, string[] pocFields)
    {
      if (!this.UseNew2015GFEHUD)
        return false;
      string str1 = this.Val("1172");
      int lineNumber = Utils.ParseInt((object) pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], 0);
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) == 0.0)
      {
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
        if (lineNumber >= 2001)
        {
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], "");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], "");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], "");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], "");
        }
      }
      if (fieldID != "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY] != "" && fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY])
      {
        string str2 = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
        if (str2 == "Seller")
        {
          str2 = "";
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
        }
        if (str2 != "")
        {
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT] != "")
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT], "");
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT] != "")
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT], "");
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT], "");
          if (str2 == "Lender" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "")
          {
            if (lineNumber >= 2001)
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], "");
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], "");
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], "");
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
            }
            else
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
              }
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
              }
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
            }
          }
          else if (str2 == "Broker" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "")
          {
            if (lineNumber == 2001 || lineNumber == 2002 || lineNumber == 2003 || lineNumber == 2004)
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], "");
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], "");
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], "");
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
            }
            else
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
              }
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
              }
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
            }
          }
          else if (str2 == "Other" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "")
          {
            if (lineNumber >= 2001)
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], "");
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], "");
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], "");
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
            }
            else
            {
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
              }
              if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "")
              {
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
                if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
                  this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
              }
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) : 0.0));
            }
          }
        }
        else
        {
          if (lineNumber >= 2001)
          {
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], "");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], "");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], "");
          }
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT] != "")
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT], "");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
          }
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "" && lineNumber < 2001)
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT], "");
          }
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "" && lineNumber < 2001)
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT], "");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT], "");
          }
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "" && lineNumber < 2001)
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT], "");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT], "");
          }
        }
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID] != "" && lineNumber < 2001)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) : 0.0));
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID] != "" && lineNumber < 2001)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) : 0.0));
      if (this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M) && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" && Utils.IsLenderObligatedFee(lineNumber))
      {
        double num = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]);
        if (num != 0.0)
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], num);
        else
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT], "");
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT], "");
      }
      if (lineNumber == 801 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "f")
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) : 0.0));
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] != "" && lineNumber < 2001)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) : 0.0));
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY] != "")
      {
        double num = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]);
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY], num);
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID] != "" && lineNumber != 2001 && lineNumber != 2002 && lineNumber != 2003 && lineNumber != 2004)
      {
        if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]) == 0.0)
        {
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT], "");
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "")
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT], "");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID], "");
        }
        else
        {
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]) - (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]) : 0.0));
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]) : 0.0));
        }
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID] != "" && lineNumber >= 2001)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]));
      double num1 = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]) : 0.0);
      if (this.GetFieldFromCal("NEWHUD2.X3335") == "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] == "NEWHUD.X572")
        this.SetVal("NEWHUD2.X3335", num1 > 0.0 ? "Y" : "");
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] != "")
      {
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT], num1);
        if (num1 == 0.0 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT] != "")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT], "");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT] != "" && lineNumber != 1011)
      {
        double num2 = 0.0;
        if (!this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M) || this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M) && !Utils.IsLenderObligatedFee(lineNumber))
          num2 = Utils.ArithmeticRounding(num1 - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]) - this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]) - (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) : 0.0), 2);
        if (num2 != 0.0)
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT], num2);
        else
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT], "");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != "" && lineNumber != 1011)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT]));
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID] != "" && lineNumber != 1011 && lineNumber < 2001)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) : 0.0));
      if (lineNumber >= 2001 && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) != 0.0)
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) - (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID])));
      this.calculateTotalFeeAmount(pocFields);
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT]) > this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]))
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
      if (fieldID != "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] != "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] != "")
      {
        if (fieldID == "3969" && this.UseNew2015GFEHUD && pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "")
        {
          string val = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED]);
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], val);
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], val);
        }
        else if (fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] && pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "")
        {
          if (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED]) == "Y")
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "Y");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "Y");
          }
          else if (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED]) == "N")
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "N");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "N");
          }
        }
        else if (fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP])
        {
          if (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y")
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "Y");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED], "Y");
          }
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "")
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED], "N");
        }
        else if (fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP])
        {
          if (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N")
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "N");
            if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "")
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED], "N");
          }
        }
        else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED] != "" && this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED]) == "Y")
        {
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "Y");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "Y");
        }
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] != "")
      {
        if (lineNumber >= 1302 && lineNumber <= 1309)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], num1 > 0.0 || this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORSELECTED]) == "Y" ? "Y" : "");
        else if (lineNumber == 902 && (str1 == "FHA" || str1 == "VA" || str1 == "FarmersHomeAdministration"))
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "N");
        else if (lineNumber == 905 || lineNumber >= 803 && lineNumber <= 833)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], "N");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] != "")
      {
        if (lineNumber == 902 && (str1 == "FHA" || str1 == "VA" || str1 == "FarmersHomeAdministration"))
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "N");
        else if (lineNumber == 905 || lineNumber >= 803 && lineNumber <= 833)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP], "N");
      }
      if (this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M) && Utils.IsLenderObligatedFee(lineNumber) && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != "")
      {
        if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]) != 0.0)
        {
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED], "Y");
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "N");
        }
        else
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED], "");
      }
      if (lineNumber >= 2001)
      {
        string str3 = this.Val("19");
        if (str3 == "ConstructionOnly" || str3 == "ConstructionToPermanent")
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_POSTCONSUMMATIONFEE], "Y");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != "" && this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) == "Y" && !this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != "" && fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] && this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) == "Y" && !this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
      {
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "N");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT] != string.Empty && (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]) > 0.0 || this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]) < 0.0 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] == "NEWHUD2.X3332"))
      {
        if (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) == "Y")
        {
          if (fieldID == null && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
          {
            if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]) > this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]))
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
            else if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]) < this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]))
            {
              double num3 = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
              if (num3 > 0.0 && this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT]) != "Y")
                this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "Y");
              if (num3 == 0.0)
                this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], num3);
              else if (!this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
              {
                this.AddLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
                this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], num3);
              }
            }
          }
          if (!this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
          else if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]) > this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]))
          {
            this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
          }
          if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]) < this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]))
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "Y");
          else
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "N");
        }
        else
        {
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "Y");
          if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
            this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], "");
        }
      }
      else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT] != "")
      {
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT], "N");
        if ((fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT] || fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] || fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]) && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]) == 0.0)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED], "N");
        if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))
          this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT], "");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != "")
      {
        if (lineNumber >= 701 && lineNumber <= 704 && this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) == "" && num1 != 0.0 && fieldID != pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO], "Other");
        else if ((fieldID == pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] || fieldID == "TemplateApplied") && pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME] != string.Empty)
        {
          switch (this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]))
          {
            case "Broker":
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.Val("VEND.X293"));
              break;
            case "Lender":
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.Val("1264"));
              break;
            case "Seller":
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.Val("638"));
              break;
            case "Investor":
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.Val("VEND.X263"));
              break;
          }
        }
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC] != "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC] != "0802")
      {
        double num4 = (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT]) : 0.0) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) : 0.0) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]) : 0.0) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) : 0.0) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) : 0.0) + (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "" ? this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) : 0.0);
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], num4 != 0.0 ? "Y" : "");
        if (lineNumber >= 2001)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PTCPOC], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]) != 0.0 ? "Y" : "");
      }
      if (num1 != 0.0 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY] != "")
      {
        if (lineNumber != 801 || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "e") && !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "f"))
        {
          if (lineNumber >= 2001)
          {
            if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) > 0.0)
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
            else if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]) > 0.0)
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Lender");
            else if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) > 0.0)
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Broker");
            else if (this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]) > 0.0)
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Other");
            else
              this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
          }
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Lender");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Broker");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Other");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Lender");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Broker");
          else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "" && this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) > 0.0)
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Other");
          else
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
        }
      }
      else if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY] != "")
      {
        if (lineNumber == 801 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "f")
        {
          if (this.Val("NEWHUD.X223") != string.Empty || this.Val("NEWHUD.X224") != string.Empty)
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "Lender");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO], "Broker");
            if (this.Val("NEWHUD2.X109") == "")
              this.SetVal("NEWHUD2.X109", this.Val("VEND.X293"));
          }
          else
          {
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO], "");
          }
        }
        else
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY], "");
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT] != "")
        this.calculateSec32PointsAndFees(pocFields);
      this.loan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization, true);
      return true;
    }

    private void calculateTotalFeeAmount(string[] pocFields)
    {
      if (!(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEPERCENT] != ""))
        return;
      double num = this.Val("1172") == "FHA" ? this.FltVal("1109") : this.FltVal("2");
      if (num == 0.0)
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEPERCENT], "");
      else
        this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEPERCENT], Utils.ArithmeticRounding(this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT]) / num * 100.0, 3));
    }

    private void calculateSec32PointsAndFees(string[] pocFields)
    {
      int num1 = Utils.ParseInt((object) pocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], 0);
      string pocField = pocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
      string str = this.Val(pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]);
      if (num1 == 801 && pocField == "f")
      {
        if (this.FltVal("NEWHUD.X225") == 0.0)
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], "");
        else if (this.Val("QM.X372") == "Y" && this.FltVal("QM.X371") > this.FltVal("NEWHUD2.X484"))
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal("QM.X371"));
        else if (this.Val("QM.X372") == "Y" && this.FltVal("QM.X371") <= this.FltVal("NEWHUD2.X484"))
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal("NEWHUD2.X484"));
        else
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal("NEWHUD2.X484"));
      }
      else if (num1 == 802 && (pocField == "e" || pocField == "f" || pocField == "g" || pocField == "h"))
      {
        if (str == "" || str == "Broker" || str == "Lender" || str == "Affiliate")
        {
          double num2 = this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID]);
          if (str == "Broker")
            num2 += this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]);
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], num2);
        }
        else
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0, true);
      }
      else if (num1 == 819 && this.Val("1172") == "FarmersHomeAdministration")
      {
        if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]))
          this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]);
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], "");
      }
      else if (num1 == 902)
      {
        if (this.Val("1172") == "Conventional")
        {
          bool showZero = this.Val("3262") == "Y";
          double num3 = !(str == "Broker") ? this.FltVal("NEWHUD2.X2191") + this.FltVal("NEWHUD2.X2197") + this.FltVal("NEWHUD2.X2203") : this.FltVal("NEWHUD2.X2191") + this.FltVal("NEWHUD2.X2197") + this.FltVal("NEWHUD2.X2203") + this.FltVal("NEWHUD2.X2200") + this.FltVal("NEWHUD2.X2194");
          if (showZero)
          {
            double num4 = num3 - this.FltVal("1109") * 1.75 / 100.0;
            num3 = num4 > 0.0 ? num4 : 0.0;
          }
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], num3, showZero);
        }
        else if (this.Val("1172") == "FHA")
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0, true);
        else if (this.Val("1172") == "FarmersHomeAdministration")
        {
          if (this.IsLocked("NEWHUD.X1301"))
          {
            if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]))
              this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]);
            this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], "");
          }
          else
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
        }
        else
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
      }
      else if (num1 == 903 || num1 == 906)
      {
        if (str == "Broker")
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]), true);
        else
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
      }
      else if (num1 == 1103 || num1 == 1104)
      {
        switch (str)
        {
          case "Affiliate":
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT]), true);
            break;
          case "":
          case "Lender":
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]), true);
            break;
          default:
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
            break;
        }
      }
      else if (907 <= num1 && num1 <= 912)
      {
        switch (num1)
        {
          case 907:
            if ((this.Val("NEWHUD2.X4415") == "Y" || this.Val("NEWHUD2.X4416") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2370", this.FltVal("NEWHUD2.X2356"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2370", 0.0);
            break;
          case 908:
            if ((this.Val("NEWHUD2.X4417") == "Y" || this.Val("NEWHUD2.X4418") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2403", this.FltVal("NEWHUD2.X2389"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2403", 0.0);
            break;
          case 909:
            if ((this.Val("NEWHUD2.X4419") == "Y" || this.Val("NEWHUD2.X4420") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2436", this.FltVal("NEWHUD2.X2422"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2436", 0.0);
            break;
          case 910:
            if ((this.Val("NEWHUD2.X4421") == "Y" || this.Val("NEWHUD2.X4422") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2469", this.FltVal("NEWHUD2.X2455"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2469", 0.0);
            break;
          case 911:
            if ((this.Val("NEWHUD2.X4423") == "Y" || this.Val("NEWHUD2.X4424") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2502", this.FltVal("NEWHUD2.X2488"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2502", 0.0);
            break;
          case 912:
            if ((this.Val("NEWHUD2.X4425") == "Y" || this.Val("NEWHUD2.X4426") == "Y") && str == "Broker")
            {
              this.SetCurrentNum("NEWHUD2.X2535", this.FltVal("NEWHUD2.X2521"));
              break;
            }
            this.SetCurrentNum("NEWHUD2.X2535", 0.0);
            break;
        }
      }
      else if (num1 == 1002 || num1 == 1006)
      {
        if (str == "Lender" || str == "Broker" || str == "Affiliate")
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
        else
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
      }
      else if (num1 == 1003)
      {
        if (this.Val("1172") != "FHA")
        {
          this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
        }
        else
        {
          if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]))
            this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]);
          this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], "");
        }
      }
      else if (1007 <= num1 && num1 <= 1009)
      {
        switch (num1)
        {
          case 1007:
            if ((this.Val("NEWHUD2.X124") == "Y" || this.Val("NEWHUD2.X125") == "Y") && (str == "Lender" || str == "Broker" || str == "Affiliate"))
            {
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
              break;
            }
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
            break;
          case 1008:
            if ((this.Val("NEWHUD2.X127") == "Y" || this.Val("NEWHUD2.X128") == "Y") && (str == "Lender" || str == "Broker" || str == "Affiliate"))
            {
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
              break;
            }
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
            break;
          case 1009:
            if ((this.Val("NEWHUD2.X130") == "Y" || this.Val("NEWHUD2.X131") == "Y") && (str == "Lender" || str == "Broker" || str == "Affiliate"))
            {
              this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
              break;
            }
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0);
            break;
        }
      }
      else if (num1 == 1010)
      {
        if (!(this.Val("1172") != "FarmersHomeAdministration"))
          return;
        if (this.IsLocked(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]))
          this.RemoveLock(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT]);
        this.SetVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], "");
      }
      else
      {
        switch (str)
        {
          case "":
          case "Lender":
          case "Affiliate":
          case "Investor":
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]));
            break;
          case "Seller":
          case "Other":
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], 0.0, true);
            break;
          case "Broker":
            this.SetCurrentNum(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT], this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]) + this.FltVal(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]));
            break;
        }
      }
    }

    private void calculate_2015LineItem801a(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X1"]);
    }

    private void calculate_2015LineItem801e(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X5"]);
    }

    private void calculate_2015LineItem801f(string id, string val)
    {
      if (this.FltVal("NEWHUD.X225") == 0.0)
      {
        this.SetVal("NEWHUD2.X482", "");
        this.SetVal("NEWHUD2.X483", "");
        this.SetVal("NEWHUD2.X484", "");
      }
      else
      {
        this.SetCurrentNum("NEWHUD2.X482", this.FltVal("NEWHUD.X225") - this.FltVal("NEWHUD2.X483"));
        this.SetCurrentNum("NEWHUD2.X484", this.FltVal("NEWHUD.X225"));
      }
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X6"]);
    }

    internal void Calc_2015LineItem802(string id, string val)
    {
      this.calculateTotalFeeAmount((string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "0802"]);
    }

    private void calculate_2015LineItem802e(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X235"]);
    }

    private void calculate_2015LineItem802f(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X236"]);
    }

    private void calculate_2015LineItem802g(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X237"]);
    }

    private void calculate_2015LineItem802h(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X238"]);
    }

    private void calculate_2015LineItem819(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X255"]);
    }

    private void calculate_2015LineItem901(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X29"]);
    }

    private void calculate_2015LineItem902(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X30"]);
    }

    private void calculate_2015LineItem903(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X31"]);
    }

    private void calculate_2015LineItem904(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X32"]);
    }

    private void calculate_2015LineItem905(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X33"]);
    }

    private void calculate_2015LineItem906(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X34"]);
    }

    private void calculate_2015LineItem907(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X35"]);
    }

    private void calculate_2015LineItem908(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X36"]);
    }

    private void calculate_2015LineItem909(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X37"]);
    }

    private void calculate_2015LineItem910(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X38"]);
    }

    private void calculate_2015LineItem911(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X316"]);
    }

    private void calculate_2015LineItem912(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X317"]);
    }

    private void calculate_2015LineItem1002(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X39"]);
    }

    private void calculate_2015LineItem1003(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X40"]);
    }

    private void calculate_2015LineItem1004(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X41"]);
    }

    private void calculate_2015LineItem1005(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X42"]);
    }

    private void calculate_2015LineItem1006(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X43"]);
    }

    private void calculate_2015LineItem1007(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X44"]);
    }

    private void calculate_2015LineItem1008(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X45"]);
    }

    private void calculate_2015LineItem1009(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X46"]);
    }

    private void calculate_2015LineItem1010(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X346"]);
    }

    private void calculate_2015LineItem1011(string id, string val)
    {
      this.SetCurrentNum("NEWHUD2.X4393", this.FltVal("558"));
      this.CalcFeeDetail(id, HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == "1011")));
    }

    private void calculate_2015TitleFees(string id, string val)
    {
      if (id == val && id == "1102c")
        this.calculate_2015LineItem1102c(id, val);
      else if (id == val && id == "1103")
        this.calculate_2015LineItem1103(id, val);
      else if (id == val && id == "1104")
      {
        this.calculate_2015LineItem1104(id, val);
      }
      else
      {
        this.calculate_2015LineItem1102c(id, val);
        this.calculate_2015LineItem1103(id, val);
        this.calculate_2015LineItem1104(id, val);
      }
    }

    private void calculate_2015LineItem1102c(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT2.X7"]);
    }

    private void calculate_2015LineItem1103(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X54"]);
    }

    private void calculate_2015LineItem1104(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X55"]);
    }

    private void calculate_2015CityStateTaxFees(string id, string val)
    {
      this.calculate_2015LineItem1202(id, val);
      this.calculate_2015LineItem1204(id, val);
      this.calculate_2015LineItem1205(id, val);
      this.calculate_2015LineItem1206(id, val);
      this.calculate_2015LineItem1207(id, val);
      this.calculate_2015LineItem1208(id, val);
    }

    private void calculate_2015LineItem1202(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X62"]);
    }

    private void calculate_2015LineItem1204(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X64"]);
    }

    private void calculate_2015LineItem1205(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X65"]);
    }

    private void calculate_2015LineItem1206(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X66"]);
    }

    private void calculate_2015LineItem1207(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X67"]);
    }

    private void calculate_2015LineItem1208(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT.X68"]);
    }

    private void calculate_2015LineItem2001(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT2.X18"]);
    }

    private void calculate_2015LineItem2002(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT2.X19"]);
    }

    private void calculate_2015LineItem2003(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT2.X20"]);
    }

    private void calculate_2015LineItem2004(string id, string val)
    {
      this.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) "POPT2.X21"]);
    }

    private void populate_2015LineItem904TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4413" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4414", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4414") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4413", "N");
      }
    }

    private void populate_2015LineItem907TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4415" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4416", "N");
        this.SetVal("NEWHUD2.X4435", "N");
      }
      else if (id == "NEWHUD2.X4416" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4415", "N");
        this.SetVal("NEWHUD2.X4435", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4435") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4415", "N");
        this.SetVal("NEWHUD2.X4416", "N");
      }
    }

    private void populate_2015LineItem908TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4417" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4418", "N");
        this.SetVal("NEWHUD2.X4436", "N");
      }
      else if (id == "NEWHUD2.X4418" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4417", "N");
        this.SetVal("NEWHUD2.X4436", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4436") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4417", "N");
        this.SetVal("NEWHUD2.X4418", "N");
      }
    }

    private void populate_2015LineItem909TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4419" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4420", "N");
        this.SetVal("NEWHUD2.X4437", "N");
      }
      else if (id == "NEWHUD2.X4420" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4419", "N");
        this.SetVal("NEWHUD2.X4437", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4437") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4419", "N");
        this.SetVal("NEWHUD2.X4420", "N");
      }
    }

    private void populate_2015LineItem910TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4421" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4422", "N");
        this.SetVal("NEWHUD2.X4438", "N");
      }
      else if (id == "NEWHUD2.X4422" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4421", "N");
        this.SetVal("NEWHUD2.X4438", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4438") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4421", "N");
        this.SetVal("NEWHUD2.X4422", "N");
      }
    }

    private void populate_2015LineItem911TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4423" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4424", "N");
        this.SetVal("NEWHUD2.X4439", "N");
      }
      else if (id == "NEWHUD2.X4424" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4423", "N");
        this.SetVal("NEWHUD2.X4439", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4439") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4423", "N");
        this.SetVal("NEWHUD2.X4424", "N");
      }
    }

    private void populate_2015LineItem912TaxesAndInsuranceIndicators(string id, string val)
    {
      if (id == "NEWHUD2.X4425" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4426", "N");
        this.SetVal("NEWHUD2.X4440", "N");
      }
      else if (id == "NEWHUD2.X4426" && val == "Y")
      {
        this.SetVal("NEWHUD2.X4425", "N");
        this.SetVal("NEWHUD2.X4440", "N");
      }
      else
      {
        if (!(id == "NEWHUD2.X4440") || !(val == "Y"))
          return;
        this.SetVal("NEWHUD2.X4425", "N");
        this.SetVal("NEWHUD2.X4426", "N");
      }
    }

    private void update2010ItemizationFrom2015Itemization(string id, string val)
    {
      if (!this.UseNew2015GFEHUD)
        return;
      for (int index1 = 0; index1 < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index1)
      {
        string[] strArray1 = HUDGFE2010Fields.WHOLEPOC_FIELDS[index1];
        if (!(strArray1[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "0802") || !(strArray1[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == ""))
        {
          int num1 = Utils.ParseInt((object) strArray1[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], 0);
          string str1 = strArray1[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
          double num2 = 0.0;
          double num3 = 0.0;
          string val1 = "";
          string val2 = "";
          int num4 = 0;
          int num5 = 0;
          double num6 = 0.0;
          bool flag1 = true;
          bool flag2 = true;
          if (num1 > 2000)
            break;
          if (num1 == 1102 && str1 == string.Empty)
          {
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            for (int index2 = 7; index2 < 13; ++index2)
            {
              string[] strArray2 = (string[]) HUDGFE2010Fields.POCPTCFIELDS[(object) ("POPT2.X" + (object) index2)];
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) > 0.0)
              {
                num2 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]);
                str2 = "";
                ++num4;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) > 0.0)
              {
                num2 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
                str2 = "Broker";
                ++num4;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) > 0.0)
              {
                num2 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]);
                str2 = "Lender";
                ++num4;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) > 0.0)
              {
                num2 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
                str2 = "Other";
                ++num4;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) > 0.0)
              {
                num3 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]);
                str4 = "Broker";
                ++num5;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) > 0.0)
              {
                num3 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]);
                str4 = "Lender";
                ++num5;
              }
              if (this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) > 0.0)
              {
                num3 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]);
                str4 = "Other";
                ++num5;
              }
              num6 += this.FltVal(strArray2[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]);
              string str6;
              string str7;
              if (index2 == 7)
              {
                if (num4 > 1)
                {
                  str6 = "";
                  flag1 = false;
                }
                if (num5 > 1)
                {
                  str7 = "";
                  flag2 = false;
                }
              }
              else
              {
                if (flag1 && (num4 > 1 || str3 != str2))
                {
                  str6 = "";
                  flag1 = false;
                }
                if (flag2 && (num5 > 1 || str5 != str4))
                {
                  str7 = "";
                  flag2 = false;
                }
              }
              num4 = 0;
              num5 = 0;
              str3 = str2;
              str5 = str4;
            }
            val1 = flag1 ? str3 : "";
            val2 = flag2 ? str5 : "";
          }
          else
          {
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]) > 0.0)
            {
              num2 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]);
              val1 = "";
              ++num4;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]) > 0.0)
            {
              num2 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
              val1 = "Broker";
              ++num4;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]) > 0.0)
            {
              num2 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]);
              val1 = "Lender";
              ++num4;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]) > 0.0)
            {
              num2 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
              val1 = "Other";
              ++num4;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]) > 0.0)
            {
              num3 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]);
              val2 = "Broker";
              ++num5;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]) > 0.0)
            {
              num3 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]);
              val2 = "Lender";
              ++num5;
            }
            if (this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]) > 0.0)
            {
              num3 += this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]);
              val2 = "Other";
              ++num5;
            }
            num6 = this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]);
            flag1 = num4 == 1;
            flag2 = num5 == 1;
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_POC] != string.Empty)
          {
            if (num2 > 0.0)
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POC], "Y");
            else
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POC], "");
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCAMT] != string.Empty)
            this.SetCurrentNum(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], num2);
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] != string.Empty)
          {
            if (flag1)
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], val1);
            else
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "");
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTC] != string.Empty)
          {
            if (num3 > 0.0 & flag2)
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTC], "Y");
            else
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTC], "");
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT] != string.Empty)
          {
            if (flag2)
              this.SetCurrentNum(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3);
            else
              this.SetCurrentNum(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], 0.0);
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY] != string.Empty)
          {
            if (flag2)
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], val2);
            else
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "");
          }
          if (strArray1[HUDGFE2010Fields.PTCPOCINDEX_FINANCED] != string.Empty)
          {
            if (num6 > 0.0)
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_FINANCED], "Y");
            else
              this.SetVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_FINANCED], "");
          }
        }
      }
    }

    internal void calculate_LastDisclosedLE(string id, string val)
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log == null && this.Val("FV.X357") == "")
        return;
      if (idisclosureTracking2015Log == null && this.Val("FV.X357") != "")
      {
        this.SetVal("FV.X357", "");
        for (int index = 203; index <= 4163; index += 33)
        {
          string id1 = "NEWHUD2.X" + (object) index;
          if (!this.excludedItemizationFields.Contains(id1))
            this.SetVal(id1, "");
        }
      }
      else
      {
        if (this.Val("FV.X357") == idisclosureTracking2015Log.Guid)
          return;
        this.SetVal("FV.X357", idisclosureTracking2015Log.Guid);
        double num;
        for (int index = 203; index <= 4361; index += 33)
        {
          string str = "NEWHUD2.X" + (object) index;
          if (!this.excludedItemizationFields.Contains(str))
          {
            string fieldId1 = "NEWHUD2.X" + (object) (index - 2);
            string fieldId2 = "NEWHUD2.X" + (object) (index + 27);
            double result1 = 0.0;
            if (fieldId1 == "NEWHUD2.X894")
              fieldId1 = "NEWHUD.X1149";
            switch (str)
            {
              case "NEWHUD2.X4196":
                str = "NEWHUD2.X4429";
                break;
              case "NEWHUD2.X4229":
                str = "NEWHUD2.X4430";
                break;
              case "NEWHUD2.X4262":
                str = "NEWHUD2.X4431";
                break;
              case "NEWHUD2.X4295":
                str = "NEWHUD2.X4432";
                break;
              case "NEWHUD2.X4328":
                str = "NEWHUD2.X4433";
                break;
              case "NEWHUD2.X4361":
                str = "NEWHUD2.X4434";
                break;
            }
            double.TryParse(idisclosureTracking2015Log.GetDisclosedField(fieldId1), out result1);
            double result2 = 0.0;
            double.TryParse(idisclosureTracking2015Log.GetDisclosedField(fieldId2), out result2);
            string id2 = str;
            num = result1 - result2;
            string val1 = num.ToString();
            this.SetVal(id2, val1);
          }
        }
        for (int index = 4447; index <= 4579; index += 33)
        {
          string str = "NEWHUD2.X" + (object) index;
          if (!this.excludedItemizationFields.Contains(str))
          {
            string fieldId3 = "NEWHUD2.X" + (object) (index - 2);
            string fieldId4 = "NEWHUD2.X" + (object) (index + 27);
            double result3 = 0.0;
            switch (str)
            {
              case "NEWHUD2.X4447":
                str = "NEWHUD2.X4645";
                break;
              case "NEWHUD2.X4480":
                str = "NEWHUD2.X4646";
                break;
              case "NEWHUD2.X4513":
                str = "NEWHUD2.X4647";
                break;
              case "NEWHUD2.X4546":
                str = "NEWHUD2.X4648";
                break;
              case "NEWHUD2.X4579":
                str = "NEWHUD2.X4649";
                break;
            }
            double.TryParse(idisclosureTracking2015Log.GetDisclosedField(fieldId3), out result3);
            double result4 = 0.0;
            double.TryParse(idisclosureTracking2015Log.GetDisclosedField(fieldId4), out result4);
            string id3 = str;
            num = result3 - result4;
            string val2 = num.ToString();
            this.SetVal(id3, val2);
          }
        }
      }
    }

    internal void calculate_LastDisclosedCD(string id, string val)
    {
      IDisclosureTracking2015Log tracking2015LogByType = this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation);
      if (tracking2015LogByType == null)
        this.SetVal("FV.X367", "");
      else
        this.SetVal("FV.X367", tracking2015LogByType.Guid);
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      if (idisclosureTracking2015Log == null)
      {
        this.SetVal("CD3.X135", "");
        this.SetVal("CD2.XLDLC", "");
        this.SetVal("CD2.XLDOC", "");
        this.SetVal("CD2.XLDLCR", "");
      }
      if (idisclosureTracking2015Log == null && this.Val("FV.X359") == "")
        return;
      if (idisclosureTracking2015Log == null && this.Val("FV.X359") != "")
      {
        this.SetVal("FV.X359", "");
        for (int index = 204; index <= 4362; index += 33)
        {
          string id1 = "NEWHUD2.X" + (object) index;
          if (!this.excludedItemizationFields.Contains(id1))
            this.SetVal(id1, "");
        }
        this.SetVal("NEWHUD2.X4392", "");
      }
      else
      {
        if (this.Val("FV.X359") == idisclosureTracking2015Log.Guid)
          return;
        this.SetVal("FV.X359", idisclosureTracking2015Log.Guid);
        for (int index = 204; index <= 4580; index += 33)
        {
          if (index == 4395)
            index = 4448;
          string id2 = "NEWHUD2.X" + (object) index;
          if (!this.excludedItemizationFields.Contains(id2))
          {
            string fieldId = "NEWHUD2.X" + (object) (index - 3);
            switch (fieldId)
            {
              case "NEWHUD2.X465":
                fieldId = "NEWHUD.X225";
                break;
              case "NEWHUD2.X894":
                fieldId = "NEWHUD.X1149";
                break;
            }
            this.SetVal(id2, idisclosureTracking2015Log.GetDisclosedField(fieldId));
          }
        }
        this.SetVal("NEWHUD2.X4392", idisclosureTracking2015Log.GetDisclosedField("NEWHUD2.X4393"));
        this.SetVal("CD3.X135", idisclosureTracking2015Log.GetDisclosedField("CD3.X129"));
        this.SetVal("CD2.XLDLC", idisclosureTracking2015Log.GetDisclosedField("CD2.XSTD"));
        this.SetVal("CD2.XLDOC", idisclosureTracking2015Log.GetDisclosedField("CD2.XSTI"));
        this.SetVal("CD2.XLDLCR", idisclosureTracking2015Log.GetDisclosedField("CD2.XSTLC"));
      }
    }

    internal void calculate_EarliestSafeHarbor(string id, string val)
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetEarliestSafeHarborIDisclosureTracking2015Log();
      if (idisclosureTracking2015Log == null)
        this.SetVal("FV.X368", "");
      else
        this.SetVal("FV.X368", idisclosureTracking2015Log.Guid);
    }

    internal void calculate_EarliestSSPL(string id, string val)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = this.loan.GetLogList().GetEarliestSSPLIDisclosureTracking2015Log();
      if (disclosureTracking2015Log == null)
        this.SetVal("FV.X369", "");
      else
        this.SetVal("FV.X369", disclosureTracking2015Log.Guid);
    }

    internal void calculate_TaxesAndInsuranceIndicators(string id, string val)
    {
      switch (id)
      {
        case "NEWHUD2.X4413":
        case "NEWHUD2.X4414":
          this.populate_2015LineItem904TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4415":
        case "NEWHUD2.X4416":
        case "NEWHUD2.X4435":
          this.populate_2015LineItem907TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4417":
        case "NEWHUD2.X4418":
        case "NEWHUD2.X4436":
          this.populate_2015LineItem908TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4419":
        case "NEWHUD2.X4420":
        case "NEWHUD2.X4437":
          this.populate_2015LineItem909TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4421":
        case "NEWHUD2.X4422":
        case "NEWHUD2.X4438":
          this.populate_2015LineItem910TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4423":
        case "NEWHUD2.X4424":
        case "NEWHUD2.X4439":
          this.populate_2015LineItem911TaxesAndInsuranceIndicators(id, val);
          break;
        case "NEWHUD2.X4425":
        case "NEWHUD2.X4426":
        case "NEWHUD2.X4440":
          this.populate_2015LineItem912TaxesAndInsuranceIndicators(id, val);
          break;
      }
    }
  }
}
