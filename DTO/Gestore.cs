using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO
{
    public class Gestore
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public bool isTombStone { get; set; }

        public Gestore() { }
       
        public Gestore(int ID, string username, string password, string nome, string cognome, bool isTombStone)
        {
            this.ID = ID;
            this.username = username;
            this.password = password; // da criptare
            this.nome = nome;
            this.cognome = cognome;
            this.isTombStone = isTombStone;
        }
        
    }
}