namespace AoC24.Common
{
    public static class MathHelper
    {
        // Greatest Common Divisor
        public static long GCD(long num1, long num2)
            => (num2 == 0) ? num1 : GCD(num2, num1 % num2);

        // Least Common Multiple
        public static long LCM(List<long> numbers)
            => numbers.Aggregate((long S, long val) => S * val / GCD(S, val));

        public static int Modulo(int a, int b)
            => (a % b + b) % b;
    }
}
