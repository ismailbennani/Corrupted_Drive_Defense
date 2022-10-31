namespace Utils
{
    public static class Uid
    {
        private static long _counter = 1;

        public static long Get()
        {
            return _counter++;
        }
    }
}
