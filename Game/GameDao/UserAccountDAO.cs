using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Emulator
{
    public static class UserAccountDAO
    {
        private static readonly Object m_Locker;
        private static XmlSerializer m_Serializator;
        private static XmlWriterSettings m_WriterSettings;
        static UserAccountDAO()
        {
            m_Locker = new Object();
            m_Serializator = new XmlSerializer(typeof(Account));
            m_WriterSettings = new XmlWriterSettings() { Indent = true };
        }

        public static String GetPath(String accountName)
        {
            return GenericUserDAO.GetAccountablePath("Account", accountName);
        }

        public static Boolean ExistsAccount(String accountName)
        {
            return GenericUserDAO.AccountablePathExists(GetPath(accountName));
        }

        public static Account loadAccount(String accName, String accPsw)
        {
            Account userAcc;

            lock (m_Locker)
            {
                try
                {
                    if (ExistsAccount(accName))
                    {
                        using (Stream st = File.OpenRead(GetPath(accName)))
                            userAcc = (Account)m_Serializator.Deserialize(st);

                        if (userAcc.Password.Equals(accPsw))
                        {
                            userAcc.Characters = UserMobDAO.GetListCharacterFromAccount(userAcc);
                            return userAcc;
                        } 
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void deleteAccount(Account acc)
        {
            File.Delete(Directory.GetCurrentDirectory() + "\\"+ GetPath(acc.UserName));
        }
        public static void CreateOrUpdateAccount(Account userAcc)
        {

            lock (m_Locker)
            {
                deleteAccount(userAcc);
                using (XmlWriter xw = XmlWriter.Create(GetPath(userAcc.UserName), m_WriterSettings))
                    m_Serializator.Serialize(xw, userAcc);

            }
        }
    }
}