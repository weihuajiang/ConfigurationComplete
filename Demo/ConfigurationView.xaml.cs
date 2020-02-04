using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Huarui
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        ConfigurationCollection configurations;
        public ConfigurationView()
        {
            InitializeComponent();
            configurations = Context.Configurations;
            ConfigList.ItemsSource = configurations;
            this.DataContext = configurations;
            //Context.Configurations.ConfigurationPropertyChanged += new PropertyChangedEventHandler(Configurations_ConfigurationPropertyChanged);
            AddMonitor(configurations);
        }

        void AddMonitor(ConfigurationCollection configs)
        {
            configs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Configurations_CollectionChanged);
            foreach (IConfiguration config in configs)
            {
                if (config is ConfigurationCollection)
                {
                    AddMonitor(config as ConfigurationCollection);
                }
            }
        }

        void Configurations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object obj in e.NewItems)
                {
                    if (obj is ConfigurationCollection)
                    {
                        AddMonitor(obj as ConfigurationCollection);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object obj in e.OldItems)
                {
                    if (obj is ConfigurationCollection)
                    {
                        (obj as ConfigurationCollection).CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Configurations_CollectionChanged);
                    }
                }
            }
            if (ConfigList.SelectedValue != null && ConfigList.SelectedItem is IConfiguration)
            {
                if (ContainerConfiguration((ConfigurationCollection)ConfigList.ItemsSource, (IConfiguration)ConfigList.SelectedValue))
                {
                    propertyGrid1.SelectedObject = null;
                    propertyGrid1.SelectedObject = ConfigList.SelectedValue;
                }
                else
                {
                    propertyGrid1.SelectedObject = null;
                }
            }
            if (ConfigList.SelectedItem == null)
            {
                if (configurations.Count > 0 && ConfigList.SelectedItem == null)
                {
                    var cfg = configurations[0];
                    var tvItem = ConfigList.ItemContainerGenerator.ContainerFromItem(configurations[0]) as TreeViewItem;
                    if (tvItem == null)
                        return;
                    if (cfg is ConfigurationCollection)
                    {
                        tvItem.IsExpanded = true;
                    }
                    tvItem.IsSelected = true;
                }
            }
        }
        bool ContainerConfiguration(ConfigurationCollection collection, IConfiguration value)
        {
            if (collection.Contains(value))
                return true;
            foreach (IConfiguration c in collection)
            {
                if (c is ConfigurationCollection)
                {
                    if (ContainerConfiguration((ConfigurationCollection)c, value))
                        return true;
                }
            }
            return false;
        }


        private void OnSave(object sender, RoutedEventArgs e)
        {
            SaveChangeInof(configurations);
            configurations.Save();
        }
        private void SaveChangeInof(object config)
        {
            if (config is ConfigurationCollection)
            {
                var cs = config as ConfigurationCollection;
                foreach (var c in cs)
                    SaveChangeInof(c);
            }
            else if (config is AbstractConfiguration)
            {
                var c = config as AbstractConfiguration;
                string name = config.GetType() + "";
                try
                {
                    name = (config.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute).DisplayName;
                }
                catch (Exception e) { }
                foreach (var t in c.Changes)
                {
                    string property = t.Item;
                    try
                    {
                        property = (config.GetType().GetProperty(property).GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute).DisplayName;
                    }
                    catch (Exception e) { }
                    Console.WriteLine(string.Format("[{0}-{1} changed from {2} to {3}",name, property, t.OldValue, t.NewValue));
                }
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            configurations.Load();
            propertyGrid1.Update();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                propertyGrid1.SelectedObject = e.NewValue;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ConfigList.IsVisible)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(100);
                    Dispatcher.InvokeAsync(() =>
                    {
                        if (configurations.Count > 0 && ConfigList.SelectedItem == null)
                        {
                            var cfg = configurations[0];
                            var tvItem = ConfigList.ItemContainerGenerator.ContainerFromItem(configurations[0]) as TreeViewItem;
                            if (tvItem == null)
                                return;
                            if (cfg is ConfigurationCollection)
                            {
                                tvItem.IsExpanded = true;
                            }
                            tvItem.IsSelected = true;
                        }
                    });
                });
            }
        }
    }
}
