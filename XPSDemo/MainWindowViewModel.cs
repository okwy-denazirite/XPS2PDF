using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XPSDemo
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<string> theItems = new ObservableCollection<string>();
     
        private object dummyNode = null;
        public string SelectedImagePath { get; set; }
      
        //DocumentViewer documentViewer1 = new DocumentViewer();

        public MainWindowViewModel()
        {

            theItems.Add("Bolu");
            theItems.Add("Bola");
            theItems.Add("Bisi");
            theItems.Add("Bala");
            theItems.Add("Bolojo");
        }

        //public FixedDocument FixedDoc
        //{
        //    get { return fixedDoc; }
        //    set
        //    {
        //        fixedDoc = value;
        //        OnPropertyChanged("FixedDoc");
        //    }
        //}

        

        //public DocumentViewer DocumentViewer1
        //{
        //    get { return documentViewer1; }
        //    set { documentViewer1 = value; }
        //}

        public ObservableCollection<string> TheItems
        {
            get
            {
                return theItems;
            }
            set
            {
                theItems = value;
                OnPropertyChanged("TheItems");
            }
        }

        //public void CreateMyWPFControlReport(MyWPFControlDataSource usefulData)
     }
}
