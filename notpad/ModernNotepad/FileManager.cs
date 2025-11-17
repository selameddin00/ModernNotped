using System.Text;

namespace ModernNotepad;

/// <summary>
/// Dosya işlemlerini yöneten sınıf.
/// UTF-8 encoding ile dosya okuma/yazma işlemlerini gerçekleştirir.
/// </summary>
public class FileManager
{
    private const string FileFilter = "Metin Dosyaları (*.txt)|*.txt|C# Dosyaları (*.cs)|*.cs|Tüm Dosyalar (*.*)|*.*";
    private const int DefaultFilterIndex = 1;

    /// <summary>
    /// Dosya açma dialog'unu gösterir ve seçilen dosyayı UTF-8 encoding ile okur.
    /// </summary>
    /// <returns>Dosya yolu ve içeriği içeren tuple. Kullanıcı iptal ederse (null, null) döner.</returns>
    public static (string? filePath, string? content) OpenFile()
    {
        try
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = FileFilter;
                openFileDialog.FilterIndex = DefaultFilterIndex;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        return (openFileDialog.FileName, content);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Dosya açılamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return (null, null);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return (null, null);
    }

    /// <summary>
    /// Dosya kaydetme dialog'unu gösterir ve içeriği UTF-8 encoding ile kaydeder.
    /// </summary>
    /// <param name="content">Kaydedilecek içerik.</param>
    /// <returns>Kaydedilen dosya yolu. Kullanıcı iptal ederse null döner.</returns>
    public static string? SaveFileAs(string content)
    {
        try
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = FileFilter;
                saveFileDialog.FilterIndex = DefaultFilterIndex;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, content, Encoding.UTF8);
                        return saveFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Dosya kaydedilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return null;
    }

    /// <summary>
    /// Mevcut dosyaya içeriği UTF-8 encoding ile kaydeder.
    /// </summary>
    /// <param name="filePath">Kaydedilecek dosya yolu.</param>
    /// <param name="content">Kaydedilecek içerik.</param>
    /// <returns>İşlem başarılı ise true, aksi halde false.</returns>
    public static bool SaveFile(string filePath, string content)
    {
        try
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Dosya kaydedilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    /// <summary>
    /// Kaydedilmemiş değişiklikler için kullanıcıya onay mesajı gösterir.
    /// </summary>
    /// <param name="message">Gösterilecek mesaj.</param>
    /// <returns>Kullanıcının seçimi: Yes, No veya Cancel.</returns>
    public static DialogResult ShowUnsavedChangesDialog(string message = "Kaydedilmemiş değişiklikler var. Kaydetmek ister misiniz?")
    {
        return MessageBox.Show(
            message,
            "Modern Notepad",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
    }
}

