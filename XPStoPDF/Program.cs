using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.IO.Packaging;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Documents;
using System.Windows.Controls;

namespace XPStoPDF
{

    public static class WPF2PDF
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

    class Program
    {
        private IDocumentPaginatorSource docViewer;

        static void Main(string[] args)
        {
            

            try
            {
                // create XPS file based on a WPF Visual, and store it in a memorystream
                MemoryStream lMemoryStream = new MemoryStream();
                Package package = Package.Open(lMemoryStream, FileMode.Create);
                XpsDocument doc = new XpsDocument(package);
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                writer.Write(WPF2PDF.CreateVisual());
             
            

                // now open this XPS stream in the NiXPS SDK, and export it as pdf
                //NOPackage lPackage = NOPackage.readPackageFromBuffer("file.xps", lMemoryStream.GetBuffer(), (uint)lMemoryStream.Length);
                //NOProgressReporter lReporter = new NOProgressReporter();
                //lPackage.getDocument(0).exportToPDF("file.pdf", lReporter);
                //NOPackage.destroyPackage(ref lPackage);

              

                //Create URI for Xps Package
                //Any Uri will actually be fine here. It acts as a place holder for the
                //Uri of the package inside of the PackageStore
                string inMemoryPackageName = string.Format("memorystream://{0}.xps", Guid.NewGuid());
                Uri packageUri = new Uri(inMemoryPackageName);

                //Add package to PackageStore
                PackageStore.AddPackage(packageUri, package);

                XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum, inMemoryPackageName);
                FixedDocumentSequence fixedDocumentSequence = xpsDoc.GetFixedDocumentSequence();

                PrintDialog dlg = new PrintDialog();
                dlg.PrintDocument(fixedDocumentSequence.DocumentPaginator, "Document title");


                PackageStore.RemovePackage(packageUri);
                xpsDoc.Close();

                doc.Close();
                package.Close();
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("EXCEPTION: 0x" + string.Format("{0:X}", e.Message));
            }
            Console.WriteLine("Conversion to WPF done!");
            Console.ReadKey();
        }

    }
}
