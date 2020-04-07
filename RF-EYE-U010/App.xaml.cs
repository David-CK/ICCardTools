using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace RF_EYE_U010
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("mwrf32.dll", EntryPoint = "rf_init", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr rf_init(Int16 port, Int32 baud);

        [DllImport("mwrf32.dll", EntryPoint = "rf_exit", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_exit(IntPtr icdev);

        [DllImport("mwrf32.dll", EntryPoint = "rf_card", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_card(IntPtr icdev, byte mode, byte[] snr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_authentication(IntPtr icdev, byte mode, byte secnr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_read", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_read(IntPtr icdev, byte blocknr, byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_read_hex", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_read_hex(IntPtr icdev, byte blocknr, byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_write", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_write(IntPtr icdev, byte blocknr, byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_write_hex", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_write_hex(IntPtr icdev, byte blocknr, byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "hex_a", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 hex_a(byte[] hex, byte[] asc, Int16 len);

        [DllImport("mwrf32.dll", EntryPoint = "a_hex", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 a_hex(byte[] asc, byte[] hex, Int16 len);

        [DllImport("mwrf32.dll", EntryPoint = "rf_encrypt", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_encrypt(byte[] key, byte[] src, UInt16 len, byte[] dest);

        [DllImport("mwrf32.dll", EntryPoint = "rf_decrypt", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_decrypt(byte[] key, byte[] src, UInt16 len, byte[] dest);
    }
}
