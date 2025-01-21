using Syncfusion.Maui.Toolkit.Charts;

namespace TECin2.MAUI.Pages.Controls
{
    public class LegendExt : ChartLegend
    {
        protected override double GetMaximumSizeCoefficient()
        {
            return 0.5;
        }
    }
}
