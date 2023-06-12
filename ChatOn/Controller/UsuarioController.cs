using ChatOn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatOn.Model;
using Microsoft.EntityFrameworkCore;
using Message = ChatOn.Model.Message;

namespace ChatOn.Controller
{
    public class UsuarioController
    {

        public static DataContext Context { get; } = new DataContext();

        internal static void SalvarUsuario(Usuarios ContaUsuarios)
        {
            if (ContaUsuarios.Login == "" || ContaUsuarios.Login.Length > 100)
            {
                throw new Exception("Login inválido");
            }
            if (ContaUsuarios.Email == "" || ContaUsuarios.Email.Length > 100)
            {
                throw new Exception("Email inválido");
            }
            if (ContaUsuarios.Senha == "" || ContaUsuarios.Senha.Length > 100)
            {
                throw new Exception("Senha inválido");
            }
            if (Context.Usuario.Any(p => p.Email == ContaUsuarios.Email))
            {
                throw new Exception("Já existe um usuário cadastrado com este email");
            }

            try
            {
                Context.Usuario.Add(ContaUsuarios);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.InnerException.Message);
            }

        }

        internal static void Atualizar(Usuarios item)
        {
            Context.Usuario.Update(item);
            Context.SaveChanges();
        }

        internal static void AtualizarNome(string login, string novoNome)
        {
            // Find the user with the provided login
            Usuarios usuario = Context.Usuario.FirstOrDefault(u => u.Login == login);
            if (usuario != null)
            {
                // Update the user's name
                usuario.NomeUsuario = novoNome;
                Context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }

        internal static void AtualizarSenha(string login, string novaSenha)
        {
            Usuarios usuario = Context.Usuario.FirstOrDefault(u => u.Login == login);
            if (usuario != null)
            {
                usuario.Senha = novaSenha;
                Context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }

        internal static void SalvarImagem(string login, byte[] imageData)
        {
            Usuarios usuario = Context.Usuario.FirstOrDefault(u => u.Login == login);
            if (usuario != null)
            {
                usuario.ImagemPerfil = imageData;
                Context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }

        internal static byte[] GetImagem(string login)
        {
            Usuarios usuario = Context.Usuario.FirstOrDefault(u => u.Login == login);
            if (usuario != null)
            {
                return usuario.ImagemPerfil;
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }

        internal static void EnviarPedidoAmizade(string senderEmail, string recipientEmail)
        {
            // Find the sender and recipient users in the database
            Usuarios sender = Context.Usuario.FirstOrDefault(u => u.Email == senderEmail);
            Usuarios recipient = Context.Usuario.FirstOrDefault(u => u.Email == recipientEmail);

            if (sender == null)
            {
                throw new Exception("Remetente não encontrado");
            }

            if (recipient == null)
            {
                throw new Exception("Destinatário não encontrado");
            }

            // Add the sender to the PedidoAmizade list of the recipient
            recipient.PedidoAmizade.Add(sender);

            // Save changes to the database
            Context.SaveChanges();
        }

        internal static List<Usuarios> GetFriendList(int userId)
        {
            // Retrieve the logged-in user
            Usuarios loggedUser = Context.Usuario
                .Include(u => u.Amigos) // Include the Amigos navigation property
                    .ThenInclude(a => a.Amigos) // Include the Amigos' Amigos navigation property
                .FirstOrDefault(u => u.Id == userId);

            if (loggedUser != null)
            {
                // Return the list of friends
                return loggedUser.Amigos.ToList();
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }

        internal static void SaveChat(Chat chat)
        {
            Context.Chat.Update(chat);
            Context.SaveChanges();
        }

        internal static Chat GetChat(Usuarios user1, Usuarios user2)
        {
            Chat chat = Context.Chat
                .Include(c => c.Messages)
                .FirstOrDefault(c =>
                    (c.User1Id == user1.Id && c.User2Id == user2.Id) ||
                    (c.User1Id == user2.Id && c.User2Id == user1.Id));

            if (chat == null)
            {
                chat = new Chat(user1, user2);
                Context.Chat.Add(chat);
                Context.SaveChanges();
            }

            return chat;
        }
    }
}