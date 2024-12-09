

using System.Text;

namespace AoC24.Day09
{

    class DiskBlock
    {
        public int id;
        public bool free;
        public DiskBlock? next;
        public DiskBlock? previous;

        public DiskBlock(int id, bool free, DiskBlock? next, DiskBlock? previous)
        {
            this.id = id;
            this.free = free;
            this.next = next;
            this.previous = previous;
        }

        public DiskBlock GetLast()
        {
            DiskBlock ptr = this;

            while (ptr.next != null)
                ptr = ptr.next;

            return ptr;
        }
    } 

    internal class Defrag
    {
        string Input;
        DiskBlock firstBlock = null;

        public void ParseInput(List<string> input)
            => Input = input[0];


        DiskBlock BuildChunk(int numBlocks, int fileID, bool isFree, DiskBlock? last)
        {
            DiskBlock chunk = new DiskBlock(fileID, isFree, null, last);
            DiskBlock current = chunk;
            for (int j = 1; j < numBlocks; j++)
            {
                DiskBlock nextBlock = new DiskBlock(fileID, isFree, null, last);
                current.next = nextBlock;
                nextBlock.previous = current;
                current = nextBlock;
            }

            return chunk;
        }

        void BuildStructure()
        {
            int fileSequence = 0;
            bool isFree = false;
            DiskBlock current = null;
            DiskBlock last = null;

            for (int i = 0; i < Input.Length; i++)
            {
                var numBlocks = int.Parse(Input[i].ToString());
                if (numBlocks == 0)
                {
                    isFree = !isFree;
                    continue;
                }

                var chunk = BuildChunk(numBlocks, fileSequence, isFree, last);

                if (firstBlock == null)
                {
                    firstBlock = chunk;
                    current = chunk;
                    last = chunk.GetLast();
                }
                else
                {
                    
                    last.next = chunk;
                    chunk.previous = last;
                    current = chunk;
                    last = chunk.GetLast();
                }

                if (!isFree)
                    fileSequence++;

                isFree = !isFree;
            }
        }

        void Display()
        {
            var ptr = firstBlock;
            StringBuilder sb = new();
            while (ptr != null)
            {
                sb.Append(!ptr.free ? ptr.id.ToString() : '.');
                ptr = ptr.next;
            }

            Console.WriteLine(sb.ToString());
        }

        long CompactDisk()
        {
            BuildStructure();

            DiskBlock tail = firstBlock.GetLast();
            DiskBlock head = firstBlock;
            bool stop = false;
            while (true)
            {
                // Step 1 - Find the first free block
                while (!head.free)
                {
                    head = head.next;
                    if ((head == null) || (tail == head))
                    {
                        stop = true;
                        break;
                    }
                }

                // Step 2 - Find the first file block from the tail
                while (tail.free && !stop)
                {
                    tail = tail.previous;
                    if ( (tail == null) || (tail == head))
                    {
                        stop = true;
                        break;
                    }
                }

                if (stop)
                    break;

                // Step 3 - Move blocks
                head.free = false;
                head.id = tail.id;
                tail.id = 0;
                tail.free = true;
            }

            head = firstBlock;
            long retVal = 0;

            long pos = 0;
            while (head != null)
            {
                if (!head.free)
                    retVal += (pos * head.id);
                head = head.next;
                pos++;
            }
            
            return retVal;    
        }

        public long Solve(int part = 1)
            => CompactDisk();
    }
}
