using System;
using System.Windows;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Ssepan.Application.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PropertyDialog : Window
    {
        private Action refreshAction = default(Action);
        private Object settingsObject = default(Object);
        //private Person Person = new Person();
        //private Vehicle Vehicle1 = new Vehicle();
        //private Vehicle Vehicle2 = new Vehicle();
        //private Place Place = new Place();

        //// names must match the data members
        private object[] ItemArray = { "Person", "Vehicle1", "Vehicle2", "Place" };

        public PropertyDialog()
        {
            InitializeComponent();

            if (this.PropertyGrid1 == null)
            {
                this.PropertyGrid1 = new WpfPropertyGrid();
            }
            //this.PropertyGrid1.HelpVisible = true;
            //this.PropertyGrid1.ToolbarVisible = true;

            this.Loaded += new RoutedEventHandler(PropertyDialog_Load);
        }

        public PropertyDialog(Object settingsControllerModelSettings, Action modelControllerRefreshAction) :
            this()
        {
            refreshAction = modelControllerRefreshAction;
            settingsObject = settingsControllerModelSettings;

            this.PropertyGrid1.SelectedObject = settingsObject;
            this.PropertyGrid1.RefreshPropertyList();
        }

        private void PropertyDialog_Load(object sender, System.EventArgs e) 
		{
			try 
			{
                if (this.Owner != null)
                {
                    // Set this Form's Text + Icon properties by using values from the parent form
                    this.Title = "Properties - " + this.Owner.Title;
                    this.Icon = this.Owner.Icon;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //// Special handling for vehicle type change
        //void Vehicle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    this.PropertyGrid1.RefreshPropertyList();
        //}

        //private void SingleSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.ItemList.ItemTemplate = this.Resources["RadioButtons"] as DataTemplate;
        //    //this.ItemList.ItemsSource = this.ItemArray;
        //    this.PropertyGrid1.SelectedObject = null;
        //}
        //private void MultiSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.ItemList.ItemTemplate = this.Resources["CheckBoxes"] as DataTemplate;
        //    //this.ItemList.ItemsSource = this.ItemArray;
        //    this.PropertyGrid1.SelectedObject = null;
        //}
        //private void NoSelection_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.ItemList.ItemTemplate = null;
        //    //this.ItemList.ItemsSource = new string[] { "(none)" };
        //    this.PropertyGrid1.SelectedObject = null;
        //}

        //private void Item_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (e.Source is RadioButton)
        //    {
        //        object selected = this.GetType().GetField((e.Source as RadioButton).Content.ToString(), 
        //            System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this);
        //        this.PropertyGrid1.SelectedObject = selected;
        //    }
        //    else if (e.Source is CheckBox && this.Radio2.IsChecked.GetValueOrDefault())
        //    {
        //        ArrayList selected = new ArrayList();

        //        for (int i = 0; i < ItemList.Items.Count; i++)
        //        {
        //            ContentPresenter container = ItemList.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
        //            DataTemplate dataTemplate = container.ContentTemplate;
        //            CheckBox chk = (CheckBox)dataTemplate.FindName("chk", container);
        //            if (chk.IsChecked.GetValueOrDefault())
        //            {
        //                object item = this.GetType().GetField(chk.Content.ToString(), 
        //                    System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this);
        //                selected.Add(item);
        //            }
        //        }
        //        this.PropertyGrid1.SelectedObjects = selected.ToArray();
        //    }
        //}
    }
}
