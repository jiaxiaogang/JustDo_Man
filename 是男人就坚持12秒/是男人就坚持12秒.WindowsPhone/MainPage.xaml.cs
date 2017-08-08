using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.UI;
//using Windows.Storage;
//using Windows.ApplicationModel;//找秒表。。。

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace 是男人就坚持12秒
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Map map;
        Map map2;

        Block[] visBlocks;//隐藏的格格;
        Block[] visBlocks2;

        List<Block> allBlocks;//所有的格格;
        List<Block> allBlocks2;

        List<string> allTags;//所有的Tag;
        List<string> allTags2;

        List<Block> BlockUplists;//所有将要上升下降的格格;
        List<Block> BlockUplists2;

        List<Block> FuBlockUplists;//所有要落下的格格;
        List<Block> FuBlockUplists2;

        List<BlockRectangle> allBlockRectangles;//所有的UI里的格格;
        List<BlockRectangle> allBlockRectangles2;

        Block[] smallMans;//所有小人可以移动到的位置数据上下文组；
        Block[] smallMans2;

        List<SmallMan> allSmallMans;//所有的小人；
        List<SmallMan> allSmallMans2;

        //创建一个秒表;
        Stopwatch sw = new Stopwatch();

        int level = 1;//关卡计数器;

        int score;//最高分;
        ApplicationDataContainer setting;//存储最高分的键值对;

        List<Theme> themes;//主题;

        List<BlockRectangle> allTapRectangle;//9条通列的点击即变色一下的条条;
        List<BlockRectangle> allTapRectangle2;

        List<SmallMan> allManPhoto;//八个小矮人的集合;
        SmallMan firstMan;//用来接收第一个tap的smallMan;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                //主题list添加
                themes = new List<Theme>();
                themes.Add(new Theme() { Name = "红色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 223, 0, 36)) });
                themes.Add(new Theme() { Name = "绿色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0)) });
                themes.Add(new Theme() { Name = "黄色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)) });
                themes.Add(new Theme() { Name = "蓝色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)) });
                themes.Add(new Theme() { Name = "深蓝", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 139)) });
                themes.Add(new Theme() { Name = "钢蓝", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 176, 196, 222)) });
                themes.Add(new Theme() { Name = "淡蓝", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250)) });
                themes.Add(new Theme() { Name = "黑色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)) });
                themes.Add(new Theme() { Name = "橘色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 233, 113, 24)) });
                themes.Add(new Theme() { Name = "土褐", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 85, 107, 47)) });
                themes.Add(new Theme() { Name = "深绿", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0)) });
                themes.Add(new Theme() { Name = "绿黄", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 173, 255, 47)) });
                themes.Add(new Theme() { Name = "金色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 215, 0)) });
                themes.Add(new Theme() { Name = "米色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 107, 142, 35)) });
                themes.Add(new Theme() { Name = "橄榄", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 0)) });
                themes.Add(new Theme() { Name = "卡其", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 240, 230, 140)) });
                themes.Add(new Theme() { Name = "橙色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 165, 0)) });
                themes.Add(new Theme() { Name = "深橙", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 140, 0)) });
                themes.Add(new Theme() { Name = "暗灰", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 105, 105, 105)) });
                themes.Add(new Theme() { Name = "灰色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)) });
                themes.Add(new Theme() { Name = "深灰", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 169, 169, 169)) });
                themes.Add(new Theme() { Name = "银白", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192)) });
                themes.Add(new Theme() { Name = "白烟", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)) });
                themes.Add(new Theme() { Name = "栗色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 128, 0, 0)) });
                themes.Add(new Theme() { Name = "白色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)) });
                themes.Add(new Theme() { Name = "紫色", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 128, 0, 128)) });
                themes.Add(new Theme() { Name = "中紫", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 147, 112, 219)) });
                themes.Add(new Theme() { Name = "洋红", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)) });
                themes.Add(new Theme() { Name = "粉红", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 192, 203)) });
                themes.Add(new Theme() { Name = "浅粉", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 182, 193)) });
                themes.Add(new Theme() { Name = "洋红", ThemeBackground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)) });
                //左边彩条的绑定和tap事件;
                lvTheme.ItemsSource = themes;
                lvTheme.Tapped += lvTheme_Tapped;
                //右边彩条的绑定和tap事件;
                lvThemeBackGround.ItemsSource = themes;
                lvThemeBackGround.Tapped += lvThemeBackGround_Tapped;

                //存储setting;
                setting = ApplicationData.Current.LocalSettings;
                if (!setting.Values.ContainsKey("score"))
                {
                    setting.Values["score"] = "0";
                }
                score = Convert.ToInt32((string)setting.Values["score"]);

                JiRu.Text = "最高记录:" + score.ToString() + "步";

                //上栏隐藏全屏游戏
                StatusBar statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();

                map = new Map();
                map2 = new Map();
                allBlocks = new List<Block>();
                allBlocks2 = new List<Block>();
                allTags = new List<string>();
                allTags2 = new List<string>();
                allBlockRectangles = new List<BlockRectangle>();
                allBlockRectangles2 = new List<BlockRectangle>();
                smallMans = new Block[5];
                smallMans2 = new Block[5];
                allSmallMans = new List<SmallMan>();
                allSmallMans2 = new List<SmallMan>();
                allTapRectangle = new List<BlockRectangle>();
                allTapRectangle2 = new List<BlockRectangle>();
                allManPhoto = new List<SmallMan>();
                //添加小人及背景色
                for (int i = 0; i < 8; i++)
                {
                    SmallMan smallman = new SmallMan(i);
                    gridMansPhoto.Children.Add(smallman);
                    Grid.SetColumn(smallman, i);
                    allManPhoto.Add(smallman);
                    smallman.Tag = (1000 + i).ToString();//这个可能用不到因为没有绑定到block;但先写上万一有用呢;
                    smallman.Tapped += smallman_Tapped;
                }
                for (int i = 0; i < 5; i++)
                {
                    //外层代码添加GridControlLeftRight到9个Rectangles.用来接收tapped事件控制左右走小人；
                    BlockRectangle recTapped = new BlockRectangle();
                    recTapped.Opacity = 0;
                    GridControlLeftRight.Children.Add(recTapped);
                    Grid.SetColumn(recTapped, i);
                    recTapped.Tag = i.ToString();
                    recTapped.Tapped += recTapped_Tapped;
                    allTapRectangle.Add(recTapped);

                    //外层代码2添加GridControlLeftRight2到9个Rectangles.用来接收tapped事件控制左右走小人；
                    BlockRectangle recTapped2 = new BlockRectangle();
                    recTapped2.Opacity = 0;
                    GridControlLeftRight2.Children.Add(recTapped2);
                    Grid.SetColumn(recTapped2, i);
                    recTapped2.Tag = i.ToString();
                    recTapped2.Tapped += recTapped2_Tapped;
                    allTapRectangle2.Add(recTapped2);

                    //内层循环代码给map里放小人还有格格;
                    for (int j = 0; j < 4; j++)
                    {
                        BlockRectangle rectangle = new BlockRectangle();

                        GridMap.Children.Add(rectangle);
                        Grid.SetRow(rectangle, j);
                        Grid.SetColumn(rectangle, i);

                        rectangle.DataContext = map[i, j];

                        rectangle.Tag = i.ToString() + j.ToString();
                        allTags.Add(rectangle.Tag);//allTags这个list似乎并没有用到。。。
                        allBlockRectangles.Add(rectangle);

                        //rectangle.Tapped += rectangle_Tapped;

                        SmallMan smallMan = new SmallMan();

                        GridSmallMan.Children.Add(smallMan);
                        Grid.SetRow(smallMan, j);
                        Grid.SetColumn(smallMan, i);

                        smallMan.DataContext = map[i, j];

                        smallMan.Tag = i.ToString() + j.ToString();
                        allSmallMans.Add(smallMan);
                        allBlocks.Add(map[i, j]);//初始化所有的格格list；或者不用这个。而转用smallManlist;
                        ///////////////////////////////////////////////////////////////////
                        //以下为第二个小地图的添加/////////////////////////////////////////
                        ///////////////////////////////////////////////////////////////////
                        BlockRectangle rectangle2 = new BlockRectangle();

                        GridMap2.Children.Add(rectangle2);
                        Grid.SetRow(rectangle2, j);
                        Grid.SetColumn(rectangle2, i);

                        rectangle2.DataContext = map2[i, j];

                        rectangle2.Tag = i.ToString() + j.ToString();
                        allTags2.Add(rectangle2.Tag);//allTags这个list似乎并没有用到。。。
                        allBlockRectangles2.Add(rectangle2);

                        //rectangle.Tapped += rectangle_Tapped;

                        SmallMan smallMan2 = new SmallMan();

                        GridSmallMan2.Children.Add(smallMan2);
                        Grid.SetRow(smallMan2, j);
                        Grid.SetColumn(smallMan2, i);

                        smallMan2.DataContext = map2[i, j];

                        smallMan2.Tag = i.ToString() + j.ToString();
                        allSmallMans2.Add(smallMan2);
                        allBlocks2.Add(map2[i, j]);//初始化所有的格格list；或者不用这个。而转用smallManlist;
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            double driveH = GameOverGrid.ActualHeight;
            lvTheme.Height = driveH;
            lvThemeBackGround.Height = driveH;
        }

        /// <summary>
        /// GridControlLeftRight表格里的9个Rectangle的tap事件;控制小人移动的事件；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void recTapped_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //20140611上午添加声音bo.mp3
            bo.Play();//·····················································
            BlockRectangle rec1 = sender as BlockRectangle;
            int x = Convert.ToInt32(rec1.Tag);
            rec1.Opacity = 0.3;
            await Task.Delay(100);
            rec1.Opacity = 0;
            //遍历出活着的小人命名为b2;
            Block b2 = new Block();
            foreach (Block item in smallMans)
            {
                if (item.IsVisSmallMan == true)
                {
                    b2 = item;
                }
            }

            if (b2 == null)
            {
                foreach (Block item in allBlocks)
                {
                    item.IsVisSmallMan = false;
                }
                smallMans[2].IsVisSmallMan = true;
                b2 = smallMans[2];
            }
            else
            {
                foreach (Block item in allBlocks)
                {
                    item.IsVisSmallMan = false;
                }
                b2.IsVisSmallMan = true;
            }

            //触摸的grid.tag就是1-12；
            if (x == b2.X)
            {
                return;
            }
            else if (x > b2.X)
            {
                b2.IsVisSmallMan = false;
                //await Task.Delay(20); //第一步让小人的位置隐藏，最后一步让点击的点显示。。中间步遍历循环控制。·····································
                for (int i = b2.X + 1; i < x; i++)
                {
                    map[i, smallMans[i].Y].IsVisSmallMan = true;
                    await Task.Delay(100);
                    map[i, smallMans[i].Y].IsVisSmallMan = false;
                }//200毫秒
                //await Task.Delay(20);
                map[x, smallMans[x].Y].IsVisSmallMan = true;
            }
            else if (x < b2.X)
            {
                b2.IsVisSmallMan = false;
                //await Task.Delay(20);
                for (int i = b2.X - 1; i > x; i--)
                {
                    map[i, smallMans[i].Y].IsVisSmallMan = true;
                    await Task.Delay(100);
                    map[i, smallMans[i].Y].IsVisSmallMan = false;
                }
                //await Task.Delay(20);
                map[x, smallMans[x].Y].IsVisSmallMan = true;
            }


            foreach (Block item in allBlocks)
            {
                item.IsVisSmallMan = false;
            }
            map[x, smallMans[x].Y].IsVisSmallMan = true;
        }
        /// <summary>
        /// GridControlLeftRight2表格里的5个Rectangle的tap事件;控制小人移动的事件;地图2;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void recTapped2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bo.Play();
            BlockRectangle rec1 = sender as BlockRectangle;
            int x = Convert.ToInt32(rec1.Tag);
            rec1.Opacity = 0.3;
            await Task.Delay(100);
            rec1.Opacity = 0;
            //遍历出活着的小人命名为b2;
            Block b2 = new Block();
            foreach (Block item in smallMans2)
            {
                if (item.IsVisSmallMan == true)
                {
                    b2 = item;
                }
            }

            if (b2 == null)
            {
                foreach (Block item in allBlocks2)
                {
                    item.IsVisSmallMan = false;
                }
                smallMans2[2].IsVisSmallMan = true;
                b2 = smallMans2[2];
            }
            else
            {
                foreach (Block item in allBlocks2)
                {
                    item.IsVisSmallMan = false;
                }
                b2.IsVisSmallMan = true;
            }
            //触摸的grid.tag就是1-5；
            if (x == b2.X)
            {
                return;
            }
            else if (x > b2.X)
            {
                b2.IsVisSmallMan = false;
                for (int i = b2.X + 1; i < x; i++)
                {
                    map2[i, smallMans2[i].Y].IsVisSmallMan = true;
                    await Task.Delay(100);
                    map2[i, smallMans2[i].Y].IsVisSmallMan = false;
                }//200毫秒
                map2[x, smallMans2[x].Y].IsVisSmallMan = true;
            }
            else if (x < b2.X)
            {
                b2.IsVisSmallMan = false;
                for (int i = b2.X - 1; i > x; i--)
                {
                    map2[i, smallMans2[i].Y].IsVisSmallMan = true;
                    await Task.Delay(100);
                    map2[i, smallMans2[i].Y].IsVisSmallMan = false;
                }
                map2[x, smallMans2[x].Y].IsVisSmallMan = true;
            }
            foreach (Block item in allBlocks2)
            {
                item.IsVisSmallMan = false;
            }
            map2[x, smallMans2[x].Y].IsVisSmallMan = true;
        }

        /// <summary>
        /// 最后一次改于20140608下午运行正常。。。未写小人下落登场动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GameOverBtn_Click(object sender, RoutedEventArgs e)
        {
            GridJianBianDonHua.Visibility = Visibility.Visible;
            sbJianBianDonHua0.Begin();
            //中线显示出来;
            CenterRec.Visibility = Visibility.Visible;
            //关于8个小矮人的判断动画;
            if (firstMan != null)
            {
                firstMan.StartSmall();
                firstMan = null;//开始游戏了,把firstman变小,并且变成null以免影响到下一次的判断;
            }
            lvTheme.Visibility = Visibility.Collapsed;//开始游戏了。把左右彩条隐藏了;
            lvThemeBackGround.Visibility = Visibility.Collapsed;
            foreach (Block item in allBlocks)
            {
                item.IsVisSmallMan = false;
            }
            //这里写地图2里的小人被隐藏;
            foreach (Block item in allBlocks2)
            {
                item.IsVisSmallMan = false;
            }
            GameOverGrid.Visibility = Visibility.Collapsed;
            if (GameOverBtn.Label.ToString() == "开始学习")
            {
                GameOverBtn.Label = "再走一圈";
            }
            if (visBlocks != null)
            {
                foreach (Block item in visBlocks)//将没显示的黑块再次变黑
                {
                    item.IsVis = true;
                }
            }
            //这里写地图2里隐藏格格再次变黑;
            if (visBlocks2 != null)
            {
                foreach (Block item in visBlocks2)//将没显示的黑块再次变黑
                {
                    item.IsVis = true;
                }
            }
            level = 1;//关卡计数器;
            //sw.Start();//秒表开始计时功能现在先不用随后如果想用反注释一下;
            //把所有的格子放到底端去;
            foreach (BlockRectangle item in allBlockRectangles)
            {
                item.Start0To1500();
            }
            //这里写地图2里所有的格子放到底端去;
            foreach (BlockRectangle item in allBlockRectangles2)
            {
                item.Start0To1500();
            }

            //二：生成uplists放到顶端
            //2.1得到新一关的visBlocks。。。//将新一关的visBlocks设置为不可见。
            visBlocks = map.EasyBlock();
            await Task.Delay(1);//修复系统Bug;
            //这里写地图2获取到简单难度的隐藏格;
            visBlocks2 = map2.EasyBlock();

            foreach (Block item in visBlocks)
            {
                item.IsVis = false;
            }
            //这里写地图2的所有获取到的隐藏格隐藏遍历隐藏起来;
            foreach (Block item in visBlocks2)
            {
                item.IsVis = false;
            }
            //2.2获取随机要压下来的Blocks//然后从1500放到-1500准备下一步的操作
            BlockUplists = map.UpBlocks(visBlocks);
            foreach (Block itemBlock in BlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.Start1500ToFu1500();
            }
            //这里写地图2里随机获取要压下来的Blocks然后从顶放到底准备下一步的操作;
            BlockUplists2 = map2.UpBlocks(visBlocks2);
            foreach (Block itemBlock in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.Start1500ToFu1500();
            }

            //获取allblocks减去要压下来的blocks。。。得到FuBlockUplists;
            foreach (Block item in BlockUplists)
            {
                FuBlockUplists = allBlocks;
                FuBlockUplists.Remove(item);
            }

            //这里写地图2//获取allblocks2减去要压下来的blocks。。。得到FuBlockUplists2;
            foreach (Block item in BlockUplists2)
            {
                FuBlockUplists2 = allBlocks2;
                FuBlockUplists2.Remove(item);
            }
            await Task.Delay(100);

            //小人可以移动到的9个小人数组初始化
            for (int i = 0; i < 5; i++)
            {
                smallMans[i] = map[i, map.smallManY[i]];
            }
            //小人初始化;//初始化前先写一个下落的动画；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；
            smallMans[2].IsVisSmallMan = true;

            //这里写地图2小人可以移动到的5个小人数组初始化
            for (int i = 0; i < 5; i++)
            {
                smallMans2[i] = map2[i, map2.smallManY[i]];
            }
            smallMans2[2].IsVisSmallMan = true;

            //将-1500处的uplists放下到300处//将1500处的FuBlockUplists放到0
            foreach (Block item in BlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500ToFu300();
            }
            foreach (Block item in FuBlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                b1.Start1500To0();
            }
            //这里写地图2//将-1500处的uplists2放下到300处//将1500处的FuBlockUplists2放到0
            foreach (Block item in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500ToFu300();
            }
            foreach (Block item in FuBlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                b1.Start1500To0();
            }
            //把Uplists加回到allBlocks;
            foreach (Block item in BlockUplists)
            {
                allBlocks.Add(item);
            }
            //这里地图2把Uplists2加回到allBlocks2;
            foreach (Block item in BlockUplists2)
            {
                allBlocks2.Add(item);
            }

            FromFu1500ToFu300.Begin();
            From1500To0.Begin();
        }

        /// <summary>
        /// 第一步。。。。-1500到-300 1500到0 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void sb1_Completed(object sender, object e)
        {
            ///////////////////////////////////////////过关的话自动开始下一关//////////////////////////////////////
            //把跑到顶端的格子移到底端去。以重新生成下一关的UPLists;
            foreach (Block item in BlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500To1500();
            }
            //这里写地图2//把跑到顶端的格子移到底端去。以重新生成下一关的UPLists2;
            foreach (Block item in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500To1500();
            }
            await Task.Delay(10);
            foreach (Block item in visBlocks)//将没显示的黑块再次变黑
            {
                item.IsVis = true;
            }
            foreach (Block item in allBlocks)
            {
                item.IsVisSmallMan = false;
            }
            //这里写地图2把没显示的块再次变黑;
            foreach (Block item in visBlocks2)//将没显示的黑块再次变黑
            {
                item.IsVis = true;
            }
            foreach (Block item in allBlocks2)
            {
                item.IsVisSmallMan = false;
            }

            //得到新一关的visBlocks。。。//将新一关的visBlocks设置为不可见。
            level++;
            Random r = new Random();
            if (level <= 3)
            {
                visBlocks = map.EasyBlock();
                await Task.Delay(1);
                visBlocks2 = map2.EasyBlock();//这里写地图2的获取简单难度格格隐藏;
            }
            else if (level <= 8)
            {
                visBlocks = map.MediumBlock();
                await Task.Delay(1);
                visBlocks2 = map2.MediumBlock();//这里写地图2的获取中等难度隐藏格格;
            }
            else if (level <= 33)
            {
                int i = r.Next(1, 11);
                if (i <= 2)
                {
                    visBlocks = map.EasyBlock();
                    await Task.Delay(1);
                    visBlocks2 = map2.EasyBlock();//这里写地图2;
                }
                else if (i <= 5)
                {
                    visBlocks = map.MediumBlock();
                    await Task.Delay(1);
                    visBlocks2 = map2.MediumBlock();//这里写地图2;
                }
                else
                {
                    visBlocks = map.HardBlock();
                    await Task.Delay(1);
                    visBlocks2 = map2.HardBlock();//这里写地图2;
                }
            }
            else
            {
                int i = r.Next(1, 11);
                if (i <= 3)
                {
                    visBlocks = map.MediumBlock();
                    await Task.Delay(1);
                    visBlocks2 = map2.MediumBlock();//这里写地图2;
                }
                else
                {
                    visBlocks = map.HardBlock();
                    await Task.Delay(1);
                    visBlocks2 = map2.HardBlock();//这里写地图2;
                }
            }

            foreach (Block item in visBlocks)
            {
                item.IsVis = false;
            }
            //这里写地图2的所有隐藏格格设为隐藏;
            foreach (Block item in visBlocks2)
            {
                item.IsVis = false;
            }

            //获取随机要压下来的Blocks//然后从1500放到-1500准备下一步的操作//获取allblocks减去要压下来的blocks。。。得到FuBlockUplists;
            BlockUplists = map.UpBlocks(visBlocks);
            foreach (Block itemBlock in BlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.Start1500ToFu1500();
            }
            foreach (Block item in BlockUplists)
            {
                FuBlockUplists = allBlocks;
                FuBlockUplists.Remove(item);
            }
            //这里写地图2//获取随机要压下来的Blocks2//然后从1500放到-1500准备下一步的操作//获取allblocks2减去要压下来的blocks。。。得到FuBlockUplists2;
            BlockUplists2 = map2.UpBlocks(visBlocks2);
            foreach (Block itemBlock in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.Start1500ToFu1500();
            }
            foreach (Block item in BlockUplists2)
            {
                FuBlockUplists2 = allBlocks2;
                FuBlockUplists2.Remove(item);
            }

            await Task.Delay(100);

            //小人可以移动到的9个小人数组初始化
            for (int i = 0; i < 5; i++)
            {
                smallMans[i] = map[i, map.smallManY[i]];
            }
            //小人初始化;//初始化前先写一个下落的动画；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；；
            smallMans[2].IsVisSmallMan = true;
            //这里写地图2//小人可以移动到的5个小人数组初始化
            for (int i = 0; i < 5; i++)
            {
                smallMans2[i] = map2[i, map2.smallManY[i]];
            }
            smallMans2[2].IsVisSmallMan = true;

            //将-1500处的uplists放下到300处//将1500处的FuBlockUplists放到0
            foreach (Block item in BlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500ToFu300();
            }
            foreach (Block item in FuBlockUplists)
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                b1.Start1500To0();
            }
            //这里写地图2//将-1500处的uplists2放下到300处//将1500处的FuBlockUplists2放到0
            foreach (Block item in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                b1.StartFu1500ToFu300();
            }
            foreach (Block item in FuBlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                b1.Start1500To0();
            }

            //把Uplists加回到allBlocks;
            foreach (Block item in BlockUplists)
            {
                allBlocks.Add(item);
            }
            //这里写地图2//把Uplists2加回到allBlocks2;
            foreach (Block item in BlockUplists2)
            {
                allBlocks2.Add(item);
            }

            FromFu1500ToFu300.Begin();
            From1500To0.Begin();
        }

        /// <summary>
        /// 第二步:等待难度秒数后抖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FromFu1500ToFu300_Completed(object sender, object e)
        {
            //生成延时 难度速度控制//地图1地图2共用计时器;
            if (level <= 4)
            {
                await Task.Delay(3600 - (300 * level));
            }
            else if (level <= 12)
            {
                await Task.Delay(2400 - (100 * (level - 4)));
            }
            else if (level <= 28)
            {
                await Task.Delay(1600 - (40 * (level - 12)));
            }
            else if (level <= 60)
            {
                await Task.Delay(960 - (20 * (level - 28)));
            }
            else if(level<=100)
            {
                await Task.Delay(320 - (8 * (level - 40)));
            }

            foreach (Block itemBlock in BlockUplists)//所有要下落的格子全抖动三下;
            {
                BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.StartDoudon();
            }
            //这里写地图2所有要下落的格格抖动三下;
            foreach (Block itemBlock in BlockUplists2)
            {
                BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == itemBlock.Tag);
                b1.StartDoudon();
            }

            FromFu300DouDon.Begin();
            await Task.Delay(100);
        }

        /// <summary>
        /// 抖动完成开始sb2.Begin()开始下压;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromFu300DouDon_Completed(object sender, object e)
        {
            sb2.Begin();
        }

        //下压完成。判断小人方位。这个是最后一步sb1.Begin();
        private async void sb2_Completed(object sender, object e)
        {
            //2014年6月7号下午到这里停止。。。。。。。。随后再写这下面的代码；；；；；；；；；；；；；；；；；；；
            //先判断小人有没过关吧？？？
            Block block = new Block();
            foreach (Block item in smallMans)
            {
                if (item.IsVisSmallMan == true)
                {
                    block = item;
                }
            }
            //这里写地图2先判断小人有没有过关吧;
            Block block2 = new Block();
            foreach (Block item in smallMans2)
            {
                if (item.IsVisSmallMan == true)
                {
                    block2 = item;
                }
            }

            if (block.X == visBlocks[0].X&&block2.X==visBlocks2[0].X)//这里写地图2添加了地图2也必须过关的判断;
            {
                //把前一关没有上去的格子取到。。以便sb1的时候送下去;
                foreach (Block item in BlockUplists)
                {
                    FuBlockUplists = allBlocks;
                    FuBlockUplists.Remove(item);
                }
                //没有上去的格子从0下到1500
                foreach (Block item in FuBlockUplists)
                {
                    BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                    b1.Start0To1500();
                }
                foreach (Block item in BlockUplists)
                {
                    BlockRectangle b1 = allBlockRectangles.First(retangle => retangle.Tag == item.Tag);
                    b1.Start0ToFu1500();
                }

                //这里写地图2//把前一关没有上去的格子取到。。以便sb1的时候送下去;
                foreach (Block item in BlockUplists2)
                {
                    FuBlockUplists2 = allBlocks2;
                    FuBlockUplists2.Remove(item);
                }
                foreach (Block item in FuBlockUplists2)
                {
                    BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                    b1.Start0To1500();
                }
                foreach (Block item in BlockUplists2)
                {
                    BlockRectangle b1 = allBlockRectangles2.First(retangle => retangle.Tag == item.Tag);
                    b1.Start0ToFu1500();
                }

                //把Uplists加回到allBlocks;
                foreach (Block item in BlockUplists)
                {
                    allBlocks.Add(item);
                }
                //这里写地图2把Uplists2加回到allBlocks2;
                foreach (Block item in BlockUplists2)
                {
                    allBlocks2.Add(item);
                }
                if (level > 100)
                {
                    await new MessageDialog("恭喜通关,您已练成凌波微步;").ShowAsync();
                    GameOverScore.Text = level.ToString() + "步";
                    GameOverGrid.Visibility = Visibility.Visible;
                    if (level > score)
                    {
                        if (setting.Values.ContainsKey("score"))
                        {
                            setting.Values["score"] = level.ToString();
                        }
                        JiRu.Text = "你已破记录!你走了:" + level + "步";
                        score = level;
                    }
                    lvThemeBackGround.Visibility = Visibility.Visible;//左右彩条死后再显示出来;
                    lvTheme.Visibility = Visibility.Visible;
                    CenterRec.Visibility = Visibility.Collapsed;//中线隐藏起来;
                    GridJianBianDonHua.Visibility = Visibility.Collapsed;
                    sbJianBianDonHua0.Stop();
                    sbJianBianDonHua1.Stop();
                    return;
                }
                sb1.Begin();
                From0To1500.Begin();
                //说明过关了。执行sb1.begin()开始下一关。在sb1完成后生成下一关的块。。。
                
            }
            else
            {
                GameOverScore.Text = level.ToString() + "步";
                GameOverGrid.Visibility = Visibility.Visible;
                mediaElement.Play();
                //string mm = sw.Elapsed.Minutes.ToString();
                //string ss = sw.Elapsed.Seconds.ToString();
                //await new MessageDialog("您坚持了：" + mm + "分" + ss + "秒").ShowAsync();
                #region 1-33关的语句描述
                if (level <= 3)
                {
                    switch (level)
                    {
                        case 0:
                            GameOverText.Text = "我还能说什么";
                            break;
                        case 1:
                            GameOverText.Text = "一步一风云";
                            break;
                        case 2:
                            GameOverText.Text = "双脚落地";
                            break;
                        case 3:
                            GameOverText.Text = "宝宝你走三步了";
                            break;
                    }
                }
                else if (level <= 100)
                {
                    switch (level/4)
                    {
                        case 1:
                            GameOverText.Text = "基础牢固";
                            break;
                        case 2:
                            GameOverText.Text = "徒儿快学会跑了";
                            break;
                        case 3:
                            GameOverText.Text = "初学乍练";
                            break;
                        case 4:
                            GameOverText.Text = "初窥门径";
                            break;
                        case 5:
                            GameOverText.Text = "初窥门径";
                            break;
                        case 6:
                            GameOverText.Text = "小试牛刀";
                            break;
                        case 7:
                            GameOverText.Text = "略有小成";
                            break;
                        case 8:
                            GameOverText.Text = "小有成就";
                            break;
                        case 9:
                            GameOverText.Text = "驾轻就熟";
                            break;
                        case 10:
                            GameOverText.Text = "移形换位";
                            break;
                        case 11:
                            GameOverText.Text = "融会贯通";
                            break;
                        case 12:
                            GameOverText.Text = "炉火纯青";
                            break;
                        case 13:
                            GameOverText.Text = "出类拔萃";
                            break;
                        case 14:
                            GameOverText.Text = "技冠群雄";
                            break;
                        case 15:
                            GameOverText.Text = "出神入化";
                            break;
                        case 16:
                            GameOverText.Text = "傲视群雄";
                            break;
                        case 17:
                            GameOverText.Text = "登峰造极";
                            break;
                        case 18:
                            GameOverText.Text = "惊世骇俗";
                            break;
                        case 19:
                            GameOverText.Text = "震古铄今";
                            break;
                        case 20:
                            GameOverText.Text = "威镇寰宇";
                            break;
                        case 21:
                            GameOverText.Text = "空前绝后";
                            break;
                        case 22:
                            GameOverText.Text = "天人合一";
                            break;
                        case 23:
                            GameOverText.Text = "返璞归真";
                            break;
                        case 24:
                            GameOverText.Text = "一步之差";
                            break;
                    }
                }
                else
                {
                    GameOverText.Text = "截图````要快！";
                }
                #endregion
                //sw.Stop();
                //sw.Reset();//秒表现在不用,用的时候再反注释一下;
                if (level > score)
                {
                    if (setting.Values.ContainsKey("score"))
                    {
                        setting.Values["score"] = level.ToString();
                    }
                    JiRu.Text = "你已破记录!你走了:" + level + "步";
                    score = level;
                }
                lvThemeBackGround.Visibility = Visibility.Visible;//左右彩条死后再显示出来;
                lvTheme.Visibility = Visibility.Visible;
                CenterRec.Visibility = Visibility.Collapsed;//中线隐藏起来;
                GridJianBianDonHua.Visibility = Visibility.Collapsed;
                sbJianBianDonHua0.Stop();
                sbJianBianDonHua1.Stop();
            }
        }

        /// <summary>
        /// 前景色即左边彩条的tapped事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lvTheme_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Theme theme = lvTheme.SelectedItem as Theme;
            TopRec.Fill = theme.ThemeBackground;
            BottomRec.Fill = theme.ThemeBackground;
            foreach (BlockRectangle item in allBlockRectangles)
            {
                item.RecTheme(theme.ThemeBackground);
            }
            foreach (BlockRectangle item in allTapRectangle)
            {
                item.RecTheme(theme.ThemeBackground);
            }
            //这里写地图2将所有块都变成新主题色;再将所有控制左右走的tap块变成主题色;
            foreach (BlockRectangle item in allBlockRectangles2)
            {
                item.RecTheme(theme.ThemeBackground);
            }
            foreach (BlockRectangle item in allTapRectangle2)
            {
                item.RecTheme(theme.ThemeBackground);
            }
        }

        /// <summary>
        /// 背景色即右边彩条的tapped事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lvThemeBackGround_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Theme theme = lvThemeBackGround.SelectedItem as Theme;
            BackGroundColor.Background = theme.ThemeBackground;
        }

        /// <summary>
        /// ManPhoto的tapped事件;选中小人则背景变亮;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void smallman_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (firstMan != null)
            {
                firstMan.StartSmall();
            }
             firstMan = sender as SmallMan;
                int i = Convert.ToInt32(firstMan.Tag) - 1000;
                firstMan.StartBig();
                foreach (SmallMan item in allSmallMans)
                {
                    item.UpdatePhoto(i);
                }//如果firstman不是null则说明第二次进入。先将firstsmall变小再重新赋值sender给firstman;这里执行结束后,要按开始游戏了。开始游戏的时候把firstman变小,然后再把firstman=null;
                foreach (SmallMan item in allSmallMans2)
                {
                    item.UpdatePhoto(i);
                }//这里写地图2
            //这里遇到一个问题：如果是第一次点击则变大即可。如果第二次点击则需要判断现在是第几次点击。并且要判断小人的大小状态。不然无法控制变小的动画。。。
            //要么就为了这个问题写一个类。来描述小人的状态。并且将小人的数据上下文绑定到这个类。或者还有一个办法。就是在这个mainpage.xaml.cs里多写几个代码：
            //用一个firstMan来接收第一次点击。用另一个SecondMan来接收第二个点击和man。。。然后只需要判断firstMan是否为null即可。。。。
            //把小人的动画中心设置成脚下中间。。。
        }

        private void sbJianBianDonHua0_Completed(object sender, object e)
        {
            sbJianBianDonHua1.Begin();
        }
    }
}
//20140607下午：小人跳起来。跳到中间位置。
//上往上走下往下走 走出屏幕再回来。。回到中间准备合的时候再抖几下。。。

