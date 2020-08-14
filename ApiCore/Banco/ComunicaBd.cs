using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ApiCore.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ApiCore.Banco
{
    public class ComunicaBd : IDisposable
    {
        private readonly MySqlCommand Cmd;
        private readonly MySqlConnection Connection;

        //Conexão com o Banco
        public ComunicaBd(string connectionString)
        {
            Cmd = new MySqlCommand();
            Connection = new MySqlConnection(connectionString);
            Cmd.Connection = Connection;
        }

        //
        // Métodos de interação com a Tabela Patrimonios do Banco de dados //
        //        

        //Retorna lista de patrimonios
        public List<Patrimonio> GetAllPatrimonios()
        {
            List<Patrimonio> patrimoniosLista = new List<Patrimonio>();

            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from patrimonios";
                Cmd.CommandType = System.Data.CommandType.Text;

                //lendo dados  
                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patrimonio patrimonio = new Patrimonio();

                        patrimonio.Nome = reader.GetString(reader.GetOrdinal("nome"));
                        patrimonio.Descricao = reader.GetString(reader.GetOrdinal("descricao"));
                        patrimonio.IdMarca = reader.GetInt32(reader.GetOrdinal("idMarca"));
                        patrimonio.NumeroTombo = reader.GetInt32(reader.GetOrdinal("numeroTombo"));

                        patrimoniosLista.Add(patrimonio);
                    }
                }
                return patrimoniosLista;
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
        }

        //Retorna patrimonio pelo ID
        public Patrimonio GetPatrimonioByNumeroTombo(int numeroTombo)
        {
            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from Patrimonios where numeroTombo = {numeroTombo}";
                Cmd.CommandType = System.Data.CommandType.Text;

                Patrimonio patrimonio = new Patrimonio();

                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        patrimonio.NumeroTombo = reader.GetInt32(reader.GetOrdinal("numeroTombo"));
                        patrimonio.Nome = reader.GetString(reader.GetOrdinal("nome"));
                        patrimonio.IdMarca = reader.GetInt32(reader.GetOrdinal("idMarca"));
                        patrimonio.Descricao = reader.GetString(reader.GetOrdinal("descricao"));
                    }
                }
                return patrimonio;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Insere patrimonio
        public void InsertPatrimonio(Patrimonio patrimonio)
        {
            if (patrimonio.Nome == "" || patrimonio.Nome == null)
            {
                throw new DataException("Insira o nome do patrimonio.");
            }
            else
            {
                try
                {
                    Connection.Open();

                    Cmd.CommandText = $"insert into patrimonios(nome,idMarca,descricao,numeroTombo) values ('{patrimonio.Nome}','{patrimonio.IdMarca}'" +
                        $",'{patrimonio.Descricao}','{buscaNumeroTomboValido()}')";
                    Cmd.CommandType = System.Data.CommandType.Text;

                    if (Cmd.ExecuteNonQuery() == 0)
                        throw new DataException("Erro ao inserir dados do patrimonio");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public int buscaNumeroTomboValido() //Busca o numero de tombo, gera um numero aleatório e verifica se existe.
        {
            int numeroTombo = 0;
            bool valido = false;
            while (valido == false)
            {
                Patrimonio patrimonio = new Patrimonio();
                numeroTombo = patrimonio.GeraNumeroTombo();
                valido = true;
                try
                {
                    Cmd.CommandText = $"select * from patrimonios where numeroTombo={numeroTombo}";
                    Cmd.CommandType = System.Data.CommandType.Text;
                     
                    using (MySqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                                valido = false;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return numeroTombo;
        }


        //Atualiza dados do patrimonio
        public void UpdatePatrimonio(Patrimonio patrimonio)
        {
            if (patrimonio.Nome == "" || patrimonio.Nome == null)
            {
                throw new DataException("Insira o nome do patrimonio.");
            }
            else
            {
                try
                {
                    Connection.Open();
                    Cmd.CommandText = $" UPDATE patrimonios SET nome = '{patrimonio.Nome}', idMarca = {patrimonio.IdMarca}," +
                        $"descricao = '{patrimonio.Descricao}' WHERE numeroTombo = {patrimonio.NumeroTombo}";
                    Cmd.CommandType = System.Data.CommandType.Text;

                    if (Cmd.ExecuteNonQuery() == 0)
                        throw new DataException("Erro ao inserir dados do patrimonio");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //Deleta patrimonio pelo numero do Tombo
        public void DeletePatrimonio(int numeroTombo)
        {
            try
            {
                Connection.Open();
                Cmd.CommandText = $"DELETE FROM patrimonios WHERE numeroTombo = {numeroTombo}";
                Cmd.CommandType = System.Data.CommandType.Text;
                if (Cmd.ExecuteNonQuery() == 0)
                    throw new DataException("Erro ao tentar remover patrimonio.");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Retorna Lista de patrimonios pelo IdMarca
        public List<Patrimonio> GetPatrimonioByIdMarca(int idMarca)
        {
            List<Patrimonio> patrimoniosList = new List<Patrimonio>();

            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from Patrimonios where idMarca = {idMarca}";
                Cmd.CommandType = System.Data.CommandType.Text;

                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patrimonio patrimonio = new Patrimonio();

                        patrimonio.NumeroTombo = reader.GetInt32(reader.GetOrdinal("numeroTombo"));
                        patrimonio.Nome = reader.GetString(reader.GetOrdinal("nome"));
                        patrimonio.IdMarca = reader.GetInt32(reader.GetOrdinal("idMarca"));
                        patrimonio.Descricao = reader.GetString(reader.GetOrdinal("descricao"));

                        patrimoniosList.Add(patrimonio);
                    }
                }
                return patrimoniosList;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //
        // Métodos de interação com a Tabela Marcas do Banco de dados //
        //   

        //Retorna lista de todas as Marcas
        public List<Marca> GetAllMarcas()
        {
            List<Marca> marcaList = new List<Marca>();

            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from marcas";
                Cmd.CommandType = System.Data.CommandType.Text;

                //lendo dados  
                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Marca marca = new Marca();
                        marca.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        marca.Nome = reader.GetString(reader.GetOrdinal("nome"));
                        marcaList.Add(marca);
                    }
                }
                return marcaList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Retorna marca pelo IdMarca
        public Marca GetMarcaByIdMarca(int idMarca)
        {
            Marca marca = new Marca();

            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from Marcas where id = {idMarca}";
                Cmd.CommandType = System.Data.CommandType.Text;

                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marca.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        marca.Nome = reader.GetString(reader.GetOrdinal("nome"));
                    }
                }
                return marca;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }
                
        public bool nomeMarcaExiste(string nome)//Verifica existencia do nome
        {
            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from Marcas where nome = '{nome}'";
                Cmd.CommandType = System.Data.CommandType.Text;
                bool existe = false;
                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existe = reader.HasRows;
                    }
                }
                return existe;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }



        //Insere Marcas
        public void InsertMarca(string nomeMarca)
        {
            try
            {
                if (nomeMarcaExiste(nomeMarca))
                {
                    throw new DataException("Esse nome de marca já existe na base de dados.");
                }
                else if(nomeMarca=="" || nomeMarca == null)
                {
                    throw new DataException("Insira um nome.");
                }
                else
                {
                    Connection.Open();
                    Cmd.CommandText = $"insert into marcas(nome) values ('{nomeMarca}')";
                    Cmd.CommandType = System.Data.CommandType.Text;

                    if (Cmd.ExecuteNonQuery() == 0)
                        throw new DataException("Erro ao inserir dados");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Atualiza dados da Marca
        public void UpdateMarca(Marca marca)
        {
            if (nomeMarcaExiste(marca.Nome))
            {
                throw new DataException("Esse nome de marca já existe na base de dados.");
            }
            else
            {
                try
                {
                    Connection.Open();
                    Cmd.CommandText = $" UPDATE marcas SET nome = '{marca.Nome}' WHERE id = {marca.Id}";
                    if (Cmd.ExecuteNonQuery() == 0)
                        throw new DataException("Erro ao inserir dados da marca");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //Deleta Marca pelo id
        public void DeleteMarca(int id)
        {
            try
            {
                Connection.Open();
                Cmd.CommandText = $" delete from marcas WHERE id = {id}";
                Cmd.CommandType = System.Data.CommandType.Text;
                if (Cmd.ExecuteNonQuery() == 0)
                    throw new DataException("Erro ao inserir dados da marca");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Count
        public bool numeroTomboExiste(int numeroTombo)
        {
            try
            {
                Connection.Open();
                Cmd.CommandText = $"select * from Patrimonios where idMarca = {numeroTombo}";
                Cmd.CommandType = System.Data.CommandType.Text;
                bool existe = false;
                using (MySqlDataReader reader = Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existe = reader.HasRows;
                    }
                }
                return existe;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        //Dispose
        #region Dispose
        bool disposed = false;

        public void Dispose()
        {
            
            Connection.Dispose();
            Cmd.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;
        }
        #endregion

    }
}
