using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.constants;

namespace ServiceSitoPanel.src.functions
{
    public static class HandleFunctions
    {
        public static string? SelectStatus(int value)
        {
            switch (value)
            {
                case 1:
                    return StatusOrder.NewStatus[Status.PendingPurchase];

                case 2:
                    return StatusOrder.NewStatus[Status.SaleToRecive];

                case 3:
                    return StatusOrder.NewStatus[Status.ReadyForDelivery];

                case 4:
                    return StatusOrder.NewStatus[Status.ConfirmSale];

                case 5:
                    return StatusOrder.NewStatus[Status.PaidPurchase];

                default:
                    return null;
            }
        }

        public static TimeZoneInfo GetTimeZone()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return tz;
        }

    }
}