//至少作小人上升和下落的动画吧。先。

//小人控件和图片控件一样显示在每个格子里。
//写一个UP方法一个Down方法
//写一个左走和右走的方法。让visblock[0].X,Y+1的Tag的小人显示出来。。。的时候就成功到下一关。否则就GameOver.convas.zindex=0;

//20140608下午UI出现问题Bug有可能是FuUplists=allBlocks的问题也可能是1000到0以前没有弄好。
//解答：结果的确是fuuplists=allblocks的问题。。。。。

//小人跳起和落下的动画好乱好晕。。。

//if (level > Convert.ToInt32((root2.Element("score")).Attribute("sco").Value))
//{
//    root2.Element("score").Attribute("sco").Value = level.ToString();
//    xdoc2.Save(streamWriter);//在方法二中将xdoc3改成了2;
//    JiRu.Text = "你已破记录!你走了:" + Convert.ToInt32(root2.Element("score").Attribute("sco").Value) + "步";
//}
//////////////20140612晚把这里的非空判断里的全放到onnavigatedto里了。并且加了async每次进游戏都会重新新建1.xml明天把这个新建改成先判断文件是否存在。如果存在就不新建了。。。
//////////////明天还需要在把这个功能全部完成后。假发到商店里试一下是否会闪退的问题。。。。。
/////////////////////////////////////////////20140610下午将click里写一个读取；；；；；；
//GameOverGrid.Canvas.ZIndex=0;//这个值取不到。明天联网再研究；--------已解决通过visibility；

