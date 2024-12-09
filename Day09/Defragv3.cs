using System.Text;

namespace AoC24.Day09
{
    internal class Defragv3
    {
        string Input = "";
        Dictionary<int, int> disk = new();
        public void ParseInput(List<string> input)
            => Input = input[0];

        void BuildDisk()
        {
            int ptr = 0;
            int fileSeq = -1;
            bool isFree = true;

            for (int i = 0; i < Input.Length; i++)
            {
                isFree = !isFree;
                fileSeq = isFree ? fileSeq : fileSeq + 1;
                
                var numBlocks = int.Parse(Input[i].ToString());
                if (numBlocks == 0)
                    continue;

                for (int j = 0; j < numBlocks; j++)
                    disk[ptr++] = isFree ? -1 : fileSeq;
            }
        }

        int FindFileBackwards(int from, int until)
        {
            var pos = from;
            while (disk[pos] == -1 && pos>until)
                pos--;
            return pos == until ? -1 : pos;
        }

        int FindSpaceForward(int from, int until)
        {
            var pos = from;
            var lastPos = disk.Keys.Max();
            while (disk[pos] != -1 && pos < until)
                pos++;
            return pos == until ? -1 : pos;
        }

        long CheckSum()
        {
            long ret = 0;
            for (int i = 0; i < disk.Keys.Max(); i++)
                ret += disk[i] == -1 ? 0 : (long) (disk[i] * i);
            return ret;
        }

        void Display()
        {
            StringBuilder sb = new();
            for(int i = 0; i<=disk.Keys.Max(); i++)
                sb.Append(disk[i] == -1 ? "." : disk[i].ToString());
            Console.WriteLine(sb.ToString());
        }

        long CompactDiskSlow()      // 93s on my machine - compared with 0.8 that take v0 of defrag
        {
            BuildDisk();
            int tail = disk.Keys.Max();
            int head = 0;

            while (true)
            {
                head = disk.Keys.First(x => disk[x] == -1);
                tail = disk.Keys.Last(x => disk[x] != -1);
                if (head > tail)
                    break;
                disk[head] = disk[tail];
                disk[tail] = -1;
            }
            return CheckSum();
        }

        long CompactDisk()      // 44s on my machine - compared with 0.8 that take v0 of defrag
        {
            BuildDisk();
            int tail = disk.Keys.Max();
            int head = 0;

            while (true)
            {
                head = FindSpaceForward(head, tail);
                if (head == -1)
                    break;

                tail = FindFileBackwards(tail, head);
                if (tail == -1)
                    break;

                disk[head] = disk[tail];
                disk[tail] = -1;
            }
            return CheckSum();
        }

        public long Solve(int part = 1)
            => CompactDisk();
    }
}
