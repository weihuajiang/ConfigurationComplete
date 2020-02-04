using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{
    public class GroupVisiblity
    {
        public bool Operator { get; set; } = true;
        public bool Admin { get; set; } = true;
        public bool Service { get; set; } = true;
        public bool IsVisible(UserGroup group)
        {
            switch (group)
            {
                case UserGroup.Operator:
                    return Operator;
                case UserGroup.Admin:
                    return Admin;
                case UserGroup.Service:
                    return Service;
                default:
                    return false;
            }
        }

        public static GroupVisiblity ALL { get; } = new GroupVisiblity();
    }
}
