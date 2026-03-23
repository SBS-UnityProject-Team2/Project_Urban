using System.IO;

public class Message
{
    private readonly MemoryStream stream = new();
    private readonly BinaryWriter writer;
    private readonly BinaryReader reader;

    public Message()
    {
        writer = new BinaryWriter(stream);
        reader = new BinaryReader(stream);
    }

    // *message << value
    public Message Write(int value)    { writer.Write(value); return this; }
    public Message Write(short value)  { writer.Write(value); return this; }
    public Message Write(byte value)   { writer.Write(value); return this; }
    public Message Write(float value)  { writer.Write(value); return this; }

    // *message >> value
    public int   ReadInt()   { return reader.ReadInt32(); }
    public short ReadShort() { return reader.ReadInt16(); }
    public byte  ReadByte()  { return reader.ReadByte();  }
    public float ReadFloat() { return reader.ReadSingle(); }

    public byte[] GetBuffer() => stream.ToArray();
    public int    DataSize   => (int)stream.Length;

    public void Reset()
    {
        stream.SetLength(0);
        stream.Position = 0;
    }

    public void ResetRead() => stream.Position = 0;
}