using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NavigationBar
{
    public partial class HobbiesForm : Form
    {
        private string activeTab = "Hobbies";
        private FlowLayoutPanel tabsPanel;

        public HobbiesForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(1200, 700);
            this.Text = "Portfolio - Hobbies";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += (s, e) => CreateNavigationBar();
        }

        private void CreateNavigationBar()
        {
            var navBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 204, 204)
            };

            // Clean rounded corners — NO darker border
            navBar.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, navBar.Width - 1, navBar.Height - 1);
                using (var path = GetRoundedRect(rect, 40))
                {
                    navBar.Region = new Region(path);
                }
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(30, 10, 40, 10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // LEFT: Back arrow + Avatar + Name
            var leftPanel = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };

            var lblBack = new Label
            {
                Text = "Back Arrow",
                Font = new Font("Arial", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(10, 0, 30, 0),
                Cursor = Cursors.Hand
            };
            lblBack.Click += (s, e) =>
            {
                this.Hide();
                new Home().Show();
            };

            var pbAvatar = new PictureBox
            {
                Size = new Size(56, 56),
                BackColor = Color.White,
                Margin = new Padding(0, 8, 18, 0)
            };
            pbAvatar.Paint += (s, e) =>
            {
                using (var p = new GraphicsPath())
                {
                    p.AddEllipse(0, 0, pbAvatar.Width - 1, pbAvatar.Height - 1);
                    pbAvatar.Region = new Region(p);
                }
                using (var f = new Font("Arial", 24F, FontStyle.Bold))
                using (var b = new SolidBrush(Color.FromArgb(0, 204, 204)))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString("A", f, b, pbAvatar.ClientRectangle, sf);
                }
            };

            var lblName = new Label
            {
                Text = "Asuncion",
                ForeColor = Color.White,
                Font = new Font("Arial", 16F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 0)
            };

            leftPanel.Controls.AddRange(new Control[] { lblBack, pbAvatar, lblName });
            layout.Controls.Add(leftPanel, 0, 0);

            // RIGHT: Tabs
            tabsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 15, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            string[] tabs = { "Home", "Educations", "Skills", "Hobbies", "Message for Sir Bill" };
            foreach (string tab in tabs)
                tabsPanel.Controls.Add(CreateTabLabel(tab));

            layout.Controls.Add(tabsPanel, 1, 0);
            navBar.Controls.Add(layout);
            this.Controls.Add(navBar);
        }

        private Label CreateTabLabel(string text)
        {
            var lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Arial", 13F, FontStyle.Regular),
                Size = new Size(140, 50),
                Cursor = Cursors.Hand,
                Margin = new Padding(12, 0, 12, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            UpdateTabAppearance(lbl, text == activeTab);

            lbl.Click += (s, e) =>
            {
                activeTab = text;
                foreach (Control c in tabsPanel.Controls)
                    if (c is Label l) UpdateTabAppearance(l, l.Text == activeTab);

                NavigateToTab(text);
            };

            lbl.MouseEnter += (s, e) => { if (text != activeTab) lbl.ForeColor = Color.FromArgb(200, 255, 255); };
            lbl.MouseLeave += (s, e) => { if (text != activeTab) lbl.ForeColor = Color.White; };

            return lbl;
        }

        private void UpdateTabAppearance(Label lbl, bool isActive)
        {
            lbl.BackColor = isActive ? Color.Black : Color.Transparent;
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Arial", 13F, isActive ? FontStyle.Bold : FontStyle.Regular);
            lbl.Invalidate();
        }

        private void NavigateToTab(string tabName)
        {
            if (tabName == "Hobbies") return;

            this.Hide();
            switch (tabName)
            {
                case "Home": new Home().Show(); break;
                case "Educations": new EducationForm().Show(); break;
                case "Skills": new SkillsForm().Show(); break;
                case "Message for Sir Bill": new MessageForm().Show(); break;
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Optional: keep only if designer has it
        private void HobbiesForm_Load(object sender, EventArgs e) { }
    }
}