using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class ControlExtensions
{
    /// <summary>
    /// Makes any control (like a Button) rounded by changing its Region.
    /// </summary>
    public static void MakeRounded(this Control control, int radius)
    {
        if (control == null) return;

        Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
        float size = radius * 2F;

        // Apply the rounded region immediately
        using (GraphicsPath path = new GraphicsPath())
        {
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, size, size, 180, 90);
            path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
            path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
            path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }

        // Hook up the resize event so it doesn't break if the button changes size later
        control.Resize -= Control_Resize;
        control.Resize += Control_Resize;
    }

    private static void Control_Resize(object sender, EventArgs e)
    {
        Control c = (Control)sender;
        // Adjust this number if you want a sharper or softer curve during resize
        int radius = 10;
        float size = radius * 2F;

        Rectangle r = new Rectangle(0, 0, c.Width, c.Height);
        using (GraphicsPath p = new GraphicsPath())
        {
            p.StartFigure();
            p.AddArc(r.X, r.Y, size, size, 180, 90);
            p.AddArc(r.Right - size, r.Y, size, size, 270, 90);
            p.AddArc(r.Right - size, r.Bottom - size, size, size, 0, 90);
            p.AddArc(r.X, r.Bottom - size, size, size, 90, 90);
            p.CloseFigure();
            c.Region = new Region(p);
        }
    }
}
