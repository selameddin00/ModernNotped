# Modern Notepad

Modern, minimal ve dark theme ile tasarlanmış bir metin editörü uygulaması. .NET 8.0 WinForms kullanılarak geliştirilmiştir.

## Öğrenci Bilgileri

**Ad/Soyad:** Selameddin Tirit  
**Öğrenci No:** 240541035  
**Bölüm/Fakülte/Şube:** Yazılım Mühendisliği - Teknoloji Fakültesi - A

## Özellikler

- 🎨 **Modern Dark Theme UI** - Minimal ve göz yormayan koyu tema tasarımı
- 📝 **Dosya İşlemleri** - UTF-8 encoding ile dosya açma/kaydetme (.txt, .cs dosyaları)
- ⌨️ **Klavye Kısayolları** - CTRL+Z, X, C, V, A gibi standart kısayollar
- 🔍 **Font Kontrolü** - CTRL + MouseWheel ile yazı boyutu değiştirme (8pt - 72pt)
- 📊 **Status Bar** - Gerçek zamanlı satır ve sütun bilgisi
- 🪟 **Custom Title Bar** - Yuvarlak köşeler, draggable, minimize/maximize/close butonları
- 💾 **Kaydedilmemiş Değişiklik Uyarıları** - Form kapanırken veya yeni dosya açarken uyarı

## Ekran Görüntüleri

![Modern Notepad Screenshot](screenshot.png)

## Gereksinimler

- .NET 8.0 Runtime veya SDK
- Windows işletim sistemi

## Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/kullaniciadi/ModernNotepad.git
cd ModernNotepad
```

2. Projeyi derleyin:
```bash
cd ModernNotepad
dotnet build
```

3. Uygulamayı çalıştırın:
```bash
dotnet run
```

## Kullanım

### Dosya İşlemleri
- **Yeni**: Dosya menüsünden "Yeni" veya CTRL+N (yakında)
- **Aç**: Dosya → Aç veya CTRL+O (yakında)
- **Kaydet**: Dosya → Kaydet veya CTRL+S (yakında)
- **Farklı Kaydet**: Dosya → Farklı Kaydet

### Düzen İşlemleri
- **Geri Al**: CTRL+Z
- **Kes**: CTRL+X
- **Kopyala**: CTRL+C
- **Yapıştır**: CTRL+V
- **Tümünü Seç**: CTRL+A

### Yazı Boyutu
- **Büyüt**: CTRL + Mouse Wheel Yukarı
- **Küçült**: CTRL + Mouse Wheel Aşağı

## Proje Yapısı

```
ModernNotepad/
├── Form1.cs              # Ana form ve event handler'lar
├── Form1.Designer.cs     # Designer kodları
├── FileManager.cs        # Dosya işlemleri yönetimi
├── UIController.cs       # UI oluşturma ve yönetimi
├── Program.cs            # Uygulama entry point
└── ModernNotepad.csproj  # Proje dosyası
```

## Teknolojiler

- .NET 8.0
- Windows Forms (WinForms)
- C# 12 (file-scoped namespaces, nullable reference types)

## Lisans

MIT License

## Katkıda Bulunma

Pull request'ler kabul edilir. Büyük değişiklikler için önce bir issue açarak neyi değiştirmek istediğinizi tartışın.

## İletişim

Sorularınız için issue açabilirsiniz.

