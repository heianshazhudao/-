using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;

public class Form1 : Form
{
    private Button button1;
    private System.ComponentModel.Container components = null;

    public Form1()

    {
        //初始化窗体中的各个组件
        InitializeComponent();
    }

    //清除程序中使用过的资源
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            if (components != null)
                components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        button1 = new Button();
        SuspendLayout();
        button1.Location = new System.Drawing.Point(64, 60);
        button1.Name = "Button1";
        button1.Size = new System.Drawing.Size(80, 32);
        button1.TabIndex = 0;
        button1.Text = "捕获";
        button1.Click += new System.EventHandler(button1_Click);

        AutoScaleBaseSize = new System.Drawing.Size(6, 14);
        ClientSize = new System.Drawing.Size(216, 125);
        Controls.Add(button1);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "Form1";
        Text = "C#捕获当前屏幕";
        ResumeLayout(false);
    }

    //声明API函数
    //位复制
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern bool BitBlt(
        IntPtr hdcDest,//目标DC的句柄
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,//源DC的句柄
        int nXSrc,
        int nYSrc,
        System.Int32 dwRop//光栅的处理数值
        );
    //获取桌面窗口句柄(最顶端的)
    [System.Runtime.InteropServices.DllImport("User32.dll")]
    private static extern IntPtr GetDesktopWindow();
    //获取窗口dc
    [System.Runtime.InteropServices.DllImport("User32.dll")]
    private static extern IntPtr GetWindowDC(IntPtr hdc);
    //释放dc
    [System.Runtime.InteropServices.DllImport("User32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd,IntPtr hDc);   



    static void Main()
    {
        Application.Run(new Form1());
    }

    private void button1_Click(Object sender, System.EventArgs e)
    {
        //获得当前屏幕的窗口DC
        IntPtr desktopwindow = GetDesktopWindow() ;
        IntPtr dc = GetWindowDC(desktopwindow);

        //创建一个屏幕大小的矩形
        Rectangle rect = new Rectangle();
        rect = Screen.GetBounds(this);

        //创建一个以当前屏幕为模板的图像
        Graphics g1 = this.CreateGraphics();
        //创建以屏幕大小为标准的位图
        Image MyImage = new Bitmap(rect.Width, rect.Height, g1);
        Graphics g2 = Graphics.FromImage(MyImage);
        //得到屏幕的DC
        IntPtr dc2 = g2.GetHdc();
        //调用此API函数实现屏幕捕获
        BitBlt(dc2, 0, 0, rect.Width, rect.Height, dc, 0, 0, 0xCC0020);
        //释放掉屏幕的DC
        int a = ReleaseDC(desktopwindow, dc);
        if(a==0)
        {
            MessageBox.Show("!无法释放屏幕dc");
            return;
        }
        g2.ReleaseHdc();
        
        //以jpg文件格式来保存
        MyImage.Save(@"c:\Capture.jpg", ImageFormat.Jpeg);
        MessageBox.Show("当前屏幕已经保存到C盘");
    }
}
