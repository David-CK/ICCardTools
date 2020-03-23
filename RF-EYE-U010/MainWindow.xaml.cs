using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RF_EYE_U010
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        IntPtr icdev = IntPtr.Zero;
        Int16 port = 0;
        Int32 baud = 0;
        Int16 st = 0;

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");
            txtOutput.ScrollToEnd();

            if (icdev.ToInt32() > 0)
            {
                txtOutput.AppendText("设备已经连接\n");
                txtOutput.ScrollToEnd();
            }
            else
            {
                icdev = App.rf_init(port, baud);
                if (icdev.ToInt32() > 0)
                {
                    txtOutput.AppendText("设备连接成功\n");
                    txtOutput.ScrollToEnd();
                }
                else
                {
                    txtOutput.AppendText("设备连接失败\n");
                    txtOutput.ScrollToEnd();
                }
            }
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");
            txtOutput.ScrollToEnd();

            if (icdev.ToInt32() > 0)
            {
                st = App.rf_exit(icdev);
                if (st == 0)
                {
                    icdev = IntPtr.Zero;

                    txtOutput.AppendText("设备断开成功\n");
                    txtOutput.ScrollToEnd();
                }
                else
                {
                    txtOutput.AppendText("设备断开失败\n");
                    txtOutput.ScrollToEnd();
                }
            }
            else
            {
                txtOutput.AppendText("设备已经断开\n");
                txtOutput.ScrollToEnd();
            }
        }
    }
}
