namespace AoC24.Day09
{
    record Sector(int id, bool free);

    class Block
    {
        public Sector contents;
        public Block prev;
        public Block next;

        public Block(Sector contents, Block prev, Block next)
        { 
            this.contents = contents;
            this.prev = prev;
            this.next = next;
        }

        public int Id
            => this.contents.id;

        public bool IsFree
            => this.contents.free;
        public Block GetLast()
        {
            Block ptr = this;
            
            while (ptr.next != null)
                ptr = ptr.next;
            
            return ptr;
        }

        public Block FindFileHead()
        {
            Block ptr = this;
            var id = this.Id;

            while (ptr.prev != null && ptr.prev.Id == id)
                ptr = ptr.prev;
            
            return ptr;
        }

        public int ContiguousBlocks()
        {
            Block ptr = this;
            bool isFree = IsFree;
            int id = Id;
            int count = 0;
            
            while (ptr != null && ptr.Id == Id && ptr.IsFree == isFree)
            {
                count++;
                ptr = ptr.next;
            }
            
            return count;
        }

        public Block FindFirstFreeUntil(Block stop)
        {
            Block ptr = this;
            while (ptr != null && !ptr.IsFree && ptr!=stop)
                ptr = ptr.next;
            
            return ptr == null ? null : ptr == stop ? null : ptr;
        }

        public Block FindFirstFileUntil(Block stop)
        {
            Block ptr = this;
            while (ptr != null && ptr.IsFree && ptr != stop)
                ptr = ptr.prev;

            return ptr == null ? null : ptr == stop ? null : ptr;
        }

        public void Move(Block other)
        {
            var tmp = this.contents;
            this.contents = other.contents;
            other.contents = tmp;
        }

        public long Checksum()
        {
            var ptr = this;
            long retVal = 0;
            long pos = 0;

            while (ptr != null)
            {
                if (!ptr.IsFree)
                    retVal += (pos * ptr.Id);
                ptr = ptr.next;
                pos++;
            }

            return retVal;
        }
    }

    internal class Defragv2
    {
        string Input = "";
        Block diskHead = null;

        public void ParseInput(List<string> input)
            => Input = input[0];

        Block BuildChunk(int numBlocks, int fileID, bool isFree)
        { 
            var sector = new Sector(fileID, isFree);
            var retVal = new Block(sector, null, null);
            var current = retVal;
            
            for (int i = 1; i < numBlocks; i++)
            { 
                var sect = new Sector(fileID, isFree);
                var block = new Block(sect, current, null);
                current.next = block;
                current = block;
            }

            return retVal;
        }

        void BuildDisk()
        {
            bool isFree = true;
            Block current = null;
            Block lastBlock = null;
            int fileSeq = -1;

            for (int i = 0; i < Input.Length; i++)
            {
                isFree = !isFree;
                fileSeq = isFree ? fileSeq : fileSeq + 1;

                var numBlocks = int.Parse(Input[i].ToString());
                if (numBlocks == 0)
                    continue;

                Block chunk = BuildChunk(numBlocks, fileSeq, isFree);
                if (current == null)
                {
                    diskHead = chunk;
                    current = chunk;
                    lastBlock = chunk.GetLast();
                    continue;
                }

                lastBlock.next = chunk;
                chunk.prev = lastBlock;
                current = chunk;
                lastBlock = chunk.GetLast();
            }
        }

        long CompactDisk()
        {
            BuildDisk();
            Block tail = diskHead.GetLast();
            Block head = diskHead;

            while (true)
            {
                head = head.FindFirstFreeUntil(tail);
                if (head == null)
                    break;

                tail = tail.FindFirstFileUntil(head);
                if (tail == null)
                    break;

                // Step 3 - Move blocks
                head.Move(tail);
            }
            return diskHead.Checksum();
        }

        long CompactDiskFiles()
        {
            BuildDisk();
            Block last = diskHead.GetLast();
            Block head = diskHead;
            HashSet<int> attempted = new HashSet<int>();

            while (true)
            {
                
                Block tail = last;
                var filePtr = tail;

                while (filePtr != null && 
                      (filePtr.IsFree || (!filePtr.IsFree && attempted.Contains(filePtr.Id))))
                        filePtr = filePtr.prev;

                if (filePtr == null)
                    break;

                // Step 2 - Calculate the blocks that the file in question occupies
                Block fileHead = filePtr.FindFileHead();
                int blockCount = fileHead.ContiguousBlocks();

                // Step 3 - Find a contiguous free space block of blockCount blocks
                head = diskHead;
                var relocation = head.FindFirstFreeUntil(fileHead);

                while(relocation!=null && relocation.ContiguousBlocks()<blockCount)
                    relocation = relocation.next.FindFirstFreeUntil(fileHead);

                attempted.Add(fileHead.Id);

                if (relocation == null)
                    continue;

                for (int i = 0; i < blockCount; i++)
                {
                    relocation.Move(fileHead);
                    fileHead = fileHead.next;
                    relocation = relocation.next;
                }
            }

            return diskHead.Checksum();
        }

        public long Solve(int part = 1)
           => part ==1 ? CompactDisk() : CompactDiskFiles();
    }
}
