using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Aforge = AForge.Imaging;
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

        public void DeleteFile(string filePath)
        {           
            //var filePath = Server.MapPath("~/Images/" + filename);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public float DifferenceImagesAforge(Bitmap image1, Bitmap image2)
        {
            Aforge.ExhaustiveTemplateMatching tm = new Aforge.ExhaustiveTemplateMatching(0);
            Aforge.TemplateMatch[] matchings = tm.ProcessImage(image1, image2);
            // benzerlik seviyesini kontrol et
            if (matchings[0].Similarity > 0.95f)
            {
                //
            }
            return matchings[0].Similarity;
        }

        public float DifferenceImages(Bitmap image1, Bitmap image2) // Benzerlik % desi
        {
            float diff = 0;
            //Birinci görüntünün yüksekliği * genişliği kadar, yani tüm pikseller
            for (int y = 0; y < image1.Height; y++)
            {
                for (int x = 0; x < image2.Width; x++)
                {
                    //Bitmap sınıfı içerisideki GetPixel metodu piksel değerini okur
                    //RGB formatı referans alınmıştır, iki görüntü için aynı piksellerin mutlak farkı
                    //hesaplanır ve maksimum değeri olan 255 ile oranlanır
                    diff += (float)Math.Abs(image1.GetPixel(x, y).R - image2.GetPixel(x, y).R) / 255;
                    diff += (float)Math.Abs(image1.GetPixel(x, y).G - image2.GetPixel(x, y).G) / 255;
                    diff += (float)Math.Abs(image1.GetPixel(x, y).B - image2.GetPixel(x, y).B) / 255;
                }
            }
            float result = ((100 * diff) / (image1.Width * image1.Height * 3));
            return result;
        }

        public void SaveDifferenceImages(Bitmap bitmap1, Bitmap bitmap2)
        {
            Bitmap yeniBitmap = new Bitmap(bitmap1.Width, bitmap1.Height);
            for (int y = 0; y < bitmap1.Height; y++)
            {
                for (int x = 0; x < bitmap2.Width; x++)
                {
                    //İkisi arasında fark var ise yeni bitmap nesnesine ata
                    if (bitmap1.GetPixel(x, y) != bitmap2.GetPixel(x, y))
                        yeniBitmap.SetPixel(x, y, bitmap2.GetPixel(x, y));
                }
            }
            yeniBitmap.Save("yeni.png");
        }

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

        public Point GetImagePoint(string fullName)
        {
            Bitmap img = new Bitmap(fullName);
            Point point = new Point(img.Width, img.Height);
            return point;
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
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

        public System.Drawing.Image ResizeImage(System.Drawing.Image source, RectangleF destinationBounds)
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

        public void Resim_Ayikla()
        {
            string[] imageDizi = Directory.GetFiles(@"C:\Images\result");
            int imageCount = imageDizi.Length;
            List<Bitmap> imagesList = new List<Bitmap>();
            List<Bitmap> imagesList2 = new List<Bitmap>();
            for (int i = 0; i < imageDizi.Length; i++)
            {
                Bitmap img = new Bitmap(imageDizi[i]);
                imagesList.Add(img);
            }
            imagesList2.AddRange(imagesList);
            int boy = imagesList.Count;
            int mod = boy % 2;
            int ilkYari = boy / 2 + mod;
            int ikinciYari = ilkYari + 1;
            float benzerlik = 10;
            for (int i = 0; i < ilkYari; i++)
            {
                for (int j = ilkYari; j < imagesList2.Count; j++)
                {
                    try
                    {
                        benzerlik = DifferenceImagesAforge(imagesList[i], imagesList2[j]);
                        //if (benzerlik <= 0.5f)
                        //{
                        //    imagesList2[j].Dispose();
                        //    DeleteFile(imageDizi[j]);
                        //}
                        if (benzerlik > 5.5f)
                        {
                            imagesList2[j].Dispose();
                            DeleteFile(imageDizi[j]);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    

                }
            }

        }

        public void ErUlanErxx7()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGosterxx7(keyValuePairs);
        }

        public void HepsiniGosterxx7(Dictionary<int, string[]> keyValuePairs)
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }
            //Bitmap final4 = new Bitmap(imageWidth, imageHeight);
            for (int kmbnsyn = 1; kmbnsyn < kombinasyonSayisi * 3 + 1; kmbnsyn++)
            {
                // Bura iyi
                Bitmap finalXX = new Bitmap(imageWidth, imageHeight);
                Image img = null;
                Image new_Image = null;
                string OutputImage = "";
                for (int klasorcu = 1; klasorcu < keyValuePairs.Count + 1; klasorcu++)
                {
                    string[] imageDizi = Directory.GetFiles(@"C:\Images\source\" + (klasorcu - 1));
                    if (imageDizi.Length == 0)
                    {
                        break;
                    }
                    img = ResimGetir(kmbnsyn, klasorcu, imageDizi);
                    new_Image = ResizeImage(img, imageWidth, imageHeight);
                    using (Graphics g = Graphics.FromImage(finalXX))
                    {
                        g.DrawImage(new_Image, 0, 0);
                    }

                    //OutputImage = @"C:\Images\result\Img-yy-" + kmbnsyn + "-" + klasorcu + "-.png";
                }//klasorcu
                OutputImage = @"C:\Images\result\Img-zz-" + kmbnsyn + "-.png";
                finalXX.Save(OutputImage);
                finalXX.Dispose();
                img.Dispose();
                new_Image.Dispose();
            }//kmbnsyn
        }//function

        private Image ResimGetir(int kmbnsyn, int klasorcu, string[] imageDizi)
        {
            int kalan;
            //int tur = Convert.ToInt32(Math.Round(Convert.ToDecimal(kmbnsyn / klasorcu), 0));
            int tur = kmbnsyn / klasorcu;
            Math.DivRem(tur, imageDizi.Length, out kalan);
            Image img = new Bitmap(imageDizi[kalan]);
            return img;
        }

        public void ErUlanEr15()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages15(keyValuePairs.Count);
        }

        public void ErUlanEr14()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages14(keyValuePairs.Count);
        }

        public void ErUlanEr13()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages13(keyValuePairs.Count);
        }

        public void ErUlanEr12()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages12(keyValuePairs.Count);
        }

        public void ErUlanEr11A()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages11A();
        }

        public void ErUlanEr11()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages11();
        }

        public void ErUlanEr10()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            SaveImages10();
        }

        public void ErUlanEr9()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster9(keyValuePairs);
        }

        public void ErUlanEr8()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster8(keyValuePairs);
        }

        public void ErUlanEr7A()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster7A(keyValuePairs);
        }

        public void ErUlanEr7()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster7(keyValuePairs);
        }

        public void ErUlanEr6()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster6(keyValuePairs);
        }

        public void ErUlanEr5()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster5(keyValuePairs);
        }

        public void ErUlanEr4()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster4(keyValuePairs);
        }

        public void ErUlanEr3()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
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
            HepsiniGoster3(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 450 + kmbnsyn + "-" + klasorcu + "-" + a + ÇEŞİT 30
        }

        public void ErUlanEr2()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster2(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 234 + a + "-" + kmbnsyn + ÇEŞİT XX
        }

        public void ErUlanEr()
        {
            HayatiminEnkotuKodununHiminaDiminaTridine();
            HepsiniGoster(keyValuePairs);      // ************ KALSIN **************     kombinasyon 90 öge 450 + kmbnsyn + "-" + klasorcu +
        }

        public void HayatiminEnkotuKodununHiminaDiminaTridine()
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

        }

        public void SaveImages15(int folderCount)
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

        public void SaveImages14(int folderCount)
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

        public void SaveImages13(int folderCount)
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

        public void SaveImages12(int folderCount)
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
        public void SaveImages11A()
        {
            int kombinasyonSayisi = 1;
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                kombinasyonSayisi *= keyValuePairs[i].Length > 0 ? keyValuePairs[i].Length : 1;
            }

            string[] strDizi = Directory.GetFiles(@"C:\Images\source\0");
            FileInfo file = new FileInfo(strDizi[0]);
            Point point = GetImagePoint(file.FullName);

            for (int kbnyn = 0; kbnyn < kombinasyonSayisi; kbnyn++)// kmbnsyn sayısınca dön i
            {
                Bitmap final3 = new Bitmap(point.X, point.Y);
                string OutputImage = "";
                string k_d = kbnyn.ToString();

                for (int dosyaci = 0; dosyaci < keyValuePairs[kbnyn].Length; dosyaci++) // i'cu klasordeki d'ninci   // dosya sayısınca dön d                                                                         
                {

                    int dosyaAdet = 0;
                    List<Image> imgList = new List<Image>();

                    List<string> cikanSayilar = new List<string>();
                    string sayilar = "";

                    for (int klasorcu = 0; klasorcu < keyValuePairs.Count; klasorcu++)// Klasor adedi kadar k  dön
                    {
                        Image img_kd = null;
                        Image new_Image = null;
                        Image img_ik = null;
                        Image new_Image2 = null;

                        strDizi = Directory.GetFiles(@"C:\Images\source\" + klasorcu);
                        file = new FileInfo(strDizi[dosyaci]);
                        point = GetImagePoint(file.FullName);

                        int kalan = 0;// kmbnsyn % keyValuePairs[klasorcu].Length;
                        int bolum = Math.DivRem(kbnyn, keyValuePairs[klasorcu].Length, out kalan);
                        sayilar = kalan + "-" + klasorcu;

                        dosyaAdet = keyValuePairs[klasorcu].Length;
                        if (dosyaAdet < 1)                  // dosyaAdet kontrol et
                        {
                            continue;
                        }
                        if (dosyaAdet <= dosyaci)
                        {
                            continue;
                        }
                        if (cikanSayilar.Contains(sayilar))
                        {
                            continue;
                        }
                        img_kd = Image.FromFile(keyValuePairs[klasorcu][dosyaci]);   // k'nıncı klasor sırasından    // d'ninci dosyayı al
                        new_Image = ResizeImage(img_kd, point.X, point.Y);
                        imgList.Add(new_Image);

                        img_ik = Image.FromFile(keyValuePairs[kbnyn][klasorcu]);   // kmbnsyn'ninci klasor sayısından      // k'nıncı dosyayı al
                        new_Image2 = ResizeImage(img_ik, point.X, point.Y);
                        imgList.Add(new_Image2);

                        using (Graphics g = Graphics.FromImage(final3))
                        {
                            foreach (var img in imgList)
                            {
                                g.DrawImage(img, 0, 0);
                            }
                        }
                        k_d = klasorcu + "_" + dosyaci;
                        OutputImage = @"C:\Images\result\Img-11A-AA-" + sayilar + ".png";
                        final3.Save(OutputImage);
                        cikanSayilar.Add(sayilar);
                        img_kd.Dispose();
                        img_ik.Dispose();
                        new_Image.Dispose();
                        new_Image2.Dispose();
                        break;
                    }// klasorcü k
                     //string i_d = kbnyn + "_" + dosyaci;
                     //OutputImage = @"C:\Images\result\Img-11A-BB-" + kbnyn + "-" + i_d + ".png";
                     //final3.Save(OutputImage);
                     //cikanSayilar.Add(sayilar);
                     //img_kd.Dispose();
                     //img_ik.Dispose();
                     //new_Image.Dispose();
                     //new_Image2.Dispose();
                     //break;
                }// dosyaci d
                OutputImage = @"C:\Images\result\Img-11A-CC-" + kbnyn + ".png";
                final3.Save(OutputImage);
                final3.Dispose();
                point = GetImagePoint(OutputImage);
            }// kbnyn i

        }

        // ********* Kombinasyon Var *********
        // ********* A Serisinde Var *********
        // ********* Kombinasyon 4 A *********
        // ********* Output Image 19 *********
        // * awesome_photo_finder ile bitene kadar sil *
        // ********* 6 Adet kaldı    *********
        // *** Ya da sadece A serisini bırak ***
        public void SaveImages11()
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
                        OutputImage = @"C:\Images\result\Img-11-AA-" + kkd + ".png";
                        final3.Save(OutputImage);
                    }
                    kkd = i + "" + d;
                    OutputImage = @"C:\Images\result\Img-11-BB-" + kkd + ".png";
                    final3.Save(OutputImage);
                }
                OutputImage = @"C:\Images\result\Img-11-CC-" + kkd + ".png";
                final3.Save(OutputImage);
                //final3.Dispose();
            }


        }

        public void SaveImages10()
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
                            OutputImage = @"C:\Images\result\Img-9-AA-" + kmbnsyn + "-" + klasorcu + "-" + a + "-.png";
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

                            OutputImage = @"C:\Images\result\Img-8-AA" + kmbnsyn + "-" + sayilar + ".png";// "-" + klasorcu + "-" + dosyaci +
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
                            Image new_Image = ResizeImage(img, imageWidth, imageHeight);
                            using (Graphics g = Graphics.FromImage(final4))
                            {
                                g.DrawImage(new_Image, 0, 0);
                            }
                            img.Dispose();
                            new_Image.Dispose();
                        }
                    }
                }
                OutputImage = @"C:\Images\result\Img-7A-" + kmbnsyn + "-.png";
                final4.Save(OutputImage);
                //img.Dispose();
                //new_Image.Dispose();
            }
            final4.Dispose();
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
                            OutputImage = @"C:\Images\result\Img-7-" + kmbnsyn + "-.png";
                            final4.Save(OutputImage);
                            img.Dispose();
                            new_Image.Dispose();
                            //Point point = GetImagePoint(OutputImage);
                            break;
                        }
                    }
                }
                final4.Dispose();
            }

        }

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
