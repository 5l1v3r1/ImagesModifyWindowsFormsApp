using ImagesModifyWindowsFormsApp.Classes;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImagesModifyWindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void KlasorOlustur_Click(object sender, EventArgs e)
        {
            label1.Text = "Klasörler oluşturuluyor..";
            bool isImagesFolder = Directory.Exists("C:\\Images\\source");
            if (!isImagesFolder)
            {
                var info = Directory.CreateDirectory("C:\\Images");
                var infoSource = Directory.CreateDirectory("C:\\Images\\source");
                var infoResult = Directory.CreateDirectory("C:\\Images\\result");
                var infoTemp = Directory.CreateDirectory("C:\\Images\\Temp");
                for (int i = 0; i < 11; i++)
                {
                    var infos = Directory.CreateDirectory("C:\\Images\\source\\" + i);
                }
            }
            label1.ForeColor = Color.Green;
            label1.Text = "Kalasörler Oluşturuldu";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Text = "Hazırlanıyor";
            bool isImagesFolder = Directory.Exists("C:\\Images\\source");
            if (isImagesFolder)
            {
                KlasorOlustur.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isImagesFolder = Directory.Exists("C:\\Images\\source");
            if (!isImagesFolder)
            {
                MessageBox.Show("Klasorler Hazır Değil!");
                return;
            }
            label1.ForeColor = Color.Blue;
            label1.Text = "Resimler Oluşturuluyor..";
            Utilities utilities = new Utilities();
            utilities.ErUlanEr();
            label1.ForeColor = Color.Green;
            label1.Text = "Resimler Hazır";
        }

        private void Basla_2_Click(object sender, EventArgs e)
        {
            bool isImagesFolder = Directory.Exists("C:\\Images\\source");
            if (!isImagesFolder)
            {
                MessageBox.Show("Klasorler Hazır Değil!");
                return;
            }
            label1.ForeColor = Color.Blue;
            label1.Text = "Resimler Oluşturuluyor..";
            Utilities utilities = new Utilities();
            utilities.ErUlanEr2();
            label1.ForeColor = Color.Green;
            label1.Text = "Resimler Hazır";
        }

        private void Basla_3_Click(object sender, EventArgs e)
        {
            bool isImagesFolder = Directory.Exists("C:\\Images\\source");
            if (!isImagesFolder)
            {
                MessageBox.Show("Klasorler Hazır Değil!");
                return;
            }
            label1.ForeColor = Color.Blue;
            label1.Text = "Resimler Oluşturuluyor..";
            Utilities utilities = new Utilities();
            utilities.ErUlanEr3();
            label1.ForeColor = Color.Green;
            label1.Text = "Resimler Hazır";
        }

        private void Ciktilari_Sil_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Çıktı dosyaları dosyalar C:\\Images\\result klasorunden silinecek. \n Silmek istediğinize emin misiniz?", "Dikkat", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.No)
            {
                return;
            }

            System.IO.DirectoryInfo klasor = new DirectoryInfo(@"C:\Images\result");

            foreach (FileInfo dosya in klasor.GetFiles())
            {
                dosya.Delete();
            }
            label1.ForeColor = Color.Green;
            label1.Text = "Çıktılar Silindi";
        }

        private void Tum_Klasorleri_Sil_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Kaydettiğiniz dosyalar C:\\Images klasoründen silinecek. \n Silmek istediğinize emin misiniz?", "Dikkat", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.No)
            {
                return;
            }

            System.IO.DirectoryInfo klasor = new DirectoryInfo(@"C:\Images");

            foreach (FileInfo dosya in klasor.GetFiles())
            {
                dosya.Delete();
            }

            DirectoryInfo[] ks = klasor.GetDirectories();
            foreach (DirectoryInfo k in ks)
            {
                k.Delete(true);
            }
            label1.ForeColor = Color.Green;
            label1.Text = "Klasorler Silindi";
        }

        private void Open_Result_Folder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", @"C:\Images\result");
        }

    }
}