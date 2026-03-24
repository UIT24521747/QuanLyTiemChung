using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyKhachHang
{
    public class RoundedPanel : Panel
    {
        [DefaultValue(24)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius { get; set; } = 24;
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundPath(rect, BorderRadius))
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class ModernButton : Button
    {
        [DefaultValue(6)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius { get; set; } = 6;

        public ModernButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.FromArgb(66, 133, 244);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundPath(rect, BorderRadius))
            {
                this.Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class RoundedTextBox : UserControl
    {
        private TextBox textBox;
        private int borderRadius = 6;
        private Color borderColor = Color.FromArgb(203, 213, 225); // Slate 300
        private int borderSize = 1;

        public RoundedTextBox()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Padding = new Padding(10, 7, 10, 7);
            this.Size = new Size(250, 35);
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.BackColor = this.BackColor;
            textBox.Font = this.Font;
            
            textBox.TextChanged += (s, e) => this.OnTextChanged(e);
            
            this.Controls.Add(textBox);
        }

        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get { return textBox.ReadOnly; }
            set { textBox.ReadOnly = value; }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                if (textBox != null) textBox.BackColor = value;
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                if (textBox != null)
                {
                    textBox.Font = value;
                    this.Height = textBox.Height + this.Padding.Top + this.Padding.Bottom;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath pathBorder = GetRoundPath(rectBorder, borderRadius))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                this.Region = new Region(pathBorder);
                graph.DrawPath(penBorder, pathBorder);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class RoundedDateTimePicker : UserControl
    {
        private DateTimePicker dtp;
        private int borderRadius = 6;
        private Color borderColor = Color.FromArgb(203, 213, 225);
        private int borderSize = 1;

        public RoundedDateTimePicker()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Padding = new Padding(1);
            this.Size = new Size(250, 35);
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            dtp = new DateTimePicker();
            dtp.Dock = DockStyle.Fill;
            dtp.Font = this.Font;
            dtp.Format = DateTimePickerFormat.Short;
            
            dtp.ValueChanged += (s, e) => this.OnValueChanged(e);
            
            this.Controls.Add(dtp);
        }

        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime Value
        {
            get { return dtp.Value; }
            set { dtp.Value = value; }
        }
        
        [DefaultValue(DateTimePickerFormat.Short)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTimePickerFormat Format
        {
            get { return dtp.Format; }
            set { dtp.Format = value; }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                if (dtp != null)
                {
                    dtp.Font = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath pathBorder = GetRoundPath(rectBorder, borderRadius))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                this.Region = new Region(pathBorder);
                graph.DrawPath(penBorder, pathBorder);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
