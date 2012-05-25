using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;

namespace CommonLibrary.WebObject
{
    public class VerifyCodeHelper
    {
        #region 静态属性
        public static VerifyCodeHelper Instance
        {
            get
            {
                return new VerifyCodeHelper("0,1,2,3,4,5,6,7,8,9", 4);
            }
        }
        #endregion

        #region 构造器

        public VerifyCodeHelper()
        {
        }
        /// <summary>
        /// 验证码构造器
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <param name="fontSize"> 验证码字体大小</param>
        /// <param name="paddding">边框补白</param>
        /// <param name="chaos">是否输出燥点</param>
        /// <param name="chaosColor">输出燥点的颜色</param>
        /// <param name="backgroundColor">自定义背景色</param>
        /// <param name="colors">自定义随机颜色数组</param>
        /// <param name="fonts">自定义字体数组</param>
        /// <param name="codeSerial">自定义随机码字符串序列(使用逗号分隔)</param>
        public VerifyCodeHelper(string codeSerial, int length, int fontSize, int paddding, bool chaos, Color chaosColor, Color backgroundColor, Color[] colors, string[] fonts)
        {
            _CodeSerial = codeSerial;
            _Length = length;
            _FontSize = fontSize;
            _Padding = paddding;
            _Chaos = chaos;
            _ChaosColor = chaosColor;
            _BackgroundColor = backgroundColor;
            _Colors = colors;
            _Fonts = fonts;
        }
        /// <summary>
        /// 验证码构造器
        /// </summary>
        /// <param name="codeSerial">自定义随机码字符串序列(使用逗号分隔)</param>
        /// <param name="length">验证码长度</param>
        public VerifyCodeHelper(string codeSerial, int length)
        {
            _CodeSerial = codeSerial;
            _Length = length;
        }
        #endregion

        #region 属性

        #region 验证码长度(默认4个验证码的长度)
        int _Length = 4;
        /// <summary>
        /// 验证码长度(默认4个验证码的长度)
        /// </summary>
        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认15像素，可以自行修改)
        int _FontSize = 15;
        /// <summary>
        /// 验证码字体大小(为了显示扭曲效果，默认15像素)
        /// </summary>
        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }
        #endregion

        #region 边框补(默认2像素)
        int _Padding = 2;
        /// <summary>
        /// 边框补白(默认2像素)
        /// </summary>
        public int Padding
        {
            get { return _Padding; }
            set { _Padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认输出)
        bool _Chaos = true;
        /// <summary>
        /// 是否输出燥点(默认输出)
        /// </summary>
        public bool Chaos
        {
            get { return _Chaos; }
            set { _Chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color _ChaosColor = Color.FromArgb(0xC8, 0xB9, 0xCB);
        /// <summary>
        /// 输出燥点的颜色(默认灰色)
        /// </summary>
        public Color ChaosColor
        {
            get { return _ChaosColor; }
            set { _ChaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color _BackgroundColor = Color.FromArgb(0xDE, 0xE3, 0xE6);
        /// <summary>
        /// 自定义背景色
        /// </summary>
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] _Colors = { Color.Black, Color.Red, Color.DarkBlue, Color.DarkGreen, Color.DarkOrange, Color.Brown, Color.DarkCyan, Color.Purple };
        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] _Fonts = { "Arial", "Georgia" };
        /// <summary>
        /// 自定义字体数组
        /// </summary>
        public string[] Fonts
        {
            get { return _Fonts; }
            set { _Fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        string _CodeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        /// <summary>
        /// 自定义随机码字符串序列(使用逗号分隔)
        /// </summary>
        public string CodeSerial
        {
            get { return _CodeSerial; }
            set { _CodeSerial = value; }
        }

        #endregion

        private string _SessionKey = "SESSION_VERIFY_CODE";
        /// <summary>
        /// 使用SESSION进行验证时使用的默认Key
        /// </summary>
        public string SessionKey
        {
            get { return _SessionKey; }
            set { _SessionKey = value; }
        }

        #endregion

        #region 方法

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion

        #region 生成校验码图片
        /// <summary>
        /// 生成校验码图片
        /// </summary>
        /// <param name="code">验证字符</param>
        /// <returns>图片</returns>
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {

                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）
            image = TwistImage(image, true, 3, 3.5);

            return image;
        }
        #endregion

        #region 将创建好的图片输出到页面
        /// <summary>
        /// 将创建好的图片输出到页面
        /// </summary>
        /// <param name="code">验证字符</param>
        /// <param name="context">http流</param>
        public void CreateImageOnPage(string code, HttpContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code);

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            context.Response.ClearContent();
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.GetBuffer());

            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #endregion

        #region 生成随机字符码
        /// <summary>
        /// 生成指定长度的随机字符码
        /// </summary>
        /// <param name="codeLen">长度</param>
        /// <returns>字符码</returns>
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        /// <summary>
        /// 生成默认长度（4）的随机字符码
        /// </summary>
        /// <returns>字符码</returns>
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }
        #endregion

        #region 验证字符码
        /// <summary>
        /// 将验证码保存在Session中，以备验证
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="sessionKey">Session key,留空则使用默认值</param>
        public void SaveVerifyCodeBySession(string code, string sessionKey)
        {
            sessionKey = string.IsNullOrEmpty(sessionKey) ? SessionKey : sessionKey;
            VerifyCodeInfo vi = new VerifyCodeInfo();
            vi.VerifyCode = code;
            vi.GenerateTime = DateTime.Now;
            SessionHelper.SetValue(sessionKey, vi);
        }
        /// <summary>
        /// 校验验证码是否正确
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="validSeconds">生成验证码后的有效间隔</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="sessionKey">Session Key,留空则使用默认值</param>
        /// <returns>是否有效</returns>
        public bool CheckVerifyCodeBySession(string code, int validSeconds, out VerifyCodeErrorType errMsg, string sessionKey)
        {
            sessionKey = string.IsNullOrEmpty(sessionKey) ? SessionKey : sessionKey;
            VerifyCodeInfo vi = SessionHelper.GetValue(sessionKey) as VerifyCodeInfo;
            if (vi == null)
            {
                errMsg = VerifyCodeErrorType.Timeout;
            }
            else
            {
                if (vi.GenerateTime == DateTime.MinValue || DateTime.Now.Subtract(vi.GenerateTime).TotalSeconds > validSeconds)
                {
                    errMsg = VerifyCodeErrorType.Timeout;
                    SessionHelper.Clear(sessionKey);
                }
                else if (code.ToUpper() != vi.VerifyCode.ToUpper())
                {
                    errMsg = VerifyCodeErrorType.Invalid;
                }
                else
                {
                    errMsg = VerifyCodeErrorType.None;
                }
            }

            return errMsg == VerifyCodeErrorType.None;
        }
        public void ClearVerifyCodeSession(string sessionKey)
        {
            sessionKey = string.IsNullOrEmpty(sessionKey) ? SessionKey : sessionKey;
            SessionHelper.Clear(sessionKey);
        }
        #endregion

        #endregion

        private class VerifyCodeInfo
        {
            public string VerifyCode { get; set; }
            public DateTime GenerateTime { get; set; }
        }

        public enum VerifyCodeErrorType
        {
            None,
            Invalid,
            Timeout,
        }
    }
}
