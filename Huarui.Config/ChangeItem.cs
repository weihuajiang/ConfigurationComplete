using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui.Config
{
    /// <summary>
    /// change information of configuration item
    /// </summary>
    public class ChangeItem
    {
        string _item;
        object _oldValue;
        object _newValue;
        /// <summary>
        /// configuration name
        /// </summary>
        public string Item
        {
            get { return _item; }
        }
        public object OldValue
        {
            get { return _oldValue; }
        }
        public object NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        public ChangeItem(string item, object oldValue, object newValue)
        {
            _item = item;
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
}
