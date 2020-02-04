# ConfigurationComplete
ConfigurationComplete是个DotNet下处理配置信息的工具，使用这个工作可以简单的完成配置信息在应用程序的全部操作，包括保存、读取、使用和显示。配置信息的显示使用了[WPFToolkit.Extended](https://archive.codeplex.com/?p=wpftoolkit)的PropertyGripd。对于一般应用，使用本类库，可以在应用中快速的增加配置所有需要的功能。
# 配置信息的定义
配置信息作为dotnet的class必须继承AbstractConfiguration，并且每个属性(Property)必须正确设置相应的Attribute
- **ConfigurationPropertyAttribute** dotnet的Configuration必须设置，**必须**
- **DisplayNameAttribute** 配置在PropertyGrid中显示的名称，可选
- **DispIdAttribute** PropertyGrid中显示的顺序，可选
- **CategoryAttribute** PropertyGrid中显示的分组名称，可选
- **DescriptionAttribute** PropertyGrid底部显示对应配置的描述，可选
- **DefaultValueAttribute** 属性默认值，可选
- **EditorAttribute** 配置在PropertyGrid中的显示和编辑器，可选
- **EnumDisplayNameAttribute** 使用Enum作为配置类型时候，每个枚举类型在编辑器中显示的名称，可选
- **其他各种ValidatorAttribute** 可以使用，可选

下边是一个简单配置的例子
```csharp
    [DisplayName("Test Workflow")]
    public class ProcessConfiguration : AbstractConfiguration
    {
        [DispId(0)]
        [Category("Test Workflow")]
        [DisplayName("Pool Type")]
        [Description("Pool Type")]
        [ConfigurationProperty("PoolSize")]
        [DefaultValue(PoolSize.Pool8)]
        public PoolSize PoolSize
        {
            get
            {
                return GetValue<PoolSize>("PoolSize");
            }
            set
            {
                SetValue("PoolSize", value);
                SetPropertyEnabled("IsPool8Enabled", value == PoolSize.Pool8);
            }
        }
        [DispId(1)]
        [Category("Test Workflow")]
        [DisplayName("Enable 8 Pool Workflow")]
        [Description("Enable 8 Pool Workflow")]
        [DefaultValue(false)]
        [ConfigurationProperty("IsPool8Enabled")]
        public bool IsPool8Enabled
        {
            get
            {
                return GetValue<bool>("IsPool8Enabled");
            }
            set
            {
                SetValue("IsPool8Enabled", value);
            }
        }
    }
    public enum PoolSize
    {
        [EnumDisplayName("8 Pool")]
        Pool8,
        [EnumDisplayName("48 Pool")]
        Pool48
    }
```
## 支持类型
目前支持的属性类型，包括string, boolean, int, float, double, enum, Datetime，其他更多类型比如Color等目前还没有进行测试
# 配置的使用
使用配置类型的Save、Load和相应的Property就可以完成配置的保存、读取和各个属性的使用。可以参考Demo程序查看具体的使用例子。
## 保存和读取
配置信息默认使用dotnet的Configuration已ConfigurationSection形式保存在程序config文件中。如果要使用ini文件保存，请将Serialization设置成IniFileConfigurationSerialization。
设置新的Serialization或者自定义IConfigurationSerialization，可以将配置信息保存在其他位置，比如其他文件或者数据库等。
## 配置间的关系
配置属性间存在关系，一个属性为特定值时候，其他属性可能没有意义，不想用户进行编辑，在属性写方法中可以使用SetPropertyEnabled方法将其他属性禁用
## 显示
配置信息的显示使用了[WPFToolkit.Extended](https://archive.codeplex.com/?p=wpftoolkit)的PropertyGripd