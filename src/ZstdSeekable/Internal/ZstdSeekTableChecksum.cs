using System;
using System.IO.Hashing;

namespace ZstdSeekable.Internal
{
    /// <summary>
    /// Computes the seekable-format checksum: the least significant 32 bits of XXH64.
    /// See https://github.com/facebook/zstd/blob/dev/contrib/seekable_format/zstd_seekable_compression_format.md#checksum.
    /// </summary>
    internal static class ZstdSeekTableChecksum
    {
        public static uint Compute(ReadOnlySpan<byte> data) =>
            unchecked((uint)XxHash64.HashToUInt64(data));
    }
}