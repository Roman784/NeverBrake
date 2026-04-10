using UnityEngine;

namespace Utils
{
    public static class NumberFormatter
    {
        public static string FormatMoney(int money)
        {
            return $"${FormatCurrency(money)}";
        }

        private static string FormatCurrency(int currency)
        {
            var absCurrency = Mathf.Abs(currency);
            var suffix = "";
            var divisor = 1.0;

            if (absCurrency >= 1e9)
            {
                suffix = "b";
                divisor = 1e9;
            }
            else if (absCurrency >= 1e6)
            {
                suffix = "m";
                divisor = 1e6;
            }
            else if (absCurrency >= 1e3)
            {
                suffix = "k";
                divisor = 1e3;
            }

            var formattedCurrency = absCurrency / divisor;

            if (divisor > 1)
                return $"{formattedCurrency:F2}{suffix}";
            else
                return Mathf.Floor(absCurrency).ToString();
        }
    }
}
