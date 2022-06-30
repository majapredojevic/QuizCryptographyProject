using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizCryptographyProject.Model;
using System.IO;
using QuizCryptographyProject.Exceptions;
using QuizCryptographyProject.Utils;
using System.Text.RegularExpressions;

namespace QuizCryptographyProject.Controllers
{
    class RegisterController
    {
        private static readonly string CERTS_PATH = Application.StartupPath + "\\Certificates";
        private static readonly string USERS_PATH = Application.StartupPath + "\\Users";

        public static string RegisterUser(User user)
        {
            GenerateKeys(user.Username);
            GenerateCertificate(user);

            if(File.Exists(CERTS_PATH + "\\private\\" + user.Username + "_private.key") && File.Exists(CERTS_PATH +"\\certs\\" + user.Username + ".crt"))
            {
                SaveUserAccountInfo(user);
                return CERTS_PATH + "\\certs\\" + user.Username + ".crt";
            }
            else
            {
                throw new QuizException("Neuspješna registracija na sistem!");
            }
        }

        private static void GenerateKeys (string username)
        {
            //generisanje privatnog kljuca korisnika
            string command = "openssl genrsa -out Certificates/private/" + username + "_private.key 2>> Certificates/keyslog.txt";
            CommandPrompt.ExecuteCommand(command);

            //generisanje javnog kljuca korisnika
            command = "openssl rsa -in Certificates/private/" + username + "_private.key -out Certificates/public/" + username + "_public.key -pubout 2>>Certificates/keyslog.txt";
            CommandPrompt.ExecuteCommand(command);
        }


        private static void GenerateCertificate (User user)
        {

  

            //kreiranje zahtjeva za izdavanje sertifikata
            string requestCommand = "openssl req -new -key Certificates/private/" + user.Username + $"_private.key -config Certificates/openssl{user.Ca}.cnf -out Certificates/requests/" + user.Username + ".csr -subj \"/OU=ETF/O=Elektrotehnicki fakultet/ST=RS/C=BA/CN=" + user.Name + "/emailAddress=" + user.Email + "\" -batch -verbose 2>>Certificates/certlog.txt";
                CommandPrompt.ExecuteCommand(requestCommand);
                //potpisivanje i izdavanje sertifikata
                string signCommand = "openssl ca -in Certificates/requests/" + user.Username + ".csr -out Certificates/certs/" + user.Username + $".crt -config Certificates/openssl{user.Ca}.cnf -key maja -batch -verbose 2>> Certificates/certlog.txt";
                CommandPrompt.ExecuteCommand(signCommand);
                string crlCommand = $"openssl ca -gencrl -out Certificates/crl/crllist{user.Ca}.crl -config Certificates/openssl{user.Ca}.cnf -key kviz 2>> Certificates/certlog.txt";
                CommandPrompt.ExecuteCommand(crlCommand);
            
           
            }

        public static void SaveUserAccountInfo(User user)
        {
            DirectoryInfo users = new DirectoryInfo(USERS_PATH);
            string[] lines = { user.Name, user.Username, user.Password, user.Email,user.Ca.ToString(), user.NumberOfLogin.ToString() };
            try
            {
                File.WriteAllLines(users.FullName + "\\_" + user.Username + ".txt", lines);
                string encryptCommand = "openssl aes-256-ecb -in Users/_" + user.Username + ".txt -out Users/" + user.Username + ".txt -pbkdf2 -k kviz -nosalt -base64";
                CommandPrompt.ExecuteCommand(encryptCommand);
                File.Delete(users.FullName + "\\_" + user.Username + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace + " : " + e.Message);
            }
        }

       
        
    }
}
