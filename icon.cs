using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poseidon
{
    class icon
    {
        public static Rectangle GetIconRect(NotifyIcon icon)
        {
            RECT rect = new RECT();
            NOTIFYICONIDENTIFIER notifyIcon = new NOTIFYICONIDENTIFIER();

            notifyIcon.cbSize = Marshal.SizeOf(notifyIcon);
            //use hWnd and id of NotifyIcon instead of guid is needed
            notifyIcon.hWnd = GetHandle(icon);
            notifyIcon.uID = GetId(icon);

            int hresult = Shell_NotifyIconGetRect(ref notifyIcon, out rect);
            //rect now has the position and size of icon

            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }
        public static Point GetFormLocation(Form frm, NotifyIcon icon)
        {
            var pos = GetIconRect(icon);
            var x = (pos.Left + pos.Right) / 2 - frm.Width / 2;
            var y = pos.Top - frm.Height;
            return new Point(x, y);
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NOTIFYICONIDENTIFIER
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uID;
            public Guid guidItem;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int Shell_NotifyIconGetRect([In]ref NOTIFYICONIDENTIFIER identifier, [Out]out RECT iconLocation);

        private static FieldInfo windowField = typeof(NotifyIcon).GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        private static IntPtr GetHandle(NotifyIcon icon)
        {
            if (windowField == null) throw new InvalidOperationException("[Useful error message]");
            NativeWindow window = windowField.GetValue(icon) as NativeWindow;

            if (window == null) throw new InvalidOperationException("[Useful error message]");  // should not happen?
            return window.Handle;
        }

        private static FieldInfo idField = typeof(NotifyIcon).GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        private static int GetId(NotifyIcon icon)
        {
            if (idField == null) throw new InvalidOperationException("[Useful error message]");
            return (int)idField.GetValue(icon);
        }
        public static void ChangeIconState()
        {
            Class1.frm_main.Invoke(new Action(() =>
            {
                var val = Class1.frmMsgBox.clb_unread_msg.Items[0].SubItems.Count != 0 || Class1.frmMsgBox.clb_unread_msg.Items[1].SubItems.Count != 0 || Class1.frmMsgBox.clb_unread_msg.Items[2].SubItems.Count != 0;
                Class1.frm_main.timer2.Enabled = val;
                if(!val)
                    Class1.frm_main.notifyIcon1.Icon = Properties.Resources.poseidon;
                foreach(var item in Class1.chatListSubItemPool)
                    Class1.chatListSubItemPool[item.Key].IsTwinkle = Class1.unReadPrivateMsgItemPool.ContainsKey(item.Key);
            }));
        }
    }
}
