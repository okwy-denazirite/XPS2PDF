using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace XPSDocumentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

     static class WPF2PDF
    {

        public static Visual CreateVisual()
        {
            const double Inch = 96;
            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();
            Pen bluePen = new Pen(Brushes.Blue, 1);
            dc.DrawRectangle(Brushes.Yellow, bluePen, new Rect(Inch / 2, Inch / 2, Inch * 1.5, Inch * 1.5));
            Brush pinkBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 255));
            Pen blackPen = new Pen(Brushes.Black, 1);
            dc.DrawEllipse(pinkBrush, blackPen, new Point(Inch * 2.25, Inch * 2), Inch * 1.25, Inch);
            dc.Close();
            return visual;
        }
    }
        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                // create XPS file based on a WPF Visual, and store it in a memorystream
                MemoryStream lMemoryStream = new MemoryStream();
                Package package = Package.Open(lMemoryStream, FileMode.Create);
                // XpsDocument doc = new XpsDocument(package);


                string inMemoryPackageName = string.Format("memorystream://{0}.xps", Guid.NewGuid());
                Uri packageUri = new Uri(inMemoryPackageName);
                //Add package to PackageStore
                PackageStore.AddPackage(packageUri, package);

                XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum, inMemoryPackageName);
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                writer.Write(WPF2PDF.CreateVisual());

                FixedDocumentSequence fixedDocumentSequence = xpsDoc.GetFixedDocumentSequence();

                
                // Do operations on xpsDoc here
                DocViewer.Document = (IDocumentPaginatorSource)fixedDocumentSequence;



                //xpsDoc.Close();
                //package.Close();               
            }
            catch (Exception ex)
            {
                //System.Console.Out.WriteLine("EXCEPTION: 0x" + string.Format("{0:X}", e.getError()));
            }
        }


        void ConvertToXps(IEnumerable<FixedDocument> fixedDocuments, Stream outputStream)
        {
            var package = Package.Open(outputStream, FileMode.Create);
            var xpsDoc = new XpsDocument(package, CompressionOption.Normal);
            var xpsWriter = xpsDoc.AddFixedDocumentSequence();

            var fixedDocSeq = xpsDoc.GetFixedDocumentSequence();

            // A4 = 210 x 297 mm = 8.267 x 11.692 inches = 793.632 * 1122.432 dots
            fixedDocSeq.DocumentPaginator.PageSize = new Size(793.632, 1122.432);

            foreach (var fixedDocument in fixedDocuments)
            {
                var docWriter = xpsWriter.AddFixedDocument();

                var pageWriter = docWriter.AddFixedPage();

                var image = pageWriter.AddImage(XpsImageType.JpegImageType);

                Stream imageStream = image.GetStream();

                //Write your image to stream

                //Write the rest of your document based on the fixedDocument object
            }
        }

            private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
