using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class MimeTypeHelper
    {
        public static string MimeToExtension(string mime)
        {
            return mime switch
            {
                "image/png" => "png",
                "image/webp" => "webp",
                "image/jpeg" => "jpeg",
                "application/pdf" => "pdf",
                _ => "unknown",
            };
        }
    }
}
