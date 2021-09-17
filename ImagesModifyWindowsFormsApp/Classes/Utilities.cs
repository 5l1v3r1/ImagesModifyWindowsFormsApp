using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

////// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
////Bir ile çalışıyorsanız BitmapSource:

////var resizedBitmap = new TransformedBitmap(
////    bitmapSource,
////    new ScaleTransform(scaleX, scaleY));
////Kalite üzerinde daha iyi kontrol istiyorsanız, önce şunu çalıştırın:

////RenderOptions.SetBitmapScalingMode(
////    bitmapSource,
////    BitmapScalingMode.HighQuality);
////(Varsayılan, BitmapScalingMode.Lineareşdeğer olandır BitmapScalingMode.LowQuality.)

namespace ImagesModifyWindowsFormsApp.Classes
{
    public class Utilities
    {
        Dictionary<int, string[]> keyValuePairs = new Dictionary<int, string[]>();
        List<FilesCounts> Filess = new List<FilesCounts>();
        List<int> Index_t = new List<int>();
        public int imageWidth { get; set; }
        public int imageHeight { get; set; }
        public Point GetImageSize()
        {
            string[] strDizi = Directory.GetFiles(@"C:\Images\source\0");
            FileInfo file = new FileInfo(strDizi[0]);
            Bitmap img = new Bitmap(strDizi[0]);
            imageHeight = img.Height;
            imageWidth = img.Width;

            Point point = new Point(imageWidth, imageHeight);
            return point;
        }

        public Point GetImagePoint(string strImage)
        {
            //string[] strDizi = Directory.GetFiles(@"C:\Images\source\0");
            //FileInfo file = new FileInfo(strFile);
            Bitmap img = new Bitmap(strImage);
            imageHeight = img.Height;
            imageWidth = img.Width;

            Point point = new Point(imageWidth, imageHeight);
            return point;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Image ResizeImage(Image source, RectangleF destinationBounds)
        {
            RectangleF sourceBounds = new RectangleF(0.0f, 0.0f, (float)source.Width, (float)source.Height);
            RectangleF scaleBounds = new RectangleF();

            Image destinationImage = new Bitmap((int)destinationBounds.Width, (int)destinationBounds.Height);
            Graphics graph = Graphics.FromImage(destinationImage);
            graph.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Fill with background color
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), destinationBounds);

            float resizeRatio, sourceRatio;
            float scaleWidth, scaleHeight;

            sourceRatio = (float)source.Width / (float)source.Height;

            if (sourceRatio >= 1.0f)
            {
                //landscape
                resizeRatio = destinationBounds.Width / sourceBounds.Width;
                scaleWidth = destinationBounds.Width;
                scaleHeight = sourceBounds.Height * resizeRatio;
                float trimValue = destinationBounds.Height - scaleHeight;
                graph.DrawImage(source, 0, (trimValue / 2), destinationBounds.Width, scaleHeight);
            }
            else
            {
                //portrait
                resizeRatio = destinationBounds.Height / sourceBounds.Height;
                scaleWidth = sourceBounds.Width * resizeRatio;
                scaleHeight = destinationBounds.Height;
                float trimValue = destinationBounds.Width - scaleWidth;
                graph.DrawImage(source, (trimValue / 2), 0, scaleWidth, destinationBounds.Height);
            }

            return destinationImage;

        }


        public string AddImageAndSave(string sourceFile, string fileNumberPath2, string resultName, int j, int i)
        {
            CreateImage(fileNumberPath2, sourceFile);
            return fileNumberPath2;
        }

        public void CreateImage(string path, string sourceFile)
        {
            using (Bitmap sourceBmp = new Bitmap(sourceFile))
            {
                using (Graphics gr = Graphics.FromImage(sourceBmp))
                {
                    gr.DrawImage(sourceBmp, 0, 0);
                }
                sourceBmp.Save(path, ImageFormat.Png);
            }
        }

