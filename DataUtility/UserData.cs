﻿using Data;

namespace DataGenerator
{

    public static class UserData
    {
        private static readonly List<Assignment> m_assignments = new List<Assignment>
        {
            new Assignment("14F831F8-2951-4FAB-8E76-68C925076793")
            {
                Name = "Bæsj",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 5,
                Emoji = "twa-pile-of-poo"
    },
            new Assignment("36A7CDC7-BD3D-4876-B962-628870C0C72C")
            {
                Name = "Tiss",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-toilet"
            },
            new Assignment("A2327835-C13C-406D-923B-6F44C992785B")
            {
                Name = "Tannpuss morgen",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-toothbrush",
                EmojiModifier = "twa-sun-with-face"
            },
            new Assignment("D8998F3D-5378-410E-86FF-D20E935173B3")
            {
                Name = "Tannpuss kveld",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-toothbrush",
                EmojiModifier = "twa-full-moon-face"
            },
            new Assignment("CA55E579-76F9-4CFD-BDBB-A04736A20222")
            {
                Name = "Matte",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 5,
                Emoji = "twa-plus"
            },
            new Assignment("D07BA9CB-E4DB-40F8-9461-A4EC36C66826")
            {
                Name = "Lese",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 5,
                Emoji = "twa-open-book"
            },
            new Assignment("CA3A3616-F1FE-4934-9D69-C9D00114E800")
            {
                Name = "Rydde rommet",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 7,
                Emoji = "twa-flexed-biceps"
            },
            new Assignment("E76BE7CD-B7E0-46BB-BE87-2296B52444F8")
            {
                Name = "Rydde fat",
                Description = "Bære fat, bestikk og glass til kjøkken",
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-fork-and-knife-with-plate"
            },
            new Assignment("1A3BA5D0-42A8-4123-82F8-DFB26756E29D")
            {
                Name = "Rydde sekk",
                Description = "Sette matboks og flaske på kjøkkenbenken",
                OncePerDay = true,
                Weight = 2,
                Emoji = "twa-backpack"
            },
            new Assignment("9A0F2632-E68B-4DDC-977C-DF40E67B7B3B")
            {
                Name = "Henge opp klær",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-coat"
            },
            new Assignment("FD3F8399-59CF-400D-A8DA-3C0884DE692C")
            {
                Name = "Kle på selv",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-jeans"
            },
            new Assignment("6707DFD0-51DA-4BBD-95B4-721521B82837")
            {
                Name = "Sykle",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-woman-mountain-biking"
            },
            new Assignment("93C83BD8-6DF3-4488-9DFB-8998D328D151")
            {
                Name = "Stallen",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-unicorn"
            },
            new Assignment("0E76A827-D309-47AE-AFCB-B2758DABEA88")
            {
                Name = "Ri",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 2,
                Emoji = "twa-horse-face"
            },
            new Assignment("4F2C9358-D6CA-4A14-B78F-748A6C8653C9")
            {
                Name = "Minecraft",
                Description = "Lage noe i Minecraft",
                OncePerDay = true,
                Weight = 2,
                Emoji = "twa-pick"
            },
            new Assignment("D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF")
            {
                Name = "Tegne",
                Description = "Lage tegning",
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-crayon"
            },
            new Assignment("3214D15A-C87C-4AAE-BA73-0FBB5C384C7D")
            {
                Name = "Lage noe",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-knot"
            }
        };

        private static readonly List<User> m_users = new List<User>
        {
            new User("FB8FBF56-CA3A-4521-A8F1-748D42B9F8E3")
            {
                Name = "Lilly",
                Avatar = "twa-girl",
                Goal = 100,
                XP = 0
            },
            new User("C56A4180-65AA-42EC-A945-5FD21DEC0538")
            {
                Name = "Robin",
                Avatar = "twa-boy",
                Goal = 100,
                XP = 0
            }
        };

        public static User GetUser(string id)
        {
            var index = m_users.FindIndex(u => u.RowKey == id);
            return m_users[index];
        }

        public static User GetUserWithAssignments(string id)
        {
            var user = GetUser(id);
            string[] assignmentIds = System.Array.Empty<string>();

            if(user.Name == "Lilly") 
            {
                assignmentIds = new string[] 
                {
                    "A2327835-C13C-406D-923B-6F44C992785B",
                    "D8998F3D-5378-410E-86FF-D20E935173B3",
                    "CA55E579-76F9-4CFD-BDBB-A04736A20222",
                    "D07BA9CB-E4DB-40F8-9461-A4EC36C66826",
                    "CA3A3616-F1FE-4934-9D69-C9D00114E800",
                    "E76BE7CD-B7E0-46BB-BE87-2296B52444F8",
                    "1A3BA5D0-42A8-4123-82F8-DFB26756E29D",
                    "9A0F2632-E68B-4DDC-977C-DF40E67B7B3B",
                    "FD3F8399-59CF-400D-A8DA-3C0884DE692C",
                    "6707DFD0-51DA-4BBD-95B4-721521B82837",
                    "93C83BD8-6DF3-4488-9DFB-8998D328D151",
                    "0E76A827-D309-47AE-AFCB-B2758DABEA88",
                    "4F2C9358-D6CA-4A14-B78F-748A6C8653C9",
                    "D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF",
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D"
                };
            } 
            else if (user.Name == "Robin")
            {
                assignmentIds = new string[] 
                { 
                    "14F831F8-2951-4FAB-8E76-68C925076793", 
                    "36A7CDC7-BD3D-4876-B962-628870C0C72C", 
                    "A2327835-C13C-406D-923B-6F44C992785B", 
                    "D8998F3D-5378-410E-86FF-D20E935173B3", 
                    "E76BE7CD-B7E0-46BB-BE87-2296B52444F8", 
                    "FD3F8399-59CF-400D-A8DA-3C0884DE692C",
                    "93C83BD8-6DF3-4488-9DFB-8998D328D151",
                    "0E76A827-D309-47AE-AFCB-B2758DABEA88",
                    "D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF",
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D"
                };
            }

            user.Assignments = new List<Assignment>(from userAssignment in m_assignments
                                                    where assignmentIds.Contains(userAssignment.RowKey)
                                                    select userAssignment);

            return user;
        }

        public static List<Assignment> GetAssignments()
        {
            return m_assignments;
        }

        public static List<User> GetUsers()
        {
            return m_users;
        }
    }
}