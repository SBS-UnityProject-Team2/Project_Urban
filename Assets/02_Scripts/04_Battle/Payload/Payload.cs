using System;
using System.Runtime.InteropServices;

public class Payload
{
    private const int BufferSize = 64;
    private readonly byte[] buffer = new byte[BufferSize];
    private int writePos;
    private int readPos;

    public virtual void Init()
    {
        writePos = 0;
        readPos  = 0;
    }

    public Payload Write<T>(T value) where T : unmanaged
    {
        MemoryMarshal.Write(buffer.AsSpan(writePos), ref value);
        writePos += Marshal.SizeOf<T>();

        return this;
    }

    public T Read<T>() where T : unmanaged
    {
        T value = MemoryMarshal.Read<T>(buffer.AsSpan(readPos));
        readPos += Marshal.SizeOf<T>();
        return value;
    }
}