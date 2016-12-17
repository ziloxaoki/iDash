using iDash.rFactor1Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace iDash
{
    public class RF1SharedMemoryReader : RFactorDataReader
    {
        private MemoryMappedFile memoryMappedFile;
        private GCHandle handle;
        private int sharedmemorysize;
        private byte[] sharedMemoryReadBuffer;
        private Boolean initialised = false;

        public class RF1StructWrapper
        {
            public long ticksWhenRead;
            public rfShared data;
        }

        protected override Boolean InitialiseInternal()
        {

            lock (this)
            {
                if (!initialised)
                {
                    try
                    {
                        memoryMappedFile = MemoryMappedFile.OpenExisting(rFactor1Constant.SharedMemoryName);
                        sharedmemorysize = Marshal.SizeOf(typeof(rfShared));
                        sharedMemoryReadBuffer = new byte[sharedmemorysize];
                        initialised = true;
                        Console.WriteLine("Initialised rFactor 1 shared memory");
                    }
                    catch (Exception)
                    {
                        initialised = false;
                    }
                }
                return initialised;
            }
        }

        public override Object ReadGameData()
        {
            lock (this)
            {
                rfShared _rf1apistruct = new rfShared();
                if (!initialised)
                {
                    if (!InitialiseInternal())
                    {
                        //throw new GameDataReadException("Failed to initialise shared memory");
                        return null;
                    }
                }
                try
                {
                    using (var sharedMemoryStreamView = memoryMappedFile.CreateViewStream())
                    {
                        BinaryReader _SharedMemoryStream = new BinaryReader(sharedMemoryStreamView);
                        sharedMemoryReadBuffer = _SharedMemoryStream.ReadBytes(sharedmemorysize);
                        handle = GCHandle.Alloc(sharedMemoryReadBuffer, GCHandleType.Pinned);
                        _rf1apistruct = (rfShared)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(rfShared));
                        handle.Free();
                    }
                    RF1StructWrapper structWrapper = new RF1StructWrapper();
                    structWrapper.ticksWhenRead = DateTime.Now.Ticks;
                    structWrapper.data = _rf1apistruct;

                    return structWrapper;
                }
                catch (Exception ex)
                {
                    //throw new GameDataReadException(ex.Message, ex);
                    return null;
                }
            }
        }

        private rfVehicleInfo[] getPopulatedVehicleInfoArray(rfVehicleInfo[] raw)
        {
            List<rfVehicleInfo> populated = new List<rfVehicleInfo>();
            foreach (rfVehicleInfo rawData in raw)
            {
                if (rawData.place > 0)
                {
                    populated.Add(rawData);
                }
            }
            if (populated.Count == 0)
            {
                populated.Add(raw[0]);
            }
            return populated.ToArray();
        }

        public override void Dispose()
        {
            if (memoryMappedFile != null)
            {
                try
                {
                    memoryMappedFile.Dispose();
                }
                catch (Exception) { }
            }
        }
    }
}
