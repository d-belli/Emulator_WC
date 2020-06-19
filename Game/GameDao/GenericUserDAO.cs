using System;
using System.IO;

namespace Emulator
{
    public static class GenericUserDAO
    {
        static GenericUserDAO()
        {
            EnsureDirectories("Account");
            EnsureDirectories("Char");
        }

        public static String GetAccountablePath(String pathType, String accountablePath)
        {
            String smallerPath = String.Empty;
            String completePath = String.Empty;

            foreach (Char thisLetter in "Ç1234567890")
            {
                if (accountablePath[0].Equals(thisLetter))
                    smallerPath = "etc";
            }

            if (smallerPath.Equals(String.Empty))
            {
                smallerPath = accountablePath[0].ToString();
            }

            completePath = (String.Format(@"{0}\{1}\{2}.xml", pathType, smallerPath, accountablePath));

            return completePath;
        }

        public static void EnsureDirectories(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            String dirs = "Q W E R T Y U I O P L K J H G F D S A Z X C V B N M etc";
            String[] splitedDirs = dirs.Split(' ');

            foreach (String thisDir in splitedDirs)
            {
                String thisPath = String.Format(@"{0}\{1}", path, thisDir);

                if (!Directory.Exists(thisPath))
                {
                    Directory.CreateDirectory(thisPath);
                }
            }
        }

        public static Boolean AccountablePathExists(String path)
        {
            Boolean exists = false;

            if (File.Exists(path))
                exists = true;

            return exists;
        }
    }
}