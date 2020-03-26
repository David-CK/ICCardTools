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
        string[] info;

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            if (icdev.ToInt32() > 0)
            {
                txtOutput.AppendText("设备已经连接\n");
            }
            else
            {
                icdev = App.rf_init(port, baud);
                if (icdev.ToInt32() > 0)
                {
                    txtOutput.AppendText("设备连接成功\n");
                }
                else
                {
                    txtOutput.AppendText("设备连接失败\n");
                }
            }
            txtOutput.ScrollToEnd();
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            if (icdev.ToInt32() > 0)
            {
                st = App.rf_exit(icdev);
                if (st == 0)
                {
                    icdev = IntPtr.Zero;

                    txtOutput.AppendText("设备断开成功\n");
                }
                else
                {
                    txtOutput.AppendText("设备断开失败\n");
                }
            }
            else
            {
                txtOutput.AppendText("设备已经断开\n");
            }
            txtOutput.ScrollToEnd();
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

            st = App.rf_card(icdev, 0, snr);
            if (st == 0)
            {
                txtOutput.AppendText("寻卡成功\n");

                byte[] snr1 = new byte[8];
                App.hex_a(snr, snr1, 4);
                txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");

                UInt32 snr2;
                snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");

                st = App.rf_authentication(icdev, 0, (byte)cmbSector.SelectedIndex);
                if (st == 0)
                {
                    txtOutput.AppendText("认证成功\n");

                    byte[] databuff = new byte[32];
                    st = App.rf_read_hex(icdev, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), databuff);
                    if (st == 0)
                    {
                        txtOutput.AppendText("读取成功\n");

                        txtReadData.Text = System.Text.Encoding.Default.GetString(databuff);
                    }
                    else
                    {
                        txtOutput.AppendText("读取失败\n");
                    }
                }
                else
                {
                    txtOutput.AppendText("认证失败\n");
                }
            }
            else
            {
                txtOutput.AppendText("寻卡失败\n");
            }
            txtOutput.ScrollToEnd();
        }

        private void WriteDataBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            st = App.rf_card(icdev, 0, snr);
            if (st == 0)
            {
                txtOutput.AppendText("寻卡成功\n");

                byte[] snr1 = new byte[8];
                App.hex_a(snr, snr1, 4);
                txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");

                UInt32 snr2;
                snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");

                st = App.rf_authentication(icdev, 0, (byte)cmbSector.SelectedIndex);
                if (st == 0)
                {
                    txtOutput.AppendText("认证成功\n");

                    byte[] databuff = new byte[32];
                    databuff = System.Text.Encoding.Default.GetBytes(txtWriteData.Text);
                    st = App.rf_write_hex(icdev, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), databuff);
                    if (st == 0)
                    {
                        txtOutput.AppendText("写入成功\n");
                    }
                    else
                    {
                        txtOutput.AppendText("写入失败\n");
                    }
                }
                else
                {
                    txtOutput.AppendText("认证失败\n");
                }
            }
            else
            {
                txtOutput.AppendText("寻卡失败\n");
            }
            txtOutput.ScrollToEnd();
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "ICD|*.icd";

            if ((bool)openFileDialog.ShowDialog())
            {
                txtOpenFile.Text = openFileDialog.FileName;

                System.IO.StreamReader streamReader = new System.IO.StreamReader(txtOpenFile.Text, Encoding.Unicode, true);
                info = streamReader.ReadToEnd().Split('\n');

                txtOutput.AppendText("打开文件成功\n");
            }
            else
            {
                txtOpenFile.Text = "";

                txtOutput.AppendText("打开文件失败\n");
            }
            txtOutput.ScrollToEnd();
        }

        private void ReadInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            st = App.rf_card(icdev, 0, snr);
            if (st == 0)
            {
                txtOutput.AppendText("寻卡成功\n");

                byte[] snr1 = new byte[8];
                App.hex_a(snr, snr1, 4);
                txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");

                UInt32 snr2;
                snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");

                for (int sectorIndex = 1; sectorIndex < 16; sectorIndex++)
                {
                    txtOutput.AppendText("\n");

                    st = App.rf_authentication(icdev, 0, (byte)sectorIndex);
                    if (st == 0)
                    {
                        txtOutput.AppendText("认证成功，区：" + sectorIndex + "\n");

                        byte[] buffer = new byte[48];
                        byte[] buff = new byte[16];

                        for (int blockIndex = 0; blockIndex < 3; blockIndex++)
                        {
                            st = App.rf_read(icdev, (byte)(sectorIndex * 4 + blockIndex), buff);
                            if (st == 0)
                            {
                                buff.CopyTo(buffer, blockIndex * 16);
                                txtOutput.AppendText("读取成功，区：" + sectorIndex + "，块：" + blockIndex + "\n");
                            }
                            else
                            {
                                txtOutput.AppendText("读取失败，区：" + sectorIndex + "，块：" + blockIndex + "\n");
                            }
                        }

                        string data = Encoding.Unicode.GetString(buffer).Replace("\0", "");

                        txtOutput.AppendText("信息：" + data + "\n");
                    }
                    else
                    {
                        txtOutput.AppendText("认证失败，区：" + sectorIndex + "\n");
                    }
                    txtOutput.ScrollToEnd();
                }
            }
            else
            {
                txtOutput.AppendText("寻卡失败\n");
            }
            txtOutput.ScrollToEnd();
        }

        private void WriteInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            if (info != null)
            {
                Int32 line = Convert.ToInt32(txtLine.Text);
                if ((line > 0) && (line < info.Length - 1))
                {
                    char[] sp = { '\t', '\r' };
                    string[] data = info[line].Split(sp);

                    st = App.rf_card(icdev, 0, snr);
                    if (st == 0)
                    {
                        txtOutput.AppendText("寻卡成功\n");

                        byte[] snr1 = new byte[8];
                        App.hex_a(snr, snr1, 4);
                        txtOutput.AppendText("卡号：" + System.Text.Encoding.Default.GetString(snr1) + "\n");

                        UInt32 snr2;
                        snr2 = (UInt32)((snr[0]) | (snr[1] << 8) | (snr[2] << 16) | (snr[3] << 24));
                        txtOutput.AppendText("卡号：" + Convert.ToString(snr2) + "\n");

                        for (int sectorIndex = 1; sectorIndex < 16; sectorIndex++)
                        {
                            txtOutput.AppendText("\n");

                            st = App.rf_authentication(icdev, 0, (byte)sectorIndex);
                            if (st == 0)
                            {
                                txtOutput.AppendText("认证成功，区：" + sectorIndex + "\n");

                                int cnt;
                                if (sectorIndex < data.Length)
                                {
                                    cnt = Encoding.Unicode.GetByteCount(data[sectorIndex - 1]);
                                }
                                else
                                {
                                    cnt = 0;
                                }

                                byte[][] buffer = new byte[3][];
                                buffer[0] = new byte[16];
                                buffer[1] = new byte[16];
                                buffer[2] = new byte[16];

                                for (int blockIndex = 0; blockIndex < 3; blockIndex++)
                                {
                                    if (cnt >= 16)
                                    {
                                        Encoding.Unicode.GetBytes(data[sectorIndex - 1], blockIndex * 8, 8, buffer[blockIndex], 0);
                                        cnt -= 16;
                                    }
                                    else if (cnt > 0 && cnt < 16)
                                    {
                                        Encoding.Unicode.GetBytes(data[sectorIndex - 1], blockIndex * 8, cnt / 2, buffer[blockIndex], 0);
                                        cnt = 0;
                                    }

                                    st = App.rf_write(icdev, (byte)(sectorIndex * 4 + blockIndex), buffer[blockIndex]);
                                    if (st == 0)
                                    {
                                        txtOutput.AppendText("写入成功，区：" + sectorIndex + "，块：" + blockIndex + "\n");
                                    }
                                    else
                                    {
                                        txtOutput.AppendText("写入失败，区：" + sectorIndex + "，块：" + blockIndex + "\n");
                                    }
                                }
                            }
                            else
                            {
                                txtOutput.AppendText("认证失败，区：" + sectorIndex + "\n");
                            }
                            txtOutput.ScrollToEnd();
                        }
                    }
                    else
                    {
                        txtOutput.AppendText("寻卡失败\n");
                    }
                }
                else
                {
                    txtOutput.AppendText("请输入信息...\n");
                }
            }
            else
            {
                txtOutput.AppendText("请打开文件...\n");
            }
            txtOutput.ScrollToEnd();
        }
    }
}
