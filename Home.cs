using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NavigationBar
{
    /// <summary>
    /// Form1 - Home Page
    /// Main landing page for the portfolio with navigation bar and tab system
    /// </summary>
    public partial class Home : Form
    {
        private string activeTab = "Home"; // Tracks currently selected tab
        private FlowLayoutPanel tabsPanel; // Container for navigation tabs

        /// <summary>
        /// Constructor - Initializes form and loads navigation bar on form load
        /// </summary>
        public Home()
        {
            InitializeComponent();
            this.ClientSize = new Size(1200, 600);
            this.Text = "Portfolio - Home";
            this.Load += (s, e) => CreateNavigationBar();
        }

        /// <summary>
        /// CreateNavigationBar() - Creates the entire navigation bar UI
        /// Includes: rounded panel, back button, avatar, name, and tabs
        /// 
        /// NOTE: The rectangular Panel previously used for the nav bar has been removed.
        /// The layout is now added directly to the form and docked to Top so there is no
        /// rectangular background drawn behind it.
        /// </summary>
        private void CreateNavigationBar()
        {
            // Create main navigation bar panel with teal background
            Panel navBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 204, 204)
            };

            // Paint event to draw rounded corners on the navigation bar
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

            // Create table layout for organizing left (back, avatar, name) and right (tabs) sections
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 80, // keep same visual height as previous nav bar
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(30, 10, 40, 10)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // LEFT PANEL: Back button, Avatar circle, User name
            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            // Back button - arrow icon
            Label lblBack = new Label
            {
                Text = "←",
                Font = new Font("Arial", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(10, 0, 30, 0),
                Cursor = Cursors.Hand,
                TabStop = false
            };

            lblBack.Click += (s, e) => MessageBox.Show("Back button clicked");

            // Avatar - white circle with user initial
            PictureBox pbAvatar = new PictureBox
            {
                Size = new Size(56, 56),
                BackColor = Color.White,
                Margin = new Padding(0, 8, 18, 0)
            };

            pbAvatar.Paint += (s, e) =>
            {
                // Draw circular region for avatar
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddEllipse(0, 0, pbAvatar.Width - 1, pbAvatar.Height - 1);
                    pbAvatar.Region = new Region(p);
                }

                // Draw user initial "A" in the center
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

            // User name label
            Label lblName = new Label
            {
                Text = "Asuncion",
                ForeColor = Color.White,
                Font = new Font("Arial", 16F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 0)
            };

            // Add left panel components
            leftPanel.Controls.Add(lblBack);
            leftPanel.Controls.Add(pbAvatar);
            leftPanel.Controls.Add(lblName);
            layout.Controls.Add(leftPanel, 0, 0);

            // RIGHT PANEL: Navigation tabs
            tabsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 15, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Create tab buttons for each page
            string[] tabs = { "Home", "Educations", "Skills", "Hobbies", "Message for Sir Bill" };

            foreach (string tab in tabs)
            {
                Label tabLabel = CreateTabLabel(tab);
                tabsPanel.Controls.Add(tabLabel);
            }

            layout.Controls.Add(tabsPanel, 1, 0);

            // Add layout directly to the form (no rectangular Panel)
            this.Controls.Add(layout);
            layout.BringToFront();
        }

        /// <summary>
        /// CreateTabLabel() - Creates individual tab button with click and hover effects
        /// </summary>
        /// <param name="text">Tab name/text to display</param>
        /// <returns>Configured label control for the tab</returns>
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

            // Set initial appearance based on whether this is the active tab
            UpdateTabAppearance(lbl, text == activeTab);

            // Handle tab click - navigate to selected page
            lbl.Click += (s, e) =>
            {
                activeTab = text;
                // Update appearance of all tabs
                foreach (Control c in tabsPanel.Controls)
                {
                    if (c is Label tabLbl)
                        UpdateTabAppearance(tabLbl, tabLbl.Text == activeTab);
                }
                this.Text = $"Portfolio - {text}";
                NavigateToTab(text);
            };

            // Hover effect - change color when mouse enters (if not active tab)
            lbl.MouseEnter += (s, e) =>
            {
                if (text != activeTab)
                    lbl.ForeColor = Color.FromArgb(200, 255, 255);
            };

            // Restore color when mouse leaves (if not active tab)
            lbl.MouseLeave += (s, e) =>
            {
                if (text != activeTab)
                    lbl.ForeColor = Color.White;
            };

            return lbl;
        }

        /// <summary>
        /// UpdateTabAppearance() - Updates visual style of tab (active vs inactive)
        /// Active tabs show black background, bold font; inactive tabs are transparent
        /// </summary>
        /// <param name="lbl">The label control to update</param>
        /// <param name="isActive">Whether this is the currently active tab</param>
        private void UpdateTabAppearance(Label lbl, bool isActive)
        {
            if (isActive)
            {
                // Active tab styling - black background with bold text
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Arial", 13F, FontStyle.Bold);
            }
            else
            {
                // Inactive tab styling - transparent background with regular text
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Arial", 13F, FontStyle.Regular);
            }

            lbl.Invalidate(); // Redraw the control
        }

        /// <summary>
        /// NavigateToTab() - Handles navigation between different form pages
        /// </summary>
        /// <param name="tabName">Name of the tab/page to navigate to</param>
        private void NavigateToTab(string tabName)
        {
            // Don't navigate if already on Home page
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

        /// <summary>
        /// GetRoundedRect() - Creates a graphics path with rounded rectangle shape
        /// Used for rounded corners on the navigation bar
        /// </summary>
        /// <param name="rect">Base rectangle to round</param>
        /// <param name="radius">Radius of corner curves</param>
        /// <returns>GraphicsPath with rounded rectangle shape</returns>
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            // Draw curved corners at each corner of the rectangle
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}