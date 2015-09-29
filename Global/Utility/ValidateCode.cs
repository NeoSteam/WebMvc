using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Global
{
    public class ValidateCode
    {
        public enum CodeType { Numeral, LowercaseLetters, UppercaseLetters, Letters, NumeralLowercaseLetters, NumeralUppercaseLetters, NumeralLetters, Math }

        private const double PI = 3.14159265358979;
        private const double PI2 = 6.28318530717959;
        private const string SESSION_NAME = "ValidateCode";

        public byte[] ValidateCodeImg(CodeType pStr_Type, int pInt_length)
        {
            string mStr_ValidateCode, mStr_value;
            mStr_value = null;
            mStr_ValidateCode = this.GetRandomData(pStr_Type, pInt_length, ref mStr_value);
            System.Web.HttpContext.Current.Session.Add(SESSION_NAME, mStr_value);
            return this.CreateCodeImage(mStr_ValidateCode);
        }

        public static bool CheckValidateCode(string input_code)
        {
            if (System.Web.HttpContext.Current.Session[SESSION_NAME] != null && !string.IsNullOrEmpty(input_code))
            {
                string vc = System.Web.HttpContext.Current.Session[SESSION_NAME].ToString();

                if (vc.Trim().ToLower() == input_code.Trim().ToLower())
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得随机组合
        /// </summary>
        /// <param name="pStr_Type">随机数类型(0,数字;a,小写英文字母;A,大写英文字母;aA,英文字母;0a,数字+小写英文字母;0A,数字+大写英文字母;0aA,数字+英文字母;M,数学式子;)</param>
        /// <param name="pInt_length">随机数长度(使用数学式子为0)</param>
        /// <returns>产生的随机字符串</returns>
        private string GetRandomData(CodeType pStr_Type, int pInt_length, ref string pStr_Value)
        {
            //string Str_CharMun = "0,1,2,3,4,5,6,7,8,9";
            //string Str_CharLetter_Low = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            //string Str_CharLetter_Upp = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            //去除歧义字符版本
            string Str_CharMun = "3,4,5,7,8";
            string Str_CharLetter_Low = "a,c,d,e,f,h,k,n,p,q,s,t,u,v,x";
            string Str_CharLetter_Upp = "A,C,D,E,F,G,H,K,N,P,Q,R,S,T,U,V,X";
            string Str_CharAll;
            string[] Str_CharAllArray;
            string Str_ReturnValue = "";
            //生成随机生成器
            Random random = new Random();
            switch (pStr_Type)
            {
                case CodeType.Numeral:
                    Str_CharAll = Str_CharMun;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, 10)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.LowercaseLetters:
                    Str_CharAll = Str_CharLetter_Low;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, 26)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.UppercaseLetters:
                    Str_CharAll = Str_CharLetter_Upp;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, Str_CharAllArray.Length)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.Letters:
                    Str_CharAll = Str_CharLetter_Low + "," + Str_CharLetter_Upp;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, Str_CharAllArray.Length)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.NumeralLowercaseLetters:
                    Str_CharAll = Str_CharMun + "," + Str_CharLetter_Low;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, Str_CharAllArray.Length)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.NumeralUppercaseLetters:
                    Str_CharAll = Str_CharMun + "," + Str_CharLetter_Upp;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, Str_CharAllArray.Length)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.NumeralLetters:
                    Str_CharAll = Str_CharMun + "," + Str_CharLetter_Low + "," + Str_CharLetter_Upp;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    for (int Int_Count = 0; Int_Count < pInt_length; Int_Count++)
                    {
                        Str_ReturnValue += Str_CharAllArray[random.Next(0, Str_CharAllArray.Length)];
                    }
                    pStr_Value = Str_ReturnValue;
                    break;
                case CodeType.Math:
                    int mInt_value = 0;
                    int mInt_Value_1, mInt_Value_2;
                    Str_CharAll = Str_CharMun;
                    Str_CharAllArray = Str_CharAll.Split(',');
                    mInt_Value_1 = random.Next(0, 10);
                    mInt_Value_2 = random.Next(0, 10);
                    Str_ReturnValue = mInt_Value_1 + "+" + mInt_Value_2 + "=";
                    mInt_value = (mInt_Value_1 + mInt_Value_2);
                    pStr_Value = mInt_value.ToString();
                    break;
                default:
                    break;
            }
            return Str_ReturnValue;
        }
        private byte[] CreateCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return null;
            int Int_fontSize = 15;

            //System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * Int_fontSize)), 40);//12.5/22
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(checkCode.Length * Int_fontSize + 5, (int)(Int_fontSize * 1.6));
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线25
                for (int i = 0; i < 4; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.FromArgb(150, 150, 150), 1), x1, y1, x2, y2);
                }

                int Int_Count_X = 0, Int_Count_Y = 0;
                for (int Int_Count_I = 0; Int_Count_I < checkCode.Length; Int_Count_I++)
                {
                    Font font = new System.Drawing.Font("Arial", Int_fontSize, FontStyle.Regular);
                    //System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                    System.Drawing.SolidBrush brush = new SolidBrush(Color.FromArgb(80, 80, 80));
                    Int_Count_X = Int_Count_I * Int_fontSize;
                    Int_Count_Y = random.Next(4);
                    g.DrawString(checkCode.Substring(Int_Count_I, 1), font, brush, Int_Count_X, Int_Count_Y);
                }
                //扭曲图片
                float Twist1, Twist2;
                //Twist1 = random.Next(1, 4) * 2; //扭曲参数随机生成;
                //Twist2 = random.Next(1, 4); //扭曲参数随机生成;
                Twist1 = 0;
                Twist2 = 0;

                image = TwistImage(image, true, -Twist1, -Twist2);
                image = TwistImage(image, false, Twist1, Twist2); //多扭曲几次也没关系，只是消耗服务器资源多些;

                //画图片的前景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线Silver
                g.DrawRectangle(new Pen(Color.Black), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
                /*
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
                */
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir"></param>
        /// <param name="nMultValue">波形的幅度倍数</param>
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
    }
}