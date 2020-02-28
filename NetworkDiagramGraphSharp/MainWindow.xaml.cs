using GraphSharp.Controls;
using QuickGraph;
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

namespace NetworkDiagramGraphSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    internal class ScreenVertex
    {
        public string Hello { get; set; }
    }

    internal class ScreenEdge : Edge<ScreenVertex>
    {
        public ScreenEdge(ScreenVertex source, ScreenVertex target)
            : base(source, target)
        {
        }
    }

    internal class ScreenLayout : GraphLayout<ScreenVertex, ScreenEdge, ScreenGraph>
    {
    }

    internal class ScreenGraph : BidirectionalGraph<ScreenVertex, ScreenEdge>
    {
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // build graph
            var screenGraph = new ScreenGraph();
            var screenVertex1 = new ScreenVertex { Hello = "1" };
            var screenVertex2 = new ScreenVertex { Hello = "2" };
            var screenVertex3 = new ScreenVertex { Hello = "3" };
            screenGraph.AddVertex(screenVertex1);
            screenGraph.AddVertex(screenVertex2);
            screenGraph.AddVertex(screenVertex3);
            screenGraph.AddEdge(new ScreenEdge(screenVertex1, screenVertex2));
            screenGraph.AddEdge(new ScreenEdge(screenVertex2, screenVertex1));
            screenGraph.AddEdge(new ScreenEdge(screenVertex1, screenVertex3));
            screenGraph.AddEdge(new ScreenEdge(screenVertex3, screenVertex1));
            screenGraph.AddEdge(new ScreenEdge(screenVertex3, screenVertex2));
            ScreenLayout.Graph = screenGraph;

            // get connections for a particular vertex
            IEnumerable<ScreenEdge> inEdges = screenGraph.InEdges(screenVertex3);
            IEnumerable<ScreenEdge> outEdges = screenGraph.OutEdges(screenVertex3);
        }
    }
}