using System.Drawing;
using System.Runtime.InteropServices;

namespace ModernNotepad;

/// <summary>
/// Modern Notepad uygulamasının ana form sınıfı.
/// UI Controller ve File Manager ile koordinasyon sağlar.
/// </summary>
public partial class Form1 : Form
{
    // Sabitler
    private const int CornerRadius = 8;
    private const float MinFontSize = 8F;
    private const float MaxFontSize = 72F;

    // Durum değişkenleri
    private bool _isMaximized = false;
    private string? currentFilePath = null;
    private string? savedContent = null;
    private Button? maximizeButton = null;

    // Windows API importları - Form drag işlemi için
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    public Form1()
    {
        InitializeComponent();
        
        // UI oluşturma
        UIController.ConfigureForm(this);
        UIController.CreateFormIcon(this);
        CreateUI();
        UIController.ApplyRoundedCorners(this);
        
        // Event bağlantıları - düzenli sıralama
        AttachFormEvents();
        AttachTextBoxEvents();
    }

    /// <summary>
    /// Tüm UI elementlerini oluşturur ve form'a ekler.
    /// UIController sınıfı yardımıyla modern arayüz oluşturulur.
    /// </summary>
    private void CreateUI()
    {
        // Title Bar Panel
        titleBarPanel = UIController.CreateTitleBarPanel();
        maximizeButton = UIController.CreateTitleBarButtons(
            titleBarPanel,
            CloseButton_Click,
            (s, e) => 
            {
                if (maximizeButton != null)
                    MaximizeButton_Click(maximizeButton, e);
            },
            MinimizeButton_Click);
        this.Controls.Add(titleBarPanel);

        // MenuStrip
        menuStrip = UIController.CreateMenuStrip();
        UIController.CreateFileMenu(
            menuStrip,
            YeniMenuItem_Click,
            AçMenuItem_Click,
            KaydetMenuItem_Click,
            FarkliKaydetMenuItem_Click,
            ÇikisMenuItem_Click);
        UIController.CreateEditMenu(
            menuStrip,
            GeriAlMenuItem_Click,
            KesMenuItem_Click,
            KopyalaMenuItem_Click,
            YapistirMenuItem_Click,
            TumunuSecMenuItem_Click);
        this.Controls.Add(menuStrip);
        this.MainMenuStrip = menuStrip;

        // Main TextBox
        mainTextBox = UIController.CreateMainTextBox();
        this.Controls.Add(mainTextBox);

        // StatusStrip
        (statusStrip, statusLabel) = UIController.CreateStatusBar();
        this.Controls.Add(statusStrip);

        // Z-order düzenleme
        mainTextBox.BringToFront();
    }

    /// <summary>
    /// Form seviyesindeki event'leri bağlar.
    /// Resize, Closing gibi form yaşam döngüsü event'leri.
    /// </summary>
    private void AttachFormEvents()
    {
        this.FormClosing += Form1_FormClosing;
        this.Resize += Form1_Resize;
        this.ResizeEnd += Form1_ResizeEnd;
        titleBarPanel.MouseDown += TitleBarPanel_MouseDown;
    }

    /// <summary>
    /// TextBox seviyesindeki event'leri bağlar.
    /// Text değişikliği, cursor hareketi ve mouse wheel event'leri.
    /// </summary>
    private void AttachTextBoxEvents()
    {
        mainTextBox.TextChanged += MainTextBox_TextChanged;
        mainTextBox.KeyUp += MainTextBox_KeyUp;
        mainTextBox.MouseUp += MainTextBox_MouseUp;
        mainTextBox.MouseWheel += MainTextBox_MouseWheel;
    }

    #region Form Events

    /// <summary>
    /// Form boyutlandırma event'i. Yuvarlak köşeleri günceller.
    /// </summary>
    private void Form1_Resize(object? sender, EventArgs e)
    {
        UIController.ApplyRoundedCorners(this);
    }

    /// <summary>
    /// Form boyutlandırma bitiş event'i. Yuvarlak köşeleri günceller.
    /// </summary>
    private void Form1_ResizeEnd(object? sender, EventArgs e)
    {
        UIController.ApplyRoundedCorners(this);
    }

