using System;
using System.Collections.Generic;
using System.Text;

namespace PollyWithHttpClientFactoryInAConsole
{
    public class Photo
    {
        public override string ToString()
        {
            return $"AlbumId:{AlbumId}, Id:{Id}, Title:{Title}, Url:{Url}, ThumbnailUrl:{ThumbnailUrl}";
        }

        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
