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
using System.Windows.Shapes;

namespace Huarui
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            var config = Context.GetConfiguration<DatabaseMaintenanceSetting>();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} = {1}\n", "Backup Enabled", config.IsBackupEnabled));
            sb.Append(string.Format("{0} = {1}\n", "Automatic Backup Interval (Days)", config.BackupInterval));
            sb.Append(string.Format("{0} = {1}\n", "Backup Destination", config.BackupDestination));
            sb.Append(string.Format("{0} = {1}\n", "Archivie Enabled", config.IsArchiveEnabled));
            sb.Append(string.Format("{0} = {1}\n", "Automatic Backup Interval (Days)", config.BackupInterval));
            ConfigText.Text = sb.ToString();
        }
    }
}
