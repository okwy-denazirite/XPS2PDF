using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace TestGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            CreateGraph();
        }


        private readonly BidirectionalGraph<object, IEdge<object>> _graph = new BidirectionalGraph<object, IEdge<object>>();

        public IBidirectionalGraph<object, IEdge<object>> Graph

        {

            get { return _graph; }

        }

        private string _layoutAlgorithm = "EfficientSugiyama";

        public string LayoutAlgorithm

        {

            get { return _layoutAlgorithm; }

            set

            {

                if (value != _layoutAlgorithm)

                {

                    _layoutAlgorithm = value;

                }

            }

        }

        private void CreateGraph()

        {

            _graph.Clear();

            SampleVertex obj1 = new SampleVertex("One");

            _graph.AddVertex(obj1);

            SampleVertex obj2 = new SampleVertex("Two");

            _graph.AddVertex(obj2);

            SampleVertex obj3 = new SampleVertex("Three");

            _graph.AddVertex(obj3);

            _graph.AddEdge(new Edge<object>(obj1, obj2));

            _graph.AddEdge(new Edge<object>(obj1, obj3));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)

        {

            var menuItem = sender as MenuItem;

            var vertex = menuItem.Tag as SampleVertex;

            vertex.Change();

        }
    }

        public class SampleVertex : INotifyPropertyChanged
        {

            private bool _active;

            private string _text;

            public bool Active

            {

                get { return _active; }

                set

                {

                    _active = value;

                    NotifyChanged("Active");

                }

            }

            public string Text

            {

                get { return _text; }

                set

                {

                    _text = value;

                    NotifyChanged("Text");

                }

            }

            public SampleVertex(string text)

            {

                Text = text;

            }

            public void Change()

            {

                Active = !Active;

            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void NotifyChanged(string propertyName)

            {

                if (PropertyChanged != null)

                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }

        [ValueConversion(typeof(bool), typeof(Brush))]

        public class ActiveConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

            {

                var state = (bool)value;

                if (state)

                {

                    return new SolidColorBrush(Colors.WhiteSmoke);

                }
                else

                {

                    return new SolidColorBrush(Colors.LightSalmon);

                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)

            {

                return null;

            }

        }
    }