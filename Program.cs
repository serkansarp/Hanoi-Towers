/*
  HANOI KULELERİ - CONSOLE UYGULAMASI
  Serkan SARP @ 2025
  www.serkansarp.com
  www.github.com/serkansarp
  www.linkedin.com/in/serkansarp
*/

// Ekranı sil, değişkenleri tanımla
Console.Clear();

int kaynak;
int hedef;
int alinanDisk;
int adim = 0;
int diskSayisi = 5;


// Kuleleri tanımlayıp birleştir
Stack<int> kule1 = new Stack<int>();
Stack<int> kule2 = new Stack<int>();
Stack<int> kule3 = new Stack<int>();
Stack<int>[] kuleler = { kule1, kule2, kule3 };


// Giriş ekranı
Console.Write("\n   \u250C");
Console.Write(new string('\u2500', 40));
Console.WriteLine("\u2510");
Console.WriteLine("   \u2502  HANOI KULELERİ by Serkan SARP @ 2025  \u2502");
Console.Write("   \u2514");
Console.Write(new string('\u2500', 40));
Console.WriteLine("\u2518\n");
Console.WriteLine("- Oyun, 3 kule ve 3-7 arası diskten oluşur.");
Console.WriteLine("- Amaç, sol kuledeki diskleri en sağdakine taşımaktır.");
Console.WriteLine("- Alınan disk, boş bir kuleye taşınabilir,");
Console.WriteLine("  konulacak kulede en üstte, kendisinden büyük bir disk olmalıdır.");
Console.WriteLine("- Alınacak ve taşınacak kule numaraları klavyeden girilerek oynanır.");
Console.WriteLine("- Oyunun herhangi bir yerinde 9 ile çıkılabilir.");
Console.WriteLine("- Disk sayısını belirlemek için 3-7 arasında bir sayı girerek");
Console.WriteLine("  başlayabilirsiniz. Herhangi başka bir tuş, oyunu 5 disk ile ");
Console.Write("  başlatır. Disk sayısı belirlemek istiyorsanız lütfen girin: ");


// Disk sayısını al, 3-7 dışındaki değerler için baştaki default 5 disk ile başlat
ConsoleKeyInfo diskKeyInfo = Console.ReadKey(true);

if (char.IsDigit(diskKeyInfo.KeyChar))
{
    int sayi = diskKeyInfo.KeyChar - '0'; // Ascii değerlerini birbirinden çıkararak int oluşturuyoruz ('0'=48)
    if (sayi >= 3 && sayi <= 7)
    {
        diskSayisi = sayi;
        Console.Write($"{sayi} disk!");
    }
    else
    {
        Console.WriteLine("\n  - Geçersiz giriş! " + diskSayisi + " disk ile başlatılıyor. -");
        Thread.Sleep(1000); // Hata mesajının okunabilmesi için ek süre (dış else'te de var)
    }
}
else
{
    Console.WriteLine("\n  - Geçersiz giriş! " + diskSayisi + " disk ile başlatılıyor. -");
    Thread.Sleep(1000);
}


Thread.Sleep(1000); // Girilen değerde hata yoksa sadece bu çalışır, yoksa üstteki sleep+bu sleep şeklinde çalışır.

// Kule1'e diskleri ekle (Alttan yukarıya doğru büyükten küçüğe) ve ekranı temizliyoruz.
for (int i = diskSayisi; i > 0; i--)
{
    kule1.Push(i);
}


Console.Clear();


// Oyun döngüsüne girmeden önce kulelerimizi yazdıralım.
KuleleriYazdir();


// Oyun döngüsü
while (true)
{

    // Kuleden disk alma kontrolü
    do
    {
        // Çıkış tuşu kontrolü
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        kaynak = keyInfo.KeyChar - '0';
        if (kaynak == 9)
        {
            Console.WriteLine("Çıkış yapılıyor!");
            Environment.Exit(0);
        }
    }
    while (!KaynakKontrol(kaynak, kuleler));


    // Şimdi diski al
    alinanDisk = kuleler[kaynak - 1].Pop();
    Console.Write(kaynak + ". kuleden alınan disk, ");


    // Başka bir kuleye koy (yine çıkış tuşu kontrolü ekledim)
    do
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        hedef = keyInfo.KeyChar - '0';
        if (hedef == 9)
        {
            Console.WriteLine("Çıkış yapılıyor!");
            Environment.Exit(0);
        }
    }
    while (!HedefKontrol(hedef, kuleler));

    kuleler[hedef - 1].Push(alinanDisk);
    Console.Write(hedef + ". kuleye taşındı.\n");

    // Gizli adım sayacımız, her disk taşıma (yani alma ve koyma) bir adım. Oyun sonunda minimum adım sayısıyla birlikte göstereceğiz.
    adim++;
    Thread.Sleep(500);  // Bu beklemeyi koymayınca, oyuncu özgüven kazanıp hızlı ilerleyebiliyor, yanlış hamlelere gidebiliyor.

    Console.Clear();

    KuleleriYazdir();

    // Oyun bitiş kontrolü ve tebrik mesajı + minimum bilgilendirmesi
    if (kule3.Count == diskSayisi)
    {

        Console.WriteLine("\n- Tebrikler! " + adim + " adımda tamamladınız.");
        if (adim > Math.Pow(2, diskSayisi) - 1)
        {
            Console.WriteLine("- Sonraki oyunda " + (Math.Pow(2, diskSayisi) - 1) + " adımda bitirmeyi deneyin!");
        }
        Console.WriteLine("- Çıkmak için herhangi bir tuşa basın.");
        Console.ReadKey(true);
        Environment.Exit(0);
    }

}

