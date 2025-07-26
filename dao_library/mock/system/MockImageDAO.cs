using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockImageDAO : DAOImage
    {
        private readonly List<Image> _images = new List<Image>();

        public MockImageDAO()
        {
            // Initialize with some mock data if needed
            _images.Add(new Image { Id = 1, Url = "http://example.com/image1.jpg" });
            _images.Add(new Image { Id = 2, Url = "http://example.com/image2.jpg" });
        }

        public Image? GetImage(string url)
        {
            return _images.FirstOrDefault(i => i.Url == url);
        }

        public Image CreateImage(string url)
        {
            var image = new Image { Url = url };
            image.Id = _images.Max(i => i.Id) + 1;
            _images.Add(image);
            return image;
        }
        public void UpdateImage(string url)
        {
            var existingImage = GetImage(url);
            if (existingImage != null)
            {
                existingImage.Url = url;
            }
        }

        public void DeleteImage(string url)
        {
            var image = GetImage(url);
            if (image != null)
            {
                _images.Remove(image);
            }
        }
    }
}
