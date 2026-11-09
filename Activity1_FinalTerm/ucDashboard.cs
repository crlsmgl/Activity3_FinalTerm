using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Activity1_FinalTerm
{
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();
        }

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            Series series = new Series("SalesData");
            series.ChartType = SeriesChartType.Line;

            series.Points.AddXY("Jan", 100);
            series.Points.AddXY("Feb", 150);
            series.Points.AddXY("Mar", 120);
            series.Points.AddXY("Apr", 200);
            series.Points.AddXY("May", 250);
            series.Points.AddXY("Jun", 300);
            series.Points.AddXY("Jul", 220);

            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "'₱'#,##0k";

            chart1.Series.Add(series);

            chart1.Titles.Clear();
            chart1.Titles.Add("Sales Data");
        }
    }
}
