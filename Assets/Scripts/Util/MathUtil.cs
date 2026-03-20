namespace Util
{
    public static class MathUtil
    {
        public static int Log2(int value)
        {
            int i;
            for (i = -1; value != 0; i++)
                value >>= 1;
            return i == -1 ? 0 : i;
        }
    
        public static int BitWidth(int value)
        {
            return value == 0 ? 0 : 1 + Log2(value);
        }
    }
}