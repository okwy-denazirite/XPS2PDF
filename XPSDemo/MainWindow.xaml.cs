using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Win32;
using PdfSharp;
using PdfSharp.Xps;

namespace XPSDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FixedDocument fixedDoc = new FixedDocument();
        MainWindowViewModel mwvm = new MainWindowViewModel();

        
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = mwvm;
        }
        

        //public ListBox TheListBox
        //{
        //    get { return theListBox; }
        //    set { theListBox = value;
        //        OnPropertyChanged("TheListBox");
        //    }
        //}

    

        public void CreateMyWPFControlReport(MainWindowViewModel usefulDataVM)
        {
     
            //Set up the WPF Control to be printed
            UserControl1 controlToPrint;
            controlToPrint = new UserControl1();
            //ListBox lbb = new UserControl1().lb1;
            controlToPrint.DataContext = usefulDataVM;


            //Setter setter = new Setter(TextBlock.BackgroundProperty, Brushes.Red);
            //Setter setter2 = new Setter(TreeViewItem.BackgroundProperty, Brushes.Red);
            //Style txttyle = new Style(typeof(TextBlock));
            //txttyle.Setters.Add(setter);

            //// txttyle.Setters.Add(setter);
            //controlToPrint.Resources.Add(object key, Style.Setters.Add(setter));

            ListBox lstbx = new ListBox();
            lstbx.ItemsSource = usefulDataVM.TheItems;
            controlToPrint.lb1.BorderBrush = null;
            lstbx.BorderBrush = null;
            // controlToPrint.GetType(TextBlock).BorderBrush = null;

            //Style rowStyle = new Style(typeof(TextBlock));



            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();
            

          
         


            //Create first page of document
       //     fixedPage.Children.Add(controlToPrint);
            fixedPage.Children.Add(lstbx);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);




            //// create XPS file based on a WPF Visual, and store it in a memorystream
            //MemoryStream lMemoryStream = new MemoryStream();
            //Package package = Package.Open(lMemoryStream, FileMode.Create);
            //// XpsDocument doc = new XpsDocument(package);


            //string inMemoryPackageName = string.Format("memorystream://{0}.xps", Guid.NewGuid());
            //Uri packageUri = new Uri(inMemoryPackageName);
            ////Add package to PackageStore
            //PackageStore.AddPackage(packageUri, package);

            //XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum, inMemoryPackageName);
            //XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
            ////writer.Write(WPF2PDF.CreateVisual());
            //writer.Write(fixedDoc.DocumentPaginator);
            //writer.Close();


           
            //Create any other required pages here

            //View the document

            DocViewer.Document = fixedDoc;
           

            //PrintDialog printDlg = new PrintDialog();
            //Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight - 100);
            //controlToPrint.Measure(pageSize);
            //controlToPrint.Arrange(new Rect(10, 50, pageSize.Width, pageSize.Height));


            //// write to PDF file
            //string tempFilename = "temp.xps";
            //File.Delete(tempFilename);
            //XpsDocument xpsDoc = new XpsDocument(tempFilename, FileAccess.Write);
            //XpsDocumentWriter xWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
            //xWriter.Write(fixedDoc.DocumentPaginator);
            //xpsDoc.Close();
            //SaveFileDialog saveFileDialog = new SaveFileDialog
            //{
            //    DefaultExt = ".pdf",
            //    Filter = "PDF Documents (.pdf)|*.pdf"
            //};
            //bool? result = saveFileDialog.ShowDialog();

            //if (result != true) return;
            //PdfSharp.Xps.XpsConverter.Convert(tempFilename, saveFileDialog.FileName, 0);
            ////PdfSharp.Xps.XpsConverter.Convert(package, );
        }

        public static void ExportVisualAsPdf(MainWindowViewModel usefulDataVM)
        {
            //Set up the WPF Control to be printed
            UserControl1 controlToPrint;
            controlToPrint = new UserControl1();
            controlToPrint.DataContext = usefulDataVM;


            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

    

            fixedPage.Children.Add(controlToPrint);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);

           

            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "PDF Documents (.pdf)|*.pdf"
            };

            bool? result = sfd.ShowDialog();

            if (result != true) return;



                MemoryStream memoryStream = new MemoryStream();
                System.IO.Packaging.Package package = System.IO.Packaging.Package.Open(memoryStream, FileMode.Create);
                XpsDocument xpsDocument = new XpsDocument(package);
                XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

                xpsDocumentWriter.Write(fixedPage);
                xpsDocument.Close();
                package.Close();

                var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(memoryStream);
                //public static void Convert(Stream xpsInStream, Stream pdfOutStream, bool closePdfStream);
               XpsConverter.Convert(pdfXpsDoc, sfd.FileName, 0);
                Process.Start(sfd.FileName);

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            mwvm.TheItems.Add("Okwy");
            ExportVisualAsPdf(mwvm);
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            mwvm.TheItems.Add("Okwy");
            CreateMyWPFControlReport(mwvm);
        }
    }
}
