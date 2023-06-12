﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatOn.Model
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public string NomeUsuario { get; set; }
        public byte[] ImagemPerfil { get; set; }

        public int FriendRequestCount { get; set; }
        // Navigation property representing the friends of the user
        public virtual ICollection<Usuarios> Amigos { get; set; }

        // Navigation property representing the pending friend requests
        public virtual ICollection<Usuarios> PedidoAmizade { get; set; }

        // Navigation property representing the chats the user is involved in
        public virtual ICollection<Chat> Chats { get; set; }


        public Usuarios()
        {
            Amigos = new List<Usuarios>();
            PedidoAmizade = new List<Usuarios>();
            Chats = new List<Chat>();
        }
    }

    public class Chat
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public Usuarios User1 { get; set; }
        public Usuarios User2 { get; set; }
        public List<Message> Messages { get; set; }

        public Chat(Usuarios user1, Usuarios user2)
        {
            User1 = user1;
            User2 = user2;
            Messages = new List<Message>();
        }

        // Parameterless constructor for EF Core
        protected Chat()
        {
            Messages = new List<Message>();
        }
    }

    public class Message
    {
        public int Id { get; set; }
        public Usuarios Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}