using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizCryptographyProject.Model;
using QuizCryptographyProject.Utils;
using QuizCryptographyProject.Exceptions;
using System.IO;
namespace QuizCryptographyProject.Controllers
{
    class LoginController
    {
        private static readonly string USERS_PATH = Application.StartupPath + "\\Users";
        private static readonly string CERTS_PATH = Application.StartupPath + "\\Certificates";

        public static User LoginUser(string username, string password)
        {


            if (UserAccountCheck.IsUserRegistered(username))
            {
                User user = GetUserInfo(username);
                

                //Provjera lozinke
                var passwordHash = UserAccountCheck.HashPassword(password);
                if (!passwordHash.Equals(user.Password))
                    throw new QuizException("Unesena lozinka je pogrešna!");

                //Provjera sertifikata
                ValidateCertificate(user);

                if (user.NumberOfLogin.Equals(2))
                { 
                    
                    string revokeCommand = "openssl ca -revoke Certificates/certs/" + user.Username + $".crt -crl_reason cessationOfOperation -config Certificates/openssl{user.Ca}.cnf -key maja 2>> Certificates/certslog.txt";
                    CommandPrompt.ExecuteCommand(revokeCommand);
                    string updateCrlListCommand = $"openssl ca -gencrl -out Certificates/crl/crllist{user.Ca}.crl -config Certificates/openssl{user.Ca}.cnf -key maja 2>> Certificates/certslog.txt";
                    CommandPrompt.ExecuteCommand(updateCrlListCommand);
                   

                }
                User userUpdate = new User(user.Name, user.Username, user.Password, user.Email, user.Ca, user.NumberOfLogin + 1);
                RegisterController.SaveUserAccountInfo(userUpdate);
                    return userUpdate;
            }
            else
                throw new QuizException("Korisnički nalog sa korisničkim imenom " + username + " ne postoji!");
        }

        private static User GetUserInfo(string username)
        {
            User user = null;

            string decryptCommand = "openssl aes-256-ecb -d -in Users/" + username + ".txt -out Users/_" + username + ".txt -pbkdf2 -k kviz -nosalt -base64";
            CommandPrompt.ExecuteCommand(decryptCommand);

            try
            {
                var path = USERS_PATH + "\\_" + username + ".txt";
                string[] lines = File.ReadAllLines(path);
                user = new User(lines[0], lines[1], lines[2], lines[3], Int32.Parse(lines[4]), Int32.Parse(lines[5]));
                File.Delete(path);
            }
            catch (Exception e)
            {
                throw new QuizException(e.StackTrace + " : " + e.Message);
            }
            return user;
        }

        private static void ValidateCertificate(User user)
        {
            string userCertPath = CERTS_PATH +"\\certs\\" + user.Username + ".crt";

            if (!File.Exists(userCertPath))
                throw new QuizException("Digitalni sertifikat korisnika " + user.Username + " nije pronađen.");
            if (!VerifiedByTrustedCA(userCertPath, user.Ca))
                throw new QuizException("Digitalni sertifikat korisnika " + user.Username + " nije izdat od strane CA tijela od povjerenja.");
           if (IsExpired(userCertPath))
                throw new QuizException("Digitalni sertifikat korinika " + user.Username + " nije važeći.");
            if (IsRevoked(userCertPath, user.Ca))
                throw new QuizException("Kviz možete igrati samo tri puta.\nDigitalni sertifikat korisnika " + user.Username + " je povučen.");
           
      
        }

        private static bool VerifiedByTrustedCA(string userCertPath, int ca)
        {
            string commandVerifyByCA = "openssl verify -trusted " + CERTS_PATH + $"\\certs\\ca{ca}.chain.crt " + userCertPath;
            var verified = CommandPrompt.ExecuteCommandWithResponse(commandVerifyByCA);
            if (verified.Contains("OK"))
            {
         
                return true;
            }
           
                return false;
                
            }
            

        

        private static bool IsExpired(string userCertPath)
        {
            string commandDates = "openssl x509 -in " + userCertPath + " -checkend 0";
            var expired = CommandPrompt.ExecuteCommandWithResponse(commandDates);
            if(expired.Equals("Certificate will expire"))
            {
                return true;
            }
            return false;

        }

        private static bool IsRevoked(string userCertPath, int ca)
        {
            string revokeCommand = $"openssl verify -crl_check -CRLfile " + CERTS_PATH + $"\\crl\\crllist{ca}.crl -CAfile " + CERTS_PATH + $"\\certs\\ca{ca}.chain.crt "  + userCertPath + " 2>&1";
               
;
            var revoked = CommandPrompt.ExecuteCommandWithResponse(revokeCommand);
            if (revoked.Contains("revoked"))
            {
                return true;
            }
            return false;
        }

       

    }
}
