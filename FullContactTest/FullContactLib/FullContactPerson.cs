namespace FullContactLib
{
    public class FullContactPerson
    {
        public class ContactInfo
        {
            public class Chats
            {
                public string handle { get; set; }
                public string client { get; set; }
            }
            public class Website
            {
                public string url { get; set; }
            }

            public string familyName { get; set; }
            public string givenName { get; set; }
            public string fullName { get; set; }
            public string[] middleNames { get; set; }

            public Website[] websites { get; set; }
            public Chats[] chats { get; set; }
        }

        public class SocialProfile
        {
            public string typeId { get; set; }
            public string typeName { get; set; }
            public string id { get; set; }
            public string username { get; set; }
            public string url { get; set; }
            public string bio { get; set; }
            public string rss { get; set; }
            public int following { get; set; }
            public int followers { get; set; }
        }

        public float likelihood { get; set; }
        public ContactInfo contactInfo { get; set; }
        public SocialProfile[] socialProfiles { get; set; }
    }
}
