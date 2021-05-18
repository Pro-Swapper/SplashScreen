using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace ProSwapperSplashScreen
{
    public partial class SplashUI : Form
    {
        public static string ProSwapperFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Pro_Swapper\";
        private readonly string SplashPath = $"{EpicGamesLauncher.GetGamePath()}\\FortniteGame\\Binaries\\Win64\\EasyAntiCheat\\Launcher\\SplashScreen.png";
        public SplashUI()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.FriendlyName);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            foreach (string image in api.GetImgurUrls())
            {
                PictureBox picturebox = new PictureBox();
                picturebox.Cursor = Cursors.Hand;
                new Thread(() =>
                {
                    picturebox.Image = ItemIcon(image);
                }).Start();

                picturebox.Tag = ProSwapperFolder + @"Images\" + image;
                picturebox.Location = new Point(3, 3);
                picturebox.Size = new Size(196, 129);
                picturebox.SizeMode = PictureBoxSizeMode.Zoom;
                picturebox.TabIndex = 1;
                picturebox.TabStop = false;
                picturebox.Click += new EventHandler(SplashClick);
                items.Controls.Add(picturebox);
            }
        }
        private void ThemeCreator_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x2, 0);
        }
        private void button1_Click(object sender, EventArgs e) => Close();
        #region CustomSplashButton
        private void CustomSplash(object sender, EventArgs e)
        {
            using (OpenFileDialog a = new OpenFileDialog())
            {
                if (a.ShowDialog() == DialogResult.OK)
                {
                    Image splash = ResizeImage(Image.FromFile(a.FileName), 640,360);
                    splash.Save(SplashPath);
                    MessageBox.Show("Set Splash Screen!", "Pro Swapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        #region CreateDir Method
        public static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }
        #endregion
        #region SplashClick
        private void SplashClick(object sender, EventArgs e)
        {
            try
            {
                File.Copy(((PictureBox)sender).Tag.ToString(), SplashPath, true);
                MessageBox.Show("Set Splash Screen!", "Pro Swapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Your Fortnite game folder could not be found! Please contact support and give them this info: {Directory.Exists(SplashPath)} | {SplashPath} | {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion
        #region ResizeImage
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
        #endregion
        #region IconDownloader
        public static Image ItemIcon(string url)
        {
            string rawpath = ProSwapperFolder + @"Images\";
            CreateDir(rawpath);
            string path = rawpath + url;
            string imageurl = "https://i.imgur.com/" + url;
            //Downloads image if doesnt exists
            start: if (!File.Exists(path))
                new WebClient().DownloadFile(imageurl, path);
            try
            {
                Image img;
                using (Bitmap bmpTemp = new Bitmap(path))
                {
                    img = new Bitmap(bmpTemp);
                }
                if (IsImage(img))
                {
                    return img;
                }
                else
                {
                    img.Dispose();
                    throw new Exception();
                }
            }
            catch
            {
                File.Delete(path);
                goto start;
            }
        }
        #endregion
        #region (Bool) IsImage
        private static bool IsImage(Image imagevar)
        {
            try
            {
                Image imgInput = imagevar;
                Graphics gInput = Graphics.FromImage(imgInput);
                ImageFormat thisFormat = imgInput.RawFormat;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region FormMoveable
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion
        #region RoundedFormCorners
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse
        );
        #endregion
    }


}
