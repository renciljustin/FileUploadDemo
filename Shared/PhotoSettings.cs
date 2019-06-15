using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UploadFileDemo.Shared
{
    public class PhotoSettings
    {
        public long MaxSize { get; set; }
        public string[] AllowedExtensions { get; set; }

        public bool IsMaxSizeExceed(long length)
        {
            return length > MaxSize;
        }

        public bool IsFileExtensionInvalid(string fileName)
        {
            return !AllowedExtensions.Any(t => t == Path.GetExtension(fileName));
        }
    }
}