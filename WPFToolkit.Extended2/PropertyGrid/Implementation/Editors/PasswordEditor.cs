using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Windows.Controls.PropertyGrid.Editors
{
    public class PasswordEditor : ITypeEditor
    {
        PasswordBox Editor = new PasswordBox();
        PropertyItem item;
        Brush background;
        public PasswordEditor()
        {
            Editor.BorderThickness = new Thickness(0);
            Editor.PasswordChanged += Editor_PasswordChanged;
            //Editor.IsEnabledChanged += Editor_IsEnabledChanged;
            background = Editor.Background;
        }

        private void Editor_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Editor.IsEnabled)
            {
                Editor.Background = background;
            }
            else
                Editor.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void Editor_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (item != null)
            {
                item.Value = Editor.Password;
            }
        }

        public void Attach(PropertyItem propertyItem)
        {
            item = propertyItem;
            try
            {
                string value = (string)propertyItem.PropertyDescriptor.GetValue(item.Instance);
                Editor.Password = value;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public FrameworkElement ResolveEditor()
        {
            return Editor;
        }
    }
}
