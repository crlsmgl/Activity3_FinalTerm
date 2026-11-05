using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundPictureBox : PictureBox
{
    public int CornerRadius { get; set; } = 30;

    protected override void OnPaint(PaintEventArgs e)
    {

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using (GraphicsPath path = new GraphicsPath())
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int radius = CornerRadius;

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);

            base.OnPaint(e);
        }

    }
}