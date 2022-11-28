using Data;
using System;
using System.Threading;
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
            new Assignment("C65FA114-273C-4305-A44B-1A70C6E01173")
            {
                Name = "Tanntråd",
                Description = string.Empty,
                OncePerDay = true,
                Weight = 3,
                Emoji = "twa-tooth"
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
            new Assignment("7147AD12-BCD8-4A7E-B7EC-B9903F0296F2")
            {
                Name = "Skrive",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-writing-hand"
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
            new Assignment("074B7017-0D61-4248-8F37-C13F9AE7AE8F")
            {
                Name = "Lage kveldsmat",
                OncePerDay = true,
                Weight = 3,
                Emoji= "twa-bread",
                EmojiModifier = "twa-first-quarter-moon-face"
            },
            new Assignment("E283F98C-6F9A-4144-B657-506FB6B46EB7")
            {
                Name = "Vaske hender etter skole/bhg",
                OncePerDay = true,
                Weight = 1,
                Emoji= "twa-palms-up-together",
                EmojiModifier = "twa-microbe"
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
            new Assignment("B69E89D5-D29B-4704-B1A9-924DC61C543E")
            {
                Name = "Delta på stevne",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-horse-racing"
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
            new Assignment("8BC602FA-E97C-4F25-ACEC-DD1A910BBF71")
            {
                Name = "Reparere noe",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 3,
                Emoji = "twa-screwdriver"
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
            new Assignment("010D44F3-CBDB-48F9-9081-01F4345167B3")
            {
                Name = "Hjelpe Robin",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-man-shrugging"
            },
            new Assignment("8BB046F4-4188-4BDA-AC54-0B6529F40B54")
            {
                Name = "Hjelpe Lilly",
                Description = string.Empty,
                OncePerDay = false,
                Weight = 2,
                Emoji = "twa-woman-shrugging"
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
            },
            new Assignment("F61DB2E2-EDA3-480A-AD89-8F7507C81976")
            {
                Name = "Smake noe nytt",
                Description = string.Empty,
                Weight = 3,
                Emoji = "twa-falafel"
            },
            new Assignment("59DD55D3-9F48-4B56-A905-BB433FF5441F")
            {
                Name = "Måle blodtrykk",
                Description = string.Empty,
                Weight = 1,
                Emoji = "twa-anatomical-heart"
            },
            new Assignment("5DA4C692-8DCB-465A-A74F-0122AB33CABD")
            {
                Name = "Leggetid",
                Description = "Overholde vanlig leggetid",
                OncePerDay = true,
                Weight = 3,
                Emoji = "twa-person-in-bed"
            },
            new Assignment("4187A11D-ABCA-4AEA-AF1B-C8E23CB28D96")
            {
                Name = "Hente post",
                Description = "Klar til henting",
                Weight = 1,
                Emoji = "twa-open-mailbox-with-raised-flag"
            },
            new Assignment("E54ABEA8-F97F-48FD-884B-B8FBB38323FC")
            {
                Name = "Vanne blomster",
                Description = "Potte nr 1",
                Weight = 1,
                Emoji = "twa-potted-plant",
                EmojiModifier = "twa-keycap-1"
            },
            new Assignment("4DFD8832-7C27-45C2-A370-FC9A3656F926")
            {
                Name = "Vanne blomster",
                Description = "Potte nr 2",
                Weight = 1,
                Emoji = "twa-potted-plant",
                EmojiModifier = "twa-keycap-2"
            },
            new Assignment("BC72D11B-E34C-494A-B882-B703FB410220")
            {
                Name = "Vanne blomster",
                Description = "Potte nr 3",
                Weight = 1,
                Emoji = "twa-potted-plant",
                EmojiModifier = "twa-keycap-3"
            },
            new Assignment("8FB2399E-1F84-4D57-BAAA-B3F7CC4F47B9")
            {
                Name = "God natt",
                Description = "Minst 7 timer nattesøvn",
                Weight = 3,
                Emoji = "twa-sleeping-face",
                EmojiModifier = "twa-alarm clock"
            },
            new Assignment("7263CFA7-3755-4B2A-8E85-24BC24198EE1")
            {
                Name = "Bursdag",
                Description = "Gratulerer med dagen!",
                Weight = 3,
                Emoji = "twa-party-popper"
            },
            new Assignment("F3824842-5363-40CC-89DA-98B1E51F5366")
            {
                Name = "Lekser",
                Description = "Starte med lekser på eget initiativ",
                Weight = 3,
                Emoji = "twa-three-thirty"
            },
            new Assignment("D822ED73-164E-46C2-856B-D6B01BC68B3D")
            {
                Name = "Stå opp",
                Description = "På skoledag, uten masing",
                Weight = 3,
                Emoji = "twa-bed",
                EmojiModifier = "twa-alarm-clock"
            },
            new Assignment("A30816D5-A4AE-4116-B3BA-E5340F5CD741")
            {
                Name = "Matlaging",
                Description = "Hjelpe til med matlaging",
                Weight = 2,
                Emoji = "twa-shallow-pan-of-food"
            },
            new Assignment("B010225A-C3E2-4082-B72C-86F8E35BEC2F")
            {
                Name = "Voltige",
                Weight = 2,
                Emoji = "twa-horse",
                EmojiModifier = "twa-person-cartwheeling"
            },
            new Assignment("9A7EFA06-D21D-4AFA-A65A-E0BB9248DFCA")
            {
                Name = "Spise opp skolematen",
                Weight = 2,
                Emoji = "twa-sandwich",
                EmojiModifier = "twa-school"
            },
            new Assignment("D50A104A-C327-49A6-9CD0-BF44A8B92A22")
            {
                Name = "Lade nettbrett",
                Weight = 1,
                Emoji = "twa-mobile-phone",
                EmojiModifier = "twa-low-battery"
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
                    "C65FA114-273C-4305-A44B-1A70C6E01173",
                    "CA55E579-76F9-4CFD-BDBB-A04736A20222",
                    "D07BA9CB-E4DB-40F8-9461-A4EC36C66826",
                    "CA3A3616-F1FE-4934-9D69-C9D00114E800",
                    "E76BE7CD-B7E0-46BB-BE87-2296B52444F8",
                    "1A3BA5D0-42A8-4123-82F8-DFB26756E29D",
                    "074B7017-0D61-4248-8F37-C13F9AE7AE8F",
                    "E283F98C-6F9A-4144-B657-506FB6B46EB7",
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
                    "721CE2ED-761F-4D34-BD3D-9F2F27F47385",
                    "F61DB2E2-EDA3-480A-AD89-8F7507C81976",
                    "5DA4C692-8DCB-465A-A74F-0122AB33CABD",
                    "B69E89D5-D29B-4704-B1A9-924DC61C543E",
                    "B010225A-C3E2-4082-B72C-86F8E35BEC2F",
                    "7147AD12-BCD8-4A7E-B7EC-B9903F0296F2",
                    "F3824842-5363-40CC-89DA-98B1E51F5366",
                    "010D44F3-CBDB-48F9-9081-01F4345167B3",
                    "8BC602FA-E97C-4F25-ACEC-DD1A910BBF71",
                    "D822ED73-164E-46C2-856B-D6B01BC68B3D",
                    "A30816D5-A4AE-4116-B3BA-E5340F5CD741",
                    "9A7EFA06-D21D-4AFA-A65A-E0BB9248DFCA",
                    "D50A104A-C327-49A6-9CD0-BF44A8B92A22",

                    "7263CFA7-3755-4B2A-8E85-24BC24198EE1"
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
                    "C65FA114-273C-4305-A44B-1A70C6E01173",
                    "E283F98C-6F9A-4144-B657-506FB6B46EB7",
                    "E76BE7CD-B7E0-46BB-BE87-2296B52444F8", 
                    "FD3F8399-59CF-400D-A8DA-3C0884DE692C",
                    "93C83BD8-6DF3-4488-9DFB-8998D328D151",
                    "0E76A827-D309-47AE-AFCB-B2758DABEA88",
                    "D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF",
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D",
                    "4F2C9358-D6CA-4A14-B78F-748A6C8653C9",
                    "5F2C5F1C-07D9-4300-A6B2-D152CBE1B857",
                    "074B7017-0D61-4248-8F37-C13F9AE7AE8F",
                    "137EF03A-D5A4-4B46-8DFE-266CFE31AAD3",
                    "2DF2B8A2-95F6-41DD-B4A0-19311E8F5DA3",
                    "BA2737ED-9F57-4E93-8A62-24F1332B79F4",
                    "AFD8667D-4BE5-42A0-BC01-B1C7E4C5FC6C",
                    "433DE750-A150-42C7-9EA1-026DFC908B1D",
                    "D07BA9CB-E4DB-40F8-9461-A4EC36C66826",
                    "CA55E579-76F9-4CFD-BDBB-A04736A20222",
                    "F61DB2E2-EDA3-480A-AD89-8F7507C81976",
                    "5DA4C692-8DCB-465A-A74F-0122AB33CABD",
                    "B69E89D5-D29B-4704-B1A9-924DC61C543E",
                    "B010225A-C3E2-4082-B72C-86F8E35BEC2F",
                    "7147AD12-BCD8-4A7E-B7EC-B9903F0296F2",
                    "8BB046F4-4188-4BDA-AC54-0B6529F40B54",
                    "8BC602FA-E97C-4F25-ACEC-DD1A910BBF71",
                    "A30816D5-A4AE-4116-B3BA-E5340F5CD741",

                    "7263CFA7-3755-4B2A-8E85-24BC24198EE1"
                };
            }
            else if (user.Name == "Eivind")
            {
                assignmentIds = new string[]
                {
                    "A2327835-C13C-406D-923B-6F44C992785B",
                    "D8998F3D-5378-410E-86FF-D20E935173B3",
                    "C65FA114-273C-4305-A44B-1A70C6E01173",
                    "93C83BD8-6DF3-4488-9DFB-8998D328D151",
                    "0E76A827-D309-47AE-AFCB-B2758DABEA88",
                    "D7573642-AF29-48C7-AFA2-4E3C6D3C7AFF",
                    "3214D15A-C87C-4AAE-BA73-0FBB5C384C7D",
                    "4390A97B-0323-4787-AC9E-02A5E2B36DEC",
                    "BA2737ED-9F57-4E93-8A62-24F1332B79F4",
                    "2766B0C7-CB8A-4568-AE08-9D7BF8D513C8",
                    "433DE750-A150-42C7-9EA1-026DFC908B1D",
                    "721CE2ED-761F-4D34-BD3D-9F2F27F47385",
                    "D0DCECAB-17B4-4FC5-ADA9-13CB31144153",
                    "F61DB2E2-EDA3-480A-AD89-8F7507C81976",
                    "59DD55D3-9F48-4B56-A905-BB433FF5441F",
                    "5DA4C692-8DCB-465A-A74F-0122AB33CABD",
                    "8BC602FA-E97C-4F25-ACEC-DD1A910BBF71",
                    "E54ABEA8-F97F-48FD-884B-B8FBB38323FC",
                    "4DFD8832-7C27-45C2-A370-FC9A3656F926",
                    "BC72D11B-E34C-494A-B882-B703FB410220",
                    "8FB2399E-1F84-4D57-BAAA-B3F7CC4F47B9",
                    "DC4CA6D6-0B0E-42DE-85D6-A7247A2994C6",

                    "7263CFA7-3755-4B2A-8E85-24BC24198EE1"
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
