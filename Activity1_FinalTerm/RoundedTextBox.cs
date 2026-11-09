using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public partial class RoundedTextBox : UserControl
{
    private TextBox innerTextBox;
    private int borderRadius = 10;
    private int borderSize = 2;

    private Color borderColor = Color.Gray;
    private Color borderActiveColor = Color.Blue;
    private Color borderInactiveColor = Color.Gray;
    private bool isFocused = false;

    private bool useSystemPasswordChar = false;
    private char passwordChar = '\0';

    // Placeholder fields
    private string placeholderText = "";
    private Color placeholderColor = Color.DarkGray;
    private bool isPlaceholderActive = false;

    public RoundedTextBox()
    {
        innerTextBox = new TextBox();
        innerTextBox.BorderStyle = BorderStyle.None;
        innerTextBox.Multiline = true;
        innerTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.Controls.Add(innerTextBox);

        this.Padding = new Padding(10, 7, 10, 7);
        this.Size = new Size(250, 35);
        this.BackColor = Color.White;

        innerTextBox.Enter += InnerTextBox_Enter;
        innerTextBox.Leave += InnerTextBox_Leave;
        innerTextBox.KeyDown += InnerTextBox_KeyDown;
        innerTextBox.TextChanged += InnerTextBox_TextChanged;

        UpdateTextBoxPosition();
    }

    public void Clear()
    {
        innerTextBox.Clear();
        ApplyPlaceholder();
    }

    [Category("Custom Properties")]
    [Description("Indicates if the text in the RoundedTextBox should appear as the platform's default password character.")]
    [DefaultValue(false)]
    public bool UseSystemPasswordChar
    {
        get => useSystemPasswordChar;
        set
        {
            useSystemPasswordChar = value;
            UpdatePasswordCharacter();
        }
    }

    [Category("Custom Properties")]
    [Description("The character used to mask characters in a single-line TextBox for password input.")]
    [DefaultValue('\0')]
    public char PasswordChar
    {
        get => passwordChar;
        set
        {
            passwordChar = value;
            UpdatePasswordCharacter();
        }
    }

    private void UpdatePasswordCharacter()
    {
        // Do not mask text if the placeholder is currently active
        if (isPlaceholderActive)
        {
            innerTextBox.PasswordChar = '\0';
            return;
        }

        if (useSystemPasswordChar)
        {
            innerTextBox.PasswordChar = '●';
        }
        else
        {
            innerTextBox.PasswordChar = passwordChar;
        }
    }

    private void InnerTextBox_TextChanged(object sender, EventArgs e)
    {
        UpdatePasswordCharacter();
    }

    private void InnerTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    private void InnerTextBox_Enter(object sender, EventArgs e)
    {
        isFocused = true;
        borderColor = borderActiveColor;

        // Remove placeholder when user clicks into the field
        if (isPlaceholderActive)
        {
            RemovePlaceholder();
        }

        this.Invalidate();
    }

    private void InnerTextBox_Leave(object sender, EventArgs e)
    {
        isFocused = false;
        borderColor = borderInactiveColor;

        // Restore placeholder if the field was left empty
        if (string.IsNullOrWhiteSpace(innerTextBox.Text))
        {
            ApplyPlaceholder();
        }

        this.Invalidate();
    }

    [Category("Custom Properties")]
    public int BorderRadius
    {
        get => borderRadius;
        set { borderRadius = value; this.Invalidate(); }
    }

    [Category("Custom Properties")]
    public Color BorderActiveColor
    {
        get => borderActiveColor;
        set { borderActiveColor = value; if (isFocused) { borderColor = value; this.Invalidate(); } }
    }

    [Category("Custom Properties")]
    public Color BorderInactiveColor
    {
        get => borderInactiveColor;
        set { borderInactiveColor = value; if (!isFocused) { borderColor = value; this.Invalidate(); } }
    }

    [Category("Custom Properties")]
    [Browsable(false)]
    public Color BorderColor
    {
        get => borderInactiveColor;
        set { borderInactiveColor = value; if (!isFocused) { borderColor = value; this.Invalidate(); } }
    }

    [Category("Custom Properties")]
    [Browsable(true)] // Forces the property to show up in the Properties Window
    [EditorBrowsable(EditorBrowsableState.Always)]
    [Description("The text that is displayed when the control is empty and does not have focus.")]
    [DefaultValue("")]
    public string PlaceholderText
    {
        get => placeholderText;
        set
        {
            placeholderText = value;
            if (string.IsNullOrWhiteSpace(innerTextBox.Text) && !isFocused)
            {
                ApplyPlaceholder();
            }
        }
    }

    [Category("Custom Properties")]
    [Browsable(true)] // Forces the property to show up in the Properties Window
    [EditorBrowsable(EditorBrowsableState.Always)]
    [Description("The color of the placeholder text.")]
    public Color PlaceholderColor
    {
        get => placeholderColor;
        set
        {
            placeholderColor = value;
            if (isPlaceholderActive)
            {
                innerTextBox.ForeColor = value;
            }
        }
    }

    public override string Text
    {
        get => isPlaceholderActive ? "" : innerTextBox.Text;
        set
        {
            if (string.IsNullOrWhiteSpace(value) && !isFocused)
            {
                ApplyPlaceholder();
            }
            else
            {
                RemovePlaceholder();
                innerTextBox.Text = value;
            }
        }
    }

    private void ApplyPlaceholder()
    {
        isPlaceholderActive = true;
        innerTextBox.Text = placeholderText;
        innerTextBox.ForeColor = placeholderColor;
        UpdatePasswordCharacter();
    }

    private void RemovePlaceholder()
    {
        isPlaceholderActive = false;
        innerTextBox.Text = "";
        innerTextBox.ForeColor = this.ForeColor;
        UpdatePasswordCharacter();
    }

    private void UpdateTextBoxPosition()
    {
        if (innerTextBox == null) return;

        innerTextBox.Font = this.Font;
        innerTextBox.BackColor = this.BackColor;

        // Retain placeholder color if active, otherwise use default control ForeColor
        innerTextBox.ForeColor = isPlaceholderActive ? placeholderColor : this.ForeColor;

        innerTextBox.Height = this.Font.Height + 4;
        int centeredTop = (this.Height - innerTextBox.Height) / 2;

        innerTextBox.Location = new Point(this.Padding.Left, centeredTop);
        innerTextBox.Width = this.Width - this.Padding.Left - this.Padding.Right;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateTextBoxPosition();
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        UpdateTextBoxPosition();
        this.Invalidate();
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        UpdateTextBoxPosition();
        this.Invalidate();
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        float size = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, size, size, 180, 90);
        path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
        path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
        path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rectBorder = new Rectangle(0, 0, this.Width, this.Height);
        rectBorder.Width -= 1;
        rectBorder.Height -= 1;

        using (GraphicsPath pathBorder = GetRoundedPath(rectBorder, borderRadius))
        {
            using (SolidBrush brushBackground = new SolidBrush(this.BackColor))
            {
                graphics.FillPath(brushBackground, pathBorder);
            }

            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                graphics.DrawPath(penBorder, pathBorder);
            }
        }
    }
}
