using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockFileDAO : DAOfile
    {
        private readonly List<File> _files = new List<File>();

        public MockFileDAO()
        {
            // Initialize with some mock data if needed
            _files.Add(new File { Id = 1, Url = "http://example.com/image1.jpg" });
            _files.Add(new File { Id = 2, Url = "http://example.com/image2.jpg" });
        }

        public File? GetFile(string url)
        {
            return _files.FirstOrDefault(i => i.Url == url);
        }

        public File CreateFile(string url)
        {
            File file = new File { Url = url };
            file.Id = _files.Max(i => i.Id) + 1;
            _files.Add(file);
            return file;
        }
        public void UpdateFile(string url)
        {
            var existingFile = GetFile(url);
            if (existingFile != null)
            {
                existingFile.Url = url;
            }
        }

        public void DeleteFile(string url)
        {
            var file = GetFile(url);
            if (file != null)
            {
                _files.Remove(file);
            }
        }
    }
}
