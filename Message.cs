using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NavigationBar
{
    public partial class MessageForm : Form
    {
        private string activeTab = "Message for Sir Bill";
        private FlowLayoutPanel tabsPanel;

        public MessageForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(1200, 600);
            this.Text = "Portfolio - Message for Sir Bill";
            this.Load += (s, e) => CreateNavigationBar();
        }

        private void CreateNavigationBar()
        {
            Panel navBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 204, 204)
            };

            navBar.Paint += (s, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, navBar.Width - 1, navBar.Height - 1);
                using (GraphicsPath path = GetRoundedRect(rect, 40))
                {
                    navBar.Region = new Region(path);
                    using (Pen pen = new Pen(Color.FromArgb(0, 180, 180), 4))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(30, 10, 40, 10)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            Label lblBack = new Label
            {
                Text = "←",
                Font = new Font("Arial", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(10, 0, 30, 0),
                Cursor = Cursors.Hand
            };

            lblBack.Click += (s, e) =>
            {
                this.Close();
                new Home().Show();
            };

            PictureBox pbAvatar = new PictureBox
            {
                Size = new Size(56, 56),
                BackColor = Color.White,
                Margin = new Padding(0, 8, 18, 0)
            };

            pbAvatar.Paint += (s, e) =>
            {
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddEllipse(0, 0, pbAvatar.Width - 1, pbAvatar.Height - 1);
                    pbAvatar.Region = new Region(p);
                }

                using (Font f = new Font("Arial", 24F, FontStyle.Bold))
                using (Brush b = new SolidBrush(Color.FromArgb(0, 204, 204)))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString("A", f, b, pbAvatar.ClientRectangle, sf);
                }
            };

            Label lblName = new Label
            {
                Text = "Asuncion",
                ForeColor = Color.White,
                Font = new Font("Arial", 16F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 0)
            };

            leftPanel.Controls.Add(lblBack);
            leftPanel.Controls.Add(pbAvatar);
            leftPanel.Controls.Add(lblName);
            layout.Controls.Add(leftPanel, 0, 0);

            tabsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 15, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            string[] tabs = { "Home", "Educations", "Skills", "Hobbies", "Message for Sir Bill" };

            foreach (string tab in tabs)
            {
                Label tabLabel = CreateTabLabel(tab);
                tabsPanel.Controls.Add(tabLabel);
            }

            layout.Controls.Add(tabsPanel, 1, 0);
            navBar.Controls.Add(layout);
            this.Controls.Add(navBar);
        }

        private Label CreateTabLabel(string text)
        {
            Label lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Arial", 13F, FontStyle.Regular),
                AutoSize = false,
                Width = 140,
                Height = 50,
                Cursor = Cursors.Hand,
                Margin = new Padding(12, 0, 12, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            UpdateTabAppearance(lbl, text == activeTab);

            lbl.Click += (s, e) =>
            {
                activeTab = text;
                foreach (Control c in tabsPanel.Controls)
                {
                    if (c is Label tabLbl)
                        UpdateTabAppearance(tabLbl, tabLbl.Text == activeTab);
                }
                NavigateToTab(text);
            };

            lbl.MouseEnter += (s, e) =>
            {
                if (text != activeTab)
                    lbl.ForeColor = Color.FromArgb(200, 255, 255);
            };

            lbl.MouseLeave += (s, e) =>
            {
                if (text != activeTab)
                    lbl.ForeColor = Color.White;
            };

            return lbl;
        }

        private void UpdateTabAppearance(Label lbl, bool isActive)
        {
            lbl.BackColor = isActive ? Color.Black : Color.Transparent;
            lbl.Font = new Font("Arial", 13F, isActive ? FontStyle.Bold : FontStyle.Regular);
            lbl.Invalidate();
        }

        private void NavigateToTab(string tabName)
        {
            if (tabName == "Message for Sir Bill") return;
            this.Close();
            switch (tabName)
            {
                case "Home": new Home().Show(); break;
                case "Educations": new EducationForm().Show(); break;
                case "Skills": new SkillsForm().Show(); break;
                case "Hobbies": new HobbiesForm().Show(); break;
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
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