    /// <summary>
    /// Form kapanırken kaydedilmemiş değişiklikleri kontrol eder.
    /// Kullanıcıya kaydetme seçeneği sunar.
    /// </summary>
    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (HasUnsavedChanges())
        {
            var result = FileManager.ShowUnsavedChangesDialog(
                "Kaydedilmemiş değişiklikler var. Kapatmadan önce kaydetmek ister misiniz?");

            if (result == DialogResult.Yes)
            {
                try
                {
                    KaydetMenuItem_Click(sender, e);
                    if (HasUnsavedChanges())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                catch
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
        }
    }

    /// <summary>
    /// Title bar'dan form'u sürüklemek için Windows API kullanır.
    /// </summary>
    private void TitleBarPanel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x2, 0);
        }
    }

    #endregion

    #region Title Bar Button Events

    /// <summary>
    /// Close butonuna tıklama event'i. Form'u kapatır.
    /// </summary>
    private void CloseButton_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    /// <summary>
    /// Maximize/Restore butonuna tıklama event'i.
    /// Pencere durumunu maximize ve normal arasında değiştirir.
    /// </summary>
    private void MaximizeButton_Click(Button btn, EventArgs e)
    {
        if (_isMaximized)
        {
            this.WindowState = FormWindowState.Normal;
            _isMaximized = false;
            btn.Text = "□";
        }
        else
        {
            this.WindowState = FormWindowState.Maximized;
            _isMaximized = true;
            btn.Text = "❐";
        }
        Application.DoEvents();
        UIController.ApplyRoundedCorners(this);
    }

    /// <summary>
    /// Minimize butonuna tıklama event'i. Form'u minimize eder.
    /// </summary>
    private void MinimizeButton_Click(object? sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
    }


    #endregion

    #region File Menu Events

    /// <summary>
    /// Yeni menü öğesi event'i. TextBox'ı temizler ve dosya yolunu sıfırlar.
    /// Kaydedilmemiş değişiklikler varsa uyarı gösterir.
    /// </summary>
    private void YeniMenuItem_Click(object? sender, EventArgs e)
    {
        if (!CheckUnsavedChanges("Kaydedilmemiş değişiklikler var. Kaydetmek ister misiniz?"))
            return;

        mainTextBox.Clear();
        currentFilePath = null;
        savedContent = null;
        UpdateFormTitle();
        UpdateStatusBar();
    }

    /// <summary>
    /// Aç menü öğesi event'i. Dosya açma dialog'unu gösterir ve seçilen dosyayı yükler.
    /// Kaydedilmemiş değişiklikler varsa uyarı gösterir.
    /// </summary>
    private void AçMenuItem_Click(object? sender, EventArgs e)
    {
        if (!CheckUnsavedChanges("Kaydedilmemiş değişiklikler var. Kaydetmek ister misiniz?"))
            return;

        var (filePath, content) = FileManager.OpenFile();
        if (filePath != null && content != null)
        {
            mainTextBox.Text = content;
            currentFilePath = filePath;
            savedContent = content;
            UpdateFormTitle();
            UpdateStatusBar();
        }
    }

    /// <summary>
    /// Kaydet menü öğesi event'i. Mevcut dosyaya kaydeder veya yeni dosya dialog'u açar.
    /// </summary>
    private void KaydetMenuItem_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            FarkliKaydetMenuItem_Click(sender, e);
        }
        else
        {
            if (FileManager.SaveFile(currentFilePath, mainTextBox.Text))
            {
                savedContent = mainTextBox.Text;
                UpdateFormTitle();
            }
        }
    }

    /// <summary>
    /// Farklı Kaydet menü öğesi event'i. Dosya kaydetme dialog'unu gösterir.
    /// </summary>
    private void FarkliKaydetMenuItem_Click(object? sender, EventArgs e)
    {
        string? filePath = FileManager.SaveFileAs(mainTextBox.Text);
        if (filePath != null)
        {
            currentFilePath = filePath;
            savedContent = mainTextBox.Text;
            UpdateFormTitle();
        }
    }

    /// <summary>
    /// Çıkış menü öğesi event'i. Uygulamayı kapatır.
    /// </summary>
    private void ÇikisMenuItem_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    #endregion

    #region Edit Menu Events

    /// <summary>
    /// Geri Al menü öğesi event'i. TextBox'ta son işlemi geri alır.
    /// </summary>
    private void GeriAlMenuItem_Click(object? sender, EventArgs e)
    {
        mainTextBox.Undo();
    }

    /// <summary>
    /// Kes menü öğesi event'i. Seçili metni keser.
    /// </summary>
    private void KesMenuItem_Click(object? sender, EventArgs e)
    {
        mainTextBox.Cut();
    }

    /// <summary>
    /// Kopyala menü öğesi event'i. Seçili metni kopyalar.
    /// </summary>
    private void KopyalaMenuItem_Click(object? sender, EventArgs e)
    {
        mainTextBox.Copy();
    }

    /// <summary>
    /// Yapıştır menü öğesi event'i. Panodan metni yapıştırır.
    /// </summary>
    private void YapistirMenuItem_Click(object? sender, EventArgs e)
    {
        mainTextBox.Paste();
    }

    /// <summary>
    /// Tümünü Seç menü öğesi event'i. TextBox'taki tüm metni seçer.
    /// </summary>
    private void TumunuSecMenuItem_Click(object? sender, EventArgs e)
    {
        mainTextBox.SelectAll();
    }

    #endregion

    #region TextBox Events

    /// <summary>
    /// TextBox metin değişikliği event'i. Status bar'ı günceller.
    /// </summary>
    private void MainTextBox_TextChanged(object? sender, EventArgs e)
    {
        UpdateStatusBar();
    }

    /// <summary>
    /// TextBox tuş bırakma event'i. Status bar'ı günceller.
    /// </summary>
    private void MainTextBox_KeyUp(object? sender, KeyEventArgs e)
    {
        UpdateStatusBar();
    }

    /// <summary>
    /// TextBox mouse bırakma event'i. Status bar'ı günceller.
    /// </summary>
    private void MainTextBox_MouseUp(object? sender, MouseEventArgs e)
    {
        UpdateStatusBar();
    }

    /// <summary>
    /// TextBox mouse wheel event'i. CTRL tuşu basılıyken font boyutunu değiştirir.
    /// Scroll up: Font büyüt, Scroll down: Font küçült.
    /// </summary>
    private void MainTextBox_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (Control.ModifierKeys == Keys.Control)
        {
            float newSize = mainTextBox.Font.Size;
            if (e.Delta > 0)
            {
                newSize = Math.Min(newSize + 1F, MaxFontSize);
            }
            else
            {
                newSize = Math.Max(newSize - 1F, MinFontSize);
            }

            if (newSize != mainTextBox.Font.Size)
            {
                mainTextBox.Font = new Font(mainTextBox.Font.FontFamily, newSize, mainTextBox.Font.Style, GraphicsUnit.Point);
            }
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Kaydedilmemiş değişiklik olup olmadığını kontrol eder.
    /// </summary>
    /// <returns>Değişiklik varsa true, aksi halde false.</returns>
    private bool HasUnsavedChanges()
    {
        return mainTextBox.Text != savedContent;
    }

    /// <summary>
    /// Kaydedilmemiş değişiklikleri kontrol eder ve kullanıcıya seçenek sunar.
    /// </summary>
    /// <param name="message">Gösterilecek mesaj.</param>
    /// <returns>İşleme devam edilebilirse true, iptal edilirse false.</returns>
    private bool CheckUnsavedChanges(string message)
    {
        if (HasUnsavedChanges())
        {
            var result = FileManager.ShowUnsavedChangesDialog(message);

            if (result == DialogResult.Yes)
            {
                KaydetMenuItem_Click(this, EventArgs.Empty);
                if (HasUnsavedChanges())
                    return false;
            }
            else if (result == DialogResult.Cancel)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Form başlığını günceller. Açık dosya varsa dosya adını gösterir.
    /// </summary>
    private void UpdateFormTitle()
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            this.Text = "Modern Notepad";
        }
        else
        {
            string fileName = Path.GetFileName(currentFilePath);
            this.Text = $"Modern Notepad - {fileName}";
        }
    }

    /// <summary>
    /// Status bar'daki satır ve sütun bilgilerini günceller.
    /// </summary>
    private void UpdateStatusBar()
    {
        if (statusLabel == null || mainTextBox == null)
            return;

        int line = mainTextBox.GetLineFromCharIndex(mainTextBox.SelectionStart) + 1;
        int column = mainTextBox.SelectionStart - mainTextBox.GetFirstCharIndexOfCurrentLine() + 1;

        statusLabel.Text = $"Satır: {line}, Sütun: {column}";
    }

    #endregion
}
