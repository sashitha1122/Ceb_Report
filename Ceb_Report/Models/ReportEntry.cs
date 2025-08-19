namespace Ceb_Report.Models
{
    public class ReportEntry
    {
        public string area_code { get; set; }
        public string acct_number { get; set; }
        public string net_type { get; set; }
        public decimal units_out { get; set; }
        public decimal units_in { get; set; }
        public decimal gen_cap { get; set; }
        public int bill_cycle { get; set; }
        public string tariff_code { get; set; }
        public decimal bf_units { get; set; }
        public decimal units_bill { get; set; }
        public string period { get; set; }
        public decimal kwh_chg { get; set; }
        public decimal fxd_chg { get; set; }
        public decimal fac_chg { get; set; }
        public decimal cf_units { get; set; }
        public decimal rate { get; set; }
        public decimal unitsale { get; set; }
        public decimal kwh_sales { get; set; }
        public string bank_code { get; set; }
        public string bran_code { get; set; }
        public string bk_ac_no { get; set; }
        public DateTime agrmnt_date { get; set; }
    }
}
