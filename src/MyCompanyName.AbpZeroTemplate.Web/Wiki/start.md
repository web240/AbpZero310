# 目录
## 一、开始
## 二、启动
## 三、菜单配置
## 四、谷歌字体
## 五、配置权限
## 六、菜单加权限
## 七、控制器加权限
## 八、实例-菜单添加
## 九、实例-商品分类管理-列表
## 十、实例-商品分类管理-新建
## 十一、实例-商品分类管理-数据检验
## 十二、实例-商品分类管理-编辑分类
## 十三、实例-商品分类管理-删除分类
## 十四、实例-商品分类管理-分类搜索及分页
## 十五、实例-商品分类管理-权限控制

# 一、开始
## 1、下载
## 2、还原Nuget包
## 3、设置Web为默认启动项目
## 4、修改配置文件
```
<add name="Default" connectionString="Server=.; Database=AbpZero3.1.0; User ID=sa;Password=sa" providerName="System.Data.SqlClient" />
```
## 5、创建数据库
```
Update-Database -Verbose
```

# 二、启动
## 1、VS直接启动
## 2、IIS启动
## 3、登录
系统默认创建2个用户  

默认用户名：admin 密码：123qwe  

租户：Default  默认用户名：admin 密码：123qwe  

# 三、菜单配置
## 1、打开文件MpaNavigationProvider.cs

【..\MyCompanyName.AbpZeroTemplate.Web\Areas\Mpa\Startup\MpaNavigationProvider.cs】
添加代码：
```
.AddItem(new MenuItemDefinition(
　　PageNames.App.Tenant.Test,//一个常量，控制菜单是否被选中
　　L("Test"),//菜单显示名称，在语言文件中配置
　　url: "Mpa/Test",//菜单路径
　　icon: "icon-globe",//菜单图标
))
```
## 2、打开文件PageNames.cs

【..\MyCompanyName.AbpZeroTemplate.Web\App_Start\Navigation\PageNames.cs】
在代码中添加一个常量
```
public static class Tenant
{
    public const string Dashboard = "Dashboard.Tenant";
    public const string Settings = "Administration.Settings.Tenant";
    public const string Test = "Test";//这里是添加的常量
}
```
## 3、打开语言文件AbpZeroTemplate-zh-CN.xml

【..\MyCompanyName.AbpZeroTemplate.Core\Localization\AbpZeroTemplate\AbpZeroTemplate-zh-CN.xml】
在最后添加一个键值对
```
<text name="Test" value="测试" />
```

保存生成，刷新页面即可显示，现在点击菜单会报404错误，这是因为我们还没有添加对应的控制器
在【..\MyCompanyName.AbpZeroTemplate.Web\Areas\Mpa\Controllers】下添加一个Test控制器，并创建Index视图

## 4、Index视图修改

此时Index视图中的代码改成这样
```
@using MyCompanyName.AbpZeroTemplate.Web.Navigation

@{
    ViewBag.CurrentPageName = PageNames.App.Tenant.Test;//上面所定义就是这个常量，作用就是选中菜单时会高亮
}

<h2>测试页面</h2>
```
## 5、生成项目

最后生成项目，刷新页面。此时不管以哪个用户登录都会显示此菜单，而实际项目中一般都会让不同角色的用户看到不同的菜单，这时就需要给菜单添加权限，只有拥有此权限的用户才显示菜单，请查看后续文章

# 四、谷歌字体
## 1、打开jtable.css文件

【..\MyCompanyName.AbpZeroTemplate.Web\libs\jquery-jtable\themes\metro\blue\jtable.css】
并注释掉谷歌相关的连接，然后保存
## 2、打开StylePaths.cs文件

【..\MyCompanyName.AbpZeroTemplate.Web\App_Start\Bundling\StylePaths.cs】
把jtable.min.css替换为jtable.css，保存并生成  

# 五、配置权限
## 1、打开AppPermissions.cs

【..\MyCompanyName.AbpZeroTemplate.Core\Authorization\AppPermissions.cs】
文件最后添加如下代码：
```
public const string Pages_Administration_Test = "Pages.Administration.Test";//权限路径（Pages【页面】-Administration【管理】-Test【测试】
```
这样添加并不会自动显示在页面中，还需写代码获取
 
