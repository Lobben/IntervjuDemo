using System;
using System.Linq;
using System.Reflection;

namespace FullContactLib
{
    //Data about a person, used to store data from FullContact Person Api
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

            //Returns a string with the properties listed on rows. Websites and Chats properties are tabbed out.
            public override string ToString()
            {
                //TODO: implement more reader-friendly solution with reflection
                string familyNameS = $"Family name: {familyName}\nGiven name: {givenName}\nFull name: {fullName}\n";
                string middleNamesS = "Middle Names: " + (middleNames != null ? string.Join(", ", middleNames) + "\n" : "\n");
                string WebSitesS = "Web sites: " + (websites != null ? "\n"+string.Join("\n", websites.Select(website => "  Url: " + website.url)) :"")+"\n";
                string chatsS = "Chats: \n" + (chats != null ? string.Join("\n\n", chats.Select(chat => "  Handle: " + chat.handle+"\n  Client: "+chat.client)) : "")+"\n";
                return familyNameS + middleNamesS + WebSitesS + chatsS;
            }
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

            //Returns a string with the properties of the class listed
            public override string ToString()
            {
                //TODO: implement more reader-friendly solution with system.reflection
                return $"TypeID: {typeId}\nTypeName: {typeName}\nID: {id}\n"+
                        $"Username: {username}\nURL: {url}\nBio: {bio}\nRSS: {rss}\n"+
                        $"Following: {following}\nFollowers: {followers}";
            }
        }

        public float Likelihood { get; set; }
        public ContactInfo contactInfo { get; set; }
        public SocialProfile[] SocialProfiles { get; set; }

        //Returns string that lists: likelihood, contactInfo and socialProfiles. 
        public override string ToString()
        {
            string likelihoodS = $"Likelihood: {Likelihood}\n\n";

            string contactInfoS = "=== Contact information ===\n\n" + 
                contactInfo.ToString()+"\n";

            string socialProfilesS = "=== Social profiles ===\n\n" +
                (SocialProfiles != null ? string.Join("\n\n",
                SocialProfiles.Select(sProfile => sProfile.ToString())) : "") +"\n\n";

            return likelihoodS + contactInfoS + socialProfilesS;
        }
    }
}
