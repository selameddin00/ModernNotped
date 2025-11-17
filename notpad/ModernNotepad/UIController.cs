using System.Drawing;
using System.Drawing.Drawing2D;

namespace ModernNotepad;

/// <summary>
/// UI elementlerini oluşturan ve yöneten sınıf.
/// Form'un görsel arayüzünü programmatically olarak oluşturur.
/// </summary>
public class UIController
{
    // UI Sabitleri
    private const int CornerRadius = 8;
    private const int TitleBarHeight = 40;
    private const int ButtonSize = 40;
    private const int StatusBarHeight = 25;

    // Renk Paleti (Dark Theme)
    private static readonly Color BackgroundColor = Color.FromArgb(0x1E, 0x1E, 0x1E);
    private static readonly Color TitleBarColor = Color.FromArgb(0x2D, 0x2D, 0x2D);
    private static readonly Color MenuBarColor = Color.FromArgb(0x3A, 0x3A, 0x3A);
    private static readonly Color TextColor = Color.White;
    private static readonly Color CloseButtonHoverColor = Color.FromArgb(0xE8, 0x11, 0x23);
    private static readonly Color ButtonHoverColor = Color.FromArgb(0x3A, 0x3A, 0x3A);

    // Font Sabitleri
    private const float DefaultFontSize = 14F;
    private const float MenuFontSize = 11F;
    private const float TitleBarFontSize = 12F;
    private const string TextFontFamily = "Consolas";
    private const string UIFontFamily = "Segoe UI";

    /// <summary>
    /// Form'un temel ayarlarını yapılandırır.
    /// Borderless, dark theme ve modern görünüm uygular.
    /// </summary>
    public static void ConfigureForm(Form form)
    {
        form.BackColor = BackgroundColor;
        form.FormBorderStyle = FormBorderStyle.None;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = "Modern Notepad";
        form.ClientSize = new Size(1000, 600);
        form.MinimumSize = new Size(400, 300);
    }

