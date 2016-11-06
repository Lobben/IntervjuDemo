using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterApiClient
{
    
    public class PictureData
    {
        public enum Gender
        {
            Male, Female
        }
        public byte[] Image;
        public string FileName;
        public int FaceCount;
        public Gender[] Genders;

        public PictureData(byte[] imageData, string fileName, Gender[] genders)
        {
            this.Image = imageData;
            this.FileName = fileName;
            this.Genders = genders;

            FaceCount = genders != null ? genders.Length : 0;

        }
    }
}
