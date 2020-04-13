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
                    txtConnectStatus.Text = "已经连接设备";
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
                    txtConnectStatus.Text = "已经断开设备";
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

                byte[] ascKey = new byte[12];
                byte[] hexKey = new byte[6];

                if (cmbKey.SelectedIndex == 0)
                {
                    ascKey = Encoding.Default.GetBytes(txtKeyA.Text);
                    App.a_hex(ascKey, hexKey, 12);
                    st = App.rf_authentication_key(icdev, 0, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), hexKey);
                }
                else
                {
                    ascKey = Encoding.Default.GetBytes(txtKeyB.Text);
                    App.a_hex(ascKey, hexKey, 12);
                    st = App.rf_authentication_key(icdev, 4, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), hexKey);
                }
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

                byte[] ascKey = new byte[12];
                byte[] hexKey = new byte[6];

                if (cmbKey.SelectedIndex == 0)
                {
                    ascKey = Encoding.Default.GetBytes(txtKeyA.Text);
                    App.a_hex(ascKey, hexKey, 12);
                    st = App.rf_authentication_key(icdev, 0, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), hexKey);
                }
                else
                {
                    ascKey = Encoding.Default.GetBytes(txtKeyB.Text);
                    App.a_hex(ascKey, hexKey, 12);
                    st = App.rf_authentication_key(icdev, 4, (byte)(cmbSector.SelectedIndex * 4 + cmbBlock.SelectedIndex), hexKey);
                }
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

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            if ((bool)chkDataKey.IsChecked)
            {
                byte[] ascDataKey = new byte[16];
                byte[] hexDataKey = new byte[8];

                byte[] ascDataBuff = new byte[32];
                byte[] hexDataBuff = new byte[16];

                byte[] ascEncryptBuff = new byte[32];
                byte[] hexEncryptBuff = new byte[16];

                ascDataKey = Encoding.Default.GetBytes(txtDataKey.Text);
                App.a_hex(ascDataKey, hexDataKey, 16);

                ascDataBuff = Encoding.Default.GetBytes(txtWriteData.Text);
                App.a_hex(ascDataBuff, hexDataBuff, 32);

                App.rf_encrypt(hexDataKey, hexDataBuff, 16, hexEncryptBuff);
                App.hex_a(hexEncryptBuff, ascEncryptBuff, 16);

                txtWriteData.Text = Encoding.Default.GetString(ascEncryptBuff);
                txtOutput.AppendText("数据加密成功\n");
            }
            else
            {
                txtOutput.AppendText("请开启数据密钥\n");
            }
            txtOutput.ScrollToEnd();
        }

        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            if ((bool)chkDataKey.IsChecked)
            {
                byte[] ascDataKey = new byte[16];
                byte[] hexDataKey = new byte[8];

                byte[] ascDataBuff = new byte[32];
                byte[] hexDataBuff = new byte[16];

                byte[] ascDecryptBuff = new byte[32];
                byte[] hexDecryptBuff = new byte[16];

                ascDataKey = Encoding.Default.GetBytes(txtDataKey.Text);
                App.a_hex(ascDataKey, hexDataKey, 16);

                ascDataBuff = Encoding.Default.GetBytes(txtReadData.Text);
                App.a_hex(ascDataBuff, hexDataBuff, 32);

                App.rf_decrypt(hexDataKey, hexDataBuff, 16, hexDecryptBuff);
                App.hex_a(hexDecryptBuff, ascDecryptBuff, 16);

                txtReadData.Text = Encoding.Default.GetString(ascDecryptBuff);
                txtOutput.AppendText("数据解密成功\n");
            }
            else
            {
                txtOutput.AppendText("请开启数据密钥\n");
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

        private void LoadKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.AppendText("\n");

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "ICK|*.ick";

            if ((bool)openFileDialog.ShowDialog())
            {
                txtLoadKey.Text = openFileDialog.FileName;

                System.IO.StreamReader streamReader = new System.IO.StreamReader(txtLoadKey.Text, Encoding.Unicode, true);

                string[] keySet = streamReader.ReadToEnd().Split('\n');

                char[] sp = { '\t', '\r' };

                for (int i = 0; i < 16; i++)
                {
                    string[] key = keySet[i + 1].Split(sp);

                    byte[] keyA = Encoding.Default.GetBytes(key[1]);
                    byte[] keyB = Encoding.Default.GetBytes(key[2]);

                    App.rf_load_key_hex(icdev, 0, (byte)i, keyA);
                    App.rf_load_key_hex(icdev, 4, (byte)i, keyB);
                }

                txtOutput.AppendText("加载密钥成功\n");
            }
            else
            {
                txtLoadKey.Text = "";

                txtOutput.AppendText("加载密钥失败\n");
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

                byte[] ascInfoKey = new byte[16];
                byte[] hexInfoKey = new byte[8];

                if ((bool)chkInfoKey.IsChecked)
                {
                    ascInfoKey = Encoding.Default.GetBytes(txtInfoKey.Text);
                    App.a_hex(ascInfoKey, hexInfoKey, 16);
                }

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
                                if ((bool)chkInfoKey.IsChecked)
                                {
                                    byte[] decrypt = new byte[16];

                                    App.rf_decrypt(hexInfoKey, buff, 16, decrypt);
                                    
                                    decrypt.CopyTo(buffer, blockIndex * 16);
                                }
                                else
                                {
                                    buff.CopyTo(buffer, blockIndex * 16);
                                }
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

                        byte[] ascInfoKey = new byte[16];
                        byte[] hexInfoKey = new byte[8];

                        if ((bool)chkInfoKey.IsChecked)
                        {
                            ascInfoKey = Encoding.Default.GetBytes(txtInfoKey.Text);
                            App.a_hex(ascInfoKey, hexInfoKey, 16);
                        }

                        int writeCnt = 0;

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

                                    if ((bool)chkInfoKey.IsChecked)
                                    {
                                        byte[] encrypt = new byte[16];

                                        App.rf_encrypt(hexInfoKey, buffer[blockIndex], 16, encrypt);

                                        st = App.rf_write(icdev, (byte)(sectorIndex * 4 + blockIndex), encrypt);
                                    }
                                    else
                                    {
                                        st = App.rf_write(icdev, (byte)(sectorIndex * 4 + blockIndex), buffer[blockIndex]);
                                    }
                                    if (st == 0)
                                    {
                                        writeCnt += 1;
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

                        if ((writeCnt == 45) && (cmbLine.SelectedIndex == 0))
                        {
                            if (line < info.Length - 2)
                            {
                                line += 1;
                                txtLine.Text = Convert.ToString(line);
                            }
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

        private void FirstInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info != null)
            {
                txtLine.Text = "1";
            }
        }

        private void PreviousInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info != null)
            {
                Int32 line = Convert.ToInt32(txtLine.Text);
                line -= 1;
                if (line < 1 || line > info.Length - 2)
                {
                    line = 1;
                }
                txtLine.Text = Convert.ToString(line);
            }
        }

        private void NextInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info != null)
            {
                Int32 line = Convert.ToInt32(txtLine.Text);
                line += 1;
                if (line < 1 || line > info.Length - 2)
                {
                    line = info.Length - 2;
                }
                txtLine.Text = Convert.ToString(line);
            }
        }

        private void LastInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (info != null)
            {
                txtLine.Text = Convert.ToString(info.Length - 2);
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Width = 1000;
            Application.Current.MainWindow.Height = 780;
            mainGrid.Width = 980;
            mainGrid.Height = 740;
            mainExpander.Width = 980;
            mainExpander.Height = 360;
            mainStatusBar.Width = 880;
            txtOutput.Width = 460;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Width = 800;
            Application.Current.MainWindow.Height = 450;
            mainGrid.Width = 780;
            mainGrid.Height = 410;
            mainExpander.Width = 100;
            mainExpander.Height = 30;
            mainStatusBar.Width = 680;
            txtOutput.Width = 260;
        }

        private void OpenCardBtn_Click(object sender, RoutedEventArgs e)
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

                //byte[] databuff3 = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                byte[] databuff3 = { 0xFF, 0xFF, 0xFF, 0xF0, 0xF1, 0xF2, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xF3, 0xF4, 0xF5 };

                for (int sectorIndex = 0; sectorIndex < 16; sectorIndex++)
                {
                    txtOutput.AppendText("\n");

                    st = App.rf_authentication(icdev, 0, (byte)sectorIndex);
                    if (st == 0)
                    {
                        txtOutput.AppendText("认证成功，区：" + sectorIndex + "\n");

                        st = App.rf_write(icdev, (byte)(sectorIndex * 4 + 3), databuff3);
                        if (st == 0)
                        {
                            txtOutput.AppendText("写入成功，区：" + sectorIndex + "\n");
                        }
                        else
                        {
                            txtOutput.AppendText("写入失败，区：" + sectorIndex + "\n");
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
            txtOutput.ScrollToEnd();
        }

        private void ReadCardBtn_Click(object sender, RoutedEventArgs e)
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

                for (int sectorIndex = 0; sectorIndex < 16; sectorIndex++)
                {
                    txtOutput.AppendText("\n");

                    st = App.rf_authentication(icdev, 0, (byte)sectorIndex);
                    if (st == 0)
                    {
                        txtOutput.AppendText("认证成功，区：" + sectorIndex + "\n");

                        byte[] databuff = new byte[32];

                        for (int blockIndex = 0; blockIndex < 4; blockIndex++)
                        {
                            st = App.rf_read_hex(icdev, (byte)(sectorIndex * 4 + blockIndex), databuff);
                            if (st == 0)
                            {
                                TextBox txtCard = FindName("txtCard" + sectorIndex.ToString("X") + blockIndex.ToString("X")) as TextBox;
                                txtCard.Text = Encoding.Default.GetString(databuff);

                                txtOutput.AppendText("读取成功，区：" + sectorIndex + "，块：" + blockIndex + "\n");
                            }
                            else
                            {
                                txtOutput.AppendText("读取失败，区：" + sectorIndex + "，块：" + blockIndex + "\n");
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
            txtOutput.ScrollToEnd();
        }

        private void WriteCardBtn_Click(object sender, RoutedEventArgs e)
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

                byte[] databuff0 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] databuff3 = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                for (int sectorIndex = 0; sectorIndex < 16; sectorIndex++)
                {
                    txtOutput.AppendText("\n");

                    st = App.rf_authentication(icdev, 0, (byte)sectorIndex);
                    if (st == 0)
                    {
                        txtOutput.AppendText("认证成功，区：" + sectorIndex + "\n");

                        for (int blockIndex = 0; blockIndex < 4; blockIndex++)
                        {
                            if (sectorIndex == 0 && blockIndex == 0)
                            {
                                continue;
                            }
                            if (blockIndex == 3)
                            {
                                st = App.rf_write(icdev, (byte)(sectorIndex * 4 + blockIndex), databuff3);
                            }
                            else
                            {
                                st = App.rf_write(icdev, (byte)(sectorIndex * 4 + blockIndex), databuff0);
                            }
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
            txtOutput.ScrollToEnd();
        }
    }
}
