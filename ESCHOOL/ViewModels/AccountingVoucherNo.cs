using System;

namespace ESCHOOL.ViewModels
{
    public class AccountingVoucherNo
    {
        public int ViewModelId { get; set; }
        public int VoucherNo { get; set; }

        public Nullable<decimal> Debt { get; set; }
        public Nullable<decimal> Credit { get; set; }
    }
}
