﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartEye2
{
    public partial class Form1 : Form
    {
        Size formSize;
        private int BorderSize = 2;
        private bool isPanelMenuExpanded = true;
        public Form1()
        {
            InitializeComponent();


            this.Padding = new Padding(BorderSize);  //Bordersize
            this.BackColor = Color.LightSteelBlue;  //Border Color
        }
        //Drag and move the title bar
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int IParam);
        private void BtnWindowExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnWindowLow_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            BtnWindowLow.Visible = false;
            BtnWindowMax.Visible = true;
        }

        private void BtnWindowHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnWindowMax_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            BtnWindowMax.Visible = false;
            BtnWindowLow.Visible = true;
        }




        //Overridden Methods
        //Close the title bar but keep the drag to top for fullscreen ability"
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }
            base.WndProc(ref m);
        }

        //}
        private void PanelTitleBar_Resize(object sender, EventArgs e) //TitleBardan tutup sürüklerken oluşan hata için düzenleme***
        {
            AdjustForm();
        }
        private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized: //Maximized form (After)
                    this.Padding = new Padding(8, 8, 8, 0);
                    break;
                case FormWindowState.Normal: //Restored form (After)
                    if (this.Padding.Top != BorderSize)
                        this.Padding = new Padding(BorderSize);
                    break;
            }
        }

        private void BtnMenuKapa_Click(object sender, EventArgs e)
        {
            
            BtnMenuKapa.Visible = false;
            BtnMenuAc.Visible = true;
            CollapseMenu();

        }
        //Menu Collapse and Extend method
        private void CollapseMenu()
        {
            if (this.PanelMenu.Width > 200)
            {
                isPanelMenuExpanded = false; //Sign with flag
                PanelMenu.Width = 76;
                pictureBoxLogo.Visible = false;
                BtnMenuAc.Dock = DockStyle.Top;
                foreach (Button BtnMenu in PanelMenu.Controls.OfType<Button>())
                {
                    BtnMenu.Text = "";
                    BtnMenu.ImageAlign = ContentAlignment.MiddleCenter;
                    BtnMenu.Padding = new Padding(10);
                }
            }
            else
            {
                isPanelMenuExpanded = true; //Sign with flag
                PanelMenu.Width = 217;
                pictureBoxLogo.Visible = true;
                BtnMenuKapa.Dock = DockStyle.None;
                foreach (Button BtnMenu in PanelMenu.Controls.OfType<Button>())
                {
                    BtnMenu.Text = "   " + BtnMenu.Tag.ToString(); //3 space on text to put space between icons and button text when menu was visible 
                    BtnMenu.ImageAlign = ContentAlignment.MiddleLeft;
                    BtnMenu.Padding = new Padding(10, 0, 0, 0);

                }
            }
        }

        private void PanelMenu_MouseEnter(object sender, EventArgs e)
        {
            if (PanelMenu.Width == 76 && !isPanelMenuExpanded)
            {

                PanelMenu.Width = 217;
                BtnMenuKapa.Visible = true;
                BtnMenuKapa.Dock = DockStyle.None;
                BtnMenuAc.Visible = false;
                pictureBoxLogo.Visible = true;
                foreach (Button BtnMenu in PanelMenu.Controls.OfType<Button>())
                {
                    BtnMenu.Text = "   " + BtnMenu.Tag.ToString(); //3 space on text to put space between icons and button text when menu was visible 
                    BtnMenu.ImageAlign = ContentAlignment.MiddleLeft;
                    BtnMenu.Padding = new Padding(10, 0, 0, 0);

                }
            }

        }

        private async void PanelMenu_MouseLeave(object sender, EventArgs e) //Ertelenmi� bir �ekilde collapse yap�yoruz.
        {
            if (!isPanelMenuExpanded) //bayrak kapal�=bar kapal�. Uygulama BAR a��k ba�lad��� i�in bar�n �zerinden ayr�l�nca kapanmas�n� engelliyor.
            {
                await Task.Delay(3000); // 3 saniye bekleyin
                PanelMenu.Width = 76;
                BtnMenuAc.Visible = true;
                BtnMenuKapa.Visible = false;
                BtnMenuAc.Dock = DockStyle.Top;

                pictureBoxLogo.Visible = false;

                foreach (Button BtnMenu in PanelMenu.Controls.OfType<Button>())
                {
                    BtnMenu.Text = "";
                    BtnMenu.ImageAlign = ContentAlignment.MiddleCenter;
                    BtnMenu.Padding = new Padding(15);
                }
            }
        }

        private void BtnMenuAc_Click(object sender, EventArgs e)
        {
            BtnMenuAc.Visible = false;
            BtnMenuKapa.Visible = true;
            CollapseMenu();
        }

        //Overridden methods
        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
        //    const int WM_SYSCOMMAND = 0x0112;
        //    const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
        //    const int SC_RESTORE = 0xF120; //Restore form (Before)
        //    const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
        //    const int resizeAreaSize = 10;

        //    #region Form Resize
        //    // Resize/WM_NCHITTEST values
        //    const int HTCLIENT = 1; //Represents the client area of the window
        //    const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
        //    const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
        //    const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
        //    const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
        //    const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
        //    const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
        //    const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
        //    const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right

        //    ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>

        //    if (m.Msg == WM_NCHITTEST)
        //    { //If the windows m is WM_NCHITTEST
        //        base.WndProc(ref m);
        //        if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
        //        {
        //            if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
        //            {
        //                Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
        //                Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          

        //                if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
        //                {
        //                    if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
        //                        m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
        //                    else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
        //                        m.Result = (IntPtr)HTTOP; //Resize vertically up
        //                    else //Resize diagonally to the right
        //                        m.Result = (IntPtr)HTTOPRIGHT;
        //                }
        //                else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
        //                {
        //                    if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
        //                        m.Result = (IntPtr)HTLEFT;
        //                    else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
        //                        m.Result = (IntPtr)HTRIGHT;
        //                }
        //                else
        //                {
        //                    if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
        //                        m.Result = (IntPtr)HTBOTTOMLEFT;
        //                    else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
        //                        m.Result = (IntPtr)HTBOTTOM;
        //                    else //Resize diagonally to the right
        //                        m.Result = (IntPtr)HTBOTTOMRIGHT;
        //                }
        //            }
        //        }
        //        return;
        //    }
        //    #endregion
        //    //Remove border and keep snap window
        //    if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
        //    {
        //        return;
        //    }

        //    //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
        //    if (m.Msg == WM_SYSCOMMAND)
        //    {
        //        /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
        //        /// Quote:
        //        /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
        //        /// are used internally by the system.To obtain the correct result when testing 
        //        /// the value of wParam, an application must combine the value 0xFFF0 with the 
        //        /// wParam value by using the bitwise AND operator.
        //        int wParam = (m.WParam.ToInt32() & 0xFFF0);

        //        if (wParam == SC_MINIMIZE)  //Before
        //            formSize = this.ClientSize;
        //        if (wParam == SC_RESTORE)// Restored form(Before)
        //            this.Size = formSize;
        //    }
        //    base.WndProc(ref m);
        //}

        private void PanelTitleBar_Paint_1(object sender, PaintEventArgs e)
        {
            
        }

        private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
    }

