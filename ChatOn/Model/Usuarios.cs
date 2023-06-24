using System;
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

        public bool HasUnreadMessages { get; set; }

        // propriedade de navegação que representa os amigos do usuário
        public virtual ICollection<Usuarios> Amigos { get; set; }

        // Propriedade de navegação que representa as solicitações de amizade pendentes
        public virtual ICollection<Usuarios> PedidoAmizade { get; set; }

        // Propriedade de navegação que representa os chats em que o usuário está envolvido
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

        [ForeignKey("User1Id")]
        public Usuarios User1 { get; set; }

        [ForeignKey("User2Id")]
        public Usuarios User2 { get; set; }

        public List<Message> Messages { get; set; }

        public Chat(Usuarios user1, Usuarios user2)
        {
            User1 = user1;
            User2 = user2;
            Messages = new List<Message>();
        }

        // construtor sem parâmetros para EF Core
        protected Chat()
        {
            Messages = new List<Message>();
        }
    }

    public class Message
    {
        public int Id { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        public Usuarios Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}