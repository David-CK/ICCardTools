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
        byte[] snr = new byte[4];

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

        private void SectorCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtOutput != null && cmbSector != null && cmbBlock != null)
            {
                txtOutput.AppendText("\n");
                txtOutput.AppendText("当前区：" + cmbSector.SelectedIndex + "\n");
                txtOutput.AppendText("当前块：" + cmbBlock.SelectedIndex + "\n");
                txtOutput.ScrollToEnd();
            }
        }

        private void BlockCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtOutput != null && cmbSector != null && cmbBlock != null)
            {
                txtOutput.AppendText("\n");
                txtOutput.AppendText("当前区：" + cmbSector.SelectedIndex + "\n");
                txtOutput.AppendText("当前块：" + cmbBlock.SelectedIndex + "\n");
                txtOutput.ScrollToEnd();
            }
        }

        private void ReadDataBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");
            txtOutput.ScrollToEnd();

            st = App.rf_card(icdev, 0, snr);
            if (st == 0)
            {
                txtOutput.AppendText("寻卡成功\n");
                txtOutput.ScrollToEnd();

                byte[] snr1 = new byte[8];
                App.hex_a(snr, snr1, 4);
                txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");
                txtOutput.ScrollToEnd();

                UInt32 snr2;
                snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");
                txtOutput.ScrollToEnd();

                st = App.rf_authentication(icdev, 0, (byte)cmbSector.SelectedIndex);
                if (st == 0)
                {
                    txtOutput.AppendText("认证成功\n");
                    txtOutput.ScrollToEnd();

                    byte[] databuff = new byte[32];
                    st = App.rf_read_hex(icdev, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), databuff);
                    if (st == 0)
                    {
                        txtOutput.AppendText("读取成功\n");
                        txtOutput.ScrollToEnd();

                        txtReadData.Text = System.Text.Encoding.Default.GetString(databuff);
                    }
                    else
                    {
                        txtOutput.AppendText("读取失败\n");
                        txtOutput.ScrollToEnd();
                    }
                }
                else
                {
                    txtOutput.AppendText("认证失败\n");
                    txtOutput.ScrollToEnd();
                }
            }
            else
            {
                txtOutput.AppendText("寻卡失败\n");
                txtOutput.ScrollToEnd();
            }
        }

        private void WriteDataBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");
            txtOutput.ScrollToEnd();

            st = App.rf_card(icdev, 0, snr);
            if (st == 0)
            {
                txtOutput.AppendText("寻卡成功\n");
                txtOutput.ScrollToEnd();

                byte[] snr1 = new byte[8];
                App.hex_a(snr, snr1, 4);
                txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");
                txtOutput.ScrollToEnd();

                UInt32 snr2;
                snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");
                txtOutput.ScrollToEnd();

                st = App.rf_authentication(icdev, 0, (byte)cmbSector.SelectedIndex);
                if (st == 0)
                {
                    txtOutput.AppendText("认证成功\n");
                    txtOutput.ScrollToEnd();

                    byte[] databuff = new byte[32];
                    databuff = System.Text.Encoding.Default.GetBytes(txtWriteData.Text);
                    st = App.rf_write_hex(icdev, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), databuff);
                    if (st == 0)
                    {
                        txtOutput.AppendText("写入成功\n");
                        txtOutput.ScrollToEnd();
                    }
                    else
                    {
                        txtOutput.AppendText("写入失败\n");
                        txtOutput.ScrollToEnd();
                    }
                }
                else
                {
                    txtOutput.AppendText("认证失败\n");
                    txtOutput.ScrollToEnd();
                }
            }
            else
            {
                txtOutput.AppendText("寻卡失败\n");
                txtOutput.ScrollToEnd();
            }
        }
    }
}
