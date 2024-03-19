using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using TajamarProyecto.Data;
using TajamarProyecto.Helpers;
using TajamarProyecto.Models;

namespace TajamarProyecto.Repositories
{
    public class RepositoryUsuarios
    {
        private UsuariosContext context;
        private readonly IConfiguration configuration;


        public RepositoryUsuarios(UsuariosContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await
                    this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }



        public async Task RegisterUser(string nombre, string email
            , string password, string linkedin)
        {
            Usuario user = new Usuario();
            user.Nombre = nombre;
            user.Email = email;
            user.Linkedin = linkedin;
            user.Role = "alumno";
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperTools.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password =
                HelperCryptography.EncryptPassword(password, user.Salt);
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        //NECESITAMOS UN METODO PARA VALIDAR AL USUARIO
        //DICHO METODO DEVOLVERA EL PROPIO USUARIO
        //COMO COMPARAMOS?? email CAMPO UNICO
        //password (12345)
        //1) RECUPERAR EL USER POR SU EMAIL
        //2) RECUPERAMOS EL SALT DEL USUARIO
        //3) CONVERTIMOS DE NUEVO EL PASSWORD CON EL SALT
        //4) RECUPERAMOS EL BYTE[] DE PASSWORD DE LA BBDD
        //5) COMPARAMOS LOS DOS ARRAYS (BBDD) Y EL GENERADO EN EL CODIGO
       

        public async Task<Usuario>
           RegisterUserAsync(string nombre, string email
           , string password, string linkedin)
        {
            Usuario user = new Usuario();
            
            user.Nombre = nombre;
            user.Email = email;
            user.Linkedin = linkedin;
            user.Role = "alumno";
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperTools.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password =
                HelperCryptography.EncryptPassword(password, user.Salt);
            user.Activo = false;
            user.TokenMail = HelperTools.GenerateTokenMail();
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task ActivateUserAsync(string token)
        {
            //BUSCAMOS EL USUARIO POR SU TOKEN
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.TokenMail == token);
            user.Activo = true;
            user.TokenMail = "";
            await this.context.SaveChangesAsync();
        }

        //NECESITAMOS UN METODO PARA VALIDAR AL USUARIO
        //DICHO METODO DEVOLVERA EL PROPIO USUARIO
        //COMO COMPARAMOS?? email CAMPO UNICO
        //password (12345)
        //1) RECUPERAR EL USER POR SU EMAIL
        //2) RECUPERAMOS EL SALT DEL USUARIO
        //3) CONVERTIMOS DE NUEVO EL PASSWORD CON EL SALT
        //4) RECUPERAMOS EL BYTE[] DE PASSWORD DE LA BBDD
        //5) COMPARAMOS LOS DOS ARRAYS (BBDD) Y EL GENERADO EN EL CODIGO
        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = user.Password;
                bool response =
                    HelperTools.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.IdUsuario == idUsuario);
        }
        public async Task<EntrevistaAlumno> FindEntrevistaAsync(int idUsuario)
        {
            return await this.context.EntrevistasAlumnos.FirstOrDefaultAsync(z => z.IdAlumno == idUsuario);
        }


        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this.context.Usuarios select datos;
            return consulta.ToList();
        }

        public Usuario FindUsuario(int id)
        {

            string sql = "SP_FIND_USUARIO @idusuario";
            //PARA DECLARAR PARAMETROS SE UTILIZA LA CLASE SqlParameter
            //DEBEMOS TENER CUIDADO CON EL NAMESPACE
            //EL NAMESPACE ES Microsoft.Data
            SqlParameter pamId =
                new SqlParameter("@idusuario", id);
            //AL SER UN PROCEDIMIENTO SELECT, PUEDO UTILIZAR 
            //EL METODO FromSqlRaw PARA EXTRAER LOS DATOS
            //SI MI CONSULTA COINCIDE CON UN MODEL, PUEDO UTILIZAR 
            //LINQ PARA MAPEAR LOS DATOS.
            //CUANDO TENEMOS UN PROCEDURE SELECT, LAS PETICIONES SE 
            //DIVIDEN EN DOS.  NO PUEDO HACER LINQ Y DESPUES UN foreach
            //DEBEMOS EXTRAER LOS DATOS EN DOS ACCIONES
            var consulta = this.context.Usuarios.FromSqlRaw(sql, pamId);
            //EXTRAER LAS ENTIDADES DE LA CONSULTA (EJECUTAR)
            //PARA EJECUTAR, NECESITAMOS AsEnumerable()
            Usuario usuario = consulta.AsEnumerable().FirstOrDefault();
            return usuario;
        }


        public async Task InsertarEmpresasEnUsuario(int idUsuario, int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("SqlTajamar")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SP_UpdateEmpresasUsuario_6", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    command.Parameters.Add(new SqlParameter("@Emp1Id", idempresa1 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Emp2Id", idempresa2 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Emp3Id", idempresa3 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Emp4Id", idempresa4 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Emp5Id", idempresa5 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Emp6Id", idempresa6 ?? (object)DBNull.Value));

                    // Ejecutar el comando
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        







    }
}

