using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alura.ListaLeitura.Seguranca
{
    internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasData(new Usuario {
                //UserName = "admin",
                //Email = "admin@example.org",
                //PasswordHash = "AQAAAAEAACcQAAAAED0tb8N23CW0B1uLCmdSzL1kfJKD1NqSU6VxzkJ/ATsHW8awVv+bBSmNiACpNR9Iqw==",

                UserName = "FranTeste",
                Email = "",
                PasswordHash = "AQAAAAEAACcQAAAAEB5r6XxrINMjwbu5orKYKqkb6ilwzmsWtbOVgbDGFYzPf4mSVlABnqHvSUjcDNZ6KQ==",


                //UserName: admin
                //Email: admin@example.org
                //PasswordHash: AQAAAAEAACcQAAAAED0tb8N23CW0B1uLCmdSzL1kfJKD1NqSU6VxzkJ/ATsHW8awVv+bBSmNiACpNR9Iqw==
                //SecurityStamp: 0ddd8b02-e773-44ea-a439-65f7b4cca909
                //ConcurrencyStamp: ef8de79a-093c-4824-879c-3df38b35b89d
            });
        }
    }
}