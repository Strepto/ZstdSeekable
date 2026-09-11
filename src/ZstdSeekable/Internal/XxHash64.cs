using System;

namespace ZstdSeekable.Internal
{
    internal static class XxHash64
    {
        const ulong Prime1 = 11400714785074694791UL;
        const ulong Prime2 = 14029467366897019727UL;
        const ulong Prime3 = 1609587929392839161UL;
        const ulong Prime4 = 9650029242287828579UL;
        const ulong Prime5 = 2870177450012600261UL;

        public static ulong Hash(ReadOnlySpan<byte> data, ulong seed = 0)
        {
            ulong hash;
            var remaining = data;

            if (data.Length >= 32)
            {
                var acc1 = seed + Prime1 + Prime2;
                var acc2 = seed + Prime2;
                var acc3 = seed;
                var acc4 = seed - Prime1;

                while (remaining.Length >= 32)
                {
                    acc1 = Round(acc1, ReadUInt64(remaining));
                    acc2 = Round(acc2, ReadUInt64(remaining.Slice(8)));
                    acc3 = Round(acc3, ReadUInt64(remaining.Slice(16)));
                    acc4 = Round(acc4, ReadUInt64(remaining.Slice(24)));
                    remaining = remaining.Slice(32);
                }

                hash = RotateLeft(acc1, 1) + RotateLeft(acc2, 7) + RotateLeft(acc3, 12) + RotateLeft(acc4, 18);
                hash = MergeRound(hash, acc1);
                hash = MergeRound(hash, acc2);
                hash = MergeRound(hash, acc3);
                hash = MergeRound(hash, acc4);
            }
            else
            {
                hash = seed + Prime5;
            }

            hash += (ulong)data.Length;

            while (remaining.Length >= 8)
            {
                hash ^= Round(0, ReadUInt64(remaining));
                hash = RotateLeft(hash, 27) * Prime1 + Prime4;
                remaining = remaining.Slice(8);
            }

            if (remaining.Length >= 4)
            {
                hash ^= (ulong)ReadUInt32(remaining) * Prime1;
                hash = RotateLeft(hash, 23) * Prime2 + Prime3;
                remaining = remaining.Slice(4);
            }

            for (var i = 0; i < remaining.Length; i++)
            {
                hash ^= remaining[i] * Prime5;
                hash = RotateLeft(hash, 11) * Prime1;
            }

            hash ^= hash >> 33;
            hash *= Prime2;
            hash ^= hash >> 29;
            hash *= Prime3;
            hash ^= hash >> 32;
            return hash;
        }

        static ulong Round(ulong accumulator, ulong lane) => RotateLeft(accumulator + lane * Prime2, 31) * Prime1;

        static ulong MergeRound(ulong accumulator, ulong value) => (accumulator ^ Round(0, value)) * Prime1 + Prime4;

        static ulong RotateLeft(ulong value, int bits) => (value << bits) | (value >> (64 - bits));

        static uint ReadUInt32(ReadOnlySpan<byte> span) =>
            (uint)(span[0] | span[1] << 8 | span[2] << 16 | span[3] << 24);

        static ulong ReadUInt64(ReadOnlySpan<byte> span) =>
            (ulong)ReadUInt32(span) | (ulong)ReadUInt32(span.Slice(4)) << 32;
    }
}