﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpNFC;
using MvcApplication2.PInvoke;
using System.Runtime.InteropServices;

namespace MvcApplication2
{
    public class NFCContext : IDisposable
    {
        protected IntPtr contextPointer;

        public NFCContext()
        {
            Functions.nfc_init(out contextPointer);

            if (contextPointer == IntPtr.Zero)
            {
                Console.WriteLine("Invalid context");
            }
        }

        public List<string> ListDeviceNames()
        {
            int someUnknownCount = 8;
            IntPtr connectionStringsPointer = Marshal.AllocHGlobal(Constants.NFC_BUFSIZE_CONNSTRING * someUnknownCount);
            var devicesCount = Functions.nfc_list_devices(contextPointer, connectionStringsPointer, (uint)someUnknownCount);

            var devices = new List<string>();
            for (int i = 0; i < devicesCount; i++)
            {
                devices.Add(Marshal.PtrToStringAnsi(connectionStringsPointer + i * someUnknownCount));
            }

            Marshal.FreeHGlobal(connectionStringsPointer);

            return devices;
        }

        public virtual NFCDevice OpenDevice(string name)
        {
            IntPtr devicePointer;

            try
            {
                devicePointer = Functions.nfc_open(contextPointer, name);
                if (devicePointer == IntPtr.Zero)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new Exception("nfc_open() failed.");
            }

            return new NFCDevice(devicePointer);
        }


        public string Version()
        {
            var ver = Functions.nfc_version();

            return ver;
        }

        public virtual void Dispose()
        {
            Functions.nfc_exit(contextPointer);
        }
    }
}