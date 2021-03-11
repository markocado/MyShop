using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class BasketSummaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotalValue { get; set; }
        public BasketSummaryViewModel()
        {
            BasketCount = 0;
            BasketTotalValue = 0;
        }
        public BasketSummaryViewModel(int basketCount, decimal basketTotalValue)
        {
            this.BasketTotalValue = basketTotalValue;
            this.BasketCount = basketCount;
        }
    }
}
