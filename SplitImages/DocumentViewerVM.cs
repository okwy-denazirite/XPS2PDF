using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace SplitImages
{
    public class DocumentViewerVM
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);
        private FixedDocument fixedDoc = new FixedDocument();


        public DocumentViewerVM()
        {
            string impath = @"C:\Users\okwunna.olive-okafor\Pictures\myexport.jpg";
            exportPreview(impath);
        }

        public FixedDocument FixedDoc
        {
            get { return fixedDoc; }
            set { fixedDoc = value; }
        }

        private void exportPreview(string path)
        {
            List<System.Drawing.Image> imgarray = new List<System.Drawing.Image>();
            List<theImages> allImages = new List<theImages>();
            var img = Image.FromFile(path);
            int imHeight = 1000;
            int imWidth = 794;
            List<System.Windows.Controls.Image> imgctrl = new List<System.Windows.Controls.Image>();
            List<System.Windows.Controls.Canvas> canva = new List<System.Windows.Controls.Canvas>();
            int imLength = img.Height / 600;
            int pages = 0;
            for (int i = 0; i < imLength; i++)
            {
                Bitmap temp = new Bitmap(imWidth, imHeight);
                imgarray.Add(temp);
                var graphics = Graphics.FromImage(imgarray[i]);
                graphics.DrawImage(img, new System.Drawing.Rectangle(0, 0, imWidth, imHeight), new System.Drawing.Rectangle(0, i * imHeight, imWidth, imHeight), GraphicsUnit.Pixel);
                graphics.Dispose();
                Bitmap img1 = (Bitmap)imgarray[i];
                int pixCount = 0;
                int pixChecked = 0;
                for (int _i = img1.Width / 4; _i < (img1.Width * 3) / 4; _i++)
                {
                    for (int _j = img1.Height / 4; _j < (img1.Height * 3) / 4; _j++)
                    {
                        pixCount++;
                        if (img1.GetPixel(_i, _j).R >= 250 && img1.GetPixel(_i, _j).G >= 250 && img1.GetPixel(_i, _j).B >= 250)
                        {
                            pixChecked++;
                        }
                    }
                }
                if (pixCount - pixChecked < 100)
                {
                    break;
                }
                allImages.Add(new theImages { bms = GetImageStream(imgarray[i]) });
                imgctrl.Add(new System.Windows.Controls.Image());
                pages++;
            }
            //lstb.ItemsSource = allImages;
            for (int i = 0; i < allImages.Count; i++)
            {
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage();
                fixedPage.Margin = new Thickness(47, 20, 0, 0);
                imgctrl[i].Source = allImages[i].bms;
                canva.Add(new Canvas());
                Canvas.SetTop(canva[i], 0);
                Canvas.SetLeft(canva[i], 0);
                Line borderLineTop = new Line();
                borderLineTop.Stroke = System.Windows.Media.Brushes.Black;
                borderLineTop.SnapsToDevicePixels = true;
                borderLineTop.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                borderLineTop.StrokeThickness = 2;
                borderLineTop.X1 = 3;
                borderLineTop.X2 = img.Width - 3;  // 150 too far
                borderLineTop.Y1 = 1;
                borderLineTop.Y2 = 1;
                Line borderLineBottom = new Line();
                borderLineBottom.Stroke = System.Windows.Media.Brushes.Black;
                borderLineBottom.SnapsToDevicePixels = true;
                borderLineBottom.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                borderLineBottom.StrokeThickness = 2;
                borderLineBottom.X1 = 3;
                borderLineBottom.X2 = img.Width - 3;  // 150 too far
                borderLineBottom.Y1 = borderLineBottom.Y2 = allImages[i].bms.Height;
                borderLineBottom.Y2 = borderLineBottom.Y2 = allImages[i].bms.Height;
                canva[i].Children.Add(imgctrl[i]);
                canva[i].Children.Add(borderLineTop);
                canva[i].Children.Add(borderLineBottom);
                fixedPage.Children.Add(canva[i]);
                ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);
            }
            
            //DocViewer.Document = fixedDoc;
        }

        public static BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }
        public class theImages
        {
            public BitmapSource bms { get; set; }
        }
    }
}
