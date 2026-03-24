using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ActionPayload
{
    private const int BufferSize = 64;
    private readonly byte[] buffer = new byte[BufferSize];
    private int writePos;
    private int readPos;

    public ActorAction actionId;
    public Actor source;
    public List<Actor> targets = new();

    public void Init()
    {
        actionId = ActorAction.None;
        source   = null;
        targets.Clear();
        writePos = 0;
        readPos  = 0;
    }

    public void Write<T>(T value) where T : unmanaged
    {
        MemoryMarshal.Write(buffer.AsSpan(writePos), ref value);
        writePos += Marshal.SizeOf<T>();
    }

    public T Read<T>() where T : unmanaged
    {
        T value = MemoryMarshal.Read<T>(buffer.AsSpan(readPos));
        readPos += Marshal.SizeOf<T>();
        return value;
    }
}