using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterApiClient
{
    //class that contains meta data for a picture
    public class PictureMetaData
    {
        public enum Gender
        {
            Male, Female
        }

        public DateTime Time;       //Stores the time when the picture was posted(alt. processed with faceRecognizer)
        public int FaceCount;       //Number of faces in the picture
        public Gender[] Genders;    //List of genders in the picture


        //Constructor: takes an array of genders or null
        public PictureMetaData(DateTime time, Gender[]genders)
        {
            this.Time = time;
            this.Genders = genders;
            FaceCount = genders != null ? genders.Length : 0;
        }
    }
}