        public void Draw()
        {
            string BackgroundImage = @"C:\Images\0\unnamed.png";
            string IconImage = @"C:\Images\1\unnamed3.png";
            string OutputImage = @"C:\Images\result\resultImg.png";
            Image imageBackground = Image.FromFile(BackgroundImage);
            Image imageOverlay = Image.FromFile(IconImage);

            System.Drawing.Image img = new Bitmap(imageBackground.Width, imageBackground.Height);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.DrawImage(imageBackground, new Point(0, 0));
                gr.DrawImage(imageOverlay, new Point(imageBackground.Width / 2 - 50, imageBackground.Height / 2 - 50));
            }
            img.Save(OutputImage, ImageFormat.Png);
        }

        public void ImsageResult()
        {
            List<ImageCountInFolder> imageCountInFolders = new List<ImageCountInFolder>();

            List<FilesCounts> filesCounts = new List<FilesCounts>();

            int xx = 0;
            string rootPath = @"C:\Images";
            int filesLength = 0;
            for (int i = 0; i < 11; i++)
            {
                string[] strDizi = Directory.GetFiles(rootPath + "\\" + i);
                filesLength = strDizi.Length;
                imageCountInFolders.Add(new ImageCountInFolder
                {
                    index = filesLength - 1,
                    ImageCount = filesLength
                });
                for (int t = 0; t < filesLength; t++)
                {
                    xx = xx + (i + t);
                    filesCounts.Add(new FilesCounts
                    {
                        X = xx,
                        FileCount = filesLength,
                        Index_i = i
                    });
                }
            }

            List<Image> imageList = new List<Image>();
            string OutputImage;
            Bitmap final = new Bitmap(imageWidth, imageHeight);
            int x = 0;

            for (int i = 0; i < 11; i++)
            {
                filesLength = imageCountInFolders[i].ImageCount;
                string[] strDizi = Directory.GetFiles(rootPath + "\\" + i);
                int sayac = 0;

                for (int t = 0; t < filesLength; t++)
                {
                    x = x + (i + t);
                    sayac = i == 10 ? +1 : sayac;

                    foreach (var fileStr in strDizi)
                    {
                        imageList.Add(Image.FromFile(fileStr));
                    }

                    using (Graphics g = Graphics.FromImage(final))
                    {
                        foreach (Image image in imageList)
                        {
                            g.DrawImage(image, 0, 0);
                        }
                    }
                    OutputImage = rootPath + "\\result\\resultImg" + x + ".png";
                    final.Save(OutputImage);
                }

            }


        }

        public void GetImage(int i)
        {
            string[] strDizi = Directory.GetFiles(@"C:\Images\source\" + i);
            int fileCount = strDizi.Length;

            string[] arrayFolderStr = Directory.GetDirectories(@"C:\Images\source");
            int folderCount = arrayFolderStr.Length;

            for (int t = 0; t < fileCount; t++)
            {
                string it = i + "" + t;
                if (Index_t.Contains(Convert.ToInt32(it)))
                {
                    continue;
                }
                if (Filess[i].IsData)
                {
                    continue;
                }
                if (Filess[i].FileCount > 0 && keyValuePairs[i].Length > 0)
                {
                    Bitmap final2 = new Bitmap(imageWidth, imageHeight);
                    List<Image> imageList = new List<Image>();
                    foreach (var fileStr in keyValuePairs[i])
                    {
                        imageList.Add(Image.FromFile(fileStr));
                    }
                    using (Graphics g = Graphics.FromImage(final2))
                    {
                        foreach (Image image in imageList)
                        {
                            g.DrawImage(image, 0, 0);
                        }
                    }

                    string OutputImage = @"C:\Images\result\resultImg-RQ-" + it + ".png";
                    Filess[i].IsData = true;
                    Index_t.Add(Convert.ToInt32(it));
                    final2.Save(OutputImage);
                    GetImage(i);

                }
            }

        }

        public void ErUlanEr3()
        {
            Point point = GetImageSize();
            string[] strDizi = Directory.GetFiles(@"C:\Images\source\" + 0);
            int filesLength = strDizi.Length;

            string[] arrayFolderStr = Directory.GetDirectories(@"C:\Images\source");
            int folderCount = arrayFolderStr.Length;

            int sayac = 0;
            int x = 0;
            for (int t = 0; t < filesLength; t++)
            {
                for (int i = 0; i < folderCount; i++)
                {
                    x += i + t;
                    strDizi = Directory.GetFiles(@"C:\Images\source\" + i);
                    filesLength = strDizi.Length;
                    keyValuePairs[i] = strDizi;
                    FilesCounts filesCounts = new FilesCounts
                    {
                        X = sayac,
                        FileCount = filesLength,
                        Index_i = i,
                        IsData = false
                    };
                    Filess.Add(filesCounts);
                    sayac++;
                }
            }
            // ******************************************
            //for (int i = 0; i < folderCount; i++)
            //{
            //    int t = 0;
            //    if (Filess[i].FileCount > 0)
            //    {
            //        strDizi = Directory.GetFiles(@"C:\Images\source\" + i);
            //        if (t >= strDizi.Length)
            //        {
            //            break;
            //        }
            //        Bitmap final2 = new Bitmap(imageWidth, imageHeight);
            //        Image img = Image.FromFile(strDizi[t]);

            //        using (Graphics g = Graphics.FromImage(final2))
            //        {
            //            g.DrawImage(img, 0, 0);
            //        }
            //        string OutputImage = @"C:\Images\result\resultImg-TZN-" + i + t + ".png";
            //        final2.Save(OutputImage);
            //        t++;
            //    }
            //    //GetImage(i);
            //    ErUlanEr();
            //}
            //*****************************************************

            //int mx = EnCokDosyaAdedi(Filess);
            //mx = mx + 2;
            //SaveImages(keyValuePairs.Count);    // ************ KALSIN **************
            //SaveImages2(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages3(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages4(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages5(keyValuePairs.Count);   // ************ KALSIN **************     Kombinasyon 128 öge 28
            //SaveImages6(keyValuePairs.Count);   // ************ KALSIN **************     Kombinasyon 128 öge 28
            //HepsiniGoster(keyValuePairs);       // ************ KALSIN **************     İPTAL     İPTAL
            //HepsiniGoster2(keyValuePairs);      // ************ KALSIN **************     İPTAL     İPTAL
            //HepsiniGoster3(keyValuePairs);      // ************ KALSIN **************     EH 4 bckgrnd ile 128 kmbnsyn 128 öge 4 FARKLI EH EH
            //HepsiniGoster4(keyValuePairs);      // ************ KALSIN **************     EH 4 bckgrnd ile 128 kmbnsyn 128 öge 4 FARKLI EH EH
            //HepsiniGoster5(keyValuePairs);      // ************ KALSIN **************     EH 4 bckgrnd ile 128 kmbnsyn 128 öge 4 FARKLI EH EH
            //HepsiniGoster6(keyValuePairs);      // ************ KALSIN **************     4 bckgrnd ile 128 kmbnsyn 640 öge OLMADI
            //HepsiniGoster7(keyValuePairs);      // ************ KALSIN **************

            // 1-2-3-3-5 kombinasyon 90           klasor adet 5
            //SaveImages(keyValuePairs.Count);    // ************ KALSIN **************
            //SaveImages2(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages3(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages4(keyValuePairs.Count);   // ************ KALSIN **************
            //SaveImages5(keyValuePairs.Count);   // ************ KALSIN **************     Kombinasyon
            //SaveImages6(keyValuePairs.Count);   // ************ KALSIN **************     Kombinasyon
            //HepsiniGoster(keyValuePairs);       // ************ KALSIN **************     İPTAL
            //HepsiniGoster2(keyValuePairs);      // ************ KALSIN **************     İPTAL
            //HepsiniGoster3(keyValuePairs);      // ************ KALSIN **************     EH
            //HepsiniGoster4(keyValuePairs);      // ************ KALSIN **************     EH
            //HepsiniGoster5(keyValuePairs);      // ************ KALSIN **************     EH
            //HepsiniGoster6(keyValuePairs);      // ************ KALSIN **************     
            //HepsiniGoster7(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 450 + kmbnsyn + "-" + klasorcu +
            //HepsiniGoster8(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 234 + a + "-" + kmbnsyn + ÇEŞİT XX
            HepsiniGoster9(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 450 + kmbnsyn + "-" + klasorcu + "-" + a + ÇEŞİT 30
        }

        public void ErUlanEr2()
        {
            Point point = GetImageSize();
            string[] strDizi = Directory.GetFiles(@"C:\Images\source\" + 0);
            int filesLength = strDizi.Length;

            string[] arrayFolderStr = Directory.GetDirectories(@"C:\Images\source");
            int folderCount = arrayFolderStr.Length;

            int sayac = 0;
            int x = 0;
            for (int t = 0; t < filesLength; t++)
            {
                for (int i = 0; i < folderCount; i++)
                {
                    x += i + t;
                    strDizi = Directory.GetFiles(@"C:\Images\source\" + i);
                    filesLength = strDizi.Length;
                    keyValuePairs[i] = strDizi;
                    FilesCounts filesCounts = new FilesCounts
                    {
                        X = sayac,
                        FileCount = filesLength,
                        Index_i = i,
                        IsData = false
                    };
                    Filess.Add(filesCounts);
                    sayac++;
                }
            }
            HepsiniGoster8(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 234 + a + "-" + kmbnsyn + ÇEŞİT XX
        }

        public void ErUlanEr()
        {
            Point point = GetImageSize();
            string[] strDizi = Directory.GetFiles(@"C:\Images\source\" + 0);
            int filesLength = strDizi.Length;

            string[] arrayFolderStr = Directory.GetDirectories(@"C:\Images\source");
            int folderCount = arrayFolderStr.Length;

            int sayac = 0;
            int x = 0;
            for (int t = 0; t < filesLength; t++)
            {
                for (int i = 0; i < folderCount; i++)
                {
                    x += i + t;
                    strDizi = Directory.GetFiles(@"C:\Images\source\" + i);
                    filesLength = strDizi.Length;
                    keyValuePairs[i] = strDizi;
                    FilesCounts filesCounts = new FilesCounts
                    {
                        X = sayac,
                        FileCount = filesLength,
                        Index_i = i,
                        IsData = false
                    };
                    Filess.Add(filesCounts);
                    sayac++;
                }
            }
            HepsiniGoster7(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 450 + kmbnsyn + "-" + klasorcu +
        }

        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        // -KOMBİNASYON SAYISINA SADIK KAL
        public void HepsiniGoster9(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 1; dosyaci <= keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        if (keyValuePairs[klasorcu][a].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                            Image new_Image = ResizeImage(img, imageWidth, imageHeight);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(new_Image, 0, 0);
                            }
                            OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-" + klasorcu + "-" + a + "-.png";
                            final4.Save(OutputImage);
                            img.Dispose();
                            new_Image.Dispose();
                            break;
                        }
                    }
                }
                final4.Dispose();
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        // -KOMBİNASYON SAYISINA SADIK KAL
        public void HepsiniGoster8(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                List<string> cikanSayilar = new List<string>();
                string sayilar = "";
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 0; dosyaci < keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                       
                        int kalan = 0;// kmbnsyn % keyValuePairs[klasorcu].Length;
                        int bolum = Math.DivRem(kmbnsyn, keyValuePairs[klasorcu].Length, out kalan);
                        sayilar = kalan + "-" + klasorcu;
                        if (keyValuePairs[klasorcu][kalan].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][kalan]);
                            Image new_Image = ResizeImage(img, imageWidth, imageHeight);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(new_Image, 0, 0);
                            }
                                                   
                            OutputImage = @"C:\Images\result\resultImg-TIT8-" + kmbnsyn + "-" + sayilar + ".png";// "-" + klasorcu + "-" + dosyaci +
                            final4.Save(OutputImage);
                            cikanSayilar.Add(sayilar);
                            img.Dispose();
                            new_Image.Dispose();
                            break;
                        }
                        if (cikanSayilar.Contains(sayilar))
                        {
                            break;
                        }

                    }


                }
                final4.Dispose();
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        // -KOMBİNASYON SAYISINA SADIK KAL
        public void HepsiniGoster7A(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            Bitmap final4 = new Bitmap(imageWidth, imageHeight);
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                string OutputImage = "";
                for (int klasorIndex = 0; klasorIndex < keyValuePairs.Count; klasorIndex++)
                {
                    if (keyValuePairs[klasorIndex].Length > 0)
                    {
                        int kalan;
                        Math.DivRem(kmbnsyn, keyValuePairs[klasorIndex].Length, out kalan);
                        if (keyValuePairs[klasorIndex][kalan].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorIndex][kalan]);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                    }
                }
                OutputImage = @"C:\Images\result\AK43P-07EYL-" + kmbnsyn + "-.png";
                final4.Save(OutputImage);
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        // -KOMBİNASYON SAYISINA SADIK KAL
        public void HepsiniGoster7(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            //int sayac = 0;
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 1; dosyaci <= keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        if (keyValuePairs[klasorcu][a].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                            Image new_Image = ResizeImage(img, imageWidth, imageHeight);
                            //int imageHeight2 = img.Height;
                            //int imageWidth2 = img.Width;
                            //int heightFark = (imageHeight - imageHeight2) / 2;
                            //int widthFark = (imageWidth - imageWidth2) / 2;
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(new_Image, 0, 0);
                            }
                            OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                            final4.Save(OutputImage);
                            img.Dispose();
                            new_Image.Dispose();
                            //Point point = GetImagePoint(OutputImage);
                            break;
                        }
                    }
                }
                final4.Dispose();
                //sayac++;
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster6(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    int max = EnCokDosyaAdedi(Filess);
                    if (keyValuePairs[klasorcu].Length > 0)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                        using (Graphics g = Graphics.FromImage(final4))
                        {
                            g.DrawImage(img, 0, 0);
                        }
                        OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-" + klasorcu + "-" + a + "-.png";
                        final4.Save(OutputImage);
                    }
                }
            }

        }


        // BURA TAMAM DOSYALARIN YARISI ÖRN :0-60 RESİM 29'DAN SONRASI SİLİNİR
        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster5(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 1; dosyaci <= keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        if (keyValuePairs[klasorcu].Length > 0 && keyValuePairs[klasorcu][a].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(img, 0, 0);
                            }
                            OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                            final4.Save(OutputImage);
                            break;
                        }
                    }
                }
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster4(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 1; dosyaci <= keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                        using (Graphics g = Graphics.FromImage(final4))
                        {
                            g.DrawImage(img, 0, 0);
                        }
                        OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                        final4.Save(OutputImage);
                        break;
                    }
                }
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster3(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 1; dosyaci <= keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        int a = kmbnsyn % keyValuePairs[klasorcu].Length;
                        if (keyValuePairs[klasorcu].Length > 0 && keyValuePairs[klasorcu][a].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][a]);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(img, 0, 0);
                            }
                            OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                            final4.Save(OutputImage);
                            break;
                        }
                    }
                }
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster2(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                // Bura iyi
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);
                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 0; dosyaci < keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        if (keyValuePairs[klasorcu].Length > 0 && keyValuePairs[klasorcu][dosyaci].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][dosyaci]);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(img, 0, 0);
                            }
                            OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                            final4.Save(OutputImage);
                            break;
                        }
                    }
                }
            }

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void HepsiniGoster(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            int dosyaciSahte = 0;
            for (int kmbnsyn = 0; kmbnsyn < kombinasyonSayisi; kmbnsyn++)
            {
                Bitmap final4 = new Bitmap(imageWidth, imageHeight);

                for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)
                {
                    string OutputImage = "";
                    for (int dosyaci = 0; dosyaci < keyValuePairs[klasorcu].Length; dosyaci++)
                    {
                        if (keyValuePairs[klasorcu].Length > 0 && keyValuePairs[klasorcu][dosyaci].Length > 0)
                        {
                            Image img = Image.FromFile(keyValuePairs[klasorcu][dosyaci]);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(img, 0, 0);
                            }
                            dosyaciSahte = dosyaci;
                            break;
                        }
                    }
                    OutputImage = @"C:\Images\result\resultImg-ATP-TIT-" + kmbnsyn + "-.png";
                    final4.Save(OutputImage);
                }
            }



        }


        public String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }


        public int EnCokDosyaAdedi(List<FilesCounts> filesCounts)
        {
            int mx = 0;

            foreach (var item in filesCounts)
            {
                for (int i = 0; i < item.FileCount; i++)
                {
                    for (int t = 0; t < item.FileCount; t++)
                    {
                        mx = Math.Max(i, t);
                    }
                }

            }

            return mx;

        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages6(int folderCount)
        {
            for (int i = 0; i < folderCount; i++)// birinci boyut  11 adet klasor
            {
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();


                    for (int k = 0; k < keyValuePairs.Count; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        imgList.Add(Image.FromFile(keyValuePairs[k][d]));   // ŞAPKASIZ ÇIKANLAR
                        imgList.Add(Image.FromFile(keyValuePairs[i][d])); // ŞAPIRT ŞAÇMA ŞAÇMALAK
                                                                          //Image img = Image.FromFile(keyValuePairs[k][d]);
                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);


                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
            }


        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages5(int folderCount)
        {
            for (int i = 0; i < folderCount; i++)// birinci boyut  11 adet klasor
            {
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();


                    for (int k = 0; k < keyValuePairs.Count; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        imgList.Add(Image.FromFile(keyValuePairs[k][d]));   // ŞAPKASIZ ÇIKANLAR
                        imgList.Add(Image.FromFile(keyValuePairs[i][d])); // ŞAPIRT ŞAÇMA ŞAÇMALAK
                                                                          //Image img = Image.FromFile(keyValuePairs[k][d]);
                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);

                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
            }


        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages4(int folderCount)
        {
            for (int i = 0; i < folderCount; i++)// birinci boyut  11 adet klasor
            {
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                    int dosyaAdet = 0;
                    for (int k = 0; k < keyValuePairs.Count; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        Image img = Image.FromFile(keyValuePairs[k][d]);
                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            g.DrawImage(img, 0, 0);
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);
                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
            }


        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages3(int folderCount)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)// birinci boyut  11 adet klasor
            {
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();


                    for (int k = 0; k < folderCount; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        imgList.Add(Image.FromFile(keyValuePairs[k][d]));   // ŞAPKASIZ ÇIKANLAR

                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);
                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
            }


        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages2(int folderCount)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)// birinci boyut  11 adet klasor
            {
                Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();


                    for (int k = 0; k < keyValuePairs[i].Length; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        imgList.Add(Image.FromFile(keyValuePairs[k][d]));   // ŞAPKASIZ ÇIKANLAR
                        imgList.Add(Image.FromFile(keyValuePairs[i][k]));   // ŞAPKALILAR GELSİN ŞAPKALILAR

                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);
                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
                OutputImage = @"C:\Images\result\resultImg-ATP-CC-" + kkd + ".png";
                final3.Save(OutputImage);
            }


        }


        // ********* BU BÖYLE KALSIN SEÇENEK OLARAK KULLANILIR ****************************
        public void SaveImages(int folderCount)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)// birinci boyut  11 adet klasor
            {
                Bitmap final3 = new Bitmap(imageWidth, imageHeight);
                string OutputImage = "";
                string kkd = i + "";
                for (int d = 0; d < keyValuePairs[i].Length; d++) // kk ikinci  boyuttaki   d   boyutu  dosya adetleri
                {
                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();


                    for (int k = 0; k < keyValuePairs[i].Length; k++)// d X kk = 1..11
                    {
                        dosyaAdet = keyValuePairs[k].Length;
                        if (dosyaAdet < 1)
                        {
                            continue;
                        }
                        if (dosyaAdet <= d)
                        {
                            continue;
                        }
                        imgList.Add(Image.FromFile(keyValuePairs[i][k]));
                        imgList.Add(Image.FromFile(keyValuePairs[k][d]));
                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        kkd = k + "" + d;
                        OutputImage = @"C:\Images\result\resultImg-ATP-AA-" + kkd + ".png";
                        final3.Save(OutputImage);
                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\resultImg-ATP-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
                OutputImage = @"C:\Images\result\resultImg-ATP-CC-" + kkd + ".png";
                final3.Save(OutputImage);
            }


        }


    }
}


//for (int i = 0; i < folderCount; i++)
//{
//    strDizi = Directory.GetFiles(@"C:\Images\source\" + i);

//    Bitmap final = new Bitmap(imageWidth, imageHeight);
//    List<Image> imageList = new List<Image>();

//    foreach (var fileStr in strDizi)
//    {
//        imageList.Add(Image.FromFile(fileStr));
//    }

//    using (Graphics g = Graphics.FromImage(final))
//    {
//        foreach (Image image in imageList)
//        {
//            g.DrawImage(image, 0, 0);
//        }
//    }

//    string OutputImage = @"C:\Images\result\resultImg-RQ-" + i + FilesCounts[i].FileCount + ".png";

//    if (FilesCounts[i].FileCount > 0)
//    {
//        GetImage(i, FilesCounts[i].FileCount);
//        final.Save(OutputImage);
//    }
//}
