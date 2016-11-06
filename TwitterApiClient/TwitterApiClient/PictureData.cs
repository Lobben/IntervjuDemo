using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterApiClient
{
    //Stores a picture, filename and has a pointer to the metaData related to the picture
    public class PictureData
    {
        
        public byte[] Image;
        public string FileName;
        public PictureMetaData MetaData;
        
        public PictureData(byte[] imageData, string fileName)
        {
            this.Image = imageData;
            this.FileName = fileName;
        }

        public PictureData(byte[] imageData, string fileName, PictureMetaData meta)
            : this(imageData,fileName)
        {
            this.MetaData = meta;
        }


    }
}