    /// <summary>
    /// Modern minimal metin editör ikonu oluşturur ve form'a atar.
    /// Dark theme ile uyumlu, doküman şeklinde bir ikon oluşturur.
    /// </summary>
    public static void CreateFormIcon(Form form)
    {
        try
        {
            Bitmap bitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(BackgroundColor);

                using (Pen pen = new Pen(TextColor, 2))
                {
                    // Doküman şekli - arka plan
                    g.FillRectangle(new SolidBrush(MenuBarColor), 4, 4, 20, 24);
                    g.DrawRectangle(pen, 4, 4, 20, 24);
                    // Sol üst köşe katlama efekti
                    g.DrawLine(pen, 4, 12, 12, 12);
                    g.DrawLine(pen, 12, 4, 12, 12);
                    // Metin satırları
                    for (int i = 0; i < 3; i++)
                    {
                        g.DrawLine(pen, 8, 18 + i * 4, 20, 18 + i * 4);
                    }
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            form.Icon = (Icon)icon.Clone();
            icon.Dispose();
            bitmap.Dispose();
        }
        catch
        {
            // Icon yüklenemezse varsayılan ikonu kullan
        }
    }

    /// <summary>
    /// Custom title bar panel'ini oluşturur.
    /// Form'un üst kısmında dock edilmiş, draggable özellikli panel.
    /// </summary>
    public static Panel CreateTitleBarPanel()
    {
        Panel titleBarPanel = new Panel
        {
            BackColor = TitleBarColor,
            Dock = DockStyle.Top,
            Height = TitleBarHeight,
            Location = new Point(0, 0),
            Name = "titleBarPanel"
        };

        return titleBarPanel;
    }

    /// <summary>
    /// Title bar'daki pencere kontrol butonlarını oluşturur.
    /// Minimize, Maximize/Restore ve Close butonları.
    /// </summary>
    /// <returns>Maximize button referansı (güncellemeler için).</returns>
    public static Button CreateTitleBarButtons(Panel titleBarPanel, 
        EventHandler? closeClick, 
        EventHandler? maximizeClick, 
        EventHandler? minimizeClick)
    {
        // Close Button - Kırmızı hover efekti ile
        Button closeButton = new Button
        {
            Text = "✕",
            FlatStyle = FlatStyle.Flat,
            Size = new Size(ButtonSize, TitleBarHeight),
            Dock = DockStyle.Right,
            BackColor = TitleBarColor,
            ForeColor = TextColor,
            Font = new Font(UIFontFamily, TitleBarFontSize, FontStyle.Regular),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        closeButton.FlatAppearance.BorderSize = 0;
        closeButton.FlatAppearance.MouseOverBackColor = CloseButtonHoverColor;
        closeButton.Click += closeClick;
        titleBarPanel.Controls.Add(closeButton);

        // Maximize/Restore Button
        Button maximizeButton = new Button
        {
            Text = "□",
            FlatStyle = FlatStyle.Flat,
            Size = new Size(ButtonSize, TitleBarHeight),
            Dock = DockStyle.Right,
            BackColor = TitleBarColor,
            ForeColor = TextColor,
            Font = new Font(UIFontFamily, TitleBarFontSize, FontStyle.Regular),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        maximizeButton.FlatAppearance.BorderSize = 0;
        maximizeButton.FlatAppearance.MouseOverBackColor = ButtonHoverColor;
        maximizeButton.Click += maximizeClick;
        titleBarPanel.Controls.Add(maximizeButton);

        // Minimize Button
        Button minimizeButton = new Button
        {
            Text = "—",
            FlatStyle = FlatStyle.Flat,
            Size = new Size(ButtonSize, TitleBarHeight),
            Dock = DockStyle.Right,
            BackColor = TitleBarColor,
            ForeColor = TextColor,
            Font = new Font(UIFontFamily, TitleBarFontSize, FontStyle.Regular),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        minimizeButton.FlatAppearance.BorderSize = 0;
        minimizeButton.FlatAppearance.MouseOverBackColor = ButtonHoverColor;
        minimizeButton.Click += minimizeClick;
        titleBarPanel.Controls.Add(minimizeButton);

        return maximizeButton;
    }

    /// <summary>
    /// Dark theme MenuStrip'i oluşturur.
    /// Custom renderer ile modern görünüm uygular.
    /// </summary>
    public static MenuStrip CreateMenuStrip()
    {
        MenuStrip menuStrip = new MenuStrip
        {
            BackColor = MenuBarColor,
            ForeColor = TextColor,
            Font = new Font(UIFontFamily, MenuFontSize, FontStyle.Regular, GraphicsUnit.Point),
            Dock = DockStyle.Top,
            RenderMode = ToolStripRenderMode.Professional
        };

        menuStrip.Renderer = new DarkMenuStripRenderer();

        return menuStrip;
    }

    /// <summary>
    /// Dosya menüsünü oluşturur ve MenuStrip'e ekler.
    /// Yeni, Aç, Kaydet, Farklı Kaydet ve Çıkış menü öğeleri.
    /// </summary>
    public static void CreateFileMenu(MenuStrip menuStrip,
        EventHandler? newClick,
        EventHandler? openClick,
        EventHandler? saveClick,
        EventHandler? saveAsClick,
        EventHandler? exitClick)
    {
        ToolStripMenuItem dosyaMenu = new ToolStripMenuItem("Dosya");
        dosyaMenu.ForeColor = TextColor;
        dosyaMenu.DropDown.BackColor = MenuBarColor;
        dosyaMenu.DropDown.ForeColor = TextColor;
        dosyaMenu.DropDown.Font = new Font(UIFontFamily, MenuFontSize, FontStyle.Regular, GraphicsUnit.Point);

        ToolStripMenuItem yeniMenuItem = new ToolStripMenuItem("Yeni");
        yeniMenuItem.ForeColor = TextColor;
        yeniMenuItem.Click += newClick;
        dosyaMenu.DropDownItems.Add(yeniMenuItem);

        ToolStripMenuItem açMenuItem = new ToolStripMenuItem("Aç");
        açMenuItem.ForeColor = TextColor;
        açMenuItem.Click += openClick;
        dosyaMenu.DropDownItems.Add(açMenuItem);

        ToolStripMenuItem kaydetMenuItem = new ToolStripMenuItem("Kaydet");
        kaydetMenuItem.ForeColor = TextColor;
        kaydetMenuItem.Click += saveClick;
        dosyaMenu.DropDownItems.Add(kaydetMenuItem);

        ToolStripMenuItem farkliKaydetMenuItem = new ToolStripMenuItem("Farklı Kaydet");
        farkliKaydetMenuItem.ForeColor = TextColor;
        farkliKaydetMenuItem.Click += saveAsClick;
        dosyaMenu.DropDownItems.Add(farkliKaydetMenuItem);

        dosyaMenu.DropDownItems.Add(new ToolStripSeparator());

        ToolStripMenuItem çikisMenuItem = new ToolStripMenuItem("Çıkış");
        çikisMenuItem.ForeColor = TextColor;
        çikisMenuItem.Click += exitClick;
        dosyaMenu.DropDownItems.Add(çikisMenuItem);

        menuStrip.Items.Add(dosyaMenu);
    }

    /// <summary>
    /// Düzen menüsünü oluşturur ve MenuStrip'e ekler.
    /// Geri Al, Kes, Kopyala, Yapıştır ve Tümünü Seç menü öğeleri.
    /// Klavye kısayolları ile birlikte.
    /// </summary>
    public static void CreateEditMenu(MenuStrip menuStrip,
        EventHandler? undoClick,
        EventHandler? cutClick,
        EventHandler? copyClick,
        EventHandler? pasteClick,
        EventHandler? selectAllClick)
    {
        ToolStripMenuItem düzenMenu = new ToolStripMenuItem("Düzen");
        düzenMenu.ForeColor = TextColor;
        düzenMenu.DropDown.BackColor = MenuBarColor;
        düzenMenu.DropDown.ForeColor = TextColor;
        düzenMenu.DropDown.Font = new Font(UIFontFamily, MenuFontSize, FontStyle.Regular, GraphicsUnit.Point);

        ToolStripMenuItem geriAlMenuItem = new ToolStripMenuItem("Geri Al");
        geriAlMenuItem.ForeColor = TextColor;
        geriAlMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
        geriAlMenuItem.ShowShortcutKeys = true;
        geriAlMenuItem.Click += undoClick;
        düzenMenu.DropDownItems.Add(geriAlMenuItem);

        düzenMenu.DropDownItems.Add(new ToolStripSeparator());

        ToolStripMenuItem kesMenuItem = new ToolStripMenuItem("Kes");
        kesMenuItem.ForeColor = TextColor;
        kesMenuItem.ShortcutKeys = Keys.Control | Keys.X;
        kesMenuItem.ShowShortcutKeys = true;
        kesMenuItem.Click += cutClick;
        düzenMenu.DropDownItems.Add(kesMenuItem);

        ToolStripMenuItem kopyalaMenuItem = new ToolStripMenuItem("Kopyala");
        kopyalaMenuItem.ForeColor = TextColor;
        kopyalaMenuItem.ShortcutKeys = Keys.Control | Keys.C;
        kopyalaMenuItem.ShowShortcutKeys = true;
        kopyalaMenuItem.Click += copyClick;
        düzenMenu.DropDownItems.Add(kopyalaMenuItem);

        ToolStripMenuItem yapistirMenuItem = new ToolStripMenuItem("Yapıştır");
        yapistirMenuItem.ForeColor = TextColor;
        yapistirMenuItem.ShortcutKeys = Keys.Control | Keys.V;
        yapistirMenuItem.ShowShortcutKeys = true;
        yapistirMenuItem.Click += pasteClick;
        düzenMenu.DropDownItems.Add(yapistirMenuItem);

        düzenMenu.DropDownItems.Add(new ToolStripSeparator());

        ToolStripMenuItem tumunuSecMenuItem = new ToolStripMenuItem("Tümünü Seç");
        tumunuSecMenuItem.ForeColor = TextColor;
        tumunuSecMenuItem.ShortcutKeys = Keys.Control | Keys.A;
        tumunuSecMenuItem.ShowShortcutKeys = true;
        tumunuSecMenuItem.Click += selectAllClick;
        düzenMenu.DropDownItems.Add(tumunuSecMenuItem);

        menuStrip.Items.Add(düzenMenu);
    }

    /// <summary>
    /// Ana TextBox'ı oluşturur.
    /// Multiline, borderless, dark theme ile yapılandırılmış metin editörü.
    /// </summary>
    public static TextBox CreateMainTextBox()
    {
        TextBox textBox = new TextBox
        {
            Multiline = true,
            BorderStyle = BorderStyle.None,
            Dock = DockStyle.Fill,
            Font = new Font(TextFontFamily, DefaultFontSize, FontStyle.Regular, GraphicsUnit.Point),
            BackColor = BackgroundColor,
            ForeColor = TextColor,
            ScrollBars = ScrollBars.Both,
            AcceptsTab = true,
            AcceptsReturn = true,
            ShortcutsEnabled = true
        };

        return textBox;
    }

    /// <summary>
    /// Status bar'ı oluşturur.
    /// Form'un alt kısmında satır ve sütun bilgilerini gösterir.
    /// </summary>
    public static (StatusStrip statusStrip, ToolStripStatusLabel statusLabel) CreateStatusBar()
    {
        StatusStrip statusStrip = new StatusStrip
        {
            BackColor = TitleBarColor,
            ForeColor = TextColor,
            Dock = DockStyle.Bottom,
            Height = StatusBarHeight
        };

        ToolStripStatusLabel statusLabel = new ToolStripStatusLabel
        {
            Text = "Satır: 1, Sütun: 1",
            ForeColor = TextColor,
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft
        };

        statusStrip.Items.Add(statusLabel);

        return (statusStrip, statusLabel);
    }

    /// <summary>
    /// Form köşelerine yuvarlak kenar efekti uygular.
    /// Maximize durumunda efekt devre dışı bırakılır.
    /// </summary>
    public static void ApplyRoundedCorners(Form form, int cornerRadius = CornerRadius)
    {
        if (form.WindowState == FormWindowState.Maximized)
        {
            form.Region = null;
            return;
        }

        GraphicsPath path = new GraphicsPath();
        Rectangle rect = new Rectangle(0, 0, form.Width, form.Height);

        // Sol üst köşe
        path.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
        // Sağ üst köşe
        path.AddArc(rect.Right - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
        // Sağ alt köşe
        path.AddArc(rect.Right - cornerRadius * 2, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
        // Sol alt köşe
        path.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        path.CloseAllFigures();

        form.Region = new Region(path);
    }

    /// <summary>
    /// Dark theme için custom MenuStrip renderer.
    /// Hover efektleri ve separator'ları dark tema ile render eder.
    /// </summary>
    private class DarkMenuStripRenderer : ToolStripProfessionalRenderer
    {
        private readonly Color hoverBackColor = Color.FromArgb(0x4A, 0x4A, 0x4A);
        private readonly Color separatorColor = Color.FromArgb(0x5A, 0x5A, 0x5A);

        public DarkMenuStripRenderer() : base(new DarkColorTable())
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
            {
                base.OnRenderMenuItemBackground(e);
            }
            else
            {
                Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(new SolidBrush(hoverBackColor), rect);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
            using (Pen pen = new Pen(separatorColor))
            {
                e.Graphics.DrawLine(pen, rect.Left + 20, rect.Height / 2, rect.Right - 5, rect.Height / 2);
            }
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color MenuItemBorder => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color MenuBorder => Color.FromArgb(0x2A, 0x2A, 0x2A);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(0x4A, 0x4A, 0x4A);
            public override Color ToolStripDropDownBackground => Color.FromArgb(0x3A, 0x3A, 0x3A);
        }
    }
}