// KULLANDIĞIM METOTLAR - Sırasıyla
// - Kaynak kule kontrolü
// - Hedef kule kontrolü
// - Kuleleri yazdır
// - Diskleri yazdır

// Kaynak kule kontrolü
bool KaynakKontrol(int kaynak, Stack<int>[] kuleler)
{

    // Kule numarası geçerli mi?
    if (kaynak < 1 || kaynak > 3)
    {
        Console.WriteLine("Geçersiz kule!");
        return false;
    }

    // Kule boş mu?
    if (kuleler[kaynak - 1].Count == 0)
    {
        Console.WriteLine($"{kaynak}. kule boş!");
        return false;
    }

    return true;
}

// Hedef kule kontrolü
bool HedefKontrol(int hedef, Stack<int>[] kuleler)
{

    // Kule numarası geçerli mi?
    if (hedef < 1 || hedef > 3)
    {
        Console.WriteLine("Geçersiz kule!");
        return false;
    }

    // Konulacak kulede en üstteki eleman uygun veya kule boş mu?
    if (kuleler[hedef - 1].Count != 0 && kuleler[hedef - 1].Peek() < alinanDisk)
    {
        Console.WriteLine($"{hedef}. kule uygun değil, yeni hedef girin.");
        return false;
    }
    return true;
}


// Kuleleri yazdır
void KuleleriYazdir()
{
    Stack<int> kule1Kopya = new Stack<int>(kule1.Reverse());
    while (kule1Kopya.Count < diskSayisi)
        kule1Kopya.Push(0);

    Stack<int> kule2Kopya = new Stack<int>(kule2.Reverse());
    while (kule2Kopya.Count < diskSayisi)
        kule2Kopya.Push(0);

    Stack<int> kule3Kopya = new Stack<int>(kule3.Reverse());
    while (kule3Kopya.Count < diskSayisi)
        kule3Kopya.Push(0);
    
    Console.Write("\n   \u250C");
    Console.Write(new string('\u2500', 40));
    Console.WriteLine("\u2510");
    Console.WriteLine("   \u2502  HANOI KULELERİ by Serkan SARP @ 2025  \u2502");
    Console.Write("   \u2514");
    Console.Write(new string('\u2500', 40));
    Console.WriteLine("\u2518\n");


    Console.Write("       \u2502               \u2502               \u2502\n");

    for (int i = 0; i < diskSayisi; i++)
    {
        Console.Write(diskYazdir(kule1Kopya.Pop()) + " " + diskYazdir(kule2Kopya.Pop()) + " " + diskYazdir(kule3Kopya.Pop()) + "\n");
    }

    Console.Write(new string('\u2500', 15));
    Console.Write(" ");
    Console.Write(new string('\u2500', 15));
    Console.Write(" ");
    Console.Write(new string('\u2500', 15));
    Console.WriteLine();

    Console.Write(new string(' ', 7));
    Console.Write("1");
    Console.Write(new string(' ', 15));
    Console.Write("2");
    Console.Write(new string(' ', 15));
    Console.Write("3\n\n");
}

// Diskleri yazdır (Karışık if-else if yerine switch-case ile ve utf karakterleri kullanarak yazdım)
string diskYazdir(int diskGenisligi)
{
    switch (diskGenisligi)
    {
        case 0:
            return "       \u2502       ";
        case 1:
            return "      \u250C\u2500\u2510      ";
        case 2:
            return "     \u250C\u2500\u2500\u2500\u2510     ";
        case 3:
            return "    \u250C\u2500\u2500\u2500\u2500\u2500\u2510    ";
        case 4:
            return "   \u250C\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510   ";
        case 5:
            return "  \u250C\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510  ";
        case 6:
            return " \u250C\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510 ";
        case 7:
            return "\u250C\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510";
        default:
            return "Hata!";

    }
}

// Kodumu incelediğiniz için teşekkürler! Elimden geldiğince yorumlarla destekleyip, olabildiğince temiz yazmaya çalıştım.
// Diğer projelerim için github.com/serkansarp adresini ziyaret edebilirsiniz. İyi kodlamalar!