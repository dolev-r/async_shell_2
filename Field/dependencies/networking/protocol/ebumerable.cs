using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

// Collection of Person objects. This class
// implements IEnumerable so that it can be used
// with ForEach syntax.
public class PacketGenerator : IEnumerable
{
    private List<byte[]> _packets;
    private int _session_id;
    private int _buffer_size;
    public PacketGenerator(byte[] data_buffer, int session_id, int buffer_size)
    {
        this._session_id = session_id;
        this._buffer_size = buffer_size;
        this.GeneratePackets(data_buffer);
    }

    private byte[] Header(byte[] data_buffer)
    {
        return BitConverter.GetBytes(data_buffer.Length);
    }
    private void GeneratePackets(byte[] data_buffer)
    {
        data_buffer = this.Header(data_buffer).Concat(data_buffer).ToArray();
        
        for (int i = 0; i < data_buffer.Length; i += this._buffer_size - 4)
        {
            Index start = i;
            Index end = i + this._buffer_size - 4;
            this._packets.Add(AddSessionID(data_buffer[start..end]));
        }
    }

    private byte[] AddSessionID(byte[] buffer)
    {
        byte[] chunk_header = BitConverter.GetBytes(this._session_id);
        return chunk_header.Concat(buffer).ToArray();
    }

// Implementation for the GetEnumerator method.
    IEnumerator IEnumerable.GetEnumerator()
    {
       return (IEnumerator) GetEnumerator();
    }

    public PeopleEnum GetEnumerator()
    {
        return new PeopleEnum(_packets);
    }
}

// When you implement IEnumerable, you must also implement IEnumerator.
public class PeopleEnum : IEnumerator
{
    public List<byte[]> _buffers;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    private int position = -1;

    public PeopleEnum(List<byte[]> list)
    {
        _buffers = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _buffers.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public byte[] Current
    {
        get
        {
            try
            {
                return _buffers[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

class App
{
    static void Main()
    {
        

        PacketGenerator peopleList = new PacketGenerator(peopleArray);
        foreach (Packet p in peopleList)
            Console.WriteLine(p.firstName + " " + p.lastName);
    }
}

/* This code produces output similar to the following:
 *
 * John Smith
 * Jim Johnson
 * Sue Rabon
 *
 */