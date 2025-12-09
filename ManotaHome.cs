using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NavigationBar
{
    public partial class ManotaHome : Form
    {
        private string activeTab = "Home";
        private FlowLayoutPanel tabsPanel;

        public ManotaHome()
        {
            InitializeComponent();
            this.ClientSize = new Size(1200, 600);
            this.Text = "Portfolio - Home";
            this.Load += (s, e) => CreateNavigationBar();
        }

        private void CreateNavigationBar()
        {
            Panel navBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(242, 216, 240),
                Margin = new Padding(20, 15, 20, 0)
            };

            navBar.Paint += (s, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, navBar.Width - 1, navBar.Height - 1);
                using (GraphicsPath path = GetRoundedRect(rect, 35))
                {
                    navBar.Region = new Region(path);
                }
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblBack = new Label
            {
                Text = "←",
                Font = new Font("Arial", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            lblBack.Click += (s, e) => MessageBox.Show("Back button clicked");
            layout.Controls.Add(lblBack, 0, 0);

            tabsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0),
                WrapContents = false
            };

            string[] tabs = { "Home", "Educations", "Skills", "Hobbies", "Message for Sir Bill" };

            foreach (string tab in tabs)
            {
                Label tabLabel = CreateTabLabel(tab);
                tabsPanel.Controls.Add(tabLabel);
            }

            TableLayoutPanel centerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };
            centerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            centerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            centerPanel.Controls.Add(tabsPanel, 0, 0);
            tabsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            tabsPanel.Margin = new Padding(0);

            layout.Controls.Add(centerPanel, 1, 0);

            Label lblRight = new Label
            {
                Text = "",
                AutoSize = true
            };
            layout.Controls.Add(lblRight, 2, 0);

            navBar.Controls.Add(layout);
            this.Controls.Add(navBar);
            navBar.BringToFront();
        }

        private Label CreateTabLabel(string text)
        {
            Label lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Arial", 12F, FontStyle.Regular),
                AutoSize = false,
                Width = 140,
                Height = 40,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add paint event to draw rectangle border for active tabs
            lbl.Paint += (s, e) =>
            {
                if (text == activeTab)
                {
                    Rectangle rect = new Rectangle(0, 0, lbl.Width - 1, lbl.Height - 1);
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
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
                this.Text = $"Portfolio - {text}";
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
            if (isActive)
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Arial", 12F, FontStyle.Bold);
            }
            else
            {
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Arial", 12F, FontStyle.Regular);
            }

            lbl.Invalidate();
        }

        private void NavigateToTab(string tabName)
        {
            if (tabName == "Home") return;

            switch (tabName)
            {
                case "Educations":
                    new EducationForm().Show();
                    this.Hide();
                    break;
                case "Skills":
                    new SkillsForm().Show();
                    this.Hide();
                    break;
                case "Hobbies":
                    new HobbiesForm().Show();
                    this.Hide();
                    break;
                case "Message for Sir Bill":
                    new MessageForm().Show();
                    this.Hide();
                    break;
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

        private void Home_Load(object sender, EventArgs e)
        {

        }
    }
}