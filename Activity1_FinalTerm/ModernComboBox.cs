using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Activity1_FinalTerm
{
    public class FlatComboBox : UserControl
    {
        // Appearance Properties
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;

        [Category("Appearance Custom")]
        [Description("The custom border color of the control.")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; this.Invalidate(); }
        }

        [Category("Appearance Custom")]
        [Description("The border thickness in pixels.")]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = Math.Max(1, value); // Ensure it's at least 1px
                UpdateControlHeight();
                this.Invalidate();
            }
        }

        private ComboBox cmbList;

        public FlatComboBox()
        {
            cmbList = new ComboBox();
            this.SuspendLayout();

            // 1. Force the ComboBox to be strictly Read-Only & Flat
            cmbList.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbList.FlatStyle = FlatStyle.Flat;

            // 2. IMPORTANT: Do NOT use DockStyle.Fill. We position manually so borders don't get eaten.
            cmbList.Dock = DockStyle.None;

            // 3. REQUIRED FOR ITEM HEIGHT CUSTOMIZATION: Enable Owner Drawing
            cmbList.DrawMode = DrawMode.OwnerDrawFixed;
            cmbList.ItemHeight = 20; // Default height for rows

            // 4. Sync colors
            cmbList.BackColor = this.BackColor;
            cmbList.ForeColor = this.ForeColor;

            // 5. Custom Drawing logic to center text perfectly inside the list
            cmbList.DrawItem += CmbList_DrawItem;

            // 6. Connect Selection Event
            cmbList.SelectedIndexChanged += (s, e) => SelectedIndexChanged?.Invoke(this, e);

            // 7. Setup Wrapper Control Properties
            this.Controls.Add(cmbList);
            this.MinimumSize = new Size(100, 20);
            this.Size = new Size(200, 30);

            this.ResumeLayout(false);

            // Apply perfect math layouts
            UpdateControlHeight();
        }

        // =========================================================================
        // PROPERTIES TO ADJUST THE ITEMS CONTAINER SIZE
        // =========================================================================

        [Category("Appearance Dropdown")]
        [Description("The height of each individual row inside the dropdown list container.")]
        public int ItemRowHeight
        {
            get => cmbList.ItemHeight;
            set
            {
                cmbList.ItemHeight = value;
                UpdateControlHeight(); // Recalculate everything so bottom border stays visible
            }
        }

        [Category("Appearance Dropdown")]
        [Description("The total width of the dropped-down container list. Can be wider than the control itself.")]
        public int DropdownContainerWidth
        {
            get => cmbList.DropDownWidth;
            set => cmbList.DropDownWidth = value;
        }

        [Category("Appearance Dropdown")]
        [Description("The maximum number of row items shown in the container list before scrolling.")]
        public int MaxVisibleRows
        {
            get => cmbList.MaxDropDownItems;
            set => cmbList.MaxDropDownItems = value;
        }

        // =========================================================================

        [Category("Data")]
        [Description("The items in the combo box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        public ComboBox.ObjectCollection Items
        {
            get { return cmbList.Items; }
        }

        public object SelectedItem { get => cmbList.SelectedItem; set => cmbList.SelectedItem = value; }
        public int SelectedIndex { get => cmbList.SelectedIndex; set => cmbList.SelectedIndex = value; }
        public event EventHandler SelectedIndexChanged;

        // MATHEMATICALLY POSITION THE INNER COMBOBOX
        private void UpdateControlHeight()
        {
            // The inner native ComboBox height is inherently bound to its item height. 
            // We calculate exactly how tall the parent user control needs to be to comfortably fit it + borders.
            int requiredControlHeight = cmbList.Height + (BorderSize * 2);
            this.Height = requiredControlHeight;

            // Anchor the inner combobox safely inside the borders without spilling over the bottom
            cmbList.Location = new Point(BorderSize, BorderSize);
            cmbList.Width = this.Width - (BorderSize * 2);
        }

        // Force a layout update if resized by stretching in the Visual Studio designer
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateControlHeight();
            this.Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            cmbList.ForeColor = this.ForeColor;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            cmbList.BackColor = this.BackColor;
        }

        // Clean layout renderer to handle row background highlight colors and centering text
        private void CmbList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                string text = cmbList.Items[e.Index].ToString();
                Rectangle rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, text, e.Font, rect, e.ForeColor, TextFormatFlags.VerticalCenter);
            }

            e.DrawFocusRectangle();
        }

        // Paint the flat layout border completely cleanly inside the safe client bounds
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw a completely closed bounding rectangle using precise dimensions
            using (Pen penBorder = new Pen(BorderColor, BorderSize))
            {
                e.Graphics.DrawRectangle(
                    penBorder,
                    BorderSize / 2,
                    BorderSize / 2,
                    this.Width - BorderSize,
                    this.Height - BorderSize
                );
            }
        }
    }
}
