using Data;
using static System.Formats.Asn1.AsnWriter;

namespace DataGenerator
{

    public static class UserData
    {
        private static readonly List<Assignment> m_assignments = new List<Assignment>
        {
            new Assignment("4390A97B-0323-4787-AC9E-02A5E2B36DEC")
            {
                Name = "Medisin",
                Description = "Morgen",
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-pill",
                EmojiModifier = "twa-sun-with-face"
            },
            new Assignment("B8C0E9D3-F4AB-4586-B648-8CEC87F865FB")
            {
                Name = "Medisin",
                Description = "Kveld",
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-pill",
                EmojiModifier = "twa-first-quarter-moon-face"
            },
            new Assignment("14F831F8-2951-4FAB-8E76-68C925076793")
            {
                Name = "Bæsj",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-pile-of-poo"
            },
            new Assignment("36A7CDC7-BD3D-4876-B962-628870C0C72C")
            {
                Name = "Tiss",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-toilet"
            },
            new Assignment("A2327835-C13C-406D-923B-6F44C992785B")
            {
                Name = "Tannpuss morgen",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 2,
                Emoji = "twa-toothbrush",
                EmojiModifier = "twa-sun-with-face"
            },
            new Assignment("D8998F3D-5378-410E-86FF-D20E935173B3")
            {
                Name = "Tannpuss kveld",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 2,
                Emoji = "twa-toothbrush",
                EmojiModifier = "twa-first-quarter-moon-face"
            },
            new Assignment("CA55E579-76F9-4CFD-BDBB-A04736A20222")
            {
                Name = "Matte",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-input-numbers"
            },
            new Assignment("D07BA9CB-E4DB-40F8-9461-A4EC36C66826")
            {
                Name = "Lese",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-open-book"
            },
            new Assignment("CA3A3616-F1FE-4934-9D69-C9D00114E800")
            {
                Name = "Rydde rommet",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 3,
                Emoji = "twa-flexed-biceps"
            },
            new Assignment("E76BE7CD-B7E0-46BB-BE87-2296B52444F8")
            {
                Name = "Rydde fat",
                Description = "Bære fat, bestikk og glass til kjøkken",
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-fork-and-knife-with-plate"
            },
            new Assignment("5F2C5F1C-07D9-4300-A6B2-D152CBE1B857")
            {
                Name = "Middag",
                Description = "Uten somling eller klager",
                OncePerDay = true,
                Weight = 3,
                Emoji = "twa-spaghetti",
                EmojiModifier = "twa-grinning-face-with-smiling-eyes"
            },
            new Assignment("1A3BA5D0-42A8-4123-82F8-DFB26756E29D")
            {
                Name = "Rydde sekk",
                Description = "Sette matboks og flaske på kjøkkenbenken",
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-backpack",
                EmojiModifier = "twa-beverage-box"
            },
            new Assignment("DC4CA6D6-0B0E-42DE-85D6-A7247A2994C6")
            {
                Name = "Lage matpakke",
                OncePerDay = true,
                Weight = 3,
                Emoji= "twa-sandwich",
                EmojiModifier = "twa-backpack"
            },
            new Assignment("9A0F2632-E68B-4DDC-977C-DF40E67B7B3B")
            {
                Name = "Henge opp klær",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-coat"
            },
            new Assignment("FD3F8399-59CF-400D-A8DA-3C0884DE692C")
            {
                Name = "Kle på selv",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-jeans"
            },
            new Assignment("2DF2B8A2-95F6-41DD-B4A0-19311E8F5DA3")
            {
                Name = "Bade/dusje",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-bathtub"
            },
            new Assignment("98CE9CA3-5A23-42DC-9792-FB701D66562E")
            {
                Name = "Kle på uten somling",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-socks",
                EmojiModifier = "twa-stopwatch"
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
                Name = "Stallarbeid",
                Description = "Møkke, koste stallgang, stelle Lynet/Oso",
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
                Weight = 3,
                Emoji = "twa-knot"
            },
            new Assignment("137EF03A-D5A4-4B46-8DFE-266CFE31AAD3")
            {
                Name = "Husarbeid",
                Description = "Hjelpe til med husarbeid",
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-broom"
            },
            new Assignment("BA2737ED-9F57-4E93-8A62-24F1332B79F4")
            {
                Name = "Gjøre noe fint for andre",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-growing-heart"
            },
            new Assignment("AFD8667D-4BE5-42A0-BC01-B1C7E4C5FC6C")
            {
                Name = "Nytt ord",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-speech-balloon"
            },
            new Assignment("2766B0C7-CB8A-4568-AE08-9D7BF8D513C8")
            {
                Name = "Veie",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-balance-scale"
            },
            new Assignment("433DE750-A150-42C7-9EA1-026DFC908B1D")
            {
                Name = "Lære noe nytt",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 1,
                Emoji = "twa-light-bulb"
            },
            new Assignment("721CE2ED-761F-4D34-BD3D-9F2F27F47385")
            {
                Name = "Tømme søppel",
                Description = "Sette ut søppeldunk",
                OncePerDay = true,
                Weight = 1,
                Emoji = "twa-wastebasket"
            },
            new Assignment("D0DCECAB-17B4-4FC5-ADA9-13CB31144153")
            {
                Name = "Gå",
                Description = "Nå dagens antall skritt",
                OncePerDay = true,
                Weight = 3,
                Emoji = "twa-person-running"
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
            },
            new User("BAA25D7F-4462-482F-B919-83F938FC72D3")
            {
                Name = "Eivind",
                Avatar = "twa-person-beard",
                Goal = 100,
                XP = 0,
                DosetteDeviceId = "0004A30B00EB730A"
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
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D",
                    "98CE9CA3-5A23-42DC-9792-FB701D66562E",
                    "5F2C5F1C-07D9-4300-A6B2-D152CBE1B857",
                    "137EF03A-D5A4-4B46-8DFE-266CFE31AAD3",
                    "2DF2B8A2-95F6-41DD-B4A0-19311E8F5DA3",
                    "DC4CA6D6-0B0E-42DE-85D6-A7247A2994C6",
                    "4390A97B-0323-4787-AC9E-02A5E2B36DEC",
                    "B8C0E9D3-F4AB-4586-B648-8CEC87F865FB",
                    "BA2737ED-9F57-4E93-8A62-24F1332B79F4",
                    "AFD8667D-4BE5-42A0-BC01-B1C7E4C5FC6C",
                    "433DE750-A150-42C7-9EA1-026DFC908B1D",
                    "721CE2ED-761F-4D34-BD3D-9F2F27F47385"
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
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D",
                    "5F2C5F1C-07D9-4300-A6B2-D152CBE1B857",
                    "137EF03A-D5A4-4B46-8DFE-266CFE31AAD3",
                    "2DF2B8A2-95F6-41DD-B4A0-19311E8F5DA3",
                    "BA2737ED-9F57-4E93-8A62-24F1332B79F4",
                    "AFD8667D-4BE5-42A0-BC01-B1C7E4C5FC6C",
                    "433DE750-A150-42C7-9EA1-026DFC908B1D",
                    "D07BA9CB-E4DB-40F8-9461-A4EC36C66826",
                    "CA55E579-76F9-4CFD-BDBB-A04736A20222"
                };
            }
            else if (user.Name == "Eivind")
            {
                assignmentIds = new string[]
                {
                    "A2327835-C13C-406D-923B-6F44C992785B",
                    "D8998F3D-5378-410E-86FF-D20E935173B3",
                    "CA3A3616-F1FE-4934-9D69-C9D00114E800",
                    "93C83BD8-6DF3-4488-9DFB-8998D328D151",
                    "0E76A827-D309-47AE-AFCB-B2758DABEA88",
                    "D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF",
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D",
                    "137EF03A-D5A4-4B46-8DFE-266CFE31AAD3",
                    "DC4CA6D6-0B0E-42DE-85D6-A7247A2994C6",
                    "4390A97B-0323-4787-AC9E-02A5E2B36DEC",
                    "BA2737ED-9F57-4E93-8A62-24F1332B79F4",
                    "2766B0C7-CB8A-4568-AE08-9D7BF8D513C8",
                    "433DE750-A150-42C7-9EA1-026DFC908B1D",
                    "721CE2ED-761F-4D34-BD3D-9F2F27F47385",
                    "D0DCECAB-17B4-4FC5-ADA9-13CB31144153"
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