## 2、打开AppAuthorizationProvider.cs

【..\MyCompanyName.AbpZeroTemplate.Core\Authorization\AppAuthorizationProvider.cs】
SetPermissions方法最后添加如下代码：
```
administration.CreateChildPermission(AppPermissions.Pages_Administration_Test, L("Test"));//L("Test")是获取语言文件中的键，这里只配置简体中文
```

## 3、项目生成

生成Web项目，刷新页面

# 六、菜单加权限
## 1\打开文件MpaNavigationProvider.cs

【..\MyCompanyName.AbpZeroTemplate.Web\Areas\Mpa\Startup\MpaNavigationProvider.cs】
继续上次添加菜单的代码，再添加一行
```
.AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Test,//一个常量，控制菜单是否被选中
                    L("Test"),//菜单显示名称，在语言文件中配置
                    url: "Mpa/Test",//菜单路径
                    icon: "icon-globe",//菜单图标
                    requiredPermissionName: AppPermissions.Pages_Administration_Test//菜单权限，登录用户所在角色有此权限才会显示出来
                    ))
```
## 2、生成项目

生成项目，刷新页面，这时测试菜单不显示了
 
## 3、配置权限

切换到角色功能，修改Admin角色，在权限列表中勾上测试，然后保存。再刷新页面，你会发现测试页面已经显示出来了。
# 七、控制器加权限
## 1、打开TestController.cs

【..\MyCompanyName.AbpZeroTemplate.Web\Areas\Mpa\Controllers\TestController.cs】
控制器上面加入注解，如下代码：
```
[AbpMvcAuthorize(AppPermissions.Pages_Administration_Test)]
    public class TestController : Controller
    {
        // GET: Mpa/Test
        public ActionResult Index()
        {
            return View();
        }
    }
```
## 2、生成项目

生成项目，访问http://localhost:8019/Mpa/Test这个控制器，并使用"Default"租户管理员登录，这时会发现系统一直登录不了，因为我没有给"Default"租户中的管理员分配"测试"这个权限，所以系统判断"Default"租户中的管理员没有"测试"这个权限时，跳转再次要求登录。
 
## 3、分配权限

现在让"Default"租户中的管理员也拥有"测试"权限，访问http://localhost:8019/Mpa，并用"Default"租户管理员登录，切换到角色功能，修改Admin角色，勾选上"测试"权限，保存刷新页面，现在可以看到菜单了。再次访问http://localhost:8019/Mpa/Test这个控制器也是没有问题的。

# 八、实例-菜单添加
## 1、打开语言文件

【..\MyCompanyName.AbpZeroTemplate.Core\Localization\AbpZeroTemplate\AbpZeroTemplate-zh-CN.xml】
添加2个键值对，如下：
```
<text name="Shop" value="商店" />
<text name="CategoryManager" value="分类管理" />
```

## 2、打开文件PageNames.cs

【..\MyCompanyName.AbpZeroTemplate.Web\App_Start\Navigation\PageNames.cs】
在Common类下添加2个常量：
```
public const string Shop = "Shop";
public const string Category = "Category";
```

## 3、打开文件MpaNavigationProvider.cs

【..\MyCompanyName.AbpZeroTemplate.Web\Areas\Mpa\Startup\MpaNavigationProvider.cs】
添加如下代码到测试菜单下：
```
.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Shop,//一个常量，控制菜单是否被选中
                    L("Shop"),//菜单显示名称，在语言文件中配置
                    icon: "icon-globe"//菜单图标
                    ).AddItem(new MenuItemDefinition(
                        //子菜单
                        PageNames.App.Common.Category,
                        L("CategoryManager"),
                        url:"Mpa/Category",
                        icon: "icon-globe"
                        ))
                )
```
 

## 4、生成项目

生成项目，刷新页面
# 九、实例-商品分类管理-列表
# 十、实例-商品分类管理-新建
# 十一、实例-商品分类管理-数据检验
# 十二、实例-商品分类管理-编辑分类
# 十三、实例-商品分类管理-删除分类
# 十四、实例-商品分类管理-分类搜索及分页
# 十五、实例-商品分类管理-权限控制


# 参考
[网址](http://www.cnblogs.com/shensigzs/category/933348.html)
