using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Emulator
{
    public static class UserMobDAO
    {

        private static readonly Object m_Locker;
        private static XmlSerializer m_Serializator;
        private static XmlWriterSettings m_WriterSettings;

        static UserMobDAO()
        {
            m_Locker = new Object();
            m_Serializator = new XmlSerializer(typeof(Character));
            m_WriterSettings = new XmlWriterSettings() { Indent = true };
        }

        public static String GetPath(String charName)
        {
            return GenericUserDAO.GetAccountablePath("Char", charName);
        }

        public static Boolean Exists(String charName)
        {
            lock (m_Locker)
            {
                return GenericUserDAO.AccountablePathExists(GetPath(charName));
            }
        }

        public static bool DeleteCharFile(Character character)
        {
            lock (m_Locker)
            {
                File.Delete(GetPath(character.Mob.Name));
                return !Exists(character.Mob.Name);
            }
        }

        public static Character CreateOrUpdateChar(Character character)
        {
            lock (m_Locker)
            {
                DeleteCharFile(character);
                SMob userMob = SMob.New();
                switch (character.Mob.ClassInfo)
                {
                    case ClassInfo.TK:
                        userMob = SMob.TK(character.Mob.Name);
                        break;
                    case ClassInfo.FM:
                        userMob = SMob.FM(character.Mob.Name);
                        break;
                    case ClassInfo.BM:
                        userMob = SMob.BM(character.Mob.Name);
                        break;
                    case ClassInfo.HT:
                        userMob = SMob.HT(character.Mob.Name);
                        break;

                }

                using (XmlWriter xw = XmlWriter.Create(GetPath(character.Mob.Name), m_WriterSettings))
                    m_Serializator.Serialize(xw, character);

                if (Exists(character.Mob.Name))
                    return new Character(userMob);

                return null;
            }
        }

        public static Character[] GetListCharacterFromAccount(Account accMob)
        {
            Character[] characterList = new Character[accMob.Characters.Length];

            for (int i = 0; i < accMob.Characters.Length; i++)
            {
                if (accMob.Characters[i] != null)
                    using (Stream st = File.OpenRead(GetPath(accMob.Characters[i].Mob.Name)))
                        characterList[i] = (Character)m_Serializator.Deserialize(st);
            }
            return characterList;
        }

    }
}