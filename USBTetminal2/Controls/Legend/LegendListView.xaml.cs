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
using Microsoft.Research.DynamicDataDisplay;

namespace USBTetminal2.Controls.Legend
{
    /// <summary>
    /// Interaction logic for CustomLegend.xaml
    /// </summary>
    public partial class LegendListView : Border
    {

      //  LegendModel context;
        public LegendListView()
        {
            InitializeComponent();
           // context = new LegendModel();
           // DataContext = context;
        }
       


        private void onLoaded(object sender, RoutedEventArgs e)
        {

           // list.ItemsSource = context.Items;
            //Binding binding = new Binding();
            //binding.Path = new PropertyPath("ItemsSource");
            //binding.Source = context.items;
            //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(list, LineGraph.DataSourceProperty, binding);
         //   list.ItemsSource = context.items; 
        }

        //public void addLegend(LegendItemModel item)
        //{

        //    Binding binding = new Binding();
        //    binding.Source = item;
        //    binding.Path = new PropertyPath("IsChecked");

        //    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //    VisibilityToCheckedConverter converter = new VisibilityToCheckedConverter();
        //    binding.Converter = converter;
        //    binding.Mode = BindingMode.TwoWay;
        //    BindingOperations.SetBinding(item.lineAndMarker.LineGraph, LineGraph.VisibilityProperty, binding);
        //    //   BindingOperations.SetBinding(item.lineAndMarker.MarkerGraph, MarkerPointsGraph.VisibilityProperty, binding);
        //    context.Items.Add(item);
        //    Visibility = System.Windows.Visibility.Visible;
        //}

       // private USBTetminal2.LegendModel.LegendItemModel modelForEditing;
        //private ListViewItem trackForEditing
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (sender.GetType() == typeof(CheckBox))
            //    return;
            //modelForEditing = ((ListViewItem)sender).Content as USBTetminal2.LegendModel.ItemModel; //Casting back to the binded Track
            //renameTB.Visibility = System.Windows.Visibility.Visible;
            //renameTB.Text = "Renameing...";

        }
        private void ThumbDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double newHeight = e.VerticalChange + this.ActualHeight;
            if (newHeight> this.MinHeight)
                this.Height = newHeight;
        }

        private void renameTBVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (renameTB.IsVisible)
            //{
            //    Mouse.Capture(renameTB, CaptureMode.Element);
            //    renameTB.Text = "Renameing...";
            //}
        }

        private void renameTBKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    modelForEditing.Name = renameTB.Text;
            //  //  list.UpdateLayout();
            //    renameTB.Text = "";
            //    renameTB.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else if (e.Key == Key.Escape)
            //{
            //    renameTB.Text = "";
            //    renameTB.Visibility = System.Windows.Visibility.Collapsed;
            //}
        }

        private void renameTBLostFocus(object sender, RoutedEventArgs e)
        {
            //renameTB.Text = "";
            //renameTB.Visibility = System.Windows.Visibility.Collapsed;
        }



        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public void Reset()
        {
          //  context.Clear(); 
           // list.ItemsSource = null;
       //     list.ItemsSource = context.Items;
          //  Